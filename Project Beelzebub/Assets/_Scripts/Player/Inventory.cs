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
        [SerializeField] private int selectedItem = 0;

        [Title("UI")]
        [SerializeField] private ToastManager toast;
        [SerializeField] private GameObject dropPrefab;
        [SerializeField] private GameObject dropsContainer;
        [SerializeField] protected GameObject itemPrefab;

        [Title("Fire")]
        [SerializeField] private GameObject firePrefab;
        [SerializeField] private Vector3 fireOffset;


        #region Short Methods

        private void Awake() => inventory.Clear();

		private void NextSelected() => selectedItem += selectedItem < inventory.Count - 1 ? 1 : 0;

        private void PreviousSelected() => selectedItem -= selectedItem > 0 ? 1 : 0;

		#endregion

		#region Inputs

		// On Fire
		public void UseItem()
        {
            if (selectedMenu != 2) return;


            InventoryItem item = inventory[selectedItem];

            // Food
            if(item.type == InventoryItem.ItemType.Food)
            {
                playerStats.AddHealth(item.heal);
                playerStats.AddHunger(item.hunger);
                playerStats.AddThirst(item.thirst);

                RemoveItem(item, 1);

            }

            else if(item.type == InventoryItem.ItemType.Weapon)
            {
                playerStats.weapon = item;

                RemoveItem(item, 1);
            }

            else if (item.name == "Fire")
            {
                GameObject fire = Instantiate(firePrefab, GameManager.Instance.transform);
            
                fire.transform.position = transform.position  + fireOffset;
            }



            print(MasterAudio.PlaySound(item.useSound));

            UpdateInventory();
        }

        // On move
        public void SelectItem()
        {

            if (selectedMenu != 2) return;

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

            inventory.RemoveAll(inv => inv.stackCount < 0);

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

                itemScript.count.GetComponent<TMP_Text>().text = item.stackCount.ToString() + "x";
                    
                if (selectedItem == i)
                    itemScript.SetTooltip();

                else
                {
                    itemScript.UnsetTooltip();
                    itemScript.SetImage(item.sprite);
                }
				inventoryObjects.Add(_invItem);

                i++;
            }

        }

        #endregion

        #region Public Methods

        public void DropItems()
        {

            for(int i = 0; i < dropsContainer.transform.childCount; i++)
            {
                Transform g = dropsContainer.transform.GetChild(i);

                Destroy(g.gameObject);
            }

            Vector3 worldPos = transform.position;
            

            foreach (InventoryItem item in inventory)
            {
                GameObject g = Instantiate(dropPrefab, Vector3.zero, Quaternion.identity, dropsContainer.transform);
                g.transform.localPosition = worldPos;
                g.GetComponent<Drop>().item = item;
                g.GetComponent<SpriteRenderer>().sprite = item.sprite;

                print(g.transform.position);

                RemoveItem(item, item.stackCount);
            }

        }
       
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

            toast.StartToast(item);

            return true;

        }

        public void RemoveItem(InventoryItem item, int amount)
        {
            foreach (InventoryItem inv in inventory)
            {
                if (inv.name != item.name) 
                    continue;
                
                inv.stackCount -= amount;

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


