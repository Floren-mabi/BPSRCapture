namespace BPSRCapture
{
    internal class AdapterItem
    {
        public int ID;
        public string Name { get; set; }

        public AdapterItem(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
