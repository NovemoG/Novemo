using Core;
using Inventories;
using Inventories.Slots;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject slotObject;

        [SerializeField] public HoverSlot HoverSlot;
        
        private void Update()
        {
            if (HoverSlot.background.activeSelf)
            {
                var mousePos = Input.mousePosition;

                HoverSlot.transform.position = new Vector2(mousePos.x, mousePos.y);
            }
        }

        public Slot[,] CreateInventory(Transform inventoryTransform, int rows, int cols)
        {
            var slots = new Slot[rows, cols];
            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var slot = Instantiate(slotObject, inventoryTransform, true);
                    var slotClass = slot.GetComponent<Slot>();
                    
                    slot.gameObject.name = "Slot";
                    slot.GetComponent<Button>().onClick.AddListener(delegate { MoveItems(slotClass, inventoryTransform.GetComponent<Inventory>()); });

                    slotClass.SlotCoordinates = new Coordinates2D(i, j);

                    slots[i, j] = slotClass;
                }
            }

            return slots;
        }
        
        public void MoveItems(Slot clickedSlot, Inventory slotInventory)
        {
            var hoverSlot = HoverSlot.slotClass;
            
            if (!hoverSlot.IsEmpty && !clickedSlot.IsFull)
            {
                var result = slotInventory.AddItems(hoverSlot.Items, clickedSlot.SlotCoordinates.X, clickedSlot.SlotCoordinates.Y);
                
                hoverSlot.RemoveItems(hoverSlot.Items.Count - result.Count);
            }
            else if (!clickedSlot.IsEmpty && !hoverSlot.IsFull)
            {
                var pivotSlot = clickedSlot.SlotGroup.PivotSlot;
                var item = pivotSlot.Items[0];
                
                var result = hoverSlot.AddItems(pivotSlot.Items);
                HoverSlot.rect.sizeDelta = new Vector2(item.sizeCols * Metrics.SlotSize.y, item.sizeRows * Metrics.SlotSize.x);
                HoverSlot.text.text = HoverSlot.slotClass.Items.Count.ToString();
                HoverSlot.icon.sprite = item.itemIcon;
                
                //TODO doesnt place items

                clickedSlot.SlotGroup.RemoveItems(pivotSlot.Items.Count - result.Count);
            }

            var setActive = hoverSlot.Items.Count != 0;
            HoverSlot.background.SetActive(setActive);
            HoverSlot.icon.gameObject.SetActive(setActive);
            HoverSlot.text.gameObject.SetActive(setActive);
        }
    }
}
