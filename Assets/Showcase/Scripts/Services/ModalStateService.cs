using System;
using global::R3;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Services
{
    public interface IModalStateService : IDisposable
    {
        ReadOnlyReactiveProperty<ModalViewDto> State { get; }
        void PushSubPopup();
        void CyclePreset();
    }

    public sealed class ModalStateService : IModalStateService
    {
        private const int PresetCount = 3;

        private readonly ReactiveProperty<ModalViewDto> _state =
            new(new ModalViewDto(1, ModalPresetId.PopupIntercept));

        public ReadOnlyReactiveProperty<ModalViewDto> State => _state;

        public void PushSubPopup()
        {
            var current = _state.Value;
            var nextPreset = (ModalPresetId)(((int)current.Preset + 1) % PresetCount);
            _state.Value = new ModalViewDto(current.Depth + 1, nextPreset);
        }

        public void CyclePreset()
        {
            var current = _state.Value;
            var nextPreset = (ModalPresetId)(((int)current.Preset + 1) % PresetCount);
            _state.Value = new ModalViewDto(current.Depth, nextPreset);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}
