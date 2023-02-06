using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProjectBeelzebub.AllCrafts;

namespace ProjectBeelzebub
{
    public class SetCraft : MonoBehaviour
    {

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

        public void SetCraftInfo(Craft item)
        {
            cname.text = item.outcome.name;
            outcome.sprite = item.outcome.sprite;

            if (item.mat1 != null)
            {
                material1.sprite = item.mat1.sprite;
                material1Amount.text = item.mat1Amount + "x";
            }
            else
            {
                material1.enabled = false;
                material1Amount.enabled = false;
            }

            if (item.mat2 != null)
            {
                material2.sprite = item.mat2.sprite;
                material2Amount.text = item.mat2Amount + "x";
            }
            else
            {
                material2.enabled = false;
                material2Amount.enabled = false;
            }


            if (item.mat3 != null)
            {
                material3.sprite = item.mat3.sprite;
                material3Amount.text = item.mat3Amount + "x";
            }
            else
            {
                material3.enabled = false;
                material3Amount.enabled = false;
            }
        }
   
        public void CraftObject()
        {
            // If i have materials
           // if ()
        }
    }
}
