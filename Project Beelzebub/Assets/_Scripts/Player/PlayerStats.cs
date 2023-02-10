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
        [SerializeField] private GameObject healthObj;
        [SerializeField] private List<GameObject> healthBar;
        [SerializeField] private GameObject healthPrefab;

		[Title("Sleep")]
		[SerializeField] private float sleep = 10;
		[SerializeField] private float maxSleep = 10;
        [SerializeField] private float sleepTimer = 60;
		[SerializeField] private float sleepMultiplier = 0.5f;
        [SerializeField] private GameObject sleepObj;
		[SerializeField] private List<GameObject> sleepBar;
        [SerializeField] private GameObject sleepPrefab;
		private float sleepCount = 0;

        [Title("Food")]
        [SerializeField] private float hunger = 10;
        [SerializeField] private float maxHunger = 10;
        [SerializeField] private float hungerTimer = 60;
        [SerializeField] private float hungerMultiplier = 1f;
        [SerializeField] private GameObject hungerObj;
        [SerializeField] private List<GameObject> hungerBar;
        [SerializeField] private GameObject hungerPrefab;
        private float hungerCount = 0;

        [Title("Thirst")]
        [SerializeField] private float thirst = 10;
        [SerializeField] private float maxThirst = 10;
        [SerializeField] private float thirstTimer = 60;
        [SerializeField] private float thirstMultiplier = 1f;
        [SerializeField] private GameObject thirstObj;
        [SerializeField] private List<GameObject> thirstBar;
        [SerializeField] private GameObject thirstPrefab;
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

            // Health
            for (int i = 0; i < maxHealth; i++)
            {
                Destroy(healthBar[i]);
                healthBar.RemoveAt(i);

				if (i > health) continue;

				GameObject healthTemp = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity, healthObj.transform);
                healthBar.Add(healthTemp);
            }


			// Hunger
			for (int i = 0; i < maxHunger; i++)
			{
				Destroy(hungerBar[i]);
				hungerBar.RemoveAt(i);

                if (i > hunger) continue;

				GameObject hungerTemp = Instantiate(hungerPrefab, Vector3.zero, Quaternion.identity, hungerObj.transform);
				hungerBar.Add(hungerTemp);
			}


			// Thirst
			for (int i = 0; i < maxThirst; i++)
			{
				Destroy(thirstBar[i]);
				thirstBar.RemoveAt(i);

				if (i > thirst) continue;

				GameObject thirstTemp = Instantiate(thirstPrefab, Vector3.zero, Quaternion.identity, thirstObj.transform);
				thirstBar.Add(thirstTemp);
			}


			// Sleep
			for (int i = 0; i < maxSleep; i++)
			{
				Destroy(sleepBar[i]);
				sleepBar.RemoveAt(i);

				if (i > sleep) continue;

				GameObject thirstTemp = Instantiate(sleepPrefab, Vector3.zero, Quaternion.identity, sleepObj.transform);
				sleepBar.Add(thirstTemp);
			}


		}

		#endregion
	}
}
