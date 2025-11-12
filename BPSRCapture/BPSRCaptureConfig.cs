using ShortcutKeys = BPSRCapture.KeySetting.ShortcutKeys;

namespace BPSRCapture
{
    internal class BPSRCaptureConfig
    {
        private readonly string C_DEFAULT_SAVEFOLDER;
        private readonly string C_DEFAULT_SAVEPATH;
        // SS保存パス
        public string SavePath { get; set; }
        // 撮影キー
        public ShortcutKeys Key { get; set; }
        // 修飾キー
        public bool CtrlKey { get; set; }
        public bool ShiftKey { get; set; }
        public bool AltKey { get; set; }
        // ウィンドウサイズ
        public int Width { get; set; }
        public int Height { get; set; }
        // デバイスID
        public int DeviceID { get; set; }
        // ファイルタイプ
        public BPSRCaptureConfigManager.FileType FileType { get; set; }
        // 効果音音量
        public int Volume { get; set; }
        // ミュート
        public bool Mute {  get; set; }

        public BPSRCaptureConfig()
        {
            // デフォルトの保存ディレクトリ
            C_DEFAULT_SAVEFOLDER = "Pictures" + Path.DirectorySeparatorChar + "StarASIA";
            C_DEFAULT_SAVEPATH = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + Path.DirectorySeparatorChar + C_DEFAULT_SAVEFOLDER;
            SavePath = C_DEFAULT_SAVEPATH;
            // デフォルトの撮影キー
            Key = ShortcutKeys.Insert;
        }
    }
}
