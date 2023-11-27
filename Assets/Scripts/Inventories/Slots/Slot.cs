using System;
using DG.Tweening;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories.Slots
{
    [RequireComponent(typeof(TooltipHandler))]
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Item item;
        public Item Item => item;

        [SerializeField] private int itemCount;
        public int ItemCount => itemCount;
        
        public bool IsEmpty => itemCount == 0;
        public bool IsFull => !IsEmpty && itemCount == item.stackLimit;

        public Image slotIcon;
        public TextMeshProUGUI stackText;
        
        public GameObject borderObject;
        public GameObject backgroundObject;

        public Toggle ToggleComponent { get; private set; }
        
        private RectTransform _slotIconRect;
        private InventoryManager _inventoryManager;
        
        private int _slotTweenId;
        [NonSerialized] public int ParentInventoryId;

        private void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;
            
            ToggleComponent = GetComponent<Toggle>();
            ToggleComponent.group = _inventoryManager.inventoryToggleGroup;

            _slotIconRect = transform.GetChild(1).GetComponent<RectTransform>();

            ParentInventoryId = transform.parent.name switch
            {
                "Inventory" => 0,
                "VaultInventory" => 1,
                "ChestInventory" => 2,
                _ => ParentInventoryId
            };

            _slotTweenId = GetHashCode();
        }

        public bool AddItem(Item itemToAdd)
        {
            if (IsFull) return false;
            if (!IsEmpty && item != itemToAdd) return false;

            itemCount += 1;
            
            switch (ItemCount)
            {
                case 1:
                    item = itemToAdd;
                    slotIcon.gameObject.SetActive(true);
                    slotIcon.sprite = item.itemIcon;
                    break;
                case > 1:
                    stackText.gameObject.SetActive(true);
                    stackText.text = itemCount.ToString();
                    break;
            }

            DOTween.Kill(6, true);
            DOTween.Sequence()
                   .Append(_slotIconRect.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.1f))
                   .Append(_slotIconRect.DOScale(new Vector3(1f, 1f, 1f), 0.1f)).intId = _slotTweenId;

            return true;
        }

        /// <summary>
        /// Adds items to a slot while simultaneously removing items one by one from provided list 
        /// </summary>
        /// <returns>A list of items that left which couldn't be added</returns>
        public int AddItems(Item itemToAdd, int count)
        {
            while (count > 0)
            {
                if (!AddItem(itemToAdd)) break;
                count--;
            }

            return count;
        }

        public bool RemoveItem()
        {
            if (IsEmpty) return false;
            
            itemCount -= 1;
            
            DOTween.Kill(7, true);
            DOTween.Sequence()
                   .Append(_slotIconRect.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.1f))
                   .Append(_slotIconRect.DOScale(new Vector3(1f, 1f, 1f), 0.1f)).intId = _slotTweenId + 1;
            
            UpdateSlot();
            
            return true;
        }

        public int RemoveItems(int count)
        {
            while (count > 0)
            {
                if (!RemoveItem()) break;
                count--;
            }

            return count;
        }

        private void UpdateSlot()
        {
            switch (itemCount)
            {
                case 0:
                    ClearSlot();
                    break;
                case 1:
                    stackText.gameObject.SetActive(false);
                    break;
                case > 1:
                    stackText.text = itemCount.ToString();
                    break;
            }
        }

        /// <summary>
        /// Removes all items from given slot
        /// </summary>
        public void ClearSlot()
        {
            item = null;
            itemCount = 0;
            DOTween.Kill(7, true);
            stackText.gameObject.SetActive(false);
            slotIcon.gameObject.SetActive(false);
        }

        public void ToggleBorder()
        {
            _inventoryManager.SelectedSlotInventory = ParentInventoryId;
            borderObject.SetActive(ToggleComponent.isOn);
        }
    }
}