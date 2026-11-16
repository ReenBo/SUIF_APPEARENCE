using System;
using global::R3;
using ObservableCollections;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Services
{
    public interface ILifecycleMonitorService : IDisposable
    {
        ReadOnlyReactiveProperty<LifecycleViewDto> State { get; }
        ObservableList<LifecycleLogEntryDto> Logs { get; }
        void TriggerStage(LifecycleStageId stage);
    }

    public sealed class LifecycleMonitorService : ILifecycleMonitorService
    {
        private const int MaxLogs = 10;

        private readonly ReactiveProperty<LifecycleViewDto> _state;

        public ReadOnlyReactiveProperty<LifecycleViewDto> State => _state;
        public ObservableList<LifecycleLogEntryDto> Logs { get; } = new();

        public LifecycleMonitorService()
        {
            Logs.Add(new LifecycleLogEntryDto(0, 0, 2, 0, LifecycleStageId.Initialize, true));
            Logs.Add(new LifecycleLogEntryDto(0, 0, 5, 0, LifecycleStageId.Open, true));
            Logs.Add(new LifecycleLogEntryDto(0, 0, 9, 0, LifecycleStageId.Show, true));

            _state = new ReactiveProperty<LifecycleViewDto>(new LifecycleViewDto(LifecycleStageId.Initialize, Logs.Count));
        }

        public void TriggerStage(LifecycleStageId stage)
        {
            var now = DateTime.UtcNow;
            Logs.Insert(0, new LifecycleLogEntryDto(
                now.Hour,
                now.Minute,
                now.Second,
                now.Millisecond / 10,
                stage));

            if (Logs.Count > MaxLogs)
            {
                Logs.RemoveAt(Logs.Count - 1);
            }

            _state.Value = new LifecycleViewDto(stage, Logs.Count);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}
