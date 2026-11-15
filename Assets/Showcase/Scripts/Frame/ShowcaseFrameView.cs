using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.R3.Extensions;
using SUIF.Showcase.Common;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Frame
{
    [UIView("ShowcaseFrameView", SUIF.API.UILayer.Screens)]
    public sealed class ShowcaseFrameView : BaseView<ShowcaseFrameViewModel>
    {
        private const string ActiveClass = "is-active";
        private const string ActiveModalClass = "is-active-modal";
        private const string HiddenClass = "is-hidden";

        private static readonly ShowcaseThemeId[] AllThemes =
        {
            ShowcaseThemeId.Mono,
            ShowcaseThemeId.Cyan,
            ShowcaseThemeId.Crimson,
            ShowcaseThemeId.Amber
        };

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
        private FixedTextSlot _footerStatusSlot;

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
            _footerStatusSlot = RegisterFixedSlot(_footerStatusLog, 48);

            Binder.ToDto(ViewModel.Navigation, this, static (dto, view) => view.ApplyNavigation(dto));
            Binder.ToDto(ViewModel.CurrentTheme, this, static (dto, view) => view.ApplyTheme(dto));

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
                AddDisposable(ViewModel.IsBenchmarkRecording.Subscribe(this, static (isRecording, view) =>
                {
                    view._runBenchmarkSlot?.SetWritten(isRecording, ShowcaseTextFormatter.WriteBenchmarkButtonText);
                    if (isRecording) view._btnRunBenchmark.AddToClassList(ActiveClass);
                    else view._btnRunBenchmark.RemoveFromClassList(ActiveClass);
                }));
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

        private void ApplyNavigation(FrameNavigationDto dto)
        {
            UpdateBtn(_navBtnLifecycle, dto.ActiveModule == ShowcaseModuleId.Lifecycle);
            UpdateBtn(_navBtnMvvm, dto.ActiveModule == ShowcaseModuleId.Counter);
            UpdateBtn(_navBtnList, dto.ActiveModule == ShowcaseModuleId.RecycledList);
            UpdateBtn(_navBtnInventory, dto.ActiveModule == ShowcaseModuleId.Inventory);
            UpdateBtn(_navBtnTheme, dto.ActiveModule == ShowcaseModuleId.ThemeMatrix);

            if (_navBtnModals != null)
            {
                if (dto.IsModalOpen) _navBtnModals.AddToClassList(ActiveModalClass);
                else _navBtnModals.RemoveFromClassList(ActiveModalClass);
            }

            if (_standbyBox != null)
            {
                _standbyBox.style.display = dto.ActiveModule == ShowcaseModuleId.Standby
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            }

            if (_headerOverlayBadge != null)
            {
                if (dto.IsModalOpen)
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

            _telemetryBaseSlot?.SetWritten(dto.ActiveModule, ShowcaseTextFormatter.WriteModuleBaseReadout);
            _telemetryModalSlot?.SetWritten(dto, ShowcaseTextFormatter.WriteModalTelemetry);
            _headerOpenStateSlot?.SetWritten(dto, ShowcaseTextFormatter.WriteFrameOpenState);
            _headerBaseTitleSlot?.SetWritten(dto.ActiveModule, ShowcaseTextFormatter.WriteModuleTitle);
            _headerLayerTagSlot?.SetWritten(dto, ShowcaseTextFormatter.WriteLayerTag);
            _closeBtnHintSlot?.SetWritten(dto, ShowcaseTextFormatter.WriteCloseButtonHint);
            _viewportIdSlot?.SetWritten(dto.ActiveModule, ShowcaseTextFormatter.WriteViewportIdBadge);
            _viewportSubSlot?.SetWritten(dto.ActiveModule, ShowcaseTextFormatter.WriteViewportSubBadge);
            _viewportCoordsSlot?.SetWritten(dto, ShowcaseTextFormatter.WriteViewportCoords);
            _footerStatusSlot?.SetWritten(dto, ShowcaseTextFormatter.WriteFooterStatus);
        }

        private void ApplyTheme(ShowcaseThemeDto dto)
        {
            for (int i = 0; i < AllThemes.Length; i++)
            {
                VisualElement.RemoveFromClassList(ShowcaseTextFormatter.GetThemeCssClass(AllThemes[i]));
            }

            VisualElement.AddToClassList(ShowcaseTextFormatter.GetThemeCssClass(dto.Id));
        }

        private void AttachWipe(Button btn, DiagonalWipeStyle style)
        {
            if (btn != null)
            {
                AddDisposable(new DiagonalWipeEffect(btn, style));
            }
        }

        private static void UpdateBtn(Button btn, bool isActive)
        {
            if (btn == null) return;
            if (isActive) btn.AddToClassList(ActiveClass);
            else btn.RemoveFromClassList(ActiveClass);
        }
    }
}
