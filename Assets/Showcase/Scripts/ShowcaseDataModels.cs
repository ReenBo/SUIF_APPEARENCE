namespace SUIF.Showcase
{
    public enum ShowcaseViewType
    {
        None = 0,
        Lifecycle = 1,
        Mvvm = 2,
        RecycledList = 3,
        Inventory = 5,
        Theme = 6
    }

    public readonly struct InventoryItem
    {
        public readonly string Code;
        public readonly string Name;
        public readonly string Type;
        public readonly string Weight;
        public readonly string Rarity;
        public readonly string Power;
        public readonly string Description;

        public InventoryItem(string code, string name, string type, string weight, string rarity, string power, string description)
        {
            Code = code;
            Name = name;
            Type = type;
            Weight = weight;
            Rarity = rarity;
            Power = power;
            Description = description;
        }
    }

    public readonly struct ModalPreset
    {
        public readonly string Title;
        public readonly string Message;

        public ModalPreset(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }

    public readonly struct RecycledNodeItem
    {
        public readonly int Index;
        public readonly string PoolNode;
        public readonly string Address;
        public readonly string Status;
        public readonly string Latency;

        public RecycledNodeItem(int index, string poolNode, string address, string status, string latency)
        {
            Index = index;
            PoolNode = poolNode;
            Address = address;
            Status = status;
            Latency = latency;
        }
    }
}
