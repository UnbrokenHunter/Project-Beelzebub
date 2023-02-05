using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class Inventory : MonoBehaviour
    {

        [Title("Inventory")]
        [SerializeField] private int inventorySize = 10;
        [SerializeField] private List<InventoryItem> inventory = new();
        [SerializeField] private List<GameObject> inventoryObjects = new();
        private int selectedItem = 0;

        [Title("UI")]
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject inventoryContainer;
        [SerializeField] private GameObject itemPrefab;

        private PlayerStats playerStats;

        private void Awake() => inventory.Clear();

        private void Start() => playerStats = GetComponent<PlayerStats>();

        private void NextSelected() => selectedItem += selectedItem < inventory.Count - 1 ? 1 : 0;

        private void PreviousSelected() => selectedItem -= selectedItem > 0 ? 1 : 0;

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


            UpdateInventory();
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

            return true;

        }

        private void UpdateInventory()
        {
            foreach (GameObject item in inventoryObjects)
            {
                Destroy(item);
            }

            inventoryObjects.Clear();

            int i = 0;

            foreach (InventoryItem item in inventory)
            {
                GameObject _invItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, inventoryContainer.transform);
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

        private void OpenInventory()
        {
            UpdateInventory();

            if (inventoryUI.activeInHierarchy)
                inventoryUI.SetActive(false);
            

            else
                inventoryUI.SetActive(true);
            
        }

        public void OnMove(InputValue value)
        {
            if (inventoryUI.activeInHierarchy)
            {
                lastInput = input;
                input = value.Get<Vector2>();

                if (CheckInputDifferecnce())
                {
                    print($"New Item Selected: {selectedItem}");

                    if (input.x > 0)
                        NextSelected();

                    else if (input.x < 0)
                        PreviousSelected();

                    UpdateInventory();
                }
            }

        }

        private Vector2 input = Vector2.zero;
        private Vector2 lastInput = Vector2.zero;

        private bool CheckInputDifferecnce()
        {
            if(input == Vector2.zero) return false;
            
            if (lastInput.x == input.x) return false;
            else return true;
        }
        public bool IsInventoryOpen() => inventoryUI.activeInHierarchy;
        public void OnInventory(InputValue value) => OpenInventory();

        public void OnFire(InputValue value) => UseItem();
    }
}


