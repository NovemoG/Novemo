using Characters.Player;
using Enums;
using Items;
using Items.Equipment;
using Managers;

namespace Inventories.Slots
{
	public class EquipmentSlot : Slot
	{
		public new bool IsFull => !IsEmpty;
		
		public EquipSlotType equipSlotType;

		private Player _playerInstance;
		private EquipmentManager _equipmentManager;

		protected override void Awake()
		{
			base.Awake();
			
			_equipmentManager = GameManager.Instance.EquipmentManager;
			_playerInstance = GameManager.Instance.PlayerManager.playerClass;
		}
		
		public override bool AddItem(Item itemToAdd)
		{
			if (IsFull) return false;
			if (itemToAdd is not Equipment equipment) return false;
			if (equipment.equipSlotType != equipSlotType) return false;

			itemCount = 1;
			item = equipment;
			
			slotIcon.gameObject.SetActive(true);
			slotIcon.sprite = item.itemIcon;
			
			_equipmentManager.EquipCharacterItem(_playerInstance, equipment);

			return true;
		}

		public bool CanAdd(Item cItem)
		{
			if (cItem is not Equipment equipment) return false;
			return equipment.equipSlotType == equipSlotType;
		}

		public override bool RemoveItem()
		{
			if (IsEmpty) return false;
			
			_equipmentManager.UnequipCharacterItem(_playerInstance, item as Equipment);
			
			slotIcon.gameObject.SetActive(false);
			itemCount = 0;
			item = null;

			return true;
		}
		
		/// <summary>
		/// It does nothing on equipment slot
		/// </summary>
		public override void ClearSlot() { }
		
		public override void SetActive() { }
		
		public override void ToggleBorder() { }
	}
}