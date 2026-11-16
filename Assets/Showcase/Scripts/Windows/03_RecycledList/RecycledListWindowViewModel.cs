using System;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Windows.RecycledList
{
    public sealed class RecycledListWindowViewModel : BaseViewModel
    {
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly IRecycledListService _listService;

        public RecycledNodeDto[] Nodes => _listService.Nodes;

        public RecycledListWindowViewModel(ShowcaseFrameViewModel frameViewModel, IRecycledListService listService)
        {
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
            _listService = listService ?? throw new ArgumentNullException(nameof(listService));
        }

        public void Close()
        {
            _frameViewModel.CloseBaseWindow();
        }
    }
}
