using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
	public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
	{
		private bool _hovering;

		private Slot _slot;
		private Transform _tooltipTransform;
		private InventoryManager _inventoryManager;

		private void Awake()
		{
			_slot = GetComponent<Slot>();
			_inventoryManager = GameManager.Instance.InventoryManager;
			_tooltipTransform = _inventoryManager.itemTooltipTransform;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (_slot.ItemCount == 0) return;

			_hovering = true;
			_tooltipTransform.gameObject.SetActive(true);
			_inventoryManager.itemTooltipText.text = _slot.Item.ItemTooltip();
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			if (_slot.ItemCount == 0 || !_hovering) return;

			_tooltipTransform.position = Input.mousePosition;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (_slot.ItemCount == 0) return;
            
			_tooltipTransform.gameObject.SetActive(false);
			_hovering = false;
		}
	}
}