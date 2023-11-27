using Inventories.Slots;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
	public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
	{
		private Slot _slot;
		private RectTransform _tooltipTransform;
		private InventoryManager _inventoryManager;

		private bool _hovering;

		private void Awake()
		{
			_slot = GetComponent<Slot>();
			_inventoryManager = GameManager.Instance.InventoryManager;
			_tooltipTransform = _inventoryManager.itemTooltipTransform;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (_slot.ItemCount == 0 || _inventoryManager.movingSlotObject.activeSelf || !_inventoryManager.ShowTooltip)
			{
				return;
			}
			
			_inventoryManager.itemTooltipText.text = _slot.Item.ItemTooltip();
			_tooltipTransform.gameObject.SetActive(true);
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			if (_slot.ItemCount == 0 || _inventoryManager.movingSlotObject.activeSelf  || !_inventoryManager.ShowTooltip)
			{
				_tooltipTransform.gameObject.SetActive(false);
				return;
			}
			
			var tooltipSizeX = _tooltipTransform.rect.width / 2;
			var tooltipSizeY = _tooltipTransform.rect.height;

			var xPosition = Mathf.Clamp(Input.mousePosition.x + tooltipSizeX + 30, tooltipSizeX + 10, Screen.width - tooltipSizeX - 10);
			var yPosition = Mathf.Clamp(Input.mousePosition.y - tooltipSizeY / 2 - 15, tooltipSizeY + 20, Screen.height);
			
			_tooltipTransform.position = new Vector2(xPosition, yPosition);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (_slot.ItemCount == 0) return;
            
			_tooltipTransform.gameObject.SetActive(false);
		}
	}
}