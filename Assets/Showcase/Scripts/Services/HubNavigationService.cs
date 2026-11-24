using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Hub;
using SUIF.Showcase.Tahoe;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;
using SUIF.Showcase.Windows.ThemeMatrix;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace SUIF.Showcase.Services
{
    public interface IHubNavigationService : IDisposable
    {
        ReadOnlyReactiveProperty<HubScreenId> ActiveScreen { get; }
        ReadOnlyReactiveProperty<HubToastDto> ToastState { get; }
        Observable<HubStyleType> OnStyleSelected { get; }

        UniTask OpenHubAsync(CancellationToken ct = default);
        UniTask OpenStyleAsync(HubStyleType style, CancellationToken ct = default);
        UniTask ReturnToHubAsync(CancellationToken ct = default);
        void TriggerFutureSlot(FutureSlotId slot);
    }

    public sealed class HubNavigationService : IHubNavigationService, ITickable
    {
        private readonly IUIFlow _uiFlow;
        private readonly ReactiveProperty<HubScreenId> _activeScreen = new(HubScreenId.Hub);
        private readonly ReactiveProperty<HubToastDto> _toastState = new(new HubToastDto(false, FutureSlotId.Future1, 0));
        private readonly Subject<HubStyleType> _onStyleSelected = new();

        private CancellationTokenSource _toastCts;
        private int _toastTriggerCounter;
        private bool _isTransitioning;
        private bool _disposed;

        public ReadOnlyReactiveProperty<HubScreenId> ActiveScreen => _activeScreen;
        public ReadOnlyReactiveProperty<HubToastDto> ToastState => _toastState;
        public Observable<HubStyleType> OnStyleSelected => _onStyleSelected;

        public HubNavigationService(IUIFlow uiFlow)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
        }

        public async UniTask OpenHubAsync(CancellationToken ct = default)
        {
            if (_disposed)
            {
                return;
            }

            _activeScreen.Value = HubScreenId.Hub;
            await _uiFlow.OpenAsync<WelcomeHubView>(ct);
        }

        public async UniTask OpenStyleAsync(HubStyleType style, CancellationToken ct = default)
        {
            if (_disposed || _isTransitioning)
            {
                return;
            }

            _isTransitioning = true;
            try
            {
                _onStyleSelected.OnNext(style);
                HideToastImmediate();

                if (style == HubStyleType.CyberPunkStyle)
                {
                    await _uiFlow.OpenAsync<ShowcaseFrameView>(ct);
                    _uiFlow.Close<WelcomeHubView>();
                    _activeScreen.Value = HubScreenId.CyberPunkStyle;
                }
                else
                {
                    await _uiFlow.OpenAsync<MacOSTahoeView>(ct);
                    _uiFlow.Close<WelcomeHubView>();
                    _activeScreen.Value = HubScreenId.MacOSTahoe;
                }
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public async UniTask ReturnToHubAsync(CancellationToken ct = default)
        {
            if (_disposed || _isTransitioning || _activeScreen.Value == HubScreenId.Hub)
            {
                return;
            }

            _isTransitioning = true;
            try
            {
                CloseCyberPunkHierarchy();
                _uiFlow.Close<MacOSTahoeView>();
                _activeScreen.Value = HubScreenId.Hub;
                await _uiFlow.OpenAsync<WelcomeHubView>(ct);
            }
            finally
            {
                _isTransitioning = false;
            }
        }

        public void TriggerFutureSlot(FutureSlotId slot)
        {
            if (_disposed)
            {
                return;
            }

            _toastCts?.Cancel();
            _toastCts?.Dispose();
            _toastCts = new CancellationTokenSource();

            _toastTriggerCounter++;
            _toastState.Value = new HubToastDto(true, slot, _toastTriggerCounter);
            AutoDismissToastAsync(slot, _toastTriggerCounter, _toastCts.Token).Forget();
        }

        private async UniTaskVoid AutoDismissToastAsync(FutureSlotId slot, int triggerId, CancellationToken ct)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(2.2), cancellationToken: ct);
                if (!_disposed && _toastState.Value.TriggerId == triggerId)
                {
                    _toastState.Value = new HubToastDto(false, slot, triggerId);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void HideToastImmediate()
        {
            _toastCts?.Cancel();
            var current = _toastState.Value;
            if (current.IsVisible)
            {
                _toastState.Value = new HubToastDto(false, current.Slot, current.TriggerId);
            }
        }

        private void CloseCyberPunkHierarchy()
        {
            _uiFlow.Close<ModalDialogView>();
            _uiFlow.Close<LifecycleWindowView>();
            _uiFlow.Close<ReactiveCounterView>();
            _uiFlow.Close<RecycledListWindowView>();
            _uiFlow.Close<InventoryWindowView>();
            _uiFlow.Close<ThemeMatrixWindowView>();
            _uiFlow.Close<ShowcaseFrameView>();
        }

        public void Tick()
        {
            if (_disposed || _isTransitioning || _activeScreen.Value == HubScreenId.Hub)
            {
                return;
            }

            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                ReturnToHubAsync(CancellationToken.None).Forget();
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _toastCts?.Cancel();
            _toastCts?.Dispose();
            _activeScreen.Dispose();
            _toastState.Dispose();
            _onStyleSelected.Dispose();
        }
    }
}
