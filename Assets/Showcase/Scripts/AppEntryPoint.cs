using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;
using UnityEngine;
using VContainer.Unity;

namespace SUIF.Showcase
{
    public sealed class AppEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IUIFlow _uiFlow;
        private readonly IShowcaseDataLoader _dataLoader;
        private readonly CancellationTokenSource _cts = new();

        public AppEntryPoint(IUIFlow uiFlow, IShowcaseDataLoader dataLoader)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _dataLoader = dataLoader ?? throw new ArgumentNullException(nameof(dataLoader));
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellation, _cts.Token);
            var ct = linked.Token;

            try
            {
                await UnityEngine.AddressableAssets.Addressables.InitializeAsync().WithCancellation(ct);
                await _uiFlow.OpenAsync<ShowcaseFrameView>(ct);
                await _dataLoader.PreloadAllAsync(ct);
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

