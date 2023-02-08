using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            print($"Selected Crafting Item: { selectedItem} ");

            int count = 0;
            foreach (GameObject craft in allCraft.GetItemsObjs())
            {

                SpriteRenderer s = craft.GetComponent<SpriteRenderer>();

                s.color = count == selectedItem ? selectedColor : defaultColor;

                count++;

            }
		}

        public void CraftSelectedItem()
		{

            if (selectedMenu != 0) return;


        }

        #endregion


        public void CraftItem(CraftableItem craft)
        {
            
            Inventory inv = GetComponent<Inventory>();

            // First Material Check
            if (!(inv.CheckMaterial(craft.material1) >= craft.material1Amount) || 
                craft.material1 == null) return;

			// Second Material Check
			if (!(inv.CheckMaterial(craft.material2) >= craft.material2Amount) || 
                craft.material1 == null) return;

			// Third Material Check
			if (!(inv.CheckMaterial(craft.material3) >= craft.material3Amount) || 
                craft.material1 == null) return;


            // Add Item (Or not if there is no space)
            if(!inv.AddItem(craft.outcomeItem)) return;

            // Remove Resources 
            // Remove Material One
            inv.RemoveItem(craft.material1, craft.material1Amount);

			// Remove Material Two
			inv.RemoveItem(craft.material2, craft.material2Amount);

			// Remove Material Three
			inv.RemoveItem(craft.material3, craft.material3Amount);


		}




	}
}
