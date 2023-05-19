using System.Collections.Generic;
using Core;
using Items;
using TMPro;
using UnityEngine;

namespace Inventories.Slots
{
	public readonly struct SlotGroup
	{
		public readonly List<Slot> Slots;
		public readonly Slot PivotSlot;
		
		private readonly TextMeshProUGUI _stackText;
		private readonly Inventory _inventory;
		private readonly GameObject _iconObject;
		private readonly RectTransform _backgroundRect;

		/// <summary>
		/// Container for items. Top left slot is the pivot that contains all item data. Bottom right manages stack text.
		/// </summary>
		/// <param name="slots">Slots collection</param>
		/// <param name="inventory">Inventory to which slots are assigned to</param>
		public SlotGroup(List<Slot> slots, Inventory inventory) : this()
		{
			Slots = slots;
			PivotSlot = Slots[0];
			_stackText = PivotSlot.stackText;
			
			for (int i = 1; i < slots.Count; i++)
			{
				Slots[i].backgroundObject.SetActive(false);
			}

			foreach (var slot in Slots)
			{
				slot.SlotGroup = this;
			}
			
			_inventory = inventory;
			_iconObject = PivotSlot.slotIcon.gameObject;
			_backgroundRect = PivotSlot.backgroundObject.GetComponent<RectTransform>();
		}

		public bool AddItem(Item item)
		{
			if (!PivotSlot.AddItem(item)) return false;
			
			if (PivotSlot.Items.Count == 1) CreateIcon(item.sizeRows - 1, item.sizeCols - 1);
			_stackText.text = $"{PivotSlot.Items.Count}";
			return true;
		}

		public List<Item> AddItems(List<Item> items)
		{
			while (AddItem(items[0]))
			{
				items.RemoveAt(0);
			}

			return items;
		}

		public bool RemoveItem()
		{
			if (!PivotSlot.RemoveItem()) return false;
			
			if (PivotSlot.IsEmpty)
			{
				for (int i = 1; i < Slots.Count; i++)
				{
					Slots[i].backgroundObject.SetActive(true);
				}
				
				_iconObject.SetActive(false);
				_stackText.gameObject.SetActive(false);
				_inventory.UsedSlots.Remove(this);
				
				for (int i = 1; i < Slots.Count; i++)
				{
					Slots[i].backgroundObject.SetActive(false);
					Slots[i].SlotGroup = new SlotGroup();
					Debug.Log(_inventory.UsedSlots.Remove(this));
				}
			}

			return true;
		}

		public int RemoveItems(int count)
		{
			while (RemoveItem())
			{
				count--;
			}

			return count;
		}

		private void CreateIcon(int xSize, int ySize)
		{
			_iconObject.SetActive(true);
			_stackText.gameObject.SetActive(true);
			
			PivotSlot.slotIcon.sprite = PivotSlot.Items[0].itemIcon;
			
			Metrics.SetSlotSize(_iconObject.GetComponent<RectTransform>(), _backgroundRect, xSize, ySize);
		}
	}
}