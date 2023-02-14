using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class Crafting : PlayerUI
    {

        // Make Selection

        // Store Selected Item

        // Make Craftable

		private AllCrafts allCraft;
        private int selectedItem = 0;

        [SerializeField]
        private Color defaultColor;

        [SerializeField]
        private Color selectedColor;

        private void Start() => allCraft = craftMenu.GetComponentInChildren<AllCrafts>();

        private void NextSelected() => selectedItem += selectedItem < allCraft.GetItems().Count - 1 ? 1 : 0;

        private void PreviousSelected() => selectedItem -= selectedItem > 0 ? 1 : 0;


        #region Inputs

        public void SelectItem()
		{

            if (selectedMenu != 0) return;

            if (input.x > 0)
                NextSelected();

            else if (input.x < 0)
                PreviousSelected();

            print($"Selected Crafting Item: { selectedItem } ");

            int count = 0;
            foreach (GameObject craft in allCraft.GetItemsObjs())
            {

                Image s = craft.GetComponent<Image>();

                print(s.gameObject.name);

                s.color = count == selectedItem ? selectedColor : defaultColor;

                count++;

            }
		}

        public void CraftSelectedItem()
		{

            if (selectedMenu != 0) return;

            CraftableItem item = allCraft.GetItems()[selectedItem];

            print(item);

			CraftItem(item);


        }

        #endregion


        public void CraftItem(CraftableItem craft)
        {
            
            Inventory inv = GetComponent<Inventory>();

            // First Material Check
            if (craft.material1 != null) {
                if (!(inv.CheckMaterial(craft.material1) >= craft.material1Amount)) return;
            }

            // Second Material Check
            if (craft.material2 != null)
            {
                if (!(inv.CheckMaterial(craft.material2) >= craft.material2Amount)) return;
            }

            // Third Material Check
            if (craft.material3 != null)
            {
                if (!(inv.CheckMaterial(craft.material3) >= craft.material3Amount)) return;
            }

            // Add Item (Or not if there is no space)
            if(!inv.AddItem(craft.outcomeItem)) return;

            MasterAudio.PlaySound("Craft");

            // Remove Resources 
            // Remove Material One
            if (craft.material1 != null)
            {
                inv.RemoveItem(craft.material1, craft.material1Amount);
            }

            // Remove Material Two
            if (craft.material2 != null)
            {
                inv.RemoveItem(craft.material2, craft.material2Amount);
            }

            // Remove Material Three
            if (craft.material3 != null)
            {
                inv.RemoveItem(craft.material3, craft.material3Amount);
            }

		}




	}
}
