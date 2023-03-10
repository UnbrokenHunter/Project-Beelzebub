using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    [CreateAssetMenu(fileName = "Inventory Item", menuName = "Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        [BoxGroup("Basic Info"), LabelWidth(100)]
        public new string name;

        [BoxGroup("Basic Info"), LabelWidth(100), TextArea]
        public string description;

        [BoxGroup("Basic Info"), LabelWidth(100)]
        public ItemType type;

        [BoxGroup("Basic Info"), LabelWidth(100)]
        public string useSound;


        [Space]

        [HorizontalGroup("Game Data", 75), PreviewField(75), HideLabel]
        public Sprite sprite;

        [Space]

        [VerticalGroup("Game Data/Stats"), LabelWidth(100)]
        public float damage;

        [VerticalGroup("Game Data/Stats"), LabelWidth(100)]
        public float durability;

        [VerticalGroup("Game Data/Stats"), LabelWidth(100)]
        public float other;

        [VerticalGroup("Game Data/Food"), LabelWidth(100)]
        public float heal;

        [VerticalGroup("Game Data/Food"), LabelWidth(100)]
		public float hunger;

		[VerticalGroup("Game Data/Food"), LabelWidth(100)]
		public float thirst;



		[PropertySpace]

        [BoxGroup("Inventory Settings"), LabelWidth(100), Range(1, 20)]
        public int stackSize = 1;

        [BoxGroup("Inventory Settings"), LabelWidth(100), Range(1, 20)]
        public int stackCount = 1;


        public enum ItemType
        {
            Weapon,
            Food, 
            Material,
            Misc
        };
    }
}
