using SUIF.Attributes;
using SUIF.Binding;
using SUIF.Controls;
using SUIF.R3.Base;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Lifecycle
{
    [UIView("LifecycleWindowView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class LifecycleWindowView : BaseReactiveDataView<LifecycleWindowViewModel, LifecycleViewDto>
    {
        private const string ActiveClass = "is-active";
        private const string LogItemClass = "fui-log-item";
        private const int MaxLogSlots = 10;
        private const int LogCharCapacity = 72;

        [UQuery("btn-stage-init")] private Button _btnStageInit;
        [UQuery("btn-stage-open")] private Button _btnStageOpen;
        [UQuery("btn-stage-show")] private Button _btnStageShow;
        [UQuery("btn-stage-hide")] private Button _btnStageHide;
        [UQuery("btn-stage-close")] private Button _btnStageClose;
        [UQuery("btn-lifecycle-modal-test")] private Button _btnModalTest;
        [UQuery("btn-window-close")] private Button _btnClose;
        [UQuery("lifecycle-logs-container")] private VisualElement _logsContainer;

        private FixedElementPool<FixedSlotLabel> _logPool;

        public LifecycleWindowView(VisualElement root) : base(root)
        {
        }

        protected override void OnBind()
        {
            if (_btnStageInit != null) Binder.Bind(_btnStageInit).ToClick(() => ViewModel.TriggerStage(LifecycleStageId.Initialize));
            if (_btnStageOpen != null) Binder.Bind(_btnStageOpen).ToClick(() => ViewModel.TriggerStage(LifecycleStageId.Open));
            if (_btnStageShow != null) Binder.Bind(_btnStageShow).ToClick(() => ViewModel.TriggerStage(LifecycleStageId.Show));
            if (_btnStageHide != null) Binder.Bind(_btnStageHide).ToClick(() => ViewModel.TriggerStage(LifecycleStageId.Hide));
            if (_btnStageClose != null) Binder.Bind(_btnStageClose).ToClick(() => ViewModel.TriggerStage(LifecycleStageId.Close));
            if (_btnModalTest != null) Binder.Bind(_btnModalTest).ToClick(ViewModel.OpenModal);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Close);

            if (_logsContainer != null)
            {
                _logsContainer.Clear();
                _logPool = new FixedElementPool<FixedSlotLabel>(
                    _logsContainer,
                    MaxLogSlots,
                    CreateLogSlot,
                    SetLogSlotActive);
                AddDisposable(_logPool);
            }
        }

        protected override void RenderData(LifecycleViewDto dto)
        {
            UpdateBtn(_btnStageInit, dto.CurrentStage == LifecycleStageId.Initialize);
            UpdateBtn(_btnStageOpen, dto.CurrentStage == LifecycleStageId.Open);
            UpdateBtn(_btnStageShow, dto.CurrentStage == LifecycleStageId.Show);
            UpdateBtn(_btnStageHide, dto.CurrentStage == LifecycleStageId.Hide);
            UpdateBtn(_btnStageClose, dto.CurrentStage == LifecycleStageId.Close);

            _logPool?.Render(dto.LogCount, ViewModel, static (slot, index, vm) =>
                slot.SetFixedWritten(vm.LifecycleLogs[index], vm.Formatter.LifecycleLogEntry));
        }

        private FixedSlotLabel CreateLogSlot(int index)
        {
            var label = new FixedSlotLabel
            {
                Capacity = LogCharCapacity
            };
            label.AddToClassList(LogItemClass);
            Binder.AddDisposable(label);
            return label;
        }

        private static void UpdateBtn(Button btn, bool isActive)
        {
            if (btn == null) return;
            if (isActive) btn.AddToClassList(ActiveClass);
            else btn.RemoveFromClassList(ActiveClass);
        }

        private static void SetLogSlotActive(FixedSlotLabel slot, bool isActive)
        {
            if (isActive)
            {
                slot.style.height = StyleKeyword.Null;
                slot.style.marginBottom = StyleKeyword.Null;
                return;
            }

            slot.SetFixedText(string.Empty);
            slot.style.height = 0f;
            slot.style.marginBottom = 0f;
        }
    }
}
