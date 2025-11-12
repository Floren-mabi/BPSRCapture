namespace BPSRCapture
{
    internal class GlobalKeyboardHookEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool Handled { get; set; } // true にするとキー入力をブロック

        public GlobalKeyboardHookEventArgs(Keys key, IntPtr wParam)
        {
            Key = key;
        }
    }
}
