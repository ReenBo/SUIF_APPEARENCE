using System;
using global::R3;
using ObservableCollections;
using SUIF.Base;
using SUIF.Showcase.Frame;

namespace SUIF.Showcase.Windows.Counter
{
    public sealed class ReactiveCounterViewModel : BaseViewModel
    {
        private readonly ShowcaseFrameViewModel _frameViewModel;

        public ReactiveProperty<int> CounterValue { get; } = new(42);
        public ObservableList<int> CounterHistory { get; } = new();

        public ReactiveCounterViewModel(ShowcaseFrameViewModel frameViewModel)
        {
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));

            AddDisposable(CounterValue);

            CounterHistory.Add(38);
            CounterHistory.Add(39);
            CounterHistory.Add(40);
            CounterHistory.Add(41);
            CounterHistory.Add(42);
        }

        public void UpdateCounter(int delta)
        {
            CounterValue.Value += delta;
            CounterHistory.Add(CounterValue.Value);
            if (CounterHistory.Count > 7)
            {
                CounterHistory.RemoveAt(0);
            }
        }

        public void ResetCounter()
        {
            CounterValue.Value = 0;
            CounterHistory.Clear();
            CounterHistory.Add(0);
        }

        public void Close()
        {
            _frameViewModel.CloseActiveWindow();
        }
    }
}

