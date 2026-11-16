using SUIF.Attributes;
using SUIF.Binding;
using SUIF.R3.Base;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Modals
{
    [UIView("ModalDialogView", SUIF.API.UILayer.Popups, UILoadingMode.Cached, false, true)]
    public sealed class ModalDialogView : BaseReactiveDataView<ModalDialogViewModel, ModalViewDto>
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

        protected override void OnBind()
        {
            if (_btnPush != null) Binder.Bind(_btnPush).ToClick(ViewModel.PushSubPopup);
            if (_btnCycle != null) Binder.Bind(_btnCycle).ToClick(ViewModel.CyclePreset);
            if (_btnDismiss != null) Binder.Bind(_btnDismiss).ToClick(ViewModel.Dismiss);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Dismiss);

            _stackCounterSlot = Binder.FixedSlot(_stackCounter, 14);
            _alertTitleSlot = Binder.FixedSlot(_alertTitle, 40);
            _bodyMessageSlot = Binder.FixedSlot(_bodyMessage, 176);
        }

        protected override void RenderData(ModalViewDto dto)
        {
            var formatter = ViewModel.Formatter;
            _stackCounterSlot?.SetWritten(dto.Depth, formatter.ModalDepth);
            _alertTitleSlot?.SetWritten(dto.Preset, formatter.ModalTitle);
            _bodyMessageSlot?.SetWritten(dto.Preset, formatter.ModalBody);
        }
    }
}
