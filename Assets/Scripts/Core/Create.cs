using Enums;
using Items;
using Items.Equipment;
using StatusEffects;
using StatusEffects.Buffs;

namespace Core
{
	public static class Create
	{
		public static Item ItemInstance(ItemData itemData)
		{
			Item item;
			
			switch (itemData.itemType)
			{
				case ItemType.Item:
					item = new Item(itemData);
					break;
				case ItemType.Equipment:
					if (itemData is EquipmentData equipmentData)
					{
						item = new Equipment(equipmentData);
						break;
					}

					item = new Equipment(itemData);
					break;
				case ItemType.Consumable:
					item = new Item(itemData);
					break;
				default:
					item = new Item(itemData);
					break;
			}
			
			item.GenerateTooltip();
			return item;
		}

		public static StatusEffect EffectInstance(EffectData effectData)
		{
			return effectData.effectType switch
			{
				EffectType.HealthRegen => new HealthRegen(effectData),
				EffectType.ManaRegen => new ManaRegen(effectData),
				_ => new StatusEffect(effectData)
			};
		}
	}
}