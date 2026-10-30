using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Modals
{
    [UIView("ModalDialogView", SUIF.API.UILayer.Popups, UILoadingMode.Destroy, false, true)]
    public sealed class ModalDialogView : BaseView<ModalDialogViewModel>
    {
        [UQuery("modal-stack-counter")] private Label _stackCounter;
        [UQuery("modal-alert-title")] private Label _alertTitle;
        [UQuery("modal-body-message")] private Label _bodyMessage;
        [UQuery("btn-modal-push")] private Button _btnPush;
        [UQuery("btn-modal-cycle")] private Button _btnCycle;
        [UQuery("btn-modal-dismiss")] private Button _btnDismiss;
        [UQuery("btn-modal-close")] private Button _btnClose;

        public ModalDialogView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnPush != null) Binder.Bind(_btnPush).ToClick(ViewModel.PushSubPopup);
            if (_btnCycle != null) Binder.Bind(_btnCycle).ToClick(ViewModel.CyclePreset);
            if (_btnDismiss != null) Binder.Bind(_btnDismiss).ToClick(ViewModel.Dismiss);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Dismiss);

            AddDisposable(ViewModel.Depth.Subscribe(OnDepthChanged));
            AddDisposable(ViewModel.PresetIndex.Subscribe(OnPresetChanged));
        }

        private void OnDepthChanged(int depth)
        {
            if (_stackCounter != null)
            {
                _stackCounter.text = ZString.Concat("DEPTH: ", depth);
            }
        }

        private void OnPresetChanged(int index)
        {
            var preset = ViewModel.GetCurrentPreset();
            if (_alertTitle != null) _alertTitle.text = preset.Title;
            if (_bodyMessage != null) _bodyMessage.text = preset.Message;
        }
    }
}
