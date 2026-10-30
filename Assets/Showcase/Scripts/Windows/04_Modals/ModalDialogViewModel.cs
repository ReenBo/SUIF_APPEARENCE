using System;
using global::R3;
using SUIF.API;
using SUIF.Base;

namespace SUIF.Showcase.Windows.Modals
{
    public readonly struct ModalPresetData
    {
        public readonly string Title;
        public readonly string Message;

        public ModalPresetData(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }

    public sealed class ModalDialogViewModel : BaseViewModel
    {
        private static readonly ModalPresetData[] Presets =
        {
            new("DANGER // POPUP_INTERCEPT", "This modal layer is non-destructively mounted over the active base window. Underlying reactive streams and lifecycle states remain preserved in memory."),
            new("CONFIRM // MEMORY_PURGE_0x04", "Verify zero-allocation pool flush before unmounting active SUIF container. Modal stack isolates user input until resolved."),
            new("SYSTEM_ALERT // OVERLAY_PRIORITY", "High-priority FUI notification injected at Z-Layer 999. Base window continues rendering beneath the scanline backdrop.")
        };

        private readonly IUIFlow _uiFlow;

        public ReactiveProperty<int> Depth { get; } = new(1);
        public ReactiveProperty<int> PresetIndex { get; } = new(0);

        public ModalDialogViewModel(IUIFlow uiFlow)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));

            AddDisposable(Depth);
            AddDisposable(PresetIndex);
        }

        public void PushSubPopup()
        {
            Depth.Value++;
            PresetIndex.Value = (PresetIndex.Value + 1) % Presets.Length;
        }

        public void CyclePreset()
        {
            PresetIndex.Value = (PresetIndex.Value + 1) % Presets.Length;
        }

        public void Dismiss()
        {
            _uiFlow.Close<ModalDialogView>();
        }

        public ModalPresetData GetCurrentPreset()
        {
            return Presets[PresetIndex.Value % Presets.Length];
        }
    }
}

