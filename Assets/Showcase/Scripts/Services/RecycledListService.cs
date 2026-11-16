using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Services
{
    public interface IRecycledListService : IShowcasePreloadable
    {
        RecycledNodeDto[] Nodes { get; }
    }

    public sealed class RecycledListService : IRecycledListService
    {
        private const int DefaultNodeCount = 18;
        private const int SlotsPerPage = 6;
        private const uint BaseAddress = 0x00F410u;

        private RecycledNodeDto[] _nodes;
        private bool _preloaded;

        public RecycledNodeDto[] Nodes => _nodes;

        public RecycledListService()
        {
            _nodes = Array.Empty<RecycledNodeDto>();
            EnsureLoaded();
        }

        public UniTask PreloadAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            EnsureLoaded();
            return UniTask.CompletedTask;
        }

        private void EnsureLoaded()
        {
            if (_preloaded)
            {
                return;
            }

            var buffer = new RecycledNodeDto[DefaultNodeCount];
            for (int i = 0; i < DefaultNodeCount; i++)
            {
                int slot = (i % SlotsPerPage) + 1;
                bool isHot = (i % 4) == 0;
                float latency = 0.12f + (i % 4) * 0.03f;
                buffer[i] = new RecycledNodeDto(
                    i + 1,
                    (byte)slot,
                    BaseAddress + (uint)i,
                    isHot ? RecycledNodeStatus.HotSwap : RecycledNodeStatus.Recycled,
                    latency);
            }

            _nodes = buffer;
            _preloaded = true;
        }
    }
}
