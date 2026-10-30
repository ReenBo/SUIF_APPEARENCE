using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using SUIF.Common;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace SUIF.Showcase
{
    public sealed class ShowcaseEntryPoint : IAsyncStartable, IDisposable
    {
        private readonly IUIFlow _uiFlow;
        private readonly IUIAssetProvider _assetProvider;
        private readonly VisualTreeAsset _preloadedHubAsset;
        private readonly CancellationTokenSource _cts = new();

        public ShowcaseEntryPoint(IUIFlow uiFlow, IUIAssetProvider assetProvider, VisualTreeAsset preloadedHubAsset = null)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
            _preloadedHubAsset = preloadedHubAsset;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellation, _cts.Token);
            var ct = linked.Token;

            if (_preloadedHubAsset != null && _assetProvider is DirectAssetProvider directProvider)
            {
                directProvider.RegisterAsset("ShowcaseHubView", _preloadedHubAsset);
            }

            try
            {
                await _uiFlow.OpenAsync<ShowcaseHubView>(ct);
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
