using System.Linq;
using DG.Tweening;
using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
	[RequireComponent(typeof(Slot))]
	public class SlotDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		private InventoryManager _inventoryManager;
		private Transform _playerTransform;
		private GameObject _movingObject;
		private Slot _startingSlot;
		private Slot _movingSlot;

		private void Awake()
		{
			_inventoryManager = GameManager.Instance.InventoryManager;
			_playerTransform = GameManager.Instance.PlayerManager.playerObject.transform;
			_movingObject = _inventoryManager.movingSlotObject;
			_movingSlot = _inventoryManager.movingSlot;
			_startingSlot = GetComponent<Slot>();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (_startingSlot.IsEmpty) return;
			
			_movingSlot.AddItems(_startingSlot.Item, _startingSlot.ItemCount);
			_startingSlot.ToggleComponent.isOn = true;
			_movingObject.SetActive(true);
		}

		public void OnDrag(PointerEventData eventData)
		{
			_movingObject.transform.position = Input.mousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_startingSlot.IsEmpty || _inventoryManager.movingSlot.IsEmpty) return;
			if (_inventoryManager.SelectedSlotInventory != _startingSlot.ParentInventoryId) return;
			
			//If user is not hovering over any ui related to inventory, drop items
			if (eventData.hovered.Count == 0 || !eventData.hovered.Any(gObject => gObject.CompareTag("Inventory")))
			{
				_startingSlot.ClearSlot();
				
				_startingSlot.ToggleComponent.interactable = false;
				_startingSlot.ToggleComponent.interactable = true;
				
				_inventoryManager.DropItems(_playerTransform.position, _movingSlot.Item, _movingSlot.ItemCount);
				return;
			}

			var current = eventData.hovered.FirstOrDefault(gObject => gObject.CompareTag("Slot"));
			if (current == null || current == _startingSlot.gameObject)
			{
				ClearMovingSlot();
				return;
			}

			var currentSlot = current.GetComponent<Slot>();
			
			if (currentSlot.ParentInventoryId == 3)
			{
				var equipmentSlot = current.GetComponent<EquipmentSlot>();

				if (!equipmentSlot.CanAdd(_startingSlot.Item))
				{
					ClearMovingSlot();
					return;
				}

				var tmpItem = equipmentSlot.Item;
				
				equipmentSlot.RemoveItem();
				equipmentSlot.AddItem(_startingSlot.Item);

				_startingSlot.ClearSlot();
				if (tmpItem != null)
				{
					_startingSlot.AddItem(tmpItem);
				}

				ClearMovingSlot();
				return;
			}
			
			if (currentSlot.Item == _startingSlot.Item)
			{
				_inventoryManager.MergeStacks(_startingSlot, currentSlot);
			}
			else
			{
				_inventoryManager.SwapItems(_startingSlot, currentSlot);
			}

			DOTween.Kill(6, true);
			DOTween.Kill(7, true);
			
			ClearMovingSlot();
		}

		private void ClearMovingSlot()
		{
			_startingSlot.ToggleComponent.interactable = false;
			_startingSlot.ToggleComponent.interactable = true;
			_inventoryManager.ClearMovingSlot();
		}
	}
}