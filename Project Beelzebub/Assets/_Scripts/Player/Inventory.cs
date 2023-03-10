using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
using Cinemachine;

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
        [SerializeField] private float randomDropRange = 1;
        [SerializeField] private ToastManager toast;
        [SerializeField] private GameObject dropPrefab;
        [SerializeField] private GameObject dropsContainer;
        [SerializeField] protected GameObject itemPrefab;

        [Title("Fire")]
        [SerializeField] private CinemachineVirtualCamera fireCam;
        [SerializeField] private CinemachineVirtualCamera playerCam;
        [SerializeField] private GameObject firePrefab;
        [SerializeField] private Vector3 fireOffset;

        [Title("Drop")]
        private Vector3 dropPosition;

        #region Short Methods

        private void Awake() => inventory.Clear();

		private void NextSelected() => selectedItem += selectedItem < inventory.Count - 1 ? 1 : 0;

        private void PreviousSelected() => selectedItem -= selectedItem > 0 ? 1 : 0;

        private void Start()
        {
            playerCam.m_Lens.OrthographicSize = 6;
        }
        #endregion

        #region Inputs

        // On Fire
        public void UseItem()
        {
            if (selectedMenu != 2) return;

            InventoryItem item = inventory[selectedItem];
            print(item.heal);
            
            // Food
            if(item.type == InventoryItem.ItemType.Food)
            {
                playerStats.AddHealth(item.heal);
                playerStats.AddHunger(item.hunger);
                playerStats.AddThirst(item.thirst);
                GetComponentInChildren<StatsMenu>().UpdateUI();

                RemoveItem(item, 1);

            }

            else if(item.type == InventoryItem.ItemType.Weapon)
            {
                playerStats.weapon = item;
                GetComponentInChildren<StatsMenu>().UpdateUI();

                RemoveItem(item, 1);
            }

            print(MasterAudio.PlaySound(item.useSound));
            GetComponentInChildren<StatsMenu>().UpdateUI();
            UpdateInventory();
            CheckSpecialCrafts(item);
        }

        // On move
        public void SelectItem()
        {

            if (selectedMenu != 2) return;

            print($"New Item Selected: {selectedItem}");

            if (input.x > 0 || input.y < 0)
                NextSelected();

            else if (input.x < 0 || input.y > 0)
                PreviousSelected();

            MasterAudio.PlaySound("Select");
                
            UpdateInventory();

        }

		#endregion

		#region Helpers 
     
        public void UpdateInventory()
        {

            inventory.RemoveAll(inv => inv.stackCount <= 0);

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

        public void DropPosition()
        {
            dropPosition = transform.position;
        }

        public void DropItems()
        {
            
            print("Drop All Items");
            for(int i = 0; i < dropsContainer.transform.childCount; i++)
            {
                Transform g = dropsContainer.transform.GetChild(i);

                Destroy(g.gameObject);
            }

            dropsContainer.transform.position = dropPosition;

            for (int i = 0; i < inventory.Count; i++)
            {
                InventoryItem item = inventory[i];
                Vector3 random = new (Random.Range(-randomDropRange, randomDropRange), Random.Range(-randomDropRange, randomDropRange), 0);

                GameObject g = Instantiate(dropPrefab, Vector3.zero, Quaternion.identity, dropsContainer.transform);
                g.transform.localPosition = random;
                g.GetComponent<Drop>().item = item;
                g.GetComponent<Drop>().amount = item.stackCount;
                g.GetComponent<SpriteRenderer>().sprite = item.sprite;

                print(item.name);

                RemoveItem(item, item.stackCount);
            }
        }

        [Button]
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
                    //print($"Item Added to Stack: {item.name} Stack Size: {item.stackCount}");
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
                    //print($"Item Added: {item.name}");
                }

                else return false;
            }

            UpdateInventory();

            CheckSpecialCrafts(item);

            toast.StartToast(item);

            return true;

        }

        public void CloseInventory()
        {
            // Always go to correct Page
            if (selectedMenu == 1) return;
            else if (selectedMenu == 0)
            {
                CycleMenus();
                GetComponent<Crafting>().CycleMenus();
            }
            else
            {
                CycleMenus();
                GetComponent<Crafting>().CycleMenus();
                CycleMenus();
                GetComponent<Crafting>().CycleMenus();

            }

        }
       
        private void CheckSpecialCrafts(InventoryItem item)
        {
            // Fire
            if (item.name == "Fire")
            {
                CloseInventory();
                GetComponent<Dialogue>().ShowDialogue("I should probably place this somewhere higher up.");

                fireCam.Priority = 11;

                StartCoroutine(waitDialogue());

            }

        }
       
        private IEnumerator waitDialogue()
        {
            yield return new WaitForSeconds(7);
            fireCam.Priority = 9;
            playerCam.m_Lens.OrthographicSize += 1.5f;
            GetComponent<Dialogue>().HideDialogue();
        }

        public void RemoveItem(InventoryItem item, int amount)
        {
            if(inventory.Contains(item)) {

                item.stackCount -= amount;

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


