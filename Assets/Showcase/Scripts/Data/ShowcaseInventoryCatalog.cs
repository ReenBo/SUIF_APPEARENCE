using System;
using SUIF.Showcase.Contracts;
using UnityEngine;

namespace SUIF.Showcase.Data
{
    [Serializable]
    public struct InventoryItemEntry
    {
        public InventoryItemCode Code;
        public InventoryItemCategory Category;
        public float WeightKg;
        public InventoryRarity Rarity;
        public InventoryStatKind StatKind;
        public int StatValue;

        public InventoryItemDto ToDto()
        {
            return new InventoryItemDto(Code, Category, WeightKg, Rarity, StatKind, StatValue);
        }
    }

    [CreateAssetMenu(menuName = "SUIF Showcase/Inventory Catalog", fileName = "ShowcaseInventoryCatalog")]
    public sealed class ShowcaseInventoryCatalog : ScriptableObject
    {
        [SerializeField] private InventoryItemEntry[] _entries;

        public InventoryItemDto[] CreateItems()
        {
            if (_entries == null || _entries.Length == 0)
            {
                return CreateDefaultItems();
            }

            var result = new InventoryItemDto[_entries.Length];
            for (int i = 0; i < _entries.Length; i++)
            {
                result[i] = _entries[i].ToDto();
            }

            return result;
        }

        public static InventoryItemDto[] CreateDefaultItems()
        {
            return new InventoryItemDto[]
            {
                new(InventoryItemCode.Cyb01, InventoryItemCategory.Cyberware, 4.2f, InventoryRarity.Iconic, InventoryStatKind.Dps, 940),
                new(InventoryItemCode.Opt02, InventoryItemCategory.Optics, 0.4f, InventoryRarity.SpecOps, InventoryStatKind.Zoom, 16),
                new(InventoryItemCode.Deck03, InventoryItemCategory.Cyberdeck, 1.1f, InventoryRarity.Legendary, InventoryStatKind.Ram, 32),
                new(InventoryItemCode.Mod04, InventoryItemCategory.Chipware, 0.1f, InventoryRarity.Rare, InventoryStatKind.TickBonusPercent, 45),
                new(InventoryItemCode.Stim05, InventoryItemCategory.Consumable, 0.2f, InventoryRarity.Common, InventoryStatKind.HealPercent, 80),
                new(InventoryItemCode.Arm06, InventoryItemCategory.Armor, 6.8f, InventoryRarity.Heavy, InventoryStatKind.Armor, 320),
                new(InventoryItemCode.Emp07, InventoryItemCategory.Ordnance, 0.5f, InventoryRarity.Tactical, InventoryStatKind.Emp, 450),
                new(InventoryItemCode.Key08, InventoryItemCategory.KeyItem, 0.0f, InventoryRarity.Encrypted, InventoryStatKind.ClearanceLevel, 5)
            };
        }
    }
}
