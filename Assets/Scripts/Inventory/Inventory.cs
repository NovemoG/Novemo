using System.Collections.Generic;
using Core;
using Inventory.Slots;
using Managers;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        private InventoryManager _inventoryManager;
        
        public int rows = 20, cols = 12;
        
        public List<Slot> Slots;

        protected virtual void Awake()
        {
            _inventoryManager = GameManager.Instance.InventoryManager;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var slot = Instantiate(_inventoryManager.slotObject, transform, true);
                    slot.gameObject.name = $"Slot{i}{j}";
                    slot.GetComponent<Slot>().slotCoordinates = new Coordinates2D(i, j);
                    Slots.Add(slot.GetComponent<Slot>());
                }
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }
    }
}