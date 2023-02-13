using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class FireScript : MonoBehaviour
    {

        [Title("Other")]
        [SerializeField] private Animator anim;
        [SerializeField] private InventoryItem fuel;

        [Title("Numbers")]
        [SerializeField] private float fireLife;
        [SerializeField] private float fuelMultiplier;

        [Title("Popup")]
        [SerializeField] private GameObject popup;
        [SerializeField] private Image materialImage;
        [SerializeField] private TMP_Text firetext;

        public bool fireActive = false;

        private float fireTimer;

        private void Start()
        {
            PutOut();
        }

        private void Update()
        {
            if (!fireActive) return;

            fireTimer += Time.deltaTime;

            if(fireTimer > fireLife)
            {
                PutOut();
            }

            firetext.text = $"{(fireLife - fireTimer).ToString("0.00")} Seconds Left";
        }

        public void ShowPopup()
        {
            popup.SetActive(true);

            materialImage.sprite = fuel.sprite;
            firetext.text = $"{(fireLife - fireTimer).ToString("0.00")} Seconds Left";
        }

        public void HidePopup()
        {
            popup.SetActive(false);
        }

        public void AddFuel(float amt)
        {

            fireLife += fuelMultiplier * amt;
        }

        public void PutOut()
        {
            GetComponent<DealDamage>().enabled = false;
            fireActive = false;
            print("Fire Out");
            anim.SetBool("enabled", false);
        }

        public void StartFire(float fuel)
        {
            fireLife = fuelMultiplier * fuel;

            GetComponent<DealDamage>().enabled = true;
            
            fireTimer = 0;
            fireActive = true;

            print("Start Fire");

            anim.SetBool("enabled", true);
        }

    }
}
