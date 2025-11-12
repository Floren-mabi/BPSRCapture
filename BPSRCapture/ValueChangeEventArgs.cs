namespace BPSRCapture
{
    internal class ValueChangeEventArgs : EventArgs
    {
        public int OldValue { get; }
        public int NewValue { get; }

        public ValueChangeEventArgs(int oldValue, int newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
    internal delegate void ValueChangeEventHandler(object sender, ValueChangeEventArgs e);
}
