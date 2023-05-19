using Characters.Player;
using Enums;
using Items.Equipment;
using Managers;

namespace Inventories
{
	public class EquipmentInventory : Inventory
	{
		public EquipSlotType inventoryType;
		
		private EquipmentManager _equipmentManager;
		private Player _playerClass;

		//TODO make slots transparent and create background corresponding to equip inventory
		
		protected override void Awake()
		{
			base.Awake();

			_equipmentManager = GameManager.Instance.EquipmentManager;
			_playerClass = GameManager.Instance.PlayerManager.playerClass;
		}

		protected override void Update()
		{
			
		}

		public bool EquipItem(Equipment item)
		{
			if (!AddItem(item)) return false;
			
			_equipmentManager.InvokeItemEquipped(_playerClass, item);
			return true;
		}

		public bool UnequipItem()
		{
			if (!RemoveItem(0, 0)) return false;
			
			_equipmentManager.InvokeItemUnequipped(_playerClass, new Equipment());
			return true;
		}
	}
}