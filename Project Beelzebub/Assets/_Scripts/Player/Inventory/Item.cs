using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace ProjectBeelzebub
{
    public class Item : MonoBehaviour
    {
        public InventoryItem stats;
        [SerializeField] private Image thisImage;
        [SerializeField] private Image highlight;

        public GameObject count;

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


        public void SetImage(Sprite sprite)
        {
            thisImage.sprite = sprite;
        }
        public void UnsetTooltip()
        {
            Tooltip.SetActive(false);
            highlight.enabled = false;
            thisImage.enabled = true;
        }

        public void SetTooltip()
        {
            Tooltip.SetActive(true);

            thisImage.enabled = false;

            highlight.enabled = true;

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
