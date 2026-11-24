using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.Base;
using SUIF.Showcase.Benchmark;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Tahoe
{
    public sealed class MacOSTahoeViewModel : BaseViewModel
    {
        private readonly ITahoeDesktopService _desktopService;
        private readonly IHubNavigationService _hubNavigation;
        private readonly ILifecycleMonitorService _lifecycleService;
        private readonly ICounterService _counterService;
        private readonly IInventoryService _inventoryService;
        private readonly BenchmarkTelemetryService _telemetryService;
        private readonly ReactiveProperty<FrameTelemetryDto> _fallbackTelemetry =
            new(new FrameTelemetryDto(60f, 0, 0, 0f, false));

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<TahoeDesktopStateDto> DesktopState => _desktopService.State;
        public ReadOnlyReactiveProperty<LifecycleViewDto> LifecycleState => _lifecycleService.State;
        public ReadOnlyReactiveProperty<CounterViewDto> CounterState => _counterService.State;
        public ReadOnlyReactiveProperty<InventoryViewDto> Inventory6State => _inventoryService.State;
        public InventoryItemDto[] Inventory6Items => _inventoryService.Items;
        public ReadOnlyReactiveProperty<FrameTelemetryDto> Telemetry =>
            _telemetryService != null ? _telemetryService.Telemetry : _fallbackTelemetry;

        public MacOSTahoeViewModel(
            ITahoeDesktopService desktopService,
            IHubNavigationService hubNavigation,
            ILifecycleMonitorService lifecycleService,
            ICounterService counterService,
            IInventoryService inventoryService,
            IShowcaseTextFormatter formatter,
            BenchmarkTelemetryService telemetryService = null)
        {
            _desktopService = desktopService ?? throw new ArgumentNullException(nameof(desktopService));
            _hubNavigation = hubNavigation ?? throw new ArgumentNullException(nameof(hubNavigation));
            _lifecycleService = lifecycleService ?? throw new ArgumentNullException(nameof(lifecycleService));
            _counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _telemetryService = telemetryService;

            AddDisposable(_fallbackTelemetry);
        }

        public void ReturnToHub()
        {
            _hubNavigation.ReturnToHubAsync(CancellationToken.None).Forget();
        }

        public void OpenOrFocusWindow(TahoeWindowId windowId)
        {
            _desktopService.OpenOrFocusWindow(windowId);
        }

        public void CloseWindow(TahoeWindowId windowId)
        {
            _desktopService.CloseWindow(windowId);
        }

        public void OpenAllWindows()
        {
            _desktopService.OpenAllWindows();
        }

        public void CloseAllWindows()
        {
            _desktopService.CloseAllWindows();
        }

        public void RunBenchmark()
        {
            _desktopService.RunBenchmarkAsync(CancellationToken.None).Forget();
        }

        public void TriggerLifecycle(LifecycleStageId stage)
        {
            _lifecycleService.TriggerStage(stage);
        }

        public void StepCounter(int delta)
        {
            _counterService.UpdateCounter(delta);
        }

        public void ResetCounter()
        {
            _counterService.ResetCounter();
        }

        public void ToggleModalSheet(bool show)
        {
            _desktopService.ToggleModalSheet(show);
        }

        public void SelectInv6(int index)
        {
            _inventoryService.SelectSlot(index);
        }

        public void SelectInv100(int index)
        {
            _desktopService.SelectInventory100(index);
        }

        public void SetTheme(TahoeThemeId themeId)
        {
            _desktopService.SetTheme(themeId);
        }
    }
}
