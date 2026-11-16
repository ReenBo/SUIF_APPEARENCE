using System;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Benchmark;
using SUIF.Showcase.Common;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
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
        private readonly ReactiveProperty<FrameTelemetryDto> _fallbackTelemetry =
            new(new FrameTelemetryDto(60f, 0, 0, 0f, false));

        public ReactiveProperty<FrameNavigationDto> Navigation { get; } =
            new(new FrameNavigationDto(ShowcaseModuleId.Standby, false, FrameActionStatus.Ready));

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<ShowcaseThemeDto> CurrentTheme => _themeService.CurrentTheme;
        public ReadOnlyReactiveProperty<FrameTelemetryDto> Telemetry =>
            _telemetryService != null ? _telemetryService.Telemetry : _fallbackTelemetry;
        public Observable<Unit> BenchmarkRequested => _benchmarkRequestedSubject;

        public ShowcaseFrameViewModel(
            IUIFlow uiFlow,
            ShowcaseThemeService themeService,
            IShowcaseTextFormatter formatter,
            BenchmarkTelemetryService telemetryService = null)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _telemetryService = telemetryService;

            AddDisposable(Navigation);
            AddDisposable(_benchmarkRequestedSubject);
            AddDisposable(_fallbackTelemetry);
        }

        public void TriggerBenchmark()
        {
            _benchmarkRequestedSubject.OnNext(Unit.Default);
        }

        public void OpenLifecycleWindow()
        {
            OpenBaseModule<LifecycleWindowView>(ShowcaseModuleId.Lifecycle);
        }

        public void OpenCounterWindow()
        {
            OpenBaseModule<ReactiveCounterView>(ShowcaseModuleId.Counter);
        }

        public void OpenRecycledListWindow()
        {
            OpenBaseModule<RecycledListWindowView>(ShowcaseModuleId.RecycledList);
        }

        public void OpenInventoryWindow()
        {
            OpenBaseModule<InventoryWindowView>(ShowcaseModuleId.Inventory);
        }

        public void OpenThemeMatrixWindow()
        {
            OpenBaseModule<ThemeMatrixWindowView>(ShowcaseModuleId.ThemeMatrix);
        }

        public void ToggleModal()
        {
            var current = Navigation.Value;
            if (current.IsModalOpen)
            {
                Navigation.Value = new FrameNavigationDto(current.ActiveModule, false, FrameActionStatus.ModalUnmounted);
                _uiFlow.Close<ModalDialogView>();
                return;
            }

            Navigation.Value = new FrameNavigationDto(current.ActiveModule, true, FrameActionStatus.ModalMounted);
            _uiFlow.OpenAsync<ModalDialogView>().Forget();
        }

        public void CloseBaseWindow()
        {
            var current = Navigation.Value;
            Navigation.Value = new FrameNavigationDto(ShowcaseModuleId.Standby, current.IsModalOpen, FrameActionStatus.ModuleClosed);
            CloseAllBaseWindows();
        }

        public void CloseActiveWindow()
        {
            var current = Navigation.Value;
            if (current.IsModalOpen)
            {
                Navigation.Value = new FrameNavigationDto(current.ActiveModule, false, FrameActionStatus.ModalUnmounted);
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
            var current = Navigation.Value;
            if (current.IsModalOpen == isOpen)
            {
                return;
            }

            Navigation.Value = new FrameNavigationDto(
                current.ActiveModule,
                isOpen,
                isOpen ? FrameActionStatus.ModalMounted : FrameActionStatus.ModalUnmounted);
        }

        private void OpenBaseModule<TView>(ShowcaseModuleId module) where TView : class, IView
        {
            var current = Navigation.Value;
            if (current.ActiveModule == module)
            {
                return;
            }

            CloseAllBaseWindows();
            Navigation.Value = new FrameNavigationDto(module, current.IsModalOpen, FrameActionStatus.ModuleOpened);
            _uiFlow.OpenAsync<TView>().Forget();
        }
    }
}
