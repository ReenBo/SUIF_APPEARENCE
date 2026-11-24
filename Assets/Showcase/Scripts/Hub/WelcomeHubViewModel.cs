using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Hub
{
    public sealed class WelcomeHubViewModel : BaseViewModel
    {
        private readonly IHubNavigationService _hubNavigation;

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<HubToastDto> ToastState => _hubNavigation.ToastState;
        public Observable<HubStyleType> OnStyleSelected => _hubNavigation.OnStyleSelected;

        public WelcomeHubViewModel(
            IHubNavigationService hubNavigation,
            IShowcaseTextFormatter formatter)
        {
            _hubNavigation = hubNavigation ?? throw new ArgumentNullException(nameof(hubNavigation));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public void OpenCyberPunk()
        {
            OpenStyleAfterTransitionAsync(HubStyleType.CyberPunkStyle).Forget();
        }

        public void OpenMacOSTahoe()
        {
            OpenStyleAfterTransitionAsync(HubStyleType.MacOSTahoe).Forget();
        }

        public void TriggerFuture1()
        {
            _hubNavigation.TriggerFutureSlot(FutureSlotId.Future1);
        }

        public void TriggerFuture2()
        {
            _hubNavigation.TriggerFutureSlot(FutureSlotId.Future2);
        }

        private async UniTaskVoid OpenStyleAfterTransitionAsync(HubStyleType style)
        {
            await UniTask.Delay(TimeSpan.FromMilliseconds(240), cancellationToken: CancellationToken.None);
            await _hubNavigation.OpenStyleAsync(style);
        }
    }
}
