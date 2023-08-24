using System.Collections.Generic;
using System.Linq;
using Inventories.Slots;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
	public class
		DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler,
			IEndDragHandler /*, IInitializePotentialDragHandler*/
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
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			foreach (var current in eventData.hovered.Where(current => current.name == "Slot"))
			{
				_startingSlot = current.GetComponent<Slot>();

				if (_startingSlot.IsEmpty)
				{
					_startingSlot = null;
					return;
				}

				_movingSlot.AddItems(new List<Item>(_startingSlot.Items));
				_movingObject.SetActive(true);
				return;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			_movingObject.transform.position = Input.mousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (_startingSlot == null) return;
			
			//If user is not hovering over any ui related to inventory, drop items
			if (eventData.hovered.Count == 0 || !eventData.hovered.Any(gObject => gObject.CompareTag("Inventory")))
			{
				_startingSlot.ClearSlot();
				_startingSlot = null;
				
				_inventoryManager.DropItems(_playerTransform, _movingSlot.Items);
				return;
			}

			var current = eventData.hovered.FirstOrDefault(gObject => gObject.CompareTag("Slot"));
			if (current == null || current == _startingSlot.gameObject)
			{
				_inventoryManager.ClearMovingSlot();
				return;
			}

			var currentSlot = current.GetComponent<Slot>();

			if (currentSlot.Peek == _startingSlot.Peek)
			{
				_inventoryManager.MergeStacks(_startingSlot, currentSlot);
			}
			else
			{
				_inventoryManager.SwapItems(_startingSlot, currentSlot);
			}

			_startingSlot = null;
			_inventoryManager.ClearMovingSlot();
		}

		/*public void OnInitializePotentialDrag(PointerEventData eventData)
		{
		    eventData.useDragThreshold = false;
		}*/
	}
}