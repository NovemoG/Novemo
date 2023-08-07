using Inventories;
using Inventories.Slots;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        public GameObject movingSlot;
        
        public Inventory playerInventory;
        public ToggleGroup inventoryToggleGroup;

        private Slot _selected;

        public Item red;
        public Item yellow;
        public Item green;

        public void InitiateItemSwap()
        {
            
        }

        public void SwapItems(Slot clicked)
        {
            
        }

        private void Update()
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                //select next
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                //select previous
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                playerInventory.AddItem(red);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                playerInventory.AddItem(yellow);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                playerInventory.AddItem(green);
            }
        }
    }
}
