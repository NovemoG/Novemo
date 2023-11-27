using System.Linq;
using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
	[RequireComponent(typeof(Slot))]
	public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler/*, IInitializePotentialDragHandler*/
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
			
			_movingSlot.AddItems(_startingSlot.Item,_startingSlot.ItemCount);
			_movingObject.SetActive(true);
		}

		public void OnDrag(PointerEventData eventData)
		{
			_movingObject.transform.position = Input.mousePosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			//If user is not hovering over any ui related to inventory, drop items
			if (eventData.hovered.Count == 0 || !eventData.hovered.Any(gObject => gObject.CompareTag("Inventory")))
			{
				_startingSlot.ClearSlot();
				
				_inventoryManager.DropItems(_playerTransform, _movingSlot.Item, _movingSlot.ItemCount);
				return;
			}

			var current = eventData.hovered.FirstOrDefault(gObject => gObject.CompareTag("Slot"));
			if (current == null || current == _startingSlot.gameObject)
			{
				_inventoryManager.ClearMovingSlot();
				return;
			}

			var currentSlot = current.GetComponent<Slot>();

			if (currentSlot.Item == _startingSlot.Item)
			{
				_inventoryManager.MergeStacks(_startingSlot, currentSlot);
			}
			else
			{
				_inventoryManager.SwapItems(_startingSlot, currentSlot);
			}
			
			_inventoryManager.ClearMovingSlot();
		}

		/*public void OnInitializePotentialDrag(PointerEventData eventData)
		{
		    eventData.useDragThreshold = false;
		}*/
	}
}