using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Data;

namespace SUIF.Showcase.Services
{
    public interface IInventoryService : IShowcasePreloadable, IDisposable
    {
        InventoryItemDto[] Items { get; }
        ReadOnlyReactiveProperty<InventoryViewDto> State { get; }
        void SelectSlot(int index);
        InventoryItemDto GetCurrentItem();
    }

    public sealed class InventoryService : IInventoryService
    {
        private readonly ShowcaseInventoryCatalog _catalog;
        private readonly ReactiveProperty<InventoryViewDto> _state;

        private InventoryItemDto[] _items;
        private float _totalWeightKg;
        private bool _preloaded;

        public InventoryItemDto[] Items => _items;
        public ReadOnlyReactiveProperty<InventoryViewDto> State => _state;

        public InventoryService(ShowcaseInventoryCatalog catalog = null)
        {
            _catalog = catalog;
            _items = Array.Empty<InventoryItemDto>();
            _state = new ReactiveProperty<InventoryViewDto>(new InventoryViewDto(0, 0f));
            EnsureLoaded();
        }

        public UniTask PreloadAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            EnsureLoaded();
            return UniTask.CompletedTask;
        }

        public void SelectSlot(int index)
        {
            EnsureLoaded();
            if (index >= 0 && index < _items.Length)
            {
                _state.Value = new InventoryViewDto(index, _totalWeightKg);
            }
        }

        public InventoryItemDto GetCurrentItem()
        {
            EnsureLoaded();
            return _items[_state.Value.SelectedIndex % _items.Length];
        }

        private void EnsureLoaded()
        {
            if (_preloaded)
            {
                return;
            }

            _items = _catalog != null ? _catalog.CreateItems() : ShowcaseInventoryCatalog.CreateDefaultItems();

            float sum = 0f;
            for (int i = 0; i < _items.Length; i++)
            {
                sum += _items[i].WeightKg;
            }

            _totalWeightKg = sum;
            _preloaded = true;
            _state.Value = new InventoryViewDto(0, _totalWeightKg);
        }

        public void Dispose()
        {
            _state.Dispose();
        }
    }
}
