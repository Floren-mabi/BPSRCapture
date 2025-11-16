namespace BPSRCapture
{
    internal class ThumbnailInfo : IDisposable
    {
        private Bitmap _img;
        private string _path;

        public Image Image { get => _img; }
        public string Path { get => _path; }

        public ThumbnailInfo(string path, Bitmap img)
        {
            _path = path;
            _img = img;
        }

        void IDisposable.Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
            }
        }
    }
}
