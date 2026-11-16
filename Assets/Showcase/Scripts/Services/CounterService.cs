using System;
using global::R3;
using ObservableCollections;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Services
{
    public interface ICounterService : IDisposable
    {
        ReadOnlyReactiveProperty<CounterViewDto> State { get; }
        ObservableList<int> History { get; }
        void UpdateCounter(int delta);
        void ResetCounter();
    }

    public sealed class CounterService : ICounterService
    {
        private const int MaxHistory = 7;

        private readonly ReactiveProperty<CounterViewDto> _state;

        public ReadOnlyReactiveProperty<CounterViewDto> State => _state;
        public ObservableList<int> History { get; } = new();

        public CounterService()
        {
            History.Add(38);
            History.Add(39);
            History.Add(40);
            History.Add(41);
            History.Add(42);

            _state = new ReactiveProperty<CounterViewDto>(new CounterViewDto(42, History.Count));
        }

        public void UpdateCounter(int delta)
        {
            int nextValue = _state.Value.CurrentValue + delta;
            History.Add(nextValue);
            if (History.Count > MaxHistory)
            {
                History.RemoveAt(0);
            }

            _state.Value = new CounterViewDto(nextValue, History.Count);
        }

        public void ResetCounter()
        {
            History.Clear();
            History.Add(0);
            _state.Value = new CounterViewDto(0, History.Count);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}
