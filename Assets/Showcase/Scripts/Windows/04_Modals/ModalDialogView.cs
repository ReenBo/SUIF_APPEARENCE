using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Modals
{
    [UIView("ModalDialogView", SUIF.API.UILayer.Popups, UILoadingMode.Cached, false, true)]
    public sealed class ModalDialogView : BaseView<ModalDialogViewModel>
    {
        [UQuery("modal-stack-counter")] private Label _stackCounter;
        [UQuery("modal-alert-title")] private Label _alertTitle;
        [UQuery("modal-body-message")] private Label _bodyMessage;
        [UQuery("btn-modal-push")] private Button _btnPush;
        [UQuery("btn-modal-cycle")] private Button _btnCycle;
        [UQuery("btn-modal-dismiss")] private Button _btnDismiss;
        [UQuery("btn-modal-close")] private Button _btnClose;

        private FixedTextSlot _stackCounterSlot;
        private FixedTextSlot _alertTitleSlot;
        private FixedTextSlot _bodyMessageSlot;

        public ModalDialogView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnPush != null) Binder.Bind(_btnPush).ToClick(ViewModel.PushSubPopup);
            if (_btnCycle != null) Binder.Bind(_btnCycle).ToClick(ViewModel.CyclePreset);
            if (_btnDismiss != null) Binder.Bind(_btnDismiss).ToClick(ViewModel.Dismiss);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Dismiss);

            _stackCounterSlot = RegisterFixedSlot(_stackCounter, 14);
            _alertTitleSlot = RegisterFixedSlot(_alertTitle, 40);
            _bodyMessageSlot = RegisterFixedSlot(_bodyMessage, 176);

            AddDisposable(ViewModel.Depth.Subscribe(OnDepthChanged));
            AddDisposable(ViewModel.PresetIndex.Subscribe(OnPresetChanged));
        }

        private FixedTextSlot RegisterFixedSlot(TextElement element, int capacity)
        {
            if (element == null)
            {
                return null;
            }

            var slot = new FixedTextSlot(element, capacity);
            AddDisposable(slot);
            return slot;
        }

        private void OnDepthChanged(int depth)
        {
            _stackCounterSlot?.SetFormat("DEPTH: {0}", depth);
        }

        private void OnPresetChanged(int index)
        {
            var preset = ViewModel.GetCurrentPreset();
            _alertTitleSlot?.SetText(preset.Title);
            _bodyMessageSlot?.SetText(preset.Message);
        }
    }
}
