using System;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;

namespace SUIF.Showcase.Windows.Modals
{
    public sealed class ModalDialogViewModel : BaseViewModel
    {
        private const int PresetCount = 3;

        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;

        public ReactiveProperty<ModalViewDto> State { get; } = new(new ModalViewDto(1, ModalPresetId.PopupIntercept));

        public ModalDialogViewModel(IUIFlow uiFlow, ShowcaseFrameViewModel frameViewModel)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));

            AddDisposable(State);
        }

        public void PushSubPopup()
        {
            var current = State.Value;
            var nextPreset = (ModalPresetId)(((int)current.Preset + 1) % PresetCount);
            State.Value = new ModalViewDto(current.Depth + 1, nextPreset);
        }

        public void CyclePreset()
        {
            var current = State.Value;
            var nextPreset = (ModalPresetId)(((int)current.Preset + 1) % PresetCount);
            State.Value = new ModalViewDto(current.Depth, nextPreset);
        }

        public void Dismiss()
        {
            _frameViewModel.SetModalState(false);
            _uiFlow.Close<ModalDialogView>();
        }
    }
}
