using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.Controls;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Lifecycle
{
    [UIView("LifecycleWindowView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class LifecycleWindowView : BaseView<LifecycleWindowViewModel>
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

        protected override void Bind()
        {
            if (_btnStageInit != null) Binder.Bind(_btnStageInit).ToClick(() => ViewModel.TriggerStage("OnInitialize"));
            if (_btnStageOpen != null) Binder.Bind(_btnStageOpen).ToClick(() => ViewModel.TriggerStage("OnOpen"));
            if (_btnStageShow != null) Binder.Bind(_btnStageShow).ToClick(() => ViewModel.TriggerStage("OnShow"));
            if (_btnStageHide != null) Binder.Bind(_btnStageHide).ToClick(() => ViewModel.TriggerStage("OnHide"));
            if (_btnStageClose != null) Binder.Bind(_btnStageClose).ToClick(() => ViewModel.TriggerStage("OnClose"));
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

            AddDisposable(ViewModel.LifecycleStage.Subscribe(OnStageChanged));
            RefreshLogs();
        }

        private void OnStageChanged(string stage)
        {
            UpdateBtn(_btnStageInit, stage == "OnInitialize");
            UpdateBtn(_btnStageOpen, stage == "OnOpen");
            UpdateBtn(_btnStageShow, stage == "OnShow");
            UpdateBtn(_btnStageHide, stage == "OnHide");
            UpdateBtn(_btnStageClose, stage == "OnClose");
            RefreshLogs();
        }

        private void UpdateBtn(Button btn, bool isActive)
        {
            if (btn == null) return;
            if (isActive) btn.AddToClassList(ActiveClass);
            else btn.RemoveFromClassList(ActiveClass);
        }

        private static FixedSlotLabel CreateLogSlot(int index)
        {
            var label = new FixedSlotLabel
            {
                Capacity = LogCharCapacity
            };
            label.AddToClassList(LogItemClass);
            return label;
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

        private void RefreshLogs()
        {
            if (_logPool == null)
            {
                return;
            }

            _logPool.Render(ViewModel.LifecycleLogs.Count, ViewModel.LifecycleLogs, BindLogSlot);
        }

        private static void BindLogSlot(FixedSlotLabel slot, int index, ObservableCollections.ObservableList<string> logs)
        {
            using var builder = ZString.CreateStringBuilder(true);
            builder.Append("> ");
            builder.Append(logs[index]);
            slot.SetFixedText(builder.AsSpan());
        }
    }
}
