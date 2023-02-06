using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class SetCraft : MonoBehaviour
    {

        [SerializeField]
        private CraftableItem craftable;

        [SerializeField]
        private TMP_Text cname;
        [SerializeField]
        private Image outcome;

        [SerializeField]
        private Image material1;
        [SerializeField]
        private TMP_Text material1Amount;

        [SerializeField]
        private Image material2;
        [SerializeField]
        private TMP_Text material2Amount;

        [SerializeField]
        private Image material3;
        [SerializeField]
        private TMP_Text material3Amount;

        public void SetCraftInfo(CraftableItem item)
        {
            craftable = item;

            cname.text = item.outcomeItem.name;
            outcome.sprite = item.outcomeItem.sprite;

            if (item.material1 != null)
            {
                material1.sprite = item.material1.sprite;
                material1Amount.text = item.material1Amount + "x";
            }
            else
            {
                material1.enabled = false;
                material1Amount.enabled = false;
            }

            if (item.material2 != null)
            {
                material2.sprite = item.material2.sprite;
                material2Amount.text = item.material2Amount + "x";
            }
            else
            {
                material2.enabled = false;
                material2Amount.enabled = false;
            }


            if (item.material3 != null)
            {
                material3.sprite = item.material3.sprite;
                material3Amount.text = item.material3Amount + "x";
            }
            else
            {
                material3.enabled = false;
                material3Amount.enabled = false;
            }
        }
   
    }
}
