using System;
using Inventory.Slots;
using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        //TODO one inventory for chests (one for each size) only changing content while opening
        [SerializeField] private GameObject hoverItem;
        public Slot HoverSlot;
        
        [NonSerialized] public Slot Source;
        [NonSerialized] public Slot Destination;

        private void Awake()
        {
            HoverSlot = hoverItem.GetComponent<Slot>();
        }
    }
}
