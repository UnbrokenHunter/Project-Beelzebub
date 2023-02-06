using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DarkTonic.MasterAudio;


namespace ProjectBeelzebub
{
    public class Inventory : PlayerUI
    {

        [Title("Inventory")]
        [SerializeField] private int inventorySize = 10;
        [SerializeField] private List<InventoryItem> inventory = new();
        [SerializeField] private List<GameObject> inventoryObjects = new();
        private int selectedItem = 0;

        [Title("UI")]
		[SerializeField] protected GameObject itemPrefab;

		#region Short Methods

		private void Awake() => inventory.Clear();

        private void Start()
        {
			move.AddListener(SelectItem);
			fire.AddListener(UseItem);
		}

		private void NextSelected() => selectedItem += selectedItem < inventory.Count - 1 ? 1 : 0;

        private void PreviousSelected() => selectedItem -= selectedItem > 0 ? 1 : 0;

		#endregion

		#region Inputs

		// On Fire
		private void UseItem()
        {

            InventoryItem item = inventory[selectedItem];

            if(item.type == InventoryItem.ItemType.Food)
            {
                playerStats.AddHealth(item.heal);
                playerStats.AddHunger(item.hunger);
                playerStats.AddThirst(item.thirst);
            }

            item.stackCount--;

            if(item.stackCount <= 0)
            {
                inventory.RemoveAt(selectedItem);
            }

            print(MasterAudio.PlaySound(item.useSound));

            UpdateInventory();
        }

        // On move
        private void SelectItem()
        {

            print($"New Item Selected: {selectedItem}");

            if (input.x > 0)
                NextSelected();

            else if (input.x < 0)
                PreviousSelected();

            MasterAudio.PlaySound("Select");
                
            UpdateInventory();

        }

		#endregion

		#region Helpers 
     
        private void UpdateInventory()
        {

            // Inventory
            foreach (GameObject item in inventoryObjects)
            {
                Destroy(item);
            }

            inventoryObjects.Clear();

            int i = 0;

            foreach (InventoryItem item in inventory)
            {
                GameObject _invItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, inventoryMenu.transform);
                Item itemScript = _invItem.GetComponentInChildren<Item>();

				itemScript.stats = item;

                itemScript.GetComponent<Image>().sprite = item.sprite;
                _invItem.GetComponentInChildren<TMP_Text>().text = item.stackCount.ToString();

                _invItem.GetComponent<Image>().enabled = selectedItem == i;

                if (selectedItem == i) 
                    itemScript.SetTooltip();
                
                else 
                    itemScript.UnsetTooltip();

				inventoryObjects.Add(_invItem);

                i++;
            }

        }

		#endregion 

		#region Public Methods
		public bool AddItem(InventoryItem item)
        {
            
            // Return false if cannot fit into inventory
            
            // If you already have some of the item, stack it
            // Unless the max stack is reached
            if (inventory.Contains(item))
            {
                if (item.stackSize > item.stackCount)
                {
                    item.stackCount++;
                    print($"Item Added to Stack: {item.name} Stack Size: {item.stackCount}");
                }
                else return false;
            }

            // If there is room in the inventory, add the item
            else
            {
                item.stackCount = 1;

                if (inventory.Count < inventorySize)
                {
                    inventory.Add(item);
                    print($"Item Added: {item.name}");
                }

                else return false;
            }

            UpdateInventory();

            return true;

        }

        public void RemoveItem(InventoryItem item, int amount)
        {
            foreach (InventoryItem inv in inventory)
            {
                if (inv.name != item.name) 
                    continue;
                
                inv.stackCount -= amount;

                if (inv.stackCount <= 0)
                    inventory.Remove(inv); 

                UpdateInventory();
            }
        }

		public int CheckMaterial(InventoryItem item)
        {
            foreach (InventoryItem inv in inventory)
            {
                if (inv.name == item.name) 
                    return inv.stackCount;
            }
            return 0;
        }

        #endregion

    }
}


