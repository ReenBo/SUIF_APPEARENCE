using System;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using global::R3;
using ObservableCollections;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Windows.Modals;

namespace SUIF.Showcase.Windows.Lifecycle
{
    public sealed class LifecycleWindowViewModel : BaseViewModel
    {
        private readonly IUIFlow _uiFlow;

        public ReactiveProperty<string> LifecycleStage { get; } = new("OnShow");
        public ObservableList<string> LifecycleLogs { get; } = new();

        public LifecycleWindowViewModel(IUIFlow uiFlow)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));

            AddDisposable(LifecycleStage);

            LifecycleLogs.Add("[00:00.02] OnInitialize() -> Memory blocks pre-warmed");
            LifecycleLogs.Add("[00:00.05] OnOpen()       -> View hierarchy attached to SUIF Root");
            LifecycleLogs.Add("[00:00.09] OnShow()       -> Transition animation completed [ACTIVE]");
        }

        public void TriggerStage(string stageName)
        {
            LifecycleStage.Value = stageName;
            var timeStamp = DateTime.UtcNow.ToString("HH:mm:ss.ff");
            LifecycleLogs.Insert(0, ZString.Concat("[", timeStamp, "] ", stageName, "() -> State transition executed"));
            if (LifecycleLogs.Count > 10)
            {
                LifecycleLogs.RemoveAt(LifecycleLogs.Count - 1);
            }
        }

        public void OpenModal()
        {
            _uiFlow.OpenAsync<ModalDialogView>().Forget();
        }
    }
}

