using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class PlayerStats : MonoBehaviour
    {

		#region Stats

		[Title("Speed")]
		[SerializeField] public float maxSpeed = 5;
		[SerializeField] public float speedMultiplier = 1;
		[SerializeField] public float slipperyness = 90;
		[SerializeField] public float deceleration = 60;

		[Title("Attack")]
		[SerializeField] public float attackRange = 2;
		[SerializeField] public float attackMultiplier = 5;

        [Title("Item")]
        [SerializeField] public InventoryItem weapon;

		[Title("Health")]
        [SerializeField] private float health = 10;
        [SerializeField] private float maxHealth = 10;
        [SerializeField] private float regenRate = 0.5f;
        [SerializeField] private Slider healthBar;

		[Title("Sleep")]
		[SerializeField] private float sleep = 10;
		[SerializeField] private float maxSleep = 10;
        [SerializeField] private float sleepTimer = 60;
		[SerializeField] private float sleepMultiplier = 0.5f;
		[SerializeField] private Slider sleepBar;
		private float sleepCount = 0;

        [Title("Food")]
        [SerializeField] private float hunger = 10;
        [SerializeField] private float maxHunger = 10;
        [SerializeField] private float hungerTimer = 60;
        [SerializeField] private float hungerMultiplier = 1f;
        [SerializeField] private Slider hungerBar;
        private float hungerCount = 0;

        [Title("Thirst")]
        [SerializeField] private float thirst = 10;
        [SerializeField] private float maxThirst = 10;
        [SerializeField] private float thirstTimer = 60;
        [SerializeField] private float thirstMultiplier = 1f;
        [SerializeField] private Slider thirstBar;
        private float thirstCount = 0;


		#endregion

		#region Effects

		private void CheckHealth()
        {
            if (health < 0)
            {
                print("Kill Player");

            }
        }

        private void CheckHunger()
        {
            if (hunger < 3)
                speedMultiplier = Mathf.Min(0.5f, speedMultiplier);
            
            if (hunger <= 0)
                health -= 1;
        }

		private void CheckThirst()
		{
			if (thirst < 3)
				speedMultiplier = Mathf.Min(0.5f, speedMultiplier);

			if (thirst <= 0)
				health -= 1;
		}


		#endregion

		#region Check/Add/Remove

		public void AddHunger(float amount)
        {
            hunger += amount;
            hunger = Mathf.Min(hunger, maxHunger);
            
            UpdateSliders();
        }

		public void AddThirst(float amount)
		{
			thirst += amount;
			thirst = Mathf.Min(thirst, maxThirst);

			UpdateSliders();
		}

        public void RemoveHealth(float amount)
        {
            health -= amount;

            UpdateSliders();

            if (health > 0)
            {
                print("Die");
            }

		}

        public void AddHealth(float amount)
        {
            health += amount;
            health = Mathf.Min(health, maxHealth);

			UpdateSliders();
		}

		#endregion

		#region Timers

		// Timers
		private void SleepTimer()
		{
			sleepCount += Time.deltaTime;

			if (sleepCount > sleepTimer)
			{
				sleepCount = 0;
				sleep -= 1 * sleepMultiplier;

				UpdateSliders();
			}
		}

		private void HungerTimer()
        {
            hungerCount += Time.deltaTime;

            if (hungerCount > hungerTimer)
            {
                hungerCount = 0;
                hunger -= 1 * hungerMultiplier;

                CheckHunger();
                UpdateSliders();
            }
        }
        
        private void ThirstTimer()
        {
            thirstCount += Time.deltaTime;

            if (thirstCount > hungerTimer)
            {
                thirstCount = 0;
                thirst -= 1 * thirstMultiplier;

                UpdateSliders();
            }
        }

		#endregion

		#region General

		// General

		private void Start() => UpdateSliders();

        private void Update()
        {
            HungerTimer();

			ThirstTimer();

            SleepTimer();

            CheckHealth();
        }

        private void UpdateSliders()
        {
            hungerBar.maxValue = maxHunger;
            hungerBar.value = hunger;

			thirstBar.maxValue = maxThirst;
			thirstBar.value = thirst;

			healthBar.maxValue = maxHealth;
            healthBar.value = health;

            sleepBar.maxValue = maxSleep;
            sleepBar.value = sleep;

        }

		#endregion
	}
}
