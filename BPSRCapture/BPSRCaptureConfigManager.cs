using System.Text.Json;

namespace BPSRCapture
{
    internal class BPSRCaptureConfigManager
    {
        private const string C_CONF_DIR = "BPSRCapture";
        private const string C_CONF_FILENAME = "settings.config";
        private string confDir;
        private string confFile;

        public BPSRCaptureConfig conf { get; private set; }

        internal enum FileType
        {
            PNG = 0,
            JPEG = 1,
            WEBP_LOSSLESS = 2,
            WEBP_LOSSY = 3,
            BITMAP = 4
        }

        internal BPSRCaptureConfigManager()
        {
            // 設定保存用ディレクトリ
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            confDir = appDataDir + Path.DirectorySeparatorChar + C_CONF_DIR;
            confFile = confDir + Path.DirectorySeparatorChar + C_CONF_FILENAME;
            conf = new BPSRCaptureConfig();
        }

        /**
         * 設定保存用ディレクトリがない場合は作成する
         */
        private void MakeConfDirectory()
        {
            if (!Directory.Exists(confDir))
                Directory.CreateDirectory(confDir);
        }

        /**
         * 設定保存
         */
        public void Save()
        {
            if (!File.Exists(confFile))
            {
                conf = new BPSRCaptureConfig();
            }
            MakeConfDirectory();
            conf!.SavePath = conf.SavePath.TrimEnd(Path.DirectorySeparatorChar);
            string data = JsonSerializer.Serialize(conf);
            File.WriteAllText(confFile, data);
        }

        /**
         * 設定読込
         */
        public void Load()
        {
            if (!File.Exists(confFile)) {
                Save();
            }
            string data = File.ReadAllText(confFile);
            conf = JsonSerializer.Deserialize<BPSRCaptureConfig>(data)!;
        }
    }
}
