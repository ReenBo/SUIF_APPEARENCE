using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
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

        [UQuery("footer-status-log")] private Label _footerStatusLog;

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

            AddDisposable(ViewModel.ActiveNavIndex.Subscribe(OnNavIndexChanged));
            AddDisposable(ViewModel.ActiveTitle.Subscribe(OnTitleChanged));
            AddDisposable(ViewModel.IsModalOpen.Subscribe(OnModalStateChanged));
            AddDisposable(ViewModel.CurrentTheme.Subscribe(OnThemeChanged));

            if (_footerStatusLog != null)
            {
                Binder.Bind(_footerStatusLog).ToText(ViewModel.FooterStatus);
            }
        }

        private void OnNavIndexChanged(int index)
        {
            UpdateBtn(_navBtnLifecycle, index == 1);
            UpdateBtn(_navBtnMvvm, index == 2);
            UpdateBtn(_navBtnList, index == 3);
            UpdateBtn(_navBtnInventory, index == 5);
            UpdateBtn(_navBtnTheme, index == 6);

            if (_viewportIdBadge != null)
            {
                _viewportIdBadge.text = ZString.Concat("VIEWPORT_0", index);
            }

            if (_viewportSubBadge != null)
            {
                _viewportSubBadge.text = index switch
                {
                    1 => "// LIFECYCLE_STATE_MACHINE",
                    2 => "// REACTIVE_PROPERTY_STREAM",
                    3 => "// ZERO_ALLOC_SCROLL_POOL",
                    5 => "// TACTICAL_GRID_STORAGE",
                    6 => "// GLOBAL_SHADER_PALETTE",
                    _ => "// STANDBY_BUFFER"
                };
            }

            if (_telemetryBaseReadout != null)
            {
                _telemetryBaseReadout.text = index switch
                {
                    1 => "01_LIFECYCLE",
                    2 => "02_R3_MVVM",
                    3 => "03_RECYCLED_LIST",
                    5 => "05_INVENTORY",
                    6 => "06_THEME_MATRIX",
                    _ => "NONE // IDLE"
                };
            }

            if (_headerOpenState != null)
            {
                _headerOpenState.text = index == 0 && !ViewModel.IsModalOpen.Value ? "closed" : "open";
            }
        }

        private void OnTitleChanged(string title)
        {
            if (_headerBaseTitle != null)
            {
                _headerBaseTitle.text = title;
            }
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

            if (_closeBtnHint != null)
            {
                _closeBtnHint.text = isOpen ? "CLS // OVR" : "CLS // WIN";
            }

            if (_telemetryModalReadout != null)
            {
                _telemetryModalReadout.text = isOpen ? "MOUNTED [1]" : "STANDBY [0]";
            }

            var index = ViewModel.ActiveNavIndex.Value;
            if (_headerLayerTag != null)
            {
                _headerLayerTag.text = isOpen
                    ? ZString.Concat("LAYER_Z // ", index, "_BASE + MODAL_999")
                    : ZString.Concat("LAYER_Z // ", index, "_BASE_100");
            }

            if (_viewportCoords != null)
            {
                _viewportCoords.text = isOpen
                    ? ZString.Concat("BASE: ", index, " // OVERLAY: MODALS_04")
                    : ZString.Concat("BASE_STATE: ", (index == 0 ? "UNMOUNTED" : "ACTIVE"));
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

        private void UpdateBtn(Button btn, bool isActive)
        {
            if (btn == null) return;
            if (isActive) btn.AddToClassList(ActiveClass);
            else btn.RemoveFromClassList(ActiveClass);
        }
    }
}
