using System;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Windows.Modals
{
    public sealed class ModalDialogViewModel : BaseViewModel, IReactiveDataViewModel<ModalViewDto>
    {
        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly IModalStateService _modalService;

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<ModalViewDto> State => _modalService.State;
        public Observable<ModalViewDto> Data => _modalService.State;

        public ModalDialogViewModel(
            IUIFlow uiFlow,
            ShowcaseFrameViewModel frameViewModel,
            IModalStateService modalService,
            IShowcaseTextFormatter formatter)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
            _modalService = modalService ?? throw new ArgumentNullException(nameof(modalService));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public void PushSubPopup()
        {
            _modalService.PushSubPopup();
        }

        public void CyclePreset()
        {
            _modalService.CyclePreset();
        }

        public void Dismiss()
        {
            _frameViewModel.SetModalState(false);
            _uiFlow.Close<ModalDialogView>();
        }
    }
}
