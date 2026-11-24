using Cysharp.Text;
using SUIF.API;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Hub;
using UnityEngine;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Tahoe
{
    [UIView("MacOSTahoeView", UILayer.Screens)]
    public sealed class MacOSTahoeView : BaseView<MacOSTahoeViewModel>
    {
        private const string FocusClass = "is-focused";
        private const string OpenClass = "is-open";
        private const string ActiveClass = "is-active";
        private const string SelectedClass = "is-selected";
        private const int WindowCount = 7;
        private const int RecycledRowCount = 60;
        private const int Inv6SlotCount = 6;
        private const int Inv100SlotCount = 100;

        [UQuery("btn-tahoe-back")] private Button _btnBack;
        [UQuery("btn-tahoe-open-all")] private Button _btnOpenAll;
        [UQuery("btn-tahoe-benchmark")] private Button _btnBenchmark;
        [UQuery("btn-tahoe-close-all")] private Button _btnCloseAll;
        [UQuery("tahoe-fps-label")] private Label _fpsLabel;
        [UQuery("tahoe-dc-label")] private Label _dcLabel;
        [UQuery("tahoe-stage-header")] private Label _stageHeader;
        [UQuery("tahoe-bench-badge")] private Label _benchBadge;
        [UQuery("tahoe-window-stage")] private VisualElement _windowStage;
        [UQuery("tahoe-empty-state")] private VisualElement _emptyState;
        [UQuery("tahoe-empty-icon")] private VisualElement _emptyIcon;

        [UQuery("tahoe-nav-lifecycle")] private Button _navLifecycle;
        [UQuery("tahoe-nav-counter")] private Button _navCounter;
        [UQuery("tahoe-nav-list")] private Button _navList;
        [UQuery("tahoe-nav-modals")] private Button _navModals;
        [UQuery("tahoe-nav-inv6")] private Button _navInv6;
        [UQuery("tahoe-nav-inv100")] private Button _navInv100;
        [UQuery("tahoe-nav-themes")] private Button _navThemes;

        [UQuery("tahoe-icon-lifecycle")] private VisualElement _iconLifecycle;
        [UQuery("tahoe-icon-counter")] private VisualElement _iconCounter;
        [UQuery("tahoe-icon-list")] private VisualElement _iconList;
        [UQuery("tahoe-icon-modals")] private VisualElement _iconModals;
        [UQuery("tahoe-icon-inv6")] private VisualElement _iconInv6;
        [UQuery("tahoe-icon-inv100")] private VisualElement _iconInv100;
        [UQuery("tahoe-icon-themes")] private VisualElement _iconThemes;

        [UQuery("tahoe-win-lifecycle")] private VisualElement _winLifecycle;
        [UQuery("tahoe-win-counter")] private VisualElement _winCounter;
        [UQuery("tahoe-win-list")] private VisualElement _winList;
        [UQuery("tahoe-win-modals")] private VisualElement _winModals;
        [UQuery("tahoe-win-inv6")] private VisualElement _winInv6;
        [UQuery("tahoe-win-inv100")] private VisualElement _winInv100;
        [UQuery("tahoe-win-themes")] private VisualElement _winThemes;

        [UQuery("tahoe-drag-lifecycle")] private VisualElement _dragLifecycle;
        [UQuery("tahoe-drag-counter")] private VisualElement _dragCounter;
        [UQuery("tahoe-drag-list")] private VisualElement _dragList;
        [UQuery("tahoe-drag-modals")] private VisualElement _dragModals;
        [UQuery("tahoe-drag-inv6")] private VisualElement _dragInv6;
        [UQuery("tahoe-drag-inv100")] private VisualElement _dragInv100;
        [UQuery("tahoe-drag-themes")] private VisualElement _dragThemes;

        [UQuery("tahoe-close-lifecycle")] private Button _closeLifecycle;
        [UQuery("tahoe-close-counter")] private Button _closeCounter;
        [UQuery("tahoe-close-list")] private Button _closeList;
        [UQuery("tahoe-close-modals")] private Button _closeModals;
        [UQuery("tahoe-close-inv6")] private Button _closeInv6;
        [UQuery("tahoe-close-inv100")] private Button _closeInv100;
        [UQuery("tahoe-close-themes")] private Button _closeThemes;

        [UQuery("tahoe-hook-init")] private Button _hookInit;
        [UQuery("tahoe-hook-open")] private Button _hookOpen;
        [UQuery("tahoe-hook-show")] private Button _hookShow;
        [UQuery("tahoe-hook-hide")] private Button _hookHide;
        [UQuery("tahoe-hook-close")] private Button _hookClose;
        [UQuery("tahoe-lifecycle-val")] private Label _lifecycleVal;

        [UQuery("tahoe-counter-val")] private Label _counterVal;
        [UQuery("tahoe-counter-dec")] private Button _counterDec;
        [UQuery("tahoe-counter-inc")] private Button _counterInc;
        [UQuery("tahoe-counter-reset")] private Button _counterReset;

        [UQuery("tahoe-list-scroll")] private ScrollView _listScroll;

        [UQuery("tahoe-modal-sheet")] private VisualElement _modalSheet;
        [UQuery("tahoe-modal-present")] private Button _modalPresent;
        [UQuery("tahoe-modal-dismiss")] private Button _modalDismiss;

        [UQuery("tahoe-inv6-grid")] private VisualElement _inv6Grid;
        [UQuery("tahoe-inv6-title")] private Label _inv6Title;
        [UQuery("tahoe-inv6-desc")] private Label _inv6Desc;
        [UQuery("tahoe-inv6-rarity")] private Label _inv6Rarity;

        [UQuery("tahoe-inv100-scroll")] private ScrollView _inv100Scroll;
        [UQuery("tahoe-inv100-grid")] private VisualElement _inv100Grid;
        [UQuery("tahoe-inv100-title")] private Label _inv100Title;
        [UQuery("tahoe-inv100-meta")] private Label _inv100Meta;
        [UQuery("tahoe-inv100-rarity")] private Label _inv100Rarity;

        [UQuery("tahoe-theme-dark")] private Button _themeDark;
        [UQuery("tahoe-theme-white")] private Button _themeWhite;
        [UQuery("tahoe-theme-indigo")] private Button _themeIndigo;
        [UQuery("tahoe-theme-crimson")] private Button _themeCrimson;

        private readonly VisualElement[] _windows = new VisualElement[WindowCount];
        private readonly VisualElement[] _dragHandles = new VisualElement[WindowCount];
        private readonly Button[] _navButtons = new Button[WindowCount];
        private readonly Vector2[] _windowOffsets = new Vector2[WindowCount];
        private readonly Button[] _inv6Buttons = new Button[Inv6SlotCount];
        private readonly Button[] _inv100Buttons = new Button[Inv100SlotCount];

        private FixedTextSlot _fpsSlot;
        private FixedTextSlot _dcSlot;
        private FixedTextSlot _stageHeaderSlot;
        private FixedTextSlot _benchBadgeSlot;
        private FixedTextSlot _lifecycleSlot;
        private FixedTextSlot _counterSlot;
        private FixedTextSlot _inv6TitleSlot;
        private FixedTextSlot _inv6DescSlot;
        private FixedTextSlot _inv6RaritySlot;
        private FixedTextSlot _inv100TitleSlot;
        private FixedTextSlot _inv100MetaSlot;
        private FixedTextSlot _inv100RaritySlot;

        private int _lastOpenMask;
        private TahoeThemeId _lastTheme = TahoeThemeId.Dark;
        private int _draggingIndex = -1;
        private int _dragPointerId = -1;
        private Vector2 _dragStartPointer;
        private Vector2 _dragStartOffset;
        private int _lastListScrollStep;
        private int _lastInv100ScrollStep;
        private VisualElement _keyRoot;

        public MacOSTahoeView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            CacheWindowArrays();
            RegisterIcons();
            InitializeWindowPositions();
            BuildRecycledListRows();
            BuildInventory6Slots();
            BuildInventory100Slots();

            if (_btnBack != null) Binder.Bind(_btnBack).ToClick(ViewModel.ReturnToHub);
            if (_btnOpenAll != null) Binder.Bind(_btnOpenAll).ToClick(ViewModel.OpenAllWindows);
            if (_btnBenchmark != null) Binder.Bind(_btnBenchmark).ToClick(ViewModel.RunBenchmark);
            if (_btnCloseAll != null) Binder.Bind(_btnCloseAll).ToClick(ViewModel.CloseAllWindows);

            BindNavigationButtons();
            BindWindowControls();
            BindWindowFocusAndDrag();

            _fpsSlot = Binder.FixedSlot(_fpsLabel, 14);
            _dcSlot = Binder.FixedSlot(_dcLabel, 14);
            _stageHeaderSlot = Binder.FixedSlot(_stageHeader, 48);
            _benchBadgeSlot = Binder.FixedSlot(_benchBadge, 36);
            _lifecycleSlot = Binder.FixedSlot(_lifecycleVal, 20);
            _counterSlot = Binder.FixedSlot(_counterVal, 16);
            _inv6TitleSlot = Binder.FixedSlot(_inv6Title, 32);
            _inv6DescSlot = Binder.FixedSlot(_inv6Desc, 84);
            _inv6RaritySlot = Binder.FixedSlot(_inv6Rarity, 16);
            _inv100TitleSlot = Binder.FixedSlot(_inv100Title, 28);
            _inv100MetaSlot = Binder.FixedSlot(_inv100Meta, 28);
            _inv100RaritySlot = Binder.FixedSlot(_inv100Rarity, 14);

            Binder.ToData(ViewModel.DesktopState, this, static (dto, view) => view.ApplyDesktopState(dto));
            Binder.ToData(ViewModel.LifecycleState, this, static (dto, view) => view.ApplyLifecycle(dto));
            Binder.ToData(ViewModel.CounterState, this, static (dto, view) => view.ApplyCounter(dto));
            Binder.ToData(ViewModel.Inventory6State, this, static (dto, view) => view.ApplyInv6(dto));
            Binder.ToData(ViewModel.Telemetry, this, static (dto, view) => view.ApplyTelemetry(dto));

            VisualElement.focusable = true;
            VisualElement.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            VisualElement.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        public override void Show()
        {
            base.Show();
            VisualElement.Focus();
        }

        public override void Unbind()
        {
            CleanupCallbacks();
            base.Unbind();
        }

        public override void Dispose()
        {
            CleanupCallbacks();
            base.Dispose();
        }

        private void CleanupCallbacks()
        {
            if (_keyRoot != null)
            {
                _keyRoot.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
                _keyRoot.UnregisterCallback<NavigationCancelEvent>(OnNavigationCancel, TrickleDown.TrickleDown);
                _keyRoot = null;
            }

            VisualElement.UnregisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            VisualElement.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            for (int i = 0; i < WindowCount; i++)
            {
                if (_windows[i] != null)
                {
                    _windows[i].UnregisterCallback<PointerDownEvent>(OnWindowPointerDown);
                }

                if (_dragHandles[i] != null)
                {
                    _dragHandles[i].UnregisterCallback<PointerDownEvent>(OnDragPointerDown);
                    _dragHandles[i].UnregisterCallback<PointerMoveEvent>(OnDragPointerMove);
                    _dragHandles[i].UnregisterCallback<PointerUpEvent>(OnDragPointerUp);
                    _dragHandles[i].UnregisterCallback<PointerCaptureOutEvent>(OnDragCaptureOut);
                }
            }
        }

        private void CacheWindowArrays()
        {
            _windows[0] = _winLifecycle;
            _windows[1] = _winCounter;
            _windows[2] = _winList;
            _windows[3] = _winModals;
            _windows[4] = _winInv6;
            _windows[5] = _winInv100;
            _windows[6] = _winThemes;

            _dragHandles[0] = _dragLifecycle;
            _dragHandles[1] = _dragCounter;
            _dragHandles[2] = _dragList;
            _dragHandles[3] = _dragModals;
            _dragHandles[4] = _dragInv6;
            _dragHandles[5] = _dragInv100;
            _dragHandles[6] = _dragThemes;

            _navButtons[0] = _navLifecycle;
            _navButtons[1] = _navCounter;
            _navButtons[2] = _navList;
            _navButtons[3] = _navModals;
            _navButtons[4] = _navInv6;
            _navButtons[5] = _navInv100;
            _navButtons[6] = _navThemes;
        }

        private void RegisterIcons()
        {
            RegisterIcon(_iconLifecycle, HubVectorIconKind.TahoeLifecycle, new Color(0.22f, 0.74f, 0.97f, 1f));
            RegisterIcon(_iconCounter, HubVectorIconKind.TahoeCounter, new Color(0.51f, 0.55f, 0.97f, 1f));
            RegisterIcon(_iconList, HubVectorIconKind.TahoeRecycledList, new Color(0.18f, 0.83f, 0.75f, 1f));
            RegisterIcon(_iconModals, HubVectorIconKind.TahoeModals, new Color(0.98f, 0.44f, 0.52f, 1f));
            RegisterIcon(_iconInv6, HubVectorIconKind.TahoeInv6, new Color(0.98f, 0.75f, 0.14f, 1f));
            RegisterIcon(_iconInv100, HubVectorIconKind.TahoeInv100, new Color(0.98f, 0.57f, 0.24f, 1f));
            RegisterIcon(_iconThemes, HubVectorIconKind.TahoeThemes, new Color(0.75f, 0.52f, 0.99f, 1f));
            RegisterIcon(_emptyIcon, HubVectorIconKind.TahoeDesktopCommand, new Color(0.65f, 0.72f, 0.85f, 0.65f));
        }

        private void RegisterIcon(VisualElement element, HubVectorIconKind kind, Color color)
        {
            if (element != null)
            {
                AddDisposable(new HubVectorIconEffect(element, kind, color, color));
            }
        }

        private void InitializeWindowPositions()
        {
            _windowOffsets[0] = new Vector2(20f, 18f);
            _windowOffsets[1] = new Vector2(420f, 22f);
            _windowOffsets[2] = new Vector2(36f, 170f);
            _windowOffsets[3] = new Vector2(470f, 174f);
            _windowOffsets[4] = new Vector2(72f, 290f);
            _windowOffsets[5] = new Vector2(450f, 280f);
            _windowOffsets[6] = new Vector2(240f, 340f);

            for (int i = 0; i < WindowCount; i++)
            {
                if (_windows[i] != null)
                {
                    _windows[i].style.translate = new Translate(_windowOffsets[i].x, _windowOffsets[i].y);
                }
            }
        }

        private void BuildRecycledListRows()
        {
            if (_listScroll == null)
            {
                return;
            }

            var formatter = ViewModel.Formatter;
            for (int i = 0; i < RecycledRowCount; i++)
            {
                var row = new VisualElement();
                row.AddToClassList("tahoe-list-row");

                var left = new VisualElement();
                left.AddToClassList("tahoe-list-row-left");

                var idxLabel = new Label();
                idxLabel.AddToClassList("tahoe-list-idx");
                idxLabel.AddToClassList("font-mono-fui");
                idxLabel.text = FormatOnce(i, formatter.TahoeListRowIndex);

                var titleLabel = new Label();
                titleLabel.AddToClassList("tahoe-list-title");
                titleLabel.text = FormatOnce(i, formatter.TahoeListRowTitle);

                var slotBadge = new Label();
                slotBadge.AddToClassList("tahoe-list-slot");
                slotBadge.AddToClassList("font-mono-fui");
                slotBadge.text = FormatOnce(i, formatter.TahoeListRowSlot);

                left.Add(idxLabel);
                left.Add(titleLabel);
                row.Add(left);
                row.Add(slotBadge);
                _listScroll.Add(row);
            }
        }

        private void BuildInventory6Slots()
        {
            if (_inv6Grid == null)
            {
                return;
            }

            var items = ViewModel.Inventory6Items;
            var formatter = ViewModel.Formatter;
            int count = Mathf.Min(Inv6SlotCount, items.Length);

            for (int i = 0; i < count; i++)
            {
                int slotIndex = i;
                var btn = new Button();
                btn.name = i switch
                {
                    0 => "tahoe-inv6-slot-0",
                    1 => "tahoe-inv6-slot-1",
                    2 => "tahoe-inv6-slot-2",
                    3 => "tahoe-inv6-slot-3",
                    4 => "tahoe-inv6-slot-4",
                    _ => "tahoe-inv6-slot-5"
                };
                btn.AddToClassList("tahoe-inv-slot");
                btn.AddToClassList("tahoe-inv6-slot");

                var catLabel = new Label();
                catLabel.AddToClassList("tahoe-inv6-cat");
                catLabel.AddToClassList("font-mono-fui");
                catLabel.pickingMode = PickingMode.Ignore;
                catLabel.text = FormatOnce(items[i].Category, formatter.InventoryCategory);

                var nameLabel = new Label();
                nameLabel.AddToClassList("tahoe-inv6-name");
                nameLabel.pickingMode = PickingMode.Ignore;
                nameLabel.text = FormatOnce(items[i].Code, formatter.InventoryName);

                btn.Add(catLabel);
                btn.Add(nameLabel);
                Binder.Bind(btn).ToClick(() => ViewModel.SelectInv6(slotIndex));
                _inv6Buttons[i] = btn;
                _inv6Grid.Add(btn);
            }
        }

        private void BuildInventory100Slots()
        {
            if (_inv100Grid == null)
            {
                return;
            }

            var formatter = ViewModel.Formatter;
            for (int i = 0; i < Inv100SlotCount; i++)
            {
                int slotIndex = i;
                var btn = new Button();
                btn.AddToClassList("tahoe-inv-slot");
                btn.AddToClassList("tahoe-inv100-slot");

                var codeLabel = new Label();
                codeLabel.AddToClassList("tahoe-inv100-code");
                codeLabel.AddToClassList("font-mono-fui");
                codeLabel.pickingMode = PickingMode.Ignore;
                codeLabel.text = FormatOnce(i, formatter.TahoeInv100SlotCode);

                btn.Add(codeLabel);
                Binder.Bind(btn).ToClick(() => ViewModel.SelectInv100(slotIndex));
                _inv100Buttons[i] = btn;
                _inv100Grid.Add(btn);
            }
        }

        private static string FormatOnce<TState>(TState state, FixedTextWriter<TState> writer)
        {
            var sb = ZString.CreateStringBuilder(true);
            try
            {
                writer(ref sb, state);
                return sb.ToString();
            }
            finally
            {
                sb.Dispose();
            }
        }

        private void BindNavigationButtons()
        {
            if (_navLifecycle != null) Binder.Bind(_navLifecycle).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.Lifecycle));
            if (_navCounter != null) Binder.Bind(_navCounter).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.Counter));
            if (_navList != null) Binder.Bind(_navList).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.RecycledList));
            if (_navModals != null) Binder.Bind(_navModals).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.Modals));
            if (_navInv6 != null) Binder.Bind(_navInv6).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.Inventory6));
            if (_navInv100 != null) Binder.Bind(_navInv100).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.Inventory100));
            if (_navThemes != null) Binder.Bind(_navThemes).ToClick(() => ViewModel.OpenOrFocusWindow(TahoeWindowId.Themes));

            if (_closeLifecycle != null) Binder.Bind(_closeLifecycle).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.Lifecycle));
            if (_closeCounter != null) Binder.Bind(_closeCounter).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.Counter));
            if (_closeList != null) Binder.Bind(_closeList).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.RecycledList));
            if (_closeModals != null) Binder.Bind(_closeModals).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.Modals));
            if (_closeInv6 != null) Binder.Bind(_closeInv6).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.Inventory6));
            if (_closeInv100 != null) Binder.Bind(_closeInv100).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.Inventory100));
            if (_closeThemes != null) Binder.Bind(_closeThemes).ToClick(() => ViewModel.CloseWindow(TahoeWindowId.Themes));
        }

        private void BindWindowControls()
        {
            if (_hookInit != null) Binder.Bind(_hookInit).ToClick(() => ViewModel.TriggerLifecycle(LifecycleStageId.Initialize));
            if (_hookOpen != null) Binder.Bind(_hookOpen).ToClick(() => ViewModel.TriggerLifecycle(LifecycleStageId.Open));
            if (_hookShow != null) Binder.Bind(_hookShow).ToClick(() => ViewModel.TriggerLifecycle(LifecycleStageId.Show));
            if (_hookHide != null) Binder.Bind(_hookHide).ToClick(() => ViewModel.TriggerLifecycle(LifecycleStageId.Hide));
            if (_hookClose != null) Binder.Bind(_hookClose).ToClick(() => ViewModel.TriggerLifecycle(LifecycleStageId.Close));

            if (_counterDec != null) Binder.Bind(_counterDec).ToClick(() => ViewModel.StepCounter(-1));
            if (_counterInc != null) Binder.Bind(_counterInc).ToClick(() => ViewModel.StepCounter(1));
            if (_counterReset != null) Binder.Bind(_counterReset).ToClick(ViewModel.ResetCounter);

            if (_modalPresent != null) Binder.Bind(_modalPresent).ToClick(() => ViewModel.ToggleModalSheet(true));
            if (_modalDismiss != null) Binder.Bind(_modalDismiss).ToClick(() => ViewModel.ToggleModalSheet(false));

            if (_themeDark != null) Binder.Bind(_themeDark).ToClick(() => ViewModel.SetTheme(TahoeThemeId.Dark));
            if (_themeWhite != null) Binder.Bind(_themeWhite).ToClick(() => ViewModel.SetTheme(TahoeThemeId.White));
            if (_themeIndigo != null) Binder.Bind(_themeIndigo).ToClick(() => ViewModel.SetTheme(TahoeThemeId.Indigo));
            if (_themeCrimson != null) Binder.Bind(_themeCrimson).ToClick(() => ViewModel.SetTheme(TahoeThemeId.Crimson));
        }

        private void BindWindowFocusAndDrag()
        {
            for (int i = 0; i < WindowCount; i++)
            {
                if (_windows[i] != null)
                {
                    _windows[i].userData = i;
                    _windows[i].RegisterCallback<PointerDownEvent>(OnWindowPointerDown);
                }

                if (_dragHandles[i] != null)
                {
                    _dragHandles[i].userData = i;
                    _dragHandles[i].RegisterCallback<PointerDownEvent>(OnDragPointerDown);
                    _dragHandles[i].RegisterCallback<PointerMoveEvent>(OnDragPointerMove);
                    _dragHandles[i].RegisterCallback<PointerUpEvent>(OnDragPointerUp);
                    _dragHandles[i].RegisterCallback<PointerCaptureOutEvent>(OnDragCaptureOut);
                }
            }
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            _keyRoot = evt.destinationPanel?.visualTree;
            if (_keyRoot != null)
            {
                _keyRoot.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
                _keyRoot.RegisterCallback<NavigationCancelEvent>(OnNavigationCancel, TrickleDown.TrickleDown);
            }

            VisualElement.Focus();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            if (_keyRoot != null)
            {
                _keyRoot.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
                _keyRoot.UnregisterCallback<NavigationCancelEvent>(OnNavigationCancel, TrickleDown.TrickleDown);
                _keyRoot = null;
            }
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Escape)
            {
                ViewModel.ReturnToHub();
                evt.StopPropagation();
            }
        }

        private void OnNavigationCancel(NavigationCancelEvent evt)
        {
            ViewModel.ReturnToHub();
            evt.StopPropagation();
        }

        private void OnWindowPointerDown(PointerDownEvent evt)
        {
            if (evt.currentTarget is VisualElement win && win.userData is int index)
            {
                ViewModel.OpenOrFocusWindow((TahoeWindowId)index);
            }
        }

        private void OnDragPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || evt.currentTarget is not VisualElement handle || handle.userData is not int index)
            {
                return;
            }

            if (evt.target is Button || (evt.target as VisualElement)?.GetFirstAncestorOfType<Button>() != null)
            {
                return;
            }

            ViewModel.OpenOrFocusWindow((TahoeWindowId)index);
            _draggingIndex = index;
            _dragPointerId = evt.pointerId;
            _dragStartPointer = evt.position;
            _dragStartOffset = _windowOffsets[index];
            handle.CapturePointer(evt.pointerId);
            evt.StopPropagation();
        }

        private void OnDragPointerMove(PointerMoveEvent evt)
        {
            if (_draggingIndex < 0 || evt.pointerId != _dragPointerId || _windowStage == null)
            {
                return;
            }

            var win = _windows[_draggingIndex];
            if (win == null)
            {
                return;
            }

            Vector2 delta = (Vector2)evt.position - _dragStartPointer;
            var stageRect = _windowStage.contentRect;
            float winW = win.layout.width > 10f ? win.layout.width : 360f;
            float maxX = Mathf.Max(0f, stageRect.width - winW);
            float maxY = Mathf.Max(0f, stageRect.height - 44f);

            float nextX = Mathf.Clamp(_dragStartOffset.x + delta.x, 0f, maxX);
            float nextY = Mathf.Clamp(_dragStartOffset.y + delta.y, 0f, maxY);

            _windowOffsets[_draggingIndex] = new Vector2(nextX, nextY);
            win.style.translate = new Translate(nextX, nextY);
        }

        private void OnDragPointerUp(PointerUpEvent evt)
        {
            if (_draggingIndex >= 0 && evt.pointerId == _dragPointerId && evt.currentTarget is VisualElement handle)
            {
                if (handle.HasPointerCapture(evt.pointerId))
                {
                    handle.ReleasePointer(evt.pointerId);
                }

                _draggingIndex = -1;
                _dragPointerId = -1;
            }
        }

        private void OnDragCaptureOut(PointerCaptureOutEvent evt)
        {
            _draggingIndex = -1;
            _dragPointerId = -1;
        }

        private void ApplyDesktopState(TahoeDesktopStateDto state)
        {
            var formatter = ViewModel.Formatter;
            bool themeChanged = state.Theme != _lastTheme;
            _lastTheme = state.Theme;

            VisualElement.RemoveFromClassList("theme-tahoe-dark");
            VisualElement.RemoveFromClassList("theme-tahoe-white");
            VisualElement.RemoveFromClassList("theme-tahoe-indigo");
            VisualElement.RemoveFromClassList("theme-tahoe-crimson");
            VisualElement.AddToClassList(formatter.GetTahoeThemeCssClass(state.Theme));

            if (themeChanged)
            {
                RefreshThemeTintedSlots();
            }

            SetClass(_themeDark, ActiveClass, state.Theme == TahoeThemeId.Dark);
            SetClass(_themeWhite, ActiveClass, state.Theme == TahoeThemeId.White);
            SetClass(_themeIndigo, ActiveClass, state.Theme == TahoeThemeId.Indigo);
            SetClass(_themeCrimson, ActiveClass, state.Theme == TahoeThemeId.Crimson);

            for (int i = 0; i < WindowCount; i++)
            {
                var id = (TahoeWindowId)i;
                bool isOpen = state.IsWindowOpen(id);
                bool wasOpen = (_lastOpenMask & (1 << i)) != 0;
                bool isFocused = state.HasFocusedWindow && state.FocusedWindow == id;

                var win = _windows[i];
                if (win != null)
                {
                    win.style.display = isOpen ? DisplayStyle.Flex : DisplayStyle.None;
                    SetClass(win, FocusClass, isFocused);
                    if (isOpen && (!wasOpen || isFocused))
                    {
                        var parent = win.parent;
                        if (parent != null && parent[parent.childCount - 1] != win)
                        {
                            win.BringToFront();
                        }
                    }
                }

                SetClass(_navButtons[i], OpenClass, isOpen);
            }

            _lastOpenMask = state.OpenMask;

            if (_emptyState != null)
            {
                _emptyState.style.display = state.OpenMask == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (_modalSheet != null)
            {
                _modalSheet.style.display = state.IsModalSheetOpen ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (_benchBadge != null)
            {
                bool showBadge = state.IsBenchmarkRunning || state.IsBenchmarkCompleted;
                _benchBadge.style.display = showBadge ? DisplayStyle.Flex : DisplayStyle.None;
                if (showBadge)
                {
                    _benchBadgeSlot?.SetWritten(state.IsBenchmarkRunning, formatter.TahoeBenchmarkBadge);
                }
            }

            _stageHeaderSlot?.SetWritten(state, formatter.TahoeStageHeader);

            for (int i = 0; i < Inv100SlotCount; i++)
            {
                SetClass(_inv100Buttons[i], SelectedClass, i == state.Inventory100SelectedIndex);
            }

            _inv100TitleSlot?.SetWritten(state.Inventory100SelectedIndex, formatter.TahoeInv100Title);
            _inv100MetaSlot?.SetWritten(state.Inventory100SelectedIndex, formatter.TahoeInv100Meta);
            _inv100RaritySlot?.SetWritten(state.Inventory100SelectedIndex, formatter.TahoeInv100Rarity);

            if (state.ListScrollStep != _lastListScrollStep && _listScroll != null)
            {
                _lastListScrollStep = state.ListScrollStep;
                _listScroll.scrollOffset = new Vector2(0f, state.ListScrollStep * 140f);
            }

            if (state.Inv100ScrollStep != _lastInv100ScrollStep && _inv100Scroll != null)
            {
                _lastInv100ScrollStep = state.Inv100ScrollStep;
                _inv100Scroll.scrollOffset = new Vector2(0f, state.Inv100ScrollStep * 110f);
            }
        }

        private void ApplyLifecycle(LifecycleViewDto dto)
        {
            _lifecycleSlot?.SetWritten(dto.CurrentStage, ViewModel.Formatter.TahoeLifecycleStage);
            SetClass(_hookInit, ActiveClass, dto.CurrentStage == LifecycleStageId.Initialize);
            SetClass(_hookOpen, ActiveClass, dto.CurrentStage == LifecycleStageId.Open);
            SetClass(_hookShow, ActiveClass, dto.CurrentStage == LifecycleStageId.Show);
            SetClass(_hookHide, ActiveClass, dto.CurrentStage == LifecycleStageId.Hide);
            SetClass(_hookClose, ActiveClass, dto.CurrentStage == LifecycleStageId.Close);
        }

        private void ApplyCounter(CounterViewDto dto)
        {
            _counterSlot?.SetWritten(dto.CurrentValue, ViewModel.Formatter.TahoeCounterValue);
        }

        private void ApplyInv6(InventoryViewDto dto)
        {
            var items = ViewModel.Inventory6Items;
            if (items.Length == 0)
            {
                return;
            }

            int idx = Mathf.Clamp(dto.SelectedIndex, 0, items.Length - 1);
            for (int i = 0; i < Inv6SlotCount; i++)
            {
                SetClass(_inv6Buttons[i], SelectedClass, i == idx);
            }

            var item = items[idx];
            var formatter = ViewModel.Formatter;
            _inv6TitleSlot?.SetWritten(item.Code, formatter.InventoryName);
            _inv6DescSlot?.SetWritten(item.Code, formatter.InventoryDescription);
            _inv6RaritySlot?.SetWritten(item.Rarity, formatter.InventoryRarityBadge);
        }

        private void ApplyTelemetry(FrameTelemetryDto dto)
        {
            var formatter = ViewModel.Formatter;
            _fpsSlot?.SetWritten(dto, formatter.TahoeFpsReadout);
            _dcSlot?.SetWritten(dto, formatter.TahoeDcReadout);
        }

        private void RefreshThemeTintedSlots()
        {
            _stageHeaderSlot?.SetText(string.Empty);
            _benchBadgeSlot?.SetText(string.Empty);
            _lifecycleSlot?.SetText(string.Empty);
            _counterSlot?.SetText(string.Empty);
            _inv6TitleSlot?.SetText(string.Empty);
            _inv6DescSlot?.SetText(string.Empty);
            _inv6RaritySlot?.SetText(string.Empty);
            _inv100TitleSlot?.SetText(string.Empty);
            _inv100MetaSlot?.SetText(string.Empty);
            _inv100RaritySlot?.SetText(string.Empty);
            _fpsSlot?.SetText(string.Empty);
            _dcSlot?.SetText(string.Empty);

            ApplyLifecycle(ViewModel.LifecycleState.CurrentValue);
            ApplyCounter(ViewModel.CounterState.CurrentValue);
            ApplyInv6(ViewModel.Inventory6State.CurrentValue);
            ApplyTelemetry(ViewModel.Telemetry.CurrentValue);
        }

        private static void SetClass(VisualElement element, string className, bool active)
        {
            if (element == null)
            {
                return;
            }

            if (active) element.AddToClassList(className);
            else element.RemoveFromClassList(className);
        }
    }
}
