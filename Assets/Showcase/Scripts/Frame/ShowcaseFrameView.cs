using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.R3.Extensions;
using SUIF.Showcase.Common;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Frame
{
    [UIView("ShowcaseFrameView", SUIF.API.UILayer.Screens)]
    public sealed class ShowcaseFrameView : BaseView<ShowcaseFrameViewModel>
    {
        private const string ActiveClass = "is-active";
        private const string ActiveModalClass = "is-active-modal";
        private const string HiddenClass = "is-hidden";

        [UQuery("telemetry-base-readout")] private Label _telemetryBaseReadout;
        [UQuery("telemetry-modal-readout")] private Label _telemetryModalReadout;
        [UQuery("telemetry-fps-readout")] private Label _telemetryFpsReadout;
        [UQuery("telemetry-dc-readout")] private Label _telemetryDcReadout;
        [UQuery("btn-run-benchmark")] private Button _btnRunBenchmark;

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
        [UQuery("viewport-standby-box")] private VisualElement _standbyBox;

        [UQuery("footer-status-log")] private Label _footerStatusLog;

        private FixedTextSlot _telemetryBaseSlot;
        private FixedTextSlot _telemetryModalSlot;
        private FixedTextSlot _runBenchmarkSlot;
        private FixedTextSlot _headerOpenStateSlot;
        private FixedTextSlot _headerBaseTitleSlot;
        private FixedTextSlot _headerLayerTagSlot;
        private FixedTextSlot _closeBtnHintSlot;
        private FixedTextSlot _viewportIdSlot;
        private FixedTextSlot _viewportSubSlot;
        private FixedTextSlot _viewportCoordsSlot;

        public ShowcaseFrameView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_navBtnLifecycle != null) Binder.Bind(_navBtnLifecycle).ToClick(ViewModel.OpenLifecycleWindow);
            if (_navBtnMvvm != null) Binder.Bind(_navBtnMvvm).ToClick(ViewModel.OpenCounterWindow);
            if (_navBtnList != null) Binder.Bind(_navBtnList).ToClick(ViewModel.OpenRecycledListWindow);
            if (_navBtnModals != null) Binder.Bind(_navBtnModals).ToClick(ViewModel.ToggleModal);
            if (_navBtnInventory != null) Binder.Bind(_navBtnInventory).ToClick(ViewModel.OpenInventoryWindow);
            if (_navBtnTheme != null) Binder.Bind(_navBtnTheme).ToClick(ViewModel.OpenThemeMatrixWindow);

            if (_btnMainClose != null) Binder.Bind(_btnMainClose).ToClick(ViewModel.CloseActiveWindow);
            if (_btnRunBenchmark != null) Binder.Bind(_btnRunBenchmark).ToClick(ViewModel.TriggerBenchmark);

            AttachWipe(_navBtnLifecycle, DiagonalWipeStyle.Navigation);
            AttachWipe(_navBtnMvvm, DiagonalWipeStyle.Navigation);
            AttachWipe(_navBtnList, DiagonalWipeStyle.Navigation);
            AttachWipe(_navBtnModals, DiagonalWipeStyle.ModalNavigation);
            AttachWipe(_navBtnInventory, DiagonalWipeStyle.Navigation);
            AttachWipe(_navBtnTheme, DiagonalWipeStyle.Navigation);
            AttachWipe(_btnMainClose, DiagonalWipeStyle.Close);

            _telemetryBaseSlot = RegisterFixedSlot(_telemetryBaseReadout, 20);
            _telemetryModalSlot = RegisterFixedSlot(_telemetryModalReadout, 16);
            _runBenchmarkSlot = RegisterFixedSlot(_btnRunBenchmark, 16);
            _headerOpenStateSlot = RegisterFixedSlot(_headerOpenState, 8);
            _headerBaseTitleSlot = RegisterFixedSlot(_headerBaseTitle, 40);
            _headerLayerTagSlot = RegisterFixedSlot(_headerLayerTag, 32);
            _closeBtnHintSlot = RegisterFixedSlot(_closeBtnHint, 12);
            _viewportIdSlot = RegisterFixedSlot(_viewportIdBadge, 16);
            _viewportSubSlot = RegisterFixedSlot(_viewportSubBadge, 32);
            _viewportCoordsSlot = RegisterFixedSlot(_viewportCoords, 32);

            AddDisposable(ViewModel.ActiveNavIndex.Subscribe(OnNavIndexChanged));
            AddDisposable(ViewModel.ActiveTitle.Subscribe(OnTitleChanged));
            AddDisposable(ViewModel.IsModalOpen.Subscribe(OnModalStateChanged));
            AddDisposable(ViewModel.CurrentTheme.Subscribe(OnThemeChanged));

            if (_telemetryFpsReadout != null)
            {
                Binder.Bind(_telemetryFpsReadout).ToFixedText(ViewModel.CurrentFps, 14, "{0:F1} FPS");
            }

            if (_telemetryDcReadout != null)
            {
                Binder.Bind(_telemetryDcReadout).ToFixedText(ViewModel.DrawCalls, 14, "[{0} DC]");
            }

            if (_btnRunBenchmark != null)
            {
                AddDisposable(ViewModel.IsBenchmarkRecording.Subscribe(isRecording =>
                {
                    _runBenchmarkSlot?.SetText(isRecording ? "TESTING..." : "RUN BENCHMARK");
                    if (isRecording) _btnRunBenchmark.AddToClassList("is-active");
                    else _btnRunBenchmark.RemoveFromClassList("is-active");
                }));
            }

            if (_footerStatusLog != null)
            {
                Binder.Bind(_footerStatusLog).ToFixedText(ViewModel.FooterStatus, 48);
            }
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

        private void OnNavIndexChanged(int index)
        {
            UpdateBtn(_navBtnLifecycle, index == 1);
            UpdateBtn(_navBtnMvvm, index == 2);
            UpdateBtn(_navBtnList, index == 3);
            UpdateBtn(_navBtnInventory, index == 5);
            UpdateBtn(_navBtnTheme, index == 6);

            _viewportIdSlot?.SetFormat("VIEWPORT_0{0}", index);

            _viewportSubSlot?.SetText(index switch
            {
                1 => "// LIFECYCLE_STATE_MACHINE",
                2 => "// REACTIVE_PROPERTY_STREAM",
                3 => "// ZERO_ALLOC_SCROLL_POOL",
                5 => "// TACTICAL_GRID_STORAGE",
                6 => "// GLOBAL_SHADER_PALETTE",
                _ => "// STANDBY_BUFFER"
            });

            _telemetryBaseSlot?.SetText(index switch
            {
                1 => "01_LIFECYCLE",
                2 => "02_R3_MVVM",
                3 => "03_RECYCLED_LIST",
                5 => "05_INVENTORY",
                6 => "06_THEME_MATRIX",
                _ => "NONE // IDLE"
            });

            _viewportCoordsSlot?.SetText(index == 0 ? "BASE_STATE: STANDBY" : "BASE_STATE: ACTIVE");

            if (_standbyBox != null)
            {
                _standbyBox.style.display = index == 0 ? DisplayStyle.Flex : DisplayStyle.None;
            }

            _headerOpenStateSlot?.SetText(index == 0 && !ViewModel.IsModalOpen.Value ? "closed" : "open");
        }

        private void OnTitleChanged(string title)
        {
            _headerBaseTitleSlot?.SetText(title);
        }

        private void OnModalStateChanged(bool isOpen)
        {
            if (_navBtnModals != null)
            {
                if (isOpen) _navBtnModals.AddToClassList(ActiveModalClass);
                else _navBtnModals.RemoveFromClassList(ActiveModalClass);
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

            _closeBtnHintSlot?.SetText(isOpen ? "CLS // OVR" : "CLS // WIN");
            _telemetryModalSlot?.SetText(isOpen ? "MOUNTED [1]" : "STANDBY [0]");

            var index = ViewModel.ActiveNavIndex.Value;
            _headerLayerTagSlot?.SetText(isOpen
                ? ZString.Concat("LAYER_Z // ", index, "_BASE + MODAL_999")
                : ZString.Concat("LAYER_Z // ", index, "_BASE_100"));

            _viewportCoordsSlot?.SetText(isOpen
                ? ZString.Concat("BASE: ", index, " // OVERLAY: MODALS_04")
                : ZString.Concat("BASE_STATE: ", (index == 0 ? "UNMOUNTED" : "ACTIVE")));
        }

        private void OnThemeChanged(string themeId)
        {
            VisualElement.RemoveFromClassList("theme-mono");
            VisualElement.RemoveFromClassList("theme-cyan");
            VisualElement.RemoveFromClassList("theme-crimson");
            VisualElement.RemoveFromClassList("theme-amber");

            VisualElement.AddToClassList(ZString.Concat("theme-", themeId));
        }

        private void AttachWipe(Button btn, DiagonalWipeStyle style)
        {
            if (btn != null)
            {
                AddDisposable(new DiagonalWipeEffect(btn, style));
            }
        }

        private void UpdateBtn(Button btn, bool isActive)
        {
            if (btn == null) return;
            if (isActive) btn.AddToClassList(ActiveClass);
            else btn.RemoveFromClassList(ActiveClass);
        }
    }
}
