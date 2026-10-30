using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using UnityEngine;
using VContainer.Unity;

namespace SUIF.Showcase
{
    public sealed class AppEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IUIFlow _uiFlow;
        private readonly CancellationTokenSource _cts = new();

        public AppEntryPoint(IUIFlow uiFlow)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellation, _cts.Token);
            var ct = linked.Token;

            try
            {
                await UnityEngine.AddressableAssets.Addressables.InitializeAsync().WithCancellation(ct);
                await _uiFlow.OpenAsync<ShowcaseFrameView>(ct);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}

