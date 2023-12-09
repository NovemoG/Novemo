using System.Linq;
using DG.Tweening;
using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
	[RequireComponent(typeof(EquipmentSlot))]
	public class EquipmentDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		private InventoryManager _inventoryManager;
		private EquipmentSlot _startingSlot;
		private Transform _playerTransform;
		private GameObject _movingObject;
		private Slot _movingSlot;

		private void Awake()
		{
			_inventoryManager = GameManager.Instance.InventoryManager;
			_playerTransform = GameManager.Instance.PlayerManager.playerObject.transform;
			_movingObject = _inventoryManager.movingSlotObject;
			_movingSlot = _inventoryManager.movingSlot;
			_startingSlot = GetComponent<EquipmentSlot>();
		}

		
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (_startingSlot.IsEmpty) return;

			_inventoryManager.SelectedSlotInventory = 3;
			_movingSlot.AddItem(_startingSlot.Item);
			_movingObject.SetActive(true);
		}

		public void OnDrag(PointerEventData eventData)
		{
			_movingObject.transform.position = Input.mousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_startingSlot.IsEmpty || _inventoryManager.movingSlot.IsEmpty) return;
			
			//If user is not hovering over any ui related to inventory, drop items
			if (eventData.hovered.Count == 0 || !eventData.hovered.Any(gObject => gObject.CompareTag("Inventory")))
			{
				_startingSlot.RemoveItem();
				
				_inventoryManager.DropItems(_playerTransform.position, _movingSlot.Item, _movingSlot.ItemCount);
				return;
			}

			var current = eventData.hovered.FirstOrDefault(gObject => gObject.CompareTag("Slot"));
			if (current == null || current == _startingSlot.gameObject)
			{
				_inventoryManager.ClearMovingSlot();
				return;
			}

			var currentSlot = current.GetComponent<Slot>();

			if (currentSlot.IsEmpty)
			{
				var tmpItem = _startingSlot.Item;
				
				_startingSlot.RemoveItem();
				currentSlot.AddItem(tmpItem);
			}
			else
			{
				if (!_startingSlot.CanAdd(currentSlot.Item))
				{
					_inventoryManager.ClearMovingSlot();
					return;
				}
				
				var tmpItem = _startingSlot.Item;
				
				_startingSlot.RemoveItem();
				_startingSlot.AddItem(currentSlot.Item);

				currentSlot.ClearSlot();
				if (tmpItem != null)
				{
					currentSlot.AddItem(tmpItem);
				}
			}
			
			_inventoryManager.ClearMovingSlot();
		}
	}
}