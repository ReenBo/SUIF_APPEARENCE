using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Services
{
    public interface ITahoeDesktopService : IDisposable
    {
        ReadOnlyReactiveProperty<TahoeDesktopStateDto> State { get; }
        void OpenOrFocusWindow(TahoeWindowId windowId);
        void CloseWindow(TahoeWindowId windowId);
        void OpenAllWindows();
        void CloseAllWindows();
        void ToggleModalSheet(bool show);
        void SelectInventory100(int index);
        void SetTheme(TahoeThemeId themeId);
        UniTask RunBenchmarkAsync(CancellationToken ct = default);
    }

    public sealed class TahoeDesktopService : ITahoeDesktopService
    {
        private const int AllWindowsMask = (1 << 7) - 1;

        private readonly ICounterService _counterService;
        private readonly ReactiveProperty<TahoeDesktopStateDto> _state;

        private CancellationTokenSource _benchmarkCts;
        private bool _disposed;

        public ReadOnlyReactiveProperty<TahoeDesktopStateDto> State => _state;

        public TahoeDesktopService(ICounterService counterService)
        {
            _counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));

            int initialMask = 1 << (int)TahoeWindowId.RecycledList;
            _state = new ReactiveProperty<TahoeDesktopStateDto>(
                new TahoeDesktopStateDto(
                    TahoeThemeId.Dark,
                    true,
                    TahoeWindowId.RecycledList,
                    initialMask,
                    false,
                    false,
                    false,
                    0,
                    0,
                    0));
        }

        public void OpenOrFocusWindow(TahoeWindowId windowId)
        {
            var current = _state.Value;
            int nextMask = current.OpenMask | (1 << (int)windowId);
            _state.Value = new TahoeDesktopStateDto(
                current.Theme,
                true,
                windowId,
                nextMask,
                current.IsModalSheetOpen,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                current.Inventory100SelectedIndex,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public void CloseWindow(TahoeWindowId windowId)
        {
            var current = _state.Value;
            int nextMask = current.OpenMask & ~(1 << (int)windowId);
            bool hasFocused = current.HasFocusedWindow;
            TahoeWindowId nextFocus = current.FocusedWindow;

            if (current.HasFocusedWindow && current.FocusedWindow == windowId)
            {
                hasFocused = TryFindFallbackWindow(nextMask, out nextFocus);
            }

            _state.Value = new TahoeDesktopStateDto(
                current.Theme,
                hasFocused,
                nextFocus,
                nextMask,
                current.IsModalSheetOpen,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                current.Inventory100SelectedIndex,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public void OpenAllWindows()
        {
            var current = _state.Value;
            _state.Value = new TahoeDesktopStateDto(
                current.Theme,
                true,
                TahoeWindowId.Themes,
                AllWindowsMask,
                current.IsModalSheetOpen,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                current.Inventory100SelectedIndex,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public void CloseAllWindows()
        {
            var current = _state.Value;
            _state.Value = new TahoeDesktopStateDto(
                current.Theme,
                false,
                TahoeWindowId.Lifecycle,
                0,
                false,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                current.Inventory100SelectedIndex,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public void ToggleModalSheet(bool show)
        {
            var current = _state.Value;
            _state.Value = new TahoeDesktopStateDto(
                current.Theme,
                current.HasFocusedWindow,
                current.FocusedWindow,
                current.OpenMask,
                show,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                current.Inventory100SelectedIndex,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public void SelectInventory100(int index)
        {
            if (index < 0 || index >= 100)
            {
                return;
            }

            var current = _state.Value;
            _state.Value = new TahoeDesktopStateDto(
                current.Theme,
                current.HasFocusedWindow,
                current.FocusedWindow,
                current.OpenMask,
                current.IsModalSheetOpen,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                index,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public void SetTheme(TahoeThemeId themeId)
        {
            var current = _state.Value;
            _state.Value = new TahoeDesktopStateDto(
                themeId,
                current.HasFocusedWindow,
                current.FocusedWindow,
                current.OpenMask,
                current.IsModalSheetOpen,
                current.IsBenchmarkRunning,
                current.IsBenchmarkCompleted,
                current.Inventory100SelectedIndex,
                current.ListScrollStep,
                current.Inv100ScrollStep);
        }

        public async UniTask RunBenchmarkAsync(CancellationToken ct = default)
        {
            if (_disposed || _state.Value.IsBenchmarkRunning)
            {
                return;
            }

            _benchmarkCts?.Cancel();
            _benchmarkCts?.Dispose();
            _benchmarkCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var linkedCt = _benchmarkCts.Token;

            var startState = _state.Value;
            _state.Value = new TahoeDesktopStateDto(
                startState.Theme,
                true,
                TahoeWindowId.Themes,
                AllWindowsMask,
                startState.IsModalSheetOpen,
                true,
                false,
                startState.Inventory100SelectedIndex,
                0,
                0);

            try
            {
                for (int step = 1; step <= 6; step++)
                {
                    await UniTask.Delay(TimeSpan.FromMilliseconds(140), cancellationToken: linkedCt);
                    _counterService.UpdateCounter(12);

                    var current = _state.Value;
                    _state.Value = new TahoeDesktopStateDto(
                        current.Theme,
                        current.HasFocusedWindow,
                        current.FocusedWindow,
                        current.OpenMask,
                        current.IsModalSheetOpen,
                        true,
                        false,
                        (step * 7) % 100,
                        step,
                        step);
                }

                var doneState = _state.Value;
                _state.Value = new TahoeDesktopStateDto(
                    doneState.Theme,
                    doneState.HasFocusedWindow,
                    doneState.FocusedWindow,
                    doneState.OpenMask,
                    doneState.IsModalSheetOpen,
                    false,
                    true,
                    doneState.Inventory100SelectedIndex,
                    doneState.ListScrollStep,
                    doneState.Inv100ScrollStep);
            }
            catch (OperationCanceledException)
            {
                var canceled = _state.Value;
                _state.Value = new TahoeDesktopStateDto(
                    canceled.Theme,
                    canceled.HasFocusedWindow,
                    canceled.FocusedWindow,
                    canceled.OpenMask,
                    canceled.IsModalSheetOpen,
                    false,
                    canceled.IsBenchmarkCompleted,
                    canceled.Inventory100SelectedIndex,
                    canceled.ListScrollStep,
                    canceled.Inv100ScrollStep);
            }
        }

        private static bool TryFindFallbackWindow(int mask, out TahoeWindowId windowId)
        {
            for (int i = 6; i >= 0; i--)
            {
                if ((mask & (1 << i)) != 0)
                {
                    windowId = (TahoeWindowId)i;
                    return true;
                }
            }

            windowId = TahoeWindowId.Lifecycle;
            return false;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _benchmarkCts?.Cancel();
            _benchmarkCts?.Dispose();
            _state.Dispose();
        }
    }
}
