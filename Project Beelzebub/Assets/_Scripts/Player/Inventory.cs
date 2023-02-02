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

        [Title("UI")]
        [SerializeField] private GameObject inventoryUI;
        [SerializeField] private GameObject inventoryContainer;
        [SerializeField] private GameObject itemPrefab;

        private void Awake() => inventory.Clear();

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

            foreach (InventoryItem item in inventory)
            {
                GameObject _invItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity, inventoryContainer.transform);
                _invItem.GetComponent<Item>().stats = item; 
                _invItem.GetComponent<Image>().sprite = item.sprite;
                _invItem.GetComponentInChildren<TMP_Text>().text = item.stackCount.ToString();
                inventoryObjects.Add(_invItem);
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
        public void OnInventory(InputValue value) => OpenInventory();

    }
}
