using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
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
        [SerializeField] private TMP_Text sleeptext;

        public bool fireActive = false;

        private float fireTimer;

        private void Start()
        {
            PutOut();
        }

        private void Update()
        {
            if (GameManager.Instance.sleepCooldown - GameManager.Instance.sleepTimer > 0)
            {
                sleeptext.text = $"{(GameManager.Instance.sleepCooldown - GameManager.Instance.sleepTimer).ToString("0.00")} Seconds Left";

            }
            else
            {
                sleeptext.text = "Ready";
            }
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
            GetComponent<DealDamage>().isSpiky = false;
            fireActive = false;
            GameManager.Instance.CheckFire();

            MasterAudio.StopPlaylist("Fire");
            anim.SetBool("enabled", false);
        }

        public void StartFire(float fuel)
        {
            fireLife = fuelMultiplier * fuel;

            GetComponent<DealDamage>().isSpiky = true;
            
            fireTimer = 0;
            fireActive = true;
            GameManager.Instance.isFireRunning = true;

            MasterAudio.StartPlaylist("Fire", "FirePlaylist");


            anim.SetBool("enabled", true);
        }
    }
}
