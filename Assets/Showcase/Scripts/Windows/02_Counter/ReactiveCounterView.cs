using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Counter
{
    [UIView("ReactiveCounterView", SUIF.API.UILayer.Windows)]
    public sealed class ReactiveCounterView : BaseView<ReactiveCounterViewModel>
    {
        private const string ActiveClass = "is-active";

        [UQuery("label-counter-value")] private Label _labelValue;
        [UQuery("btn-counter-dec")] private Button _btnDec;
        [UQuery("btn-counter-inc")] private Button _btnInc;
        [UQuery("btn-counter-boost")] private Button _btnBoost;
        [UQuery("btn-counter-reset")] private Button _btnReset;
        [UQuery("counter-marbles-container")] private VisualElement _marblesContainer;

        public ReactiveCounterView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnDec != null) Binder.Bind(_btnDec).ToClick(() => ViewModel.UpdateCounter(-1));
            if (_btnInc != null) Binder.Bind(_btnInc).ToClick(() => ViewModel.UpdateCounter(1));
            if (_btnBoost != null) Binder.Bind(_btnBoost).ToClick(() => ViewModel.UpdateCounter(10));
            if (_btnReset != null) Binder.Bind(_btnReset).ToClick(ViewModel.ResetCounter);

            AddDisposable(ViewModel.CounterValue.Subscribe(OnValueChanged));
            RefreshMarbles();
        }

        private void OnValueChanged(int value)
        {
            if (_labelValue != null)
            {
                _labelValue.text = value.ToString("D3");
            }
            RefreshMarbles();
        }

        private void RefreshMarbles()
        {
            if (_marblesContainer == null) return;
            _marblesContainer.Clear();
            for (var i = 0; i < ViewModel.CounterHistory.Count; i++)
            {
                var val = ViewModel.CounterHistory[i];
                var pill = new Label(val.ToString());
                pill.AddToClassList("fui-marble-pill");
                if (i == ViewModel.CounterHistory.Count - 1)
                {
                    pill.AddToClassList(ActiveClass);
                }
                _marblesContainer.Add(pill);

                if (i < ViewModel.CounterHistory.Count - 1)
                {
                    var arrow = new Label("->");
                    arrow.AddToClassList("fui-marble-arrow");
                    _marblesContainer.Add(arrow);
                }
            }
        }
    }
}

