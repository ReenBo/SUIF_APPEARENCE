using SUIF.Base;

namespace SUIF.Showcase.Windows.RecycledList
{
    public readonly struct RecycledNodeData
    {
        public readonly int Index;
        public readonly string PoolNode;
        public readonly string Address;
        public readonly string Status;
        public readonly string Latency;

        public RecycledNodeData(int index, string poolNode, string address, string status, string latency)
        {
            Index = index;
            PoolNode = poolNode;
            Address = address;
            Status = status;
            Latency = latency;
        }
    }

    public sealed class RecycledListWindowViewModel : BaseViewModel
    {
        public static readonly RecycledNodeData[] Nodes =
        {
            new(1, "NODE_01", "0x00F410", "HOT_SWAP", "0.12ms"),
            new(2, "NODE_02", "0x00F411", "RECYCLED", "0.15ms"),
            new(3, "NODE_03", "0x00F412", "RECYCLED", "0.18ms"),
            new(4, "NODE_04", "0x00F413", "RECYCLED", "0.21ms"),
            new(5, "NODE_05", "0x00F414", "HOT_SWAP", "0.12ms"),
            new(6, "NODE_06", "0x00F415", "RECYCLED", "0.15ms"),
            new(7, "NODE_01", "0x00F416", "RECYCLED", "0.18ms"),
            new(8, "NODE_02", "0x00F417", "RECYCLED", "0.21ms"),
            new(9, "NODE_03", "0x00F418", "HOT_SWAP", "0.12ms"),
            new(10, "NODE_04", "0x00F419", "RECYCLED", "0.15ms"),
            new(11, "NODE_05", "0x00F41A", "RECYCLED", "0.18ms"),
            new(12, "NODE_06", "0x00F41B", "RECYCLED", "0.21ms"),
            new(13, "NODE_01", "0x00F41C", "HOT_SWAP", "0.12ms"),
            new(14, "NODE_02", "0x00F41D", "RECYCLED", "0.15ms"),
            new(15, "NODE_03", "0x00F41E", "RECYCLED", "0.18ms"),
            new(16, "NODE_04", "0x00F41F", "RECYCLED", "0.21ms"),
            new(17, "NODE_05", "0x00F420", "HOT_SWAP", "0.12ms"),
            new(18, "NODE_06", "0x00F421", "RECYCLED", "0.15ms")
        };
    }
}

