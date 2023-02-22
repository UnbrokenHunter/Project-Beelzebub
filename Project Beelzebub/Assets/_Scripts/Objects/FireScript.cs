using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

namespace ProjectBeelzebub
{
    public class FireScript : MonoBehaviour
    {

        [Title("Other")]
		[SerializeField] GameObject lighting;
		[SerializeField] ParticleSystem particle;
		[SerializeField] private Animator anim;
        [SerializeField] private InventoryItem fuel;
        [SerializeField] private InventoryItem glasses;

        [Title("Numbers")]
        [SerializeField] private float fireLife;
        [SerializeField] private float fuelMultiplier;

        [Title("Popup")]
        [SerializeField] private GameObject popup;
        [SerializeField] private Image materialImage;
        [SerializeField] private Image material2Image;
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

            GameManager.Instance.fireTimer = $"{(fireLife - fireTimer).ToString("0.00")} Seconds Left";
            firetext.text = GameManager.Instance.fireTimer;

        }

        public void ShowPopup()
        {
            popup.SetActive(true);

            materialImage.sprite = fuel.sprite;
            material2Image.sprite = glasses.sprite;
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
            lighting.SetActive(false);
            particle.Pause();


			GetComponent<DealDamage>().isSpiky = false;
            fireActive = false;
            GameManager.Instance.CheckFire();

            MasterAudio.StopPlaylist("Fire");
            anim.SetBool("enabled", false);
        }

        public void StartFire(float fuel)
        {
            fireLife = fuelMultiplier * fuel;

            lighting.SetActive(true);
            particle.Play();


			GetComponent<DealDamage>().isSpiky = true;
            GetComponent<MMF_Player>().PlayFeedbacks();
            
            fireTimer = 0;
            fireActive = true;
            GameManager.Instance.isFireRunning = true;
            GameManager.Instance.hasFireRun = true;

            MasterAudio.StartPlaylist("Fire", "FirePlaylist");


            anim.SetBool("enabled", true);
        }
    }
}
