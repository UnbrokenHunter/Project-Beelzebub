using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectBeelzebub
{
    public class Item : MonoBehaviour
    {
        public InventoryItem stats;

        [Title("Tooltip")]

        [SerializeField, LabelWidth(100)]
        private GameObject Tooltip;

        [SerializeField, LabelWidth(100)]
        private GameObject image;

        [SerializeField, LabelWidth(100)]
        private GameObject itemName;

        [SerializeField, LabelWidth(100)]
        private GameObject description;

        [SerializeField, LabelWidth(100)]
        private GameObject stat1;

        [SerializeField, LabelWidth(100)]
        private GameObject stat2;

        [SerializeField, LabelWidth(100)]
        private GameObject stat3;

        public void UnsetTooltip() => Tooltip.SetActive(false);

        public void SetTooltip()
        {
            Tooltip.SetActive(true);

            image.GetComponent<Image>().sprite = stats.sprite;

            itemName.GetComponent<TMP_Text>().text = stats.name;

            description.GetComponent<TMP_Text>().text = stats.description;

            if (stats.type == InventoryItem.ItemType.Weapon)
            {
                stat1.GetComponent<TMP_Text>().text = $"Damage: {stats.damage}";

                stat2.GetComponent<TMP_Text>().text = $"Duribility: {stats.durability}";

                stat3.GetComponent<TMP_Text>().text = $"Other: {stats.other}";

            }
            else if (stats.type == InventoryItem.ItemType.Food)
            {
                stat1.GetComponent<TMP_Text>().text = $"Heal: {stats.heal}";

                stat2.GetComponent<TMP_Text>().text = $"Hunger: {stats.hunger}";

                stat3.GetComponent<TMP_Text>().text = $"Thirst: {stats.thirst}";
            }

        }
    }
}
