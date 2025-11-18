using NAudio.Gui;
using NAudio.Wave;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Webp;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Buffer = System.Buffer;
using Device = SharpDX.Direct3D11.Device;
using FileType = BPSRCapture.BPSRCaptureConfigManager.FileType;
using Format = SharpDX.DXGI.Format;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
using Resource = SharpDX.DXGI.Resource;
using ShortcutKeys = BPSRCapture.KeySetting.ShortcutKeys;

namespace BPSRCapture
{
    public partial class BPSRCaptureMain : Form
    {
        // 設定ファイルマネージャ
        private BPSRCaptureConfigManager configManager;

        // キーフッククラス
        private GlobalKeyboardHook _keyboardHook;

        // キャプチャ対象ウィンドウタイトル(部分一致)
        private const string windowName = "ブループロトコル";
        private const string processName = "StarASIA";

        // 効果音用オブジェクト
        private readonly WaveFileReader _waveReader;
        private readonly WaveOutEvent _waveOut = new WaveOutEvent();

        // Viewerウィンドウ
        private Viewer _viewer;

        public BPSRCaptureMain()
        {
            InitializeComponent();
            InitializeKeyboardHook();

            // コンボボックス生成(キー)
            comboBox_key.Items.Clear();
            comboBox_key.ValueMember = "Key";
            comboBox_key.DisplayMember = "Name";
            List<KeySetting> comboData = new List<KeySetting>();
            foreach (ShortcutKeys item in Enum.GetValues(typeof(ShortcutKeys)))
            {
                string? name = Enum.GetName(typeof(ShortcutKeys), item);
                if (name == null) continue;
                comboData.Add(new KeySetting(item, name));
            }
            comboBox_key.DataSource = comboData;
            // コンボボックス生成(グラフィックアダプタ)
            comboBox_adapter.Items.Clear();
            comboBox_adapter.ValueMember = "ID";
            comboBox_adapter.DisplayMember = "Name";
            List<AdapterItem> comboData2 = new List<AdapterItem>();
            using (Factory1 fa = new Factory1())
            {
                foreach (var adapter in fa.Adapters)
                {
                    AdapterItem item = new AdapterItem(adapter.Description.DeviceId, adapter.Description.Description);
                    comboData2.Add(item);
                }
            }
            comboBox_adapter.DataSource = comboData2;
            // コンボボックス(ファイルタイプ)はItemsプロパティ直指定

            // 設定読込
            configManager = new BPSRCaptureConfigManager();
            configManager.Load();

            if (configManager.conf != null)
            {
                textBox_savePath.Text = configManager.conf.SavePath;
                comboBox_key.SelectedValue = configManager.conf.Key;
                checkBox_ctrl.Checked = configManager.conf.CtrlKey;
                checkBox_shift.Checked = configManager.conf.ShiftKey;
                checkBox_alt.Checked = configManager.conf.AltKey;
                Size = new Size(configManager.conf.Width, configManager.conf.Height);
                AdapterItem selectedItem = comboData2.Find(x => x.ID == configManager.conf.DeviceID)!;
                if (selectedItem != null)
                {
                    comboBox_adapter.SelectedItem = selectedItem;
                }
                else
                {
                    comboBox_adapter.SelectedIndex = 0;
                }
                comboBox_filetype.SelectedIndex = (int)configManager.conf.FileType;
                volumeSlider.CurrentValue = configManager.conf.Volume;
                UpdateVolumeImg();
            }

            // 音声プリロード
            using var stream = Properties.Resources.Camera;
            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Position = 0;
            _waveReader = new WaveFileReader(memoryStream);
            _waveOut.Init(_waveReader);

            // ビューワーウィンドウサイズデフォルト値設定
            if (configManager.conf.viewerWidth <= 0) configManager.conf.viewerWidth = 800;
            if (configManager.conf.viewerHeight <= 0) configManager.conf.viewerHeight = 600;
        }

        /**
         * 終了時に設定保存
         */
        private void BPSRCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            configManager.conf.SavePath = textBox_savePath.Text;
            configManager.conf.Key = (ShortcutKeys)comboBox_key.SelectedValue!;
            configManager.conf.CtrlKey = checkBox_ctrl.Checked;
            configManager.conf.ShiftKey = checkBox_shift.Checked;
            configManager.conf.AltKey = checkBox_alt.Checked;
            configManager.conf.Width = Size.Width;
            configManager.conf.Height = Size.Height;
            if (comboBox_adapter.SelectedValue != null)
                configManager.conf.DeviceID = ((AdapterItem)comboBox_adapter.SelectedValue!).ID;
            configManager.conf.FileType = (FileType)comboBox_filetype.SelectedIndex;
            configManager.conf.Volume = volumeSlider.CurrentValue;
            configManager.Save();
            _keyboardHook?.Dispose();
        }

        /**
         * 保存先フォルダ設定
         */
        private void button_selectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "SS保存先を選択";
            dialog.RootFolder = Environment.SpecialFolder.MyPictures;
            dialog.SelectedPath = textBox_savePath.Text;
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox_savePath.Text = dialog.SelectedPath;
            }
        }

        /**
         * 初期表示時、保存先フォルダが存在しない場合、ユーザーに選択させる
         */
        private void BPSRCaptureMain_Shown(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox_savePath.Text))
            {
                MessageBox.Show("保存先のフォルダを選択してください", "えらー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button_selectPath_Click(this, new EventArgs());
            }
        }

        /**
         * フック時の処理
         */
        private void OnKeyHook(object sender, GlobalKeyboardHookEventArgs e)
        {
            Keys modifier = (checkBox_ctrl.Checked ? Keys.Control : Keys.None) |
                (checkBox_shift.Checked ? Keys.Shift : Keys.None) |
                (checkBox_alt.Checked ? Keys.Alt : Keys.None);

            Keys tgtKey = (Keys)comboBox_key.SelectedValue!;
            if (e.Key == tgtKey && modifier == Control.ModifierKeys)
            {
                DoCapture();
                e.Handled = true;
            }
        }

        /**
         * キャプチャ実行
         */
        private void DoCapture()
        {
            var now = DateTime.Now;
            long ticks = now.Ticks;
            int milliseconds = (int)((ticks / TimeSpan.TicksPerMillisecond) % 1000);
            int microseconds = (int)((ticks % TimeSpan.TicksPerMillisecond) * 100);
            string fileName = textBox_savePath.Text + Path.DirectorySeparatorChar + $"{now:yyyyMMddHHmmss}_{milliseconds:D3}_{microseconds:D6}";

            switch ((FileType)comboBox_filetype.SelectedIndex)
            {
                case FileType.PNG:
                    using (Bitmap bmp = CaptureWindowWithDXGI(windowName, processName, ((AdapterItem)comboBox_adapter.SelectedValue!).ID))
                    {
                        if (bmp != null) bmp.Save(fileName + ".png", ImageFormat.Png);
                    }
                    break;
                case FileType.JPEG:
                    using (Bitmap bmp = CaptureWindowWithDXGI(windowName, processName, ((AdapterItem)comboBox_adapter.SelectedValue!).ID))
                    {
                        if (bmp != null) bmp.Save(fileName + ".jpg", ImageFormat.Jpeg);
                    }
                    break;
                case FileType.WEBP_LOSSY:
                    SaveAsWebpThread(((AdapterItem)comboBox_adapter.SelectedValue!).ID, fileName + ".webp", false, 100);
                    break;
                case FileType.WEBP_LOSSLESS:
                    SaveAsWebpThread(((AdapterItem)comboBox_adapter.SelectedValue!).ID, fileName + ".webp", true, 100);
                    break;
                case FileType.BITMAP:
                    using (Bitmap bmp = CaptureWindowWithDXGI(windowName, processName, ((AdapterItem)comboBox_adapter.SelectedValue!).ID))
                    {
                        if (bmp != null) bmp.Save(fileName + ".bmp", ImageFormat.Bmp);
                    }
                    break;
            }
            PlaySound();
        }

        /**
         * キーフックイベント登録
         */
        private void InitializeKeyboardHook()
        {
            try
            {
                _keyboardHook = new GlobalKeyboardHook();

                _keyboardHook.KeyDown += (sender, e) =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        OnKeyHook(sender!, e);
                    });
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"フック失敗: {ex.Message}\r\n\r\n→ 管理者として実行してください。",
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void SaveAsWebpThread(int adapterId, string filename, bool isLossless, int quality = -1)
        {
            Task.Run(() =>
            {
                SaveAsWebp(adapterId, filename, isLossless, quality);
            });
        }

        public static void SaveAsWebp(int adapterId, string filename, bool isLossless, int quality = -1)
        {
            using Bitmap bmp = CaptureWindowWithDXGI(windowName, processName, adapterId);
            SaveAsWebp(bmp, filename, isLossless, quality);
        }

        public static void SaveAsWebp(Bitmap bmp, string filename, bool isLossless, int quality = -1)
        {
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            using var image = SixLabors.ImageSharp.Image.Load(ms);

            quality = quality >= 0 && quality <= 100 ? quality : 100;
            var encoder = isLossless
                ? new WebpEncoder { FileFormat = WebpFileFormatType.Lossless, Quality = quality }
                : new WebpEncoder { FileFormat = WebpFileFormatType.Lossy, Quality = quality };

            string tempPath = Path.GetTempFileName();
            using var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
            image.Save(fs, encoder);
            fs.Close();
            File.Move(tempPath, filename);
        }

        public static Bitmap CaptureWindowWithDXGI(string titleContains, string processName, int devideId)
        {
            try
            {
                // 対象ウィンドウのハンドル取得
                IntPtr hWnd = FindWindowByTitleAndProcess(titleContains, processName);
                if (hWnd == IntPtr.Zero)
                {
                    Console.WriteLine("ウィンドウが見つかりません");
                    return null;
                }

                // ウィンドウのクライアント領域取得
                if (!Win32.GetClientRect(hWnd, out Win32.RECT clientRect))
                    return null;

                if (!Win32.GetWindowRect(hWnd, out Win32.RECT windowRect))
                    return null;

                // ウィンドウが最小化されている場合はキャプチャ不可
                if (Win32.IsIconic(hWnd))
                {
                    Console.WriteLine("ウィンドウが最小化されています");
                    return null;
                }

                // デバイスが存在しない場合処理しない
                using (var fa = new Factory1())
                {
                    if (fa.Adapters.ToList().Where(x => x.Description.DeviceId == devideId).Count() == 0) return null;
                }

                // スクリーン座標に変換
                Point topLeft = new Point(clientRect.Left, clientRect.Top);
                Point bottomRight = new Point(clientRect.Right, clientRect.Bottom);

                if (!Win32.ClientToScreen(hWnd, ref topLeft)) return null;
                if (!Win32.ClientToScreen(hWnd, ref bottomRight)) return null;

                // 結果を clientRect に反映
                clientRect.Left = topLeft.X;
                clientRect.Top = topLeft.Y;
                clientRect.Right = bottomRight.X;
                clientRect.Bottom = bottomRight.Y;

                int width = clientRect.Width;
                int height = clientRect.Height;

                if (width <= 0 || height <= 0) return null;

                // DXGI を使ってデスクトップを複製
                using (var factory = new Factory1())
                using (var adapter = factory.Adapters.ToList().Find(x => x.Description.DeviceId == devideId))
                {
                    int adapterIndex = 0;
                    for (int i = 0; i < factory.Adapters.Length; i++)
                    {
                        if (factory.Adapters[i].Description.DeviceId == devideId)
                        {
                            adapterIndex = i;
                            break;
                        }
                    }
                    using (var device = new Device(adapter, DeviceCreationFlags.BgraSupport))
                    using (var output1 = adapter.GetOutput(GetDisplayIndexFromRect(clientRect, adapterIndex)).QueryInterface<Output1>())
                    using (var duplication = output1.DuplicateOutput(device))
                    {
                        // ダミーフレーム回避
                        AcquireAndReleaseFrame(duplication);

                        // 1フレーム取得
                        Resource desktopResource = null;
                        OutputDuplicateFrameInformation frameInfo;

                        var result = duplication.TryAcquireNextFrame(100, out frameInfo, out desktopResource);
                        if (!result.Success)
                        {
                            Console.WriteLine("フレーム取得失敗（タイムアウトまたはエラー）");
                            return null;
                        }
                        try
                        {
                            using (var texture = desktopResource.QueryInterface<Texture2D>())
                            {
                                // テクスチャの説明を取得
                                var desc = texture.Description;
                                desc.MipLevels = 1;
                                desc.ArraySize = 1;
                                desc.Format = Format.B8G8R8A8_UNorm;
                                desc.SampleDescription.Count = 1;
                                desc.Usage = ResourceUsage.Staging;
                                desc.BindFlags = BindFlags.None;
                                desc.CpuAccessFlags = CpuAccessFlags.Read;
                                desc.OptionFlags = ResourceOptionFlags.None;

                                using (var staging = new Texture2D(device, desc))
                                {
                                    device.ImmediateContext.CopyResource(texture, staging);

                                    var mapSource = device.ImmediateContext.MapSubresource(
                                        staging, 0, 0, MapMode.Read, MapFlags.None, out DataStream stream);

                                    try
                                    {
                                        // Bitmap 作成
                                        var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                                        var bounds = new Rectangle(0, 0, width, height);
                                        var bitmapData = bitmap.LockBits(bounds, ImageLockMode.WriteOnly, bitmap.PixelFormat);

                                        try
                                        {
                                            int srcRowPitch = mapSource.RowPitch;
                                            int dstRowPitch = bitmapData.Stride;

                                            IntPtr srcPtr = mapSource.DataPointer;
                                            IntPtr dstPtr = bitmapData.Scan0;

                                            int cropX = clientRect.Left;
                                            int cropY = clientRect.Top;

                                            // 必要な部分だけコピー（ウィンドウ位置に合わせてクロップ）
                                            unsafe
                                            {
                                                byte* src = (byte*)srcPtr.ToPointer();
                                                byte* dst = (byte*)dstPtr.ToPointer();

                                                for (int y = 0; y < height; y++)
                                                {
                                                    int srcOffset = (cropY + y) * srcRowPitch + cropX * 4;
                                                    int dstOffset = y * dstRowPitch;
                                                    Buffer.MemoryCopy(src + srcOffset, dst + dstOffset, dstRowPitch, width * 4);
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            bitmap.UnlockBits(bitmapData);
                                        }

                                        return bitmap;
                                    }
                                    finally
                                    {
                                        device.ImmediateContext.UnmapSubresource(staging, 0);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            duplication.ReleaseFrame();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラー: " + ex.Message);
                return null;
            }
        }

        // ダミーフレーム取得・解放（最初の黒フレームスキップ）
        private static void AcquireAndReleaseFrame(OutputDuplication duplication)
        {
            Resource dummyResource = null;
            OutputDuplicateFrameInformation dummyInfo;
            var result = duplication.TryAcquireNextFrame(50, out dummyInfo, out dummyResource);
            if (result.Success && dummyResource != null)
            {
                using (dummyResource) { }  // 即解放
            }
            if (result.Success)
            {
                duplication.ReleaseFrame();
            }
        }

        public static int GetDisplayIndexFromRect(Win32.RECT clientRect, int adapterIndex)
        {
            try
            {
                using (var factory = new Factory1())
                {
                    int displayIndex = 0;
                    using (var adapter = factory.GetAdapter(adapterIndex))
                    {
                        for (int outputIndex = 0; outputIndex < adapter.Outputs.Length; outputIndex++)
                        {
                            using (var output = adapter.GetOutput(outputIndex))
                            {
                                var desc = output.Description;
                                var bounds = desc.DesktopBounds;

                                // ウィンドウの中心点がモニタ内にあるか？
                                int centerX = clientRect.Left + (clientRect.Width / 2);
                                int centerY = clientRect.Top + (clientRect.Height / 2);

                                if (centerX >= bounds.Left && centerX < bounds.Right &&
                                    centerY >= bounds.Top && centerY < bounds.Bottom)
                                {
                                    Console.WriteLine($"ディスプレイ発見: {desc.DeviceName} (Index: {displayIndex})");
                                    return displayIndex;
                                }

                                displayIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ディスプレイ検索エラー: {ex.Message}");
            }

            return -1; // 見つからない
        }

        // ウィンドウ検索（タイトルとプロセス名で）
        static IntPtr FindWindowByTitleAndProcess(string titleContains, string processName)
        {
            foreach (var proc in System.Diagnostics.Process.GetProcesses())
            {
                if (proc.ProcessName.IndexOf(processName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (!string.IsNullOrEmpty(proc.MainWindowTitle) &&
                        proc.MainWindowTitle.IndexOf(titleContains, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return proc.MainWindowHandle;
                    }
                }
            }
            return IntPtr.Zero;
        }

        private void button_openFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox_savePath.Text))
            {
                System.Diagnostics.Process.Start("explorer.exe", textBox_savePath.Text);
            }
            else
            {
                MessageBox.Show("フォルダがないよ", "えらー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void volumeMuteButton_Click(object sender, EventArgs e)
        {
            configManager.conf.Mute = !configManager.conf.Mute;
            UpdateVolumeImg();
        }

        private void UpdateVolumeImg()
        {
            if (configManager.conf.Mute)
            {
                volumeMuteButton.Image = Properties.Resources.AudioMute;
            }
            else
            {
                volumeMuteButton.Image = Properties.Resources.Volume;
            }
        }

        private void PlaySound()
        {
            if (_waveOut.PlaybackState == PlaybackState.Playing)
            {
                _waveReader.Position = 0;
            }
            else
            {
                float volumeValue = configManager.conf.Mute ? 0.0f : (float)volumeSlider.CurrentValue / 100.0f;
                _waveReader.Position = 0;
                _waveOut.Volume = volumeValue;
                _waveOut.Play();
            }
        }

        private void volumeSlider_ValueChanged(object sender, ValueChangeEventArgs e)
        {
            configManager.conf.Volume = e.NewValue;
        }

        private void openViewButton_Click(object sender, EventArgs e)
        {
            Point pos = new Point(Location.X, Location.Y + Size.Height);
            if (_viewer != null && !_viewer.IsDisposed)
            {
                _viewer.Location = pos;
                _viewer.Activate();
            }
            else
            {
                _viewer = new Viewer(textBox_savePath.Text, configManager);
                _viewer.Owner = this;
                _viewer.FormClosing += ViewerClosing;
                _viewer.Width = configManager.conf.viewerWidth;
                _viewer.Height = configManager.conf.viewerHeight;
                _viewer.Location = pos;
                _viewer.Show();
            }
        }

        private void ViewerClosing(Object? sender, FormClosingEventArgs e)
        {
            configManager.conf.RecentCnt = (int)_viewer.recentLimit.Value;
            configManager.conf.RecentFlg = _viewer.recentChk.Checked;
            configManager.conf.viewerWidth = _viewer.Size.Width;
            configManager.conf.viewerHeight = _viewer.Size.Height;
        }

        private void BPSRCaptureMain_LocationChanged(object sender, EventArgs e)
        {
            Point pos = new Point(Location.X, Location.Y + Size.Height);
            if (_viewer != null && !_viewer.IsDisposed)
            {
                _viewer.Location = pos;
            }
        }

        private void BPSRCaptureMain_Activated(object sender, EventArgs e)
        {
            if (_viewer != null && !_viewer.IsDisposed)
            {
                //_viewer.BringToFront();
            }
        }
    }

    public static class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => Right - Left;
            public int Height => Bottom - Top;

            public Point LeftTop
            {
                get => new Point(Left, Top);
                set { Left = value.X; Top = value.Y; }
            }
            public Point RightBottom
            {
                get => new Point(Right, Bottom);
                set { Right = value.X; Bottom = value.Y; }
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);
    }
}
