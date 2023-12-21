using DG.Tweening;
using Items;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventories.Slots
{
    [RequireComponent(typeof(SlotTooltipHandler))]
    public class Slot : MonoBehaviour
    {
        [SerializeField] protected Item item;
        public Item Item => item;

        [SerializeField] protected int itemCount;
        public int ItemCount => itemCount;

        [SerializeField] protected Image slotIcon;
        [SerializeField] protected TextMeshProUGUI stackText;
        
        [SerializeField] protected GameObject borderObject;
        [SerializeField] protected GameObject backgroundObject;
        
        public bool IsEmpty => itemCount == 0;
        public bool IsFull => !IsEmpty && itemCount == item.StackLimit;

        public int ParentInventoryId { get; private set; }
        public Toggle ToggleComponent { get; private set; }
        
        protected int SlotTweenId;
        protected RectTransform SlotIconRect;
        protected InventoryManager InventoryManager;

        protected virtual void Awake()
        {
            if (itemCount == 0) item = null;
            
            InventoryManager = GameManager.Instance.InventoryManager;

            SlotIconRect = transform.GetChild(1).GetComponent<RectTransform>();

            if (stackText != null)
            {
                ToggleComponent = GetComponent<Toggle>();
                ToggleComponent.group = InventoryManager.inventoryToggleGroup;
            }
            
            ParentInventoryId = transform.parent.name switch
            {
                "Inventory" => 0,
                "VaultInventory" => 1,
                "ChestInventory" => 2,
                "EquipmentInventory" => 3,
                _ => ParentInventoryId
            };

            SlotTweenId = GetHashCode();
        }

        public virtual bool AddItem(Item itemToAdd, bool submitStats = true)
        {
            if (IsFull) return false;
            if (!IsEmpty && item != itemToAdd) return false;

            itemCount += 1;
            
            switch (ItemCount)
            {
                case 1:
                    item = itemToAdd;
                    slotIcon.gameObject.SetActive(true);
                    slotIcon.sprite = item.Icon;
                    break;
                case > 1:
                    stackText.gameObject.SetActive(true);
                    stackText.text = itemCount.ToString();
                    break;
            }

            DOTween.Kill(6, true);
            DOTween.Sequence()
                   .Append(SlotIconRect.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.1f))
                   .Append(SlotIconRect.DOScale(new Vector3(1f, 1f, 1f), 0.1f)).intId = SlotTweenId;

            return true;
        }
        
        public int AddItems(Item itemToAdd, int count)
        {
            while (count > 0)
            {
                if (!AddItem(itemToAdd)) break;
                count--;
            }

            return count;
        }

        public virtual bool RemoveItem()
        {
            if (IsEmpty) return false;
            
            itemCount -= 1;
            
            DOTween.Kill(7, true);
            DOTween.Sequence()
                   .Append(SlotIconRect.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.1f))
                   .Append(SlotIconRect.DOScale(new Vector3(1f, 1f, 1f), 0.1f)).intId = SlotTweenId + 1;
            
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
        public virtual void ClearSlot()
        {
            item = null;
            itemCount = 0;
            DOTween.Kill(7, true);
            stackText.gameObject.SetActive(false);
            slotIcon.gameObject.SetActive(false);
        }

        public virtual void SetActive()
        {
            ToggleComponent.isOn = true;
        }

        public virtual void ToggleBorder()
        {
            InventoryManager.SelectedSlotInventory = ParentInventoryId;
            borderObject.SetActive(ToggleComponent.isOn);
        }
    }
}