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


		#region Inputs

		private void Start()
		{
			move.AddListener(SelectItem);
			fire.AddListener(CraftSelectedItem);
		}

		private void SelectItem()
		{

		}

		private void CraftSelectedItem()
		{

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
