using System;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;

namespace SUIF.Showcase.Windows.RecycledList
{
    public sealed class RecycledListWindowViewModel : BaseViewModel
    {
        private readonly ShowcaseFrameViewModel _frameViewModel;

        public static readonly RecycledNodeDto[] Nodes =
        {
            new(1, 1, 0x00F410u, RecycledNodeStatus.HotSwap, 0.12f),
            new(2, 2, 0x00F411u, RecycledNodeStatus.Recycled, 0.15f),
            new(3, 3, 0x00F412u, RecycledNodeStatus.Recycled, 0.18f),
            new(4, 4, 0x00F413u, RecycledNodeStatus.Recycled, 0.21f),
            new(5, 5, 0x00F414u, RecycledNodeStatus.HotSwap, 0.12f),
            new(6, 6, 0x00F415u, RecycledNodeStatus.Recycled, 0.15f),
            new(7, 1, 0x00F416u, RecycledNodeStatus.Recycled, 0.18f),
            new(8, 2, 0x00F417u, RecycledNodeStatus.Recycled, 0.21f),
            new(9, 3, 0x00F418u, RecycledNodeStatus.HotSwap, 0.12f),
            new(10, 4, 0x00F419u, RecycledNodeStatus.Recycled, 0.15f),
            new(11, 5, 0x00F41Au, RecycledNodeStatus.Recycled, 0.18f),
            new(12, 6, 0x00F41Bu, RecycledNodeStatus.Recycled, 0.21f),
            new(13, 1, 0x00F41Cu, RecycledNodeStatus.HotSwap, 0.12f),
            new(14, 2, 0x00F41Du, RecycledNodeStatus.Recycled, 0.15f),
            new(15, 3, 0x00F41Eu, RecycledNodeStatus.Recycled, 0.18f),
            new(16, 4, 0x00F41Fu, RecycledNodeStatus.Recycled, 0.21f),
            new(17, 5, 0x00F420u, RecycledNodeStatus.HotSwap, 0.12f),
            new(18, 6, 0x00F421u, RecycledNodeStatus.Recycled, 0.15f)
        };

        public RecycledListWindowViewModel(ShowcaseFrameViewModel frameViewModel)
        {
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
        }

        public void Close()
        {
            _frameViewModel.CloseBaseWindow();
        }
    }
}
