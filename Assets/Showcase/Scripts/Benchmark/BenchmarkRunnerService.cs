using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.Showcase.Frame;
using UnityEngine;
using VContainer.Unity;

namespace SUIF.Showcase.Benchmark
{
    public sealed class BenchmarkRunnerService : IAsyncStartable, IDisposable
    {
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly BenchmarkTelemetryService _telemetryService;
        private readonly CancellationTokenSource _cts = new();
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<bool> IsRunning { get; } = new(false);
        public ReactiveProperty<string> BenchmarkStatusText { get; } = new("BENCHMARK // IDLE");

        public BenchmarkRunnerService(
            ShowcaseFrameViewModel frameViewModel,
            BenchmarkTelemetryService telemetryService)
        {
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
            _telemetryService = telemetryService ?? throw new ArgumentNullException(nameof(telemetryService));

            _frameViewModel.BenchmarkRequested
                .Subscribe(_ => RunBenchmarkAsync().Forget())
                .AddTo(_disposables);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellation, _cts.Token);
            var ct = linked.Token;

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.5), cancellationToken: ct);

                string autoBenchmarkParam = WebGLBenchmarkBridge.GetQueryParameter("benchmark");
                string autoTestParam = WebGLBenchmarkBridge.GetQueryParameter("autotest");

                bool shouldRunAutoBenchmark = string.Equals(autoBenchmarkParam, "1", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(autoBenchmarkParam, "true", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(autoTestParam, "1", StringComparison.OrdinalIgnoreCase) ||
                                             string.Equals(autoTestParam, "true", StringComparison.OrdinalIgnoreCase);

                if (shouldRunAutoBenchmark)
                {
                    RunBenchmarkAsync(ct).Forget();
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public async UniTask<BenchmarkMetrics> RunBenchmarkAsync(CancellationToken cancellationToken = default)
        {
            if (IsRunning.Value)
            {
                return null;
            }

            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cts.Token);
            var ct = linked.Token;

            IsRunning.Value = true;
            BenchmarkStatusText.Value = "BENCHMARK // WARMUP";

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.8), cancellationToken: ct);

                _telemetryService.StartRecording();
                BenchmarkStatusText.Value = "BENCHMARK // RECORDING";

                BenchmarkStatusText.Value = "BENCHMARK // 01_LIFECYCLE";
                _frameViewModel.OpenLifecycleWindow();
                await UniTask.Delay(TimeSpan.FromSeconds(1.0), cancellationToken: ct);

                BenchmarkStatusText.Value = "BENCHMARK // 02_R3_COUNTER";
                _frameViewModel.OpenCounterWindow();
                await UniTask.Delay(TimeSpan.FromSeconds(1.0), cancellationToken: ct);

                BenchmarkStatusText.Value = "BENCHMARK // 03_RECYCLED_LIST";
                _frameViewModel.OpenRecycledListWindow();
                await UniTask.Delay(TimeSpan.FromSeconds(1.2), cancellationToken: ct);

                BenchmarkStatusText.Value = "BENCHMARK // 05_INVENTORY";
                _frameViewModel.OpenInventoryWindow();
                await UniTask.Delay(TimeSpan.FromSeconds(1.0), cancellationToken: ct);

                BenchmarkStatusText.Value = "BENCHMARK // 06_THEME_MATRIX";
                _frameViewModel.OpenThemeMatrixWindow();
                await UniTask.Delay(TimeSpan.FromSeconds(1.0), cancellationToken: ct);

                BenchmarkStatusText.Value = "BENCHMARK // 04_MODAL_TEST";
                _frameViewModel.ToggleModal();
                await UniTask.Delay(TimeSpan.FromSeconds(0.6), cancellationToken: ct);
                _frameViewModel.ToggleModal();
                await UniTask.Delay(TimeSpan.FromSeconds(0.4), cancellationToken: ct);

                _frameViewModel.ToggleModal();
                await UniTask.Delay(TimeSpan.FromSeconds(0.6), cancellationToken: ct);
                _frameViewModel.ToggleModal();
                await UniTask.Delay(TimeSpan.FromSeconds(0.4), cancellationToken: ct);

                BenchmarkStatusText.Value = "BENCHMARK // STANDBY";
                _frameViewModel.CloseBaseWindow();
                await UniTask.Delay(TimeSpan.FromSeconds(0.8), cancellationToken: ct);

                BenchmarkMetrics metrics = _telemetryService.StopRecording();
                string jsonReport = JsonUtility.ToJson(metrics, true);

                WebGLBenchmarkBridge.ReportComplete(jsonReport);
                BenchmarkStatusText.Value = $"BENCHMARK // DONE ({metrics.averageFps:F1} FPS)";

                return metrics;
            }
            finally
            {
                IsRunning.Value = false;
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _disposables.Dispose();
            IsRunning.Dispose();
            BenchmarkStatusText.Dispose();
        }
    }
}
