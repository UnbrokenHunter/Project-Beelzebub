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
        private bool inventoryEnabled = false;
        private int selectedItem = 0;

        [Title("Other")]
        [SerializeField] private float scrollTimer = 0.5f;

        [Title("UI")]
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject inventoryContainer;
        [SerializeField] private GameObject itemPrefab;

        private void Awake() => inventory.Clear();

        public void NextSelected() => selectedItem = selectedItem < inventory.Count ? selectedItem++ : selectedItem;

        public void PreviousSelected() => selectedItem = selectedItem > 0 ? selectedItem-- : selectedItem;

        public void ViewItem()
        {

        }

        public void UseItem()
        {
            
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

                _invItem.GetComponentInChildren<Image>().sprite = item.sprite;
                _invItem.GetComponentInChildren<TMP_Text>().text = item.stackCount.ToString();

                print(_invItem.transform.name);
                _invItem.GetComponent<Image>().enabled = selectedItem == i;

				inventoryObjects.Add(_invItem);

                i++;
            }
        }

        private void OpenInventory()
        {
            UpdateInventory();

            if (inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(false);
                inventoryEnabled = false;
            }

            else
            {
                inventoryUI.SetActive(true);
                inventoryEnabled = true;
            }
        }

        public void OnMove(InputValue value)
        {
            Vector2 movement;


            if (inventoryUI.activeInHierarchy)
            {
                movement = value.Get<Vector2>();

                if (!ScrollTimer()) return;

                print($"New Item Selected: {selectedItem}");

                if (movement.x > 0)
                    NextSelected();

                else if(movement.x < 0) 
                    PreviousSelected();

            }

        }

        private float scrollTime;
        private bool ScrollTimer()
        {
            scrollTime += Time.deltaTime;

            if (scrollTime > scrollTimer)
            {
                scrollTime = 0;
                return true;
            }
            return false;
        }

        public void OnInventory(InputValue value) => OpenInventory();

    }
}


