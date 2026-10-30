using System;
using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.Showcase
{
    [UIView("ShowcaseHubView", SUIF.API.UILayer.Screens)]
    public sealed class ShowcaseHubView : BaseView<ShowcaseHubViewModel>
    {
        private const string ActiveClass = "is-active";
        private const string ActiveModalClass = "is-active-modal";
        private const string HiddenClass = "is-hidden";
        private const string SelectedClass = "is-selected";

        [UQuery("telemetry-base-readout")] private Label _telemetryBaseReadout;
        [UQuery("telemetry-modal-readout")] private Label _telemetryModalReadout;
        [UQuery("header-open-state")] private Label _headerOpenState;
        [UQuery("header-base-title")] private Label _headerBaseTitle;
        [UQuery("header-overlay-badge")] private VisualElement _headerOverlayBadge;
        [UQuery("header-layer-tag")] private Label _headerLayerTag;

        [UQuery("nav-btn-lifecycle")] private Button _navBtnLifecycle;
        [UQuery("nav-btn-mvvm")] private Button _navBtnMvvm;
        [UQuery("nav-btn-list")] private Button _navBtnList;
        [UQuery("nav-btn-modals")] private Button _navBtnModals;
        [UQuery("nav-btn-inventory")] private Button _navBtnInventory;
        [UQuery("nav-btn-theme")] private Button _navBtnTheme;

        [UQuery("btn-main-close")] private Button _btnMainClose;
        [UQuery("close-btn-hint")] private Label _closeBtnHint;
        [UQuery("viewport-id-badge")] private Label _viewportIdBadge;
        [UQuery("viewport-sub-badge")] private Label _viewportSubBadge;
        [UQuery("viewport-coords")] private Label _viewportCoords;

        [UQuery("stage-lifecycle")] private VisualElement _stageLifecycle;
        [UQuery("stage-mvvm")] private VisualElement _stageMvvm;
        [UQuery("stage-list")] private VisualElement _stageList;
        [UQuery("stage-inventory")] private VisualElement _stageInventory;
        [UQuery("stage-theme")] private VisualElement _stageTheme;
        [UQuery("stage-idle")] private VisualElement _stageIdle;

        [UQuery("modal-overlay-layer")] private VisualElement _modalOverlayLayer;
        [UQuery("modal-underlying-label")] private Label _modalUnderlyingLabel;
        [UQuery("modal-stack-counter")] private Label _modalStackCounter;
        [UQuery("modal-alert-title")] private Label _modalAlertTitle;
        [UQuery("modal-body-message")] private Label _modalBodyMessage;
        [UQuery("btn-modal-push")] private Button _btnModalPush;
        [UQuery("btn-modal-cycle")] private Button _btnModalCycle;
        [UQuery("btn-modal-dismiss")] private Button _btnModalDismiss;
        [UQuery("btn-modal-close")] private Button _btnModalClose;

        [UQuery("btn-stage-init")] private Button _btnStageInit;
        [UQuery("btn-stage-open")] private Button _btnStageOpen;
        [UQuery("btn-stage-show")] private Button _btnStageShow;
        [UQuery("btn-stage-hide")] private Button _btnStageHide;
        [UQuery("btn-stage-close")] private Button _btnStageClose;
        [UQuery("btn-lifecycle-modal-test")] private Button _btnLifecycleModalTest;
        [UQuery("lifecycle-logs-container")] private VisualElement _lifecycleLogsContainer;

        [UQuery("label-counter-value")] private Label _labelCounterValue;
        [UQuery("btn-counter-dec")] private Button _btnCounterDec;
        [UQuery("btn-counter-inc")] private Button _btnCounterInc;
        [UQuery("btn-counter-boost")] private Button _btnCounterBoost;
        [UQuery("btn-counter-reset")] private Button _btnCounterReset;
        [UQuery("counter-marbles-container")] private VisualElement _counterMarblesContainer;

        [UQuery("list-nodes-container")] private ScrollView _listNodesContainer;

        [UQuery("inventory-slots-container")] private VisualElement _inventorySlotsContainer;
        [UQuery("inventory-inspector-name")] private Label _inventoryInspectorName;
        [UQuery("inventory-inspector-rarity")] private Label _inventoryInspectorRarity;
        [UQuery("inventory-inspector-power")] private Label _inventoryInspectorPower;
        [UQuery("inventory-inspector-desc")] private Label _inventoryInspectorDesc;
        [UQuery("btn-inventory-modal-inspect")] private Button _btnInventoryModalInspect;

        [UQuery("btn-theme-mono")] private Button _btnThemeMono;
        [UQuery("btn-theme-cyan")] private Button _btnThemeCyan;
        [UQuery("btn-theme-crimson")] private Button _btnThemeCrimson;
        [UQuery("btn-theme-amber")] private Button _btnThemeAmber;

        [UQuery("footer-status-log")] private Label _footerStatusLog;

        public ShowcaseHubView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            BindNavButtons();
            BindModalControls();
            BindLifecycleControls();
            BindCounterControls();
            BindListControls();
            BindInventoryControls();
            BindThemeControls();

            if (_btnMainClose != null)
            {
                Binder.Bind(_btnMainClose).ToClick(ViewModel.HandleMainCloseButton);
            }

            AddDisposable(ViewModel.ActiveBaseView.Subscribe(OnActiveBaseViewChanged));
            AddDisposable(ViewModel.IsModalOpen.Subscribe(OnModalOpenChanged));
            AddDisposable(ViewModel.ModalDepth.Subscribe(OnModalDepthChanged));
            AddDisposable(ViewModel.ModalPresetIndex.Subscribe(OnModalPresetChanged));
            AddDisposable(ViewModel.CounterValue.Subscribe(OnCounterValueChanged));
            AddDisposable(ViewModel.LifecycleStage.Subscribe(OnLifecycleStageChanged));
            AddDisposable(ViewModel.SelectedInventoryIndex.Subscribe(OnInventorySelectionChanged));
            AddDisposable(ViewModel.CurrentTheme.Subscribe(OnThemeChanged));

            if (_footerStatusLog != null)
            {
                Binder.Bind(_footerStatusLog).ToText(ViewModel.FooterStatusLog);
            }

            RefreshLifecycleLogs();
            RefreshCounterMarbles();
            PopulateRecycledList();
            PopulateInventorySlots();
        }

        private void BindNavButtons()
        {
            if (_navBtnLifecycle != null) Binder.Bind(_navBtnLifecycle).ToClick(() => ViewModel.SelectBaseView(ShowcaseViewType.Lifecycle));
            if (_navBtnMvvm != null) Binder.Bind(_navBtnMvvm).ToClick(() => ViewModel.SelectBaseView(ShowcaseViewType.Mvvm));
            if (_navBtnList != null) Binder.Bind(_navBtnList).ToClick(() => ViewModel.SelectBaseView(ShowcaseViewType.RecycledList));
            if (_navBtnModals != null) Binder.Bind(_navBtnModals).ToClick(ViewModel.ToggleModalOverlay);
            if (_navBtnInventory != null) Binder.Bind(_navBtnInventory).ToClick(() => ViewModel.SelectBaseView(ShowcaseViewType.Inventory));
            if (_navBtnTheme != null) Binder.Bind(_navBtnTheme).ToClick(() => ViewModel.SelectBaseView(ShowcaseViewType.Theme));
        }

        private void BindModalControls()
        {
            if (_btnModalPush != null) Binder.Bind(_btnModalPush).ToClick(ViewModel.SpawnSubPopup);
            if (_btnModalCycle != null) Binder.Bind(_btnModalCycle).ToClick(ViewModel.CycleModalPreset);
            if (_btnModalDismiss != null) Binder.Bind(_btnModalDismiss).ToClick(ViewModel.CloseModalOverlay);
            if (_btnModalClose != null) Binder.Bind(_btnModalClose).ToClick(ViewModel.CloseModalOverlay);
        }

        private void BindLifecycleControls()
        {
            if (_btnStageInit != null) Binder.Bind(_btnStageInit).ToClick(() => ViewModel.TriggerLifecycleStage("OnInitialize"));
            if (_btnStageOpen != null) Binder.Bind(_btnStageOpen).ToClick(() => ViewModel.TriggerLifecycleStage("OnOpen"));
            if (_btnStageShow != null) Binder.Bind(_btnStageShow).ToClick(() => ViewModel.TriggerLifecycleStage("OnShow"));
            if (_btnStageHide != null) Binder.Bind(_btnStageHide).ToClick(() => ViewModel.TriggerLifecycleStage("OnHide"));
            if (_btnStageClose != null) Binder.Bind(_btnStageClose).ToClick(() => ViewModel.TriggerLifecycleStage("OnClose"));
            if (_btnLifecycleModalTest != null) Binder.Bind(_btnLifecycleModalTest).ToClick(ViewModel.ToggleModalOverlay);
        }

        private void BindCounterControls()
        {
            if (_btnCounterDec != null) Binder.Bind(_btnCounterDec).ToClick(() => ViewModel.UpdateCounter(-1));
            if (_btnCounterInc != null) Binder.Bind(_btnCounterInc).ToClick(() => ViewModel.UpdateCounter(1));
            if (_btnCounterBoost != null) Binder.Bind(_btnCounterBoost).ToClick(() => ViewModel.UpdateCounter(10));
            if (_btnCounterReset != null) Binder.Bind(_btnCounterReset).ToClick(ViewModel.ResetCounter);
        }

        private void BindListControls()
        {
        }

        private void BindInventoryControls()
        {
            if (_btnInventoryModalInspect != null)
            {
                Binder.Bind(_btnInventoryModalInspect).ToClick(ViewModel.ToggleModalOverlay);
            }
        }

        private void BindThemeControls()
        {
            if (_btnThemeMono != null) Binder.Bind(_btnThemeMono).ToClick(() => ViewModel.ApplyTheme("mono"));
            if (_btnThemeCyan != null) Binder.Bind(_btnThemeCyan).ToClick(() => ViewModel.ApplyTheme("cyan"));
            if (_btnThemeCrimson != null) Binder.Bind(_btnThemeCrimson).ToClick(() => ViewModel.ApplyTheme("crimson"));
            if (_btnThemeAmber != null) Binder.Bind(_btnThemeAmber).ToClick(() => ViewModel.ApplyTheme("amber"));
        }

        private void OnActiveBaseViewChanged(ShowcaseViewType type)
        {
            UpdateNavButtonState(_navBtnLifecycle, type == ShowcaseViewType.Lifecycle);
            UpdateNavButtonState(_navBtnMvvm, type == ShowcaseViewType.Mvvm);
            UpdateNavButtonState(_navBtnList, type == ShowcaseViewType.RecycledList);
            UpdateNavButtonState(_navBtnInventory, type == ShowcaseViewType.Inventory);
            UpdateNavButtonState(_navBtnTheme, type == ShowcaseViewType.Theme);

            SetStageVisibility(_stageLifecycle, type == ShowcaseViewType.Lifecycle);
            SetStageVisibility(_stageMvvm, type == ShowcaseViewType.Mvvm);
            SetStageVisibility(_stageList, type == ShowcaseViewType.RecycledList);
            SetStageVisibility(_stageInventory, type == ShowcaseViewType.Inventory);
            SetStageVisibility(_stageTheme, type == ShowcaseViewType.Theme);
            SetStageVisibility(_stageIdle, type == ShowcaseViewType.None);

            UpdateHeaderBreadcrumbs(type, ViewModel.IsModalOpen.Value);
        }

        private void OnModalOpenChanged(bool isOpen)
        {
            if (_modalOverlayLayer != null)
            {
                if (isOpen)
                {
                    _modalOverlayLayer.RemoveFromClassList(HiddenClass);
                    _modalOverlayLayer.style.display = DisplayStyle.Flex;
                }
                else
                {
                    _modalOverlayLayer.AddToClassList(HiddenClass);
                    _modalOverlayLayer.style.display = DisplayStyle.None;
                }
            }

            if (_navBtnModals != null)
            {
                if (isOpen)
                {
                    _navBtnModals.AddToClassList(ActiveModalClass);
                }
                else
                {
                    _navBtnModals.RemoveFromClassList(ActiveModalClass);
                }
            }

            if (_headerOverlayBadge != null)
            {
                if (isOpen)
                {
                    _headerOverlayBadge.RemoveFromClassList(HiddenClass);
                    _headerOverlayBadge.style.display = DisplayStyle.Flex;
                }
                else
                {
                    _headerOverlayBadge.AddToClassList(HiddenClass);
                    _headerOverlayBadge.style.display = DisplayStyle.None;
                }
            }

            if (_closeBtnHint != null)
            {
                _closeBtnHint.text = isOpen ? "CLS // OVR" : "CLS // WIN";
            }

            if (_telemetryModalReadout != null)
            {
                _telemetryModalReadout.text = isOpen
                    ? ZString.Concat("MOUNTED [", ViewModel.ModalDepth.Value, "]")
                    : "STANDBY [0]";
            }

            UpdateHeaderBreadcrumbs(ViewModel.ActiveBaseView.Value, isOpen);
        }

        private void OnModalDepthChanged(int depth)
        {
            if (_modalStackCounter != null)
            {
                _modalStackCounter.text = ZString.Concat("DEPTH: ", depth);
            }
            if (_telemetryModalReadout != null && ViewModel.IsModalOpen.Value)
            {
                _telemetryModalReadout.text = ZString.Concat("MOUNTED [", depth, "]");
            }
        }

        private void OnModalPresetChanged(int index)
        {
            var preset = ViewModel.GetCurrentModalPreset();
            if (_modalAlertTitle != null) _modalAlertTitle.text = preset.Title;
            if (_modalBodyMessage != null) _modalBodyMessage.text = preset.Message;
        }

        private void OnCounterValueChanged(int value)
        {
            if (_labelCounterValue != null)
            {
                _labelCounterValue.text = value.ToString("D3");
            }
            RefreshCounterMarbles();
        }

        private void OnLifecycleStageChanged(string stage)
        {
            UpdateLifecycleButton(_btnStageInit, stage == "OnInitialize");
            UpdateLifecycleButton(_btnStageOpen, stage == "OnOpen");
            UpdateLifecycleButton(_btnStageShow, stage == "OnShow");
            UpdateLifecycleButton(_btnStageHide, stage == "OnHide");
            UpdateLifecycleButton(_btnStageClose, stage == "OnClose");
            RefreshLifecycleLogs();
        }

        private void OnInventorySelectionChanged(int index)
        {
            var item = ViewModel.GetCurrentInventoryItem();
            if (_inventoryInspectorName != null) _inventoryInspectorName.text = item.Name;
            if (_inventoryInspectorRarity != null) _inventoryInspectorRarity.text = ZString.Concat("[", item.Rarity, "]");
            if (_inventoryInspectorPower != null) _inventoryInspectorPower.text = item.Power;
            if (_inventoryInspectorDesc != null) _inventoryInspectorDesc.text = item.Description;

            if (_inventorySlotsContainer != null)
            {
                for (var i = 0; i < _inventorySlotsContainer.childCount; i++)
                {
                    var child = _inventorySlotsContainer[i];
                    if (i == index)
                    {
                        child.AddToClassList(SelectedClass);
                    }
                    else
                    {
                        child.RemoveFromClassList(SelectedClass);
                    }
                }
            }
        }

        private void OnThemeChanged(string themeId)
        {
            VisualElement.RemoveFromClassList("theme-mono");
            VisualElement.RemoveFromClassList("theme-cyan");
            VisualElement.RemoveFromClassList("theme-crimson");
            VisualElement.RemoveFromClassList("theme-amber");

            VisualElement.AddToClassList(ZString.Concat("theme-", themeId));
        }

        private void UpdateHeaderBreadcrumbs(ShowcaseViewType type, bool isModalOpen)
        {
            if (_headerOpenState != null)
            {
                _headerOpenState.text = (type == ShowcaseViewType.None && !isModalOpen) ? "closed" : "open";
            }

            var typeName = type switch
            {
                ShowcaseViewType.Lifecycle => "Basic Window Lifecycle",
                ShowcaseViewType.Mvvm => "MVVM & R3 Reactive Counter",
                ShowcaseViewType.RecycledList => "Virtualized Recycled List",
                ShowcaseViewType.Inventory => "INVENTORY",
                ShowcaseViewType.Theme => "Dynamic Theme Switcher",
                _ => "[ NO_ACTIVE_VIEW // IDLE ]"
            };

            if (_headerBaseTitle != null)
            {
                _headerBaseTitle.text = typeName;
            }

            if (_modalUnderlyingLabel != null)
            {
                _modalUnderlyingLabel.text = typeName;
            }

            if (_telemetryBaseReadout != null)
            {
                _telemetryBaseReadout.text = type switch
                {
                    ShowcaseViewType.Lifecycle => "01_LIFECYCLE",
                    ShowcaseViewType.Mvvm => "02_R3_MVVM",
                    ShowcaseViewType.RecycledList => "03_RECYCLED_LIST",
                    ShowcaseViewType.Inventory => "05_INVENTORY",
                    ShowcaseViewType.Theme => "06_THEME_MATRIX",
                    _ => "NONE // IDLE"
                };
            }

            if (_viewportIdBadge != null)
            {
                _viewportIdBadge.text = type switch
                {
                    ShowcaseViewType.Lifecycle => "VIEWPORT_01",
                    ShowcaseViewType.Mvvm => "VIEWPORT_02",
                    ShowcaseViewType.RecycledList => "VIEWPORT_03",
                    ShowcaseViewType.Inventory => "VIEWPORT_05",
                    ShowcaseViewType.Theme => "VIEWPORT_06",
                    _ => "VIEWPORT_00"
                };
            }

            if (_viewportSubBadge != null)
            {
                _viewportSubBadge.text = type switch
                {
                    ShowcaseViewType.Lifecycle => "// LIFECYCLE_STATE_MACHINE",
                    ShowcaseViewType.Mvvm => "// REACTIVE_PROPERTY_STREAM",
                    ShowcaseViewType.RecycledList => "// ZERO_ALLOC_SCROLL_POOL",
                    ShowcaseViewType.Inventory => "// TACTICAL_GRID_STORAGE",
                    ShowcaseViewType.Theme => "// GLOBAL_SHADER_PALETTE",
                    _ => "// STANDBY_BUFFER"
                };
            }

            if (_viewportCoords != null)
            {
                _viewportCoords.text = isModalOpen
                    ? ZString.Concat("BASE: ", (int)type, " // OVERLAY: MODALS_04")
                    : ZString.Concat("BASE_STATE: ", (type == ShowcaseViewType.None ? "UNMOUNTED" : "ACTIVE"));
            }

            if (_headerLayerTag != null)
            {
                _headerLayerTag.text = isModalOpen
                    ? ZString.Concat("LAYER_Z // ", (int)type, "_BASE + MODAL_999")
                    : ZString.Concat("LAYER_Z // ", (int)type, "_BASE_100");
            }
        }

        private void UpdateNavButtonState(Button button, bool isActive)
        {
            if (button == null) return;
            if (isActive) button.AddToClassList(ActiveClass);
            else button.RemoveFromClassList(ActiveClass);
        }

        private void SetStageVisibility(VisualElement stage, bool isVisible)
        {
            if (stage == null) return;
            stage.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void UpdateLifecycleButton(Button button, bool isActive)
        {
            if (button == null) return;
            if (isActive) button.AddToClassList(ActiveClass);
            else button.RemoveFromClassList(ActiveClass);
        }

        private void RefreshLifecycleLogs()
        {
            if (_lifecycleLogsContainer == null) return;
            _lifecycleLogsContainer.Clear();
            for (var i = 0; i < ViewModel.LifecycleLogs.Count; i++)
            {
                var label = new Label(ZString.Concat("> ", ViewModel.LifecycleLogs[i]));
                label.AddToClassList("fui-log-item");
                _lifecycleLogsContainer.Add(label);
            }
        }

        private void RefreshCounterMarbles()
        {
            if (_counterMarblesContainer == null) return;
            _counterMarblesContainer.Clear();
            for (var i = 0; i < ViewModel.CounterHistory.Count; i++)
            {
                var val = ViewModel.CounterHistory[i];
                var pill = new Label(val.ToString());
                pill.AddToClassList("fui-marble-pill");
                if (i == ViewModel.CounterHistory.Count - 1)
                {
                    pill.AddToClassList(ActiveClass);
                }
                _counterMarblesContainer.Add(pill);

                if (i < ViewModel.CounterHistory.Count - 1)
                {
                    var arrow = new Label("->");
                    arrow.AddToClassList("fui-marble-arrow");
                    _counterMarblesContainer.Add(arrow);
                }
            }
        }

        private void PopulateRecycledList()
        {
            if (_listNodesContainer == null) return;
            _listNodesContainer.Clear();
            var nodes = ShowcaseHubViewModel.RecycledNodes;
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                var row = new VisualElement();
                row.AddToClassList("fui-recycled-row");

                var left = new VisualElement();
                left.AddToClassList("fui-recycled-row__left");

                var indexLabel = new Label(ZString.Concat("#", node.Index.ToString("D4")));
                indexLabel.AddToClassList("fui-recycled-row__index");
                var nameLabel = new Label(node.PoolNode);
                nameLabel.AddToClassList("fui-recycled-row__name");
                var addrLabel = new Label(ZString.Concat("MEM: ", node.Address));
                addrLabel.AddToClassList("fui-recycled-row__addr");

                left.Add(indexLabel);
                left.Add(nameLabel);
                left.Add(addrLabel);

                var right = new VisualElement();
                right.AddToClassList("fui-recycled-row__right");

                var statusLabel = new Label(node.Status);
                statusLabel.AddToClassList("fui-recycled-row__status");
                if (node.Status == "HOT_SWAP")
                {
                    statusLabel.AddToClassList("is-hot");
                }
                var latLabel = new Label(node.Latency);
                latLabel.AddToClassList("fui-recycled-row__latency");

                right.Add(statusLabel);
                right.Add(latLabel);

                row.Add(left);
                row.Add(right);
                _listNodesContainer.Add(row);
            }
        }

        private void PopulateInventorySlots()
        {
            if (_inventorySlotsContainer == null) return;
            _inventorySlotsContainer.Clear();
            var items = ShowcaseHubViewModel.InventoryItems;
            for (var i = 0; i < items.Length; i++)
            {
                var slotIndex = i;
                var item = items[i];

                var slot = new Button(() => ViewModel.SelectInventorySlot(slotIndex));
                slot.AddToClassList("fui-inventory-slot");
                if (i == ViewModel.SelectedInventoryIndex.Value)
                {
                    slot.AddToClassList(SelectedClass);
                }

                var header = new VisualElement();
                header.AddToClassList("fui-inventory-slot__header");
                var codeLabel = new Label(item.Code);
                codeLabel.AddToClassList("fui-inventory-slot__code");
                var weightLabel = new Label(item.Weight);
                weightLabel.AddToClassList("fui-inventory-slot__weight");
                header.Add(codeLabel);
                header.Add(weightLabel);

                var nameLabel = new Label(item.Name);
                nameLabel.AddToClassList("fui-inventory-slot__name");

                var typeLabel = new Label(item.Type);
                typeLabel.AddToClassList("fui-inventory-slot__type");

                slot.Add(header);
                slot.Add(nameLabel);
                slot.Add(typeLabel);

                _inventorySlotsContainer.Add(slot);
            }
        }
    }
}
