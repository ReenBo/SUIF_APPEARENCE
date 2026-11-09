using System;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Benchmark;
using SUIF.Showcase.Common;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;
using SUIF.Showcase.Windows.ThemeMatrix;

namespace SUIF.Showcase.Frame
{
    public sealed class ShowcaseFrameViewModel : BaseViewModel
    {
        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseThemeService _themeService;
        private readonly BenchmarkTelemetryService _telemetryService;
        private readonly Subject<Unit> _benchmarkRequestedSubject = new();
        private readonly ReactiveProperty<float> _fallbackFps = new(60f);
        private readonly ReactiveProperty<long> _fallbackDrawCalls = new(0);
        private readonly ReactiveProperty<bool> _fallbackRecording = new(false);

        public ReactiveProperty<int> ActiveNavIndex { get; } = new(0);
        public ReactiveProperty<string> ActiveTitle { get; } = new("[ NO_ACTIVE_VIEW // IDLE ]");
        public ReactiveProperty<bool> IsModalOpen { get; } = new(false);
        public ReactiveProperty<string> FooterStatus { get; } = new("STATE_SYNC // READY");

        public ReadOnlyReactiveProperty<string> CurrentTheme => _themeService.CurrentTheme;
        public ReadOnlyReactiveProperty<float> CurrentFps => _telemetryService != null ? _telemetryService.CurrentFps : _fallbackFps;
        public ReadOnlyReactiveProperty<long> DrawCalls => _telemetryService != null ? _telemetryService.DrawCalls : _fallbackDrawCalls;
        public ReadOnlyReactiveProperty<bool> IsBenchmarkRecording => _telemetryService != null ? _telemetryService.IsRecording : _fallbackRecording;
        public Observable<Unit> BenchmarkRequested => _benchmarkRequestedSubject;

        public ShowcaseFrameViewModel(
            IUIFlow uiFlow, 
            ShowcaseThemeService themeService,
            BenchmarkTelemetryService telemetryService = null)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _telemetryService = telemetryService;

            AddDisposable(ActiveNavIndex);
            AddDisposable(ActiveTitle);
            AddDisposable(IsModalOpen);
            AddDisposable(FooterStatus);
            AddDisposable(_benchmarkRequestedSubject);
            AddDisposable(_fallbackFps);
            AddDisposable(_fallbackDrawCalls);
            AddDisposable(_fallbackRecording);
        }

        public void TriggerBenchmark()
        {
            _benchmarkRequestedSubject.OnNext(Unit.Default);
        }

        public void OpenLifecycleWindow()
        {
            if (ActiveNavIndex.Value == 1)
            {
                return;
            }

            CloseAllBaseWindows();
            ActiveNavIndex.Value = 1;
            ActiveTitle.Value = "Basic Window Lifecycle";
            FooterStatus.Value = "OPENED // LIFECYCLE_WINDOW";
            _uiFlow.OpenAsync<LifecycleWindowView>().Forget();
        }

        public void OpenCounterWindow()
        {
            if (ActiveNavIndex.Value == 2)
            {
                return;
            }

            CloseAllBaseWindows();
            ActiveNavIndex.Value = 2;
            ActiveTitle.Value = "MVVM & R3 Reactive Counter";
            FooterStatus.Value = "OPENED // COUNTER_WINDOW";
            _uiFlow.OpenAsync<ReactiveCounterView>().Forget();
        }

        public void OpenRecycledListWindow()
        {
            if (ActiveNavIndex.Value == 3)
            {
                return;
            }

            CloseAllBaseWindows();
            ActiveNavIndex.Value = 3;
            ActiveTitle.Value = "Virtualized Recycled List";
            FooterStatus.Value = "OPENED // RECYCLED_LIST_WINDOW";
            _uiFlow.OpenAsync<RecycledListWindowView>().Forget();
        }

        public void ToggleModal()
        {
            if (IsModalOpen.Value)
            {
                IsModalOpen.Value = false;
                FooterStatus.Value = "UNMOUNTED // MODAL_OVERLAY";
                _uiFlow.Close<ModalDialogView>();
            }
            else
            {
                IsModalOpen.Value = true;
                FooterStatus.Value = "MOUNTED // MODAL_OVERLAY";
                _uiFlow.OpenAsync<ModalDialogView>().Forget();
            }
        }

        public void OpenInventoryWindow()
        {
            if (ActiveNavIndex.Value == 5)
            {
                return;
            }

            CloseAllBaseWindows();
            ActiveNavIndex.Value = 5;
            ActiveTitle.Value = "INVENTORY";
            FooterStatus.Value = "OPENED // INVENTORY_WINDOW";
            _uiFlow.OpenAsync<InventoryWindowView>().Forget();
        }

        public void OpenThemeMatrixWindow()
        {
            if (ActiveNavIndex.Value == 6)
            {
                return;
            }

            CloseAllBaseWindows();
            ActiveNavIndex.Value = 6;
            ActiveTitle.Value = "Dynamic Theme Switcher";
            FooterStatus.Value = "OPENED // THEME_MATRIX_WINDOW";
            _uiFlow.OpenAsync<ThemeMatrixWindowView>().Forget();
        }

        public void CloseBaseWindow()
        {
            ActiveNavIndex.Value = 0;
            ActiveTitle.Value = "[ NO_ACTIVE_VIEW // IDLE ]";
            FooterStatus.Value = "CLOSED // BASE_WINDOW";
            CloseAllBaseWindows();
        }

        public void CloseActiveWindow()
        {
            if (IsModalOpen.Value)
            {
                IsModalOpen.Value = false;
                FooterStatus.Value = "CLOSED // MODAL_OVERLAY";
                _uiFlow.Close<ModalDialogView>();
                return;
            }

            CloseBaseWindow();
        }

        public void CloseAllBaseWindows()
        {
            _uiFlow.Close<LifecycleWindowView>();
            _uiFlow.Close<ReactiveCounterView>();
            _uiFlow.Close<RecycledListWindowView>();
            _uiFlow.Close<InventoryWindowView>();
            _uiFlow.Close<ThemeMatrixWindowView>();
        }

        public void SetModalState(bool isOpen)
        {
            IsModalOpen.Value = isOpen;
        }
    }
}
