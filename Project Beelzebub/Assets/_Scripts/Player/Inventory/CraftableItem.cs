using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
	[CreateAssetMenu(fileName = "Craftable Item", menuName = "Craftable Item")]
	public class CraftableItem : ScriptableObject
    {

		[FoldoutGroup("Craftable")]
		public InventoryItem outcomeItem;

		[TitleGroup("Craftable/Material One")]
		public InventoryItem material1;

		[TitleGroup("Craftable/Material One")]
		public int material1Amount;


		[TitleGroup("Craftable/Material Two")]
		public InventoryItem material2;

		[TitleGroup("Craftable/Material Two")]
		public int material2Amount;


		[TitleGroup("Craftable/Material Three")]
		public InventoryItem material3;

		[TitleGroup("Craftable/Material Three")]
		public int material3Amount;


	}
}
