using Characters.Player;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories.Stats
{
	public class StatTooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
	{
		private Stat _stat;
		private Player _playerClass;
		private RectTransform _tooltipTransform;
		private InventoryManager _inventoryManager;
		
		private void Awake()
		{
			_stat = GetComponent<Stat>();
			_playerClass = GameManager.Instance.PlayerManager.playerClass;
			_inventoryManager = GameManager.Instance.InventoryManager;
			_tooltipTransform = _inventoryManager.statTooltipTransform;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			var stat = _playerClass.Stats[_stat.statIndex];
			_inventoryManager.statTooltipText.text = $"Base value: {stat.BaseValue}\nBonus value: {stat.BonusValue()}";
			_tooltipTransform.gameObject.SetActive(true);
		}

		public void OnPointerMove(PointerEventData eventData)
		{
			var tooltipSizeX = _tooltipTransform.rect.width / 2 + 10;
			var tooltipSizeY = _tooltipTransform.rect.height / 2 + 10;

			var xPosition = Input.mousePosition.x + tooltipSizeX;
			var yPosition = Input.mousePosition.y + tooltipSizeY;
			
			_tooltipTransform.position = new Vector2(xPosition, yPosition);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_tooltipTransform.gameObject.SetActive(false);
		}
	}
}