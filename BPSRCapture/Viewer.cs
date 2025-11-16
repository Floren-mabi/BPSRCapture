using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Image = SixLabors.ImageSharp.Image;

namespace BPSRCapture
{
    public partial class Viewer : Form
    {
        private const int thumbsWidth = 160;
        private const int thumbsHeight = 160;
        static List<string> filterList;
        private string path;
        BPSRCaptureConfigManager configManager;

        internal Viewer(string path, BPSRCaptureConfigManager mgr)
        {
            this.path = path;
            configManager = mgr;
            InitializeComponent();
        }

        private void UpdateList()
        {
            ClearList();
            if (!Directory.Exists(path)) return;
            UpdateListMain(path);
        }

        private void ClearList()
        {
            Action clearAction = () =>
            {
                itemListView.BeginUpdate();
                try
                {
                    if (itemListView.VirtualMode) itemListView.VirtualListSize = 0;
                    itemListView.Items.Clear();
                    itemListView.Groups.Clear();
                    itemListView.LargeImageList.Images.Clear();
                    itemListView.Invalidate();
                }
                finally
                {
                    itemListView.EndUpdate();
                }
            };
            if (itemListView.InvokeRequired)
            {
                itemListView.Invoke(clearAction);
            }
            else
            {
                clearAction();
            }
        }

        private void Viewer_Shown(object sender, EventArgs e)
        {
            fileSystemWatcher.EnableRaisingEvents = true;
            UpdateListTimerStart();
        }

        private async void UpdateListMain(string path)
        {
            List<string> list = Directory.GetFiles(path).ToList();
            if (recentChk.Checked)
            {
                list = list.OrderByDescending(x => x).Take((int)recentLimit.Value).ToList();
            }
            ConcurrentBag<ThumbnailInfo> infoBag = new ConcurrentBag<ThumbnailInfo>();
            var tasks = list.Select(str => Task.Run(() => AddListWithThumbs(str, infoBag)));
            await Task.WhenAll(tasks);
            List<ThumbnailInfo> infoList = infoBag.ToList();
            infoList = infoList.OrderByDescending(x => x.Path).ToList();
            for (int i = 0; i < infoList.Count; i++)
            {
                itemListView.LargeImageList.Images.Add(infoList[i].Image);
                itemListView.Items.Add(Path.GetFileName(infoList[i].Path), i);
            }
        }

        static void AddListWithThumbs(string filePath, ConcurrentBag<ThumbnailInfo> info)
        {
            try
            {
                string ext = Regex.Match(filePath, "\\..+$").Value;

                if (ext != null)
                {
                    ext = "*" + ext.ToLower();
                }
                else
                {
                    return;
                }
                if (!filterList.Contains(ext)) return;

                using (Image loadImg = Image.Load(filePath))
                using (MemoryStream ms = new MemoryStream())
                {
                    bool isPortrait = loadImg.Height > loadImg.Width;
                    var (width, height) = CalculateFitSize(loadImg.Width, loadImg.Height);
                    loadImg.Mutate(x => x.Resize(width, height));
                    var hoge = loadImg.PixelType;
                    loadImg.Save(ms, new PngEncoder());
                    ms.Position = 0;
                    using Bitmap thumbsBmp = new Bitmap(ms);
                    using Bitmap newBmp = new Bitmap(thumbsWidth, thumbsHeight);
                    using Graphics g = Graphics.FromImage(newBmp);
                    g.FillRectangle(Brushes.White, new System.Drawing.Rectangle(0, 0, thumbsWidth, thumbsHeight));
                    g.DrawImage(thumbsBmp, (float)(thumbsWidth - thumbsBmp.Width) / 2f, (float)(thumbsHeight - thumbsBmp.Height) / 2f);
                    g.Dispose();
                    info.Add(new ThumbnailInfo(filePath, new Bitmap(newBmp)));
                }
            } catch (UnknownImageFormatException)
            {
                Debug.WriteLine("Unsupported Format");
            } catch (Exception)
            {
                Debug.WriteLine("その他のエラー");
            }
        }

        static (int newWidth, int newHeight) CalculateFitSize(int originalWidth, int originalHeight)
        {
            // 縦長か判定
            bool isPortrait = originalHeight > originalWidth;

            if (isPortrait)
            {
                // 縦長 → 90x160 にフィット
                double ratio = Math.Min(90.0 / originalWidth, 160.0 / originalHeight);
                return ((int)(originalWidth * ratio), (int)(originalHeight * ratio));
            }
            else
            {
                // 横長 or 正方形 → 160x90 にフィット
                double ratio = Math.Min(160.0 / originalWidth, 90.0 / originalHeight);
                return ((int)(originalWidth * ratio), (int)(originalHeight * ratio));
            }
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            // 設定ロード
            recentChk.Checked = configManager.conf.RecentFlg;
            filterList = @"*.png\r\n*.jpg\r\n*.jpeg\r\n*.webp\r\n*.bmp".Split(@"\r\n").ToList();
            fileSystemWatcher.Path = path;

            fileSystemWatcher.NotifyFilter = (
                NotifyFilters.Attributes
                | NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.FileName);
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = false;
            thumbsList.ImageSize = new System.Drawing.Size(thumbsWidth, thumbsHeight);
            itemListView.LargeImageList = thumbsList;
            // 最後にしないとValueChangeが先に走ってしまうので注意
            recentLimit.Value = configManager.conf.RecentCnt <= 0 ? 10 : configManager.conf.RecentCnt;

            // ファイル監視イベント
            fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher.Changed += new FileSystemEventHandler(watcher_Created);
        }

        private void recentLimit_ValueChanged(object sender, EventArgs e)
        {
            if (!recentChk.Checked) return;
            UpdateListTimerStart();
        }

        private void recentChk_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListTimerStart();
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            UpdateListTimerStart();
        }

        private void Rotate(float angle)
        {
            if (itemListView.SelectedItems == null || itemListView.SelectedItems.Count == 0) return;
            string fileName = itemListView.SelectedItems[0].Text;
            string filePath = path + Path.DirectorySeparatorChar + fileName;
            using (Image loadImg = Image.Load(filePath))
            {
                loadImg.Mutate(x => x.Rotate(angle));
                var format = loadImg.Metadata.DecodedImageFormat;
                string newFilePath = path + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filePath) + "_rotate" + Path.GetExtension(filePath);
                using FileStream fs = new FileStream(newFilePath, FileMode.Create);
                switch (format.Name)
                {
                    case "PNG":
                        loadImg.Save(fs, new PngEncoder());
                        break;
                    case "JPEG":
                        loadImg.Save(fs, new JpegEncoder());
                        break;
                    case "Webp":
                        if (format == null) return;
                        var metaData = loadImg.Metadata.GetWebpMetadata();
                        using (Bitmap bmp = ToBitmap(loadImg))
                        {
                            WebpEncoder encoder = metaData.FileFormat == WebpFileFormatType.Lossless
                                ? new WebpEncoder { FileFormat = WebpFileFormatType.Lossless, Quality = 100 }
                                : new WebpEncoder { FileFormat = WebpFileFormatType.Lossy, Quality = 100 };
                            loadImg.Save(fs, encoder);
                        }
                        break;
                    case "BMP":
                        loadImg.Save(fs, new BmpEncoder());
                        break;
                }
            }
        }

        private Bitmap ToBitmap(Image image)
        {
            var rgbaImage = image.CloneAs<Rgba32>();
            int width = rgbaImage.Width;
            int height = rgbaImage.Height;

            byte[] pixelData = new byte[width * height * 4];  // 4 = R,G,B,A
            rgbaImage.CopyPixelDataTo(pixelData);

            // 2. Bitmap 作成
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // 3. ピクセルデータを Bitmap にロックしてコピー
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelData, 0, bitmapData.Scan0, pixelData.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        private void toolStripMenuItem_RotateRight90_Click(object sender, EventArgs e)
        {
            Rotate(90f);
        }

        private void ToolStripMenuItem_RotateLeft90_Click(object sender, EventArgs e)
        {
            Rotate(-90f);
        }

        private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (itemListView.SelectedItems == null || itemListView.SelectedItems.Count == 0) e.Cancel = true;
        }

        private void UpdateListTimerStart()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void _debounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            UpdateList();
        }
    }
}
