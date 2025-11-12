namespace BPSRCapture
{
    public class KeySetting
    {
        public ShortcutKeys Key { get; set; }
        public string Name { get; set; }
        public KeySetting(ShortcutKeys key, string name)
        {
            Key = key;
            Name = name;
        }

        public enum ShortcutKeys
        {
            F1 = Keys.F1, 
            F2 = Keys.F2, 
            F3 = Keys.F3, 
            F4 = Keys.F4, 
            F5 = Keys.F5, 
            F6 = Keys.F6, 
            F7 = Keys.F7, 
            F8 = Keys.F8, 
            F9 = Keys.F9, 
            F10 = Keys.F10, 
            F11 = Keys.F11,
            F12 = Keys.F12,
            PrintScreen = Keys.PrintScreen,
            Insert = Keys.Insert
        }
    }
}
