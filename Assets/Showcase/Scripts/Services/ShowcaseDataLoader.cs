using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Showcase.Common;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Services
{
    public interface IShowcaseDataLoader : IDisposable
    {
        ReadOnlyReactiveProperty<LoadingStateDto> State { get; }
        UniTask PreloadAllAsync(CancellationToken ct = default);
        void StepSpinner();
    }

    public sealed class ShowcaseDataLoader : IShowcaseDataLoader
    {
        private readonly IUIFlow _uiFlow;
        private readonly IReadOnlyList<IShowcasePreloadable> _preloadables;
        private readonly UniTask[] _taskBuffer;
        private readonly ReactiveProperty<LoadingStateDto> _state;

        public ReadOnlyReactiveProperty<LoadingStateDto> State => _state;

        public ShowcaseDataLoader(IUIFlow uiFlow, IReadOnlyList<IShowcasePreloadable> preloadables)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _preloadables = preloadables ?? Array.Empty<IShowcasePreloadable>();
            _taskBuffer = _preloadables.Count > 0 ? new UniTask[_preloadables.Count] : Array.Empty<UniTask>();
            _state = new ReactiveProperty<LoadingStateDto>(new LoadingStateDto(LoadingPhase.Idle, 0, _preloadables.Count, 0));
        }

        public async UniTask PreloadAllAsync(CancellationToken ct = default)
        {
            int total = _preloadables.Count;
            if (total == 0)
            {
                _state.Value = new LoadingStateDto(LoadingPhase.Completed, 0, 0, 0);
                return;
            }

            _state.Value = new LoadingStateDto(LoadingPhase.Preloading, 0, total, 0);
            await _uiFlow.OpenAsync<LoadingSpinnerView>(ct);

            try
            {
                for (int i = 0; i < total; i++)
                {
                    _taskBuffer[i] = RunTrackedAsync(_preloadables[i], ct);
                }

                await UniTask.WhenAll(_taskBuffer);
                _state.Value = new LoadingStateDto(LoadingPhase.Completed, total, total, _state.Value.SpinnerStep + 1);
            }
            finally
            {
                _uiFlow.Close<LoadingSpinnerView>();
            }
        }

        public void StepSpinner()
        {
            var current = _state.Value;
            if (current.Phase != LoadingPhase.Preloading)
            {
                return;
            }

            _state.Value = new LoadingStateDto(current.Phase, current.CompletedTasks, current.TotalTasks, current.SpinnerStep + 1);
        }

        private async UniTask RunTrackedAsync(IShowcasePreloadable preloadable, CancellationToken ct)
        {
            await preloadable.PreloadAsync(ct);
            var current = _state.Value;
            int nextCompleted = current.CompletedTasks + 1;
            if (nextCompleted > current.TotalTasks)
            {
                nextCompleted = current.TotalTasks;
            }

            _state.Value = new LoadingStateDto(current.Phase, nextCompleted, current.TotalTasks, current.SpinnerStep + 1);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}
