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

        [Title("General")]
        [SerializeField] Vector3 spawnLocation;
        [SerializeField] private PlayerVisuals visuals;
        [SerializeField] public InventoryItem weapon;
        [SerializeField] public Color darkness;
        [SerializeField] private GameObject gameOverMenu;


		[Title("Speed")]
		[SerializeField, FoldoutGroup("Speed")] public float maxSpeed = 5;
		[SerializeField, FoldoutGroup("Speed")] public float speedMultiplier = 1;
		[SerializeField, FoldoutGroup("Speed")] public float slipperyness = 90;
		[SerializeField, FoldoutGroup("Speed")] public float deceleration = 60;

		[Title("Attack")]
		[SerializeField, FoldoutGroup("Attack")] public float attackRange = 2;
		[SerializeField, FoldoutGroup("Attack")] public float attackMultiplier = 5;


		[Title("Health")]
        [SerializeField, FoldoutGroup("Health")] public float health = 10;
        [SerializeField, FoldoutGroup("Health")] public float maxHealth = 10;
        [SerializeField, FoldoutGroup("Health")] private float regenRate = 0.5f;
        [SerializeField, FoldoutGroup("Health")] private GameObject healthObj;
        [SerializeField, FoldoutGroup("Health")] private List<GameObject> healthBar;
        [SerializeField, FoldoutGroup("Health")] private GameObject healthPrefab;

		[Title("Sleep")]
		[SerializeField, FoldoutGroup("Sleep")] public float sleep = 10;
		[SerializeField, FoldoutGroup("Sleep")] public float maxSleep = 10;
        [SerializeField, FoldoutGroup("Sleep")] private float sleepTimer = 60;
		[SerializeField, FoldoutGroup("Sleep")] private float sleepMultiplier = 0.5f;
        [SerializeField, FoldoutGroup("Sleep")] private GameObject sleepObj;
		[SerializeField, FoldoutGroup("Sleep")] private List<GameObject> sleepBar;
        [SerializeField, FoldoutGroup("Sleep")] private GameObject sleepPrefab;
		private float sleepCount = 0;

        [Title("Food")]
        [SerializeField, FoldoutGroup("Hunger")] public float hunger = 10;
        [SerializeField, FoldoutGroup("Hunger")] public float maxHunger = 10;
        [SerializeField, FoldoutGroup("Hunger")] private float hungerTimer = 60;
        [SerializeField, FoldoutGroup("Hunger")] private float hungerMultiplier = 1f;
        [SerializeField, FoldoutGroup("Hunger")] private GameObject hungerObj;
        [SerializeField, FoldoutGroup("Hunger")] private List<GameObject> hungerBar;
        [SerializeField, FoldoutGroup("Hunger")] private GameObject hungerPrefab;
        private float hungerCount = 0;

        [Title("Thirst")]
        [SerializeField, FoldoutGroup("Thirst")] public float thirst = 10;
        [SerializeField, FoldoutGroup("Thirst")] public float maxThirst = 10;
        [SerializeField, FoldoutGroup("Thirst")] private float thirstTimer = 60;
        [SerializeField, FoldoutGroup("Thirst")] private float thirstMultiplier = 1f;
        [SerializeField, FoldoutGroup("Thirst")] private GameObject thirstObj;
        [SerializeField, FoldoutGroup("Thirst")] private List<GameObject> thirstBar;
        [SerializeField, FoldoutGroup("Thirst")] private GameObject thirstPrefab;
        private float thirstCount = 0;


		#endregion

		#region Effects

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

        [Button]
        public void RemoveHealth(float amount)
        {
            health -= amount;

            UpdateSliders();

            if (health < 0)
                KillPlayer();
            
		}

        public void AddHealth(float amount)
        {
            health += amount;
            health = Mathf.Min(health, maxHealth);

			UpdateSliders();
		}

        private void KillPlayer()
        {
            print("Die");

            visuals.StartDeath();

            GetComponent<Inventory>().DropItems();
            
            gameOverMenu.SetActive(true);
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

        public void Reset()
        {
            transform.position = spawnLocation;

            StartCoroutine(visuals.StartRespawn());

            health = maxHealth;
            hunger = 10;
            thirst = 10;
            sleep = 10;
        }

        private void Start() => ResetUI();

        private void Update()
        {
            HungerTimer();

			ThirstTimer();

            SleepTimer();
        }

        private void ResetUI()
        {
            foreach (GameObject g in healthBar)
                Destroy(g);


            healthBar.Clear();

            // Health
            for (int i = 0; i < maxHealth; i++)
            {
                GameObject healthTemp = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity, healthObj.transform);
                healthBar.Add(healthTemp);
            }


            UpdateSliders();

        }

        [Button]
        private void UpdateSliders()
        {

            for (int i = 0; i < maxHealth; i++)
            {
                if (i >= health) healthBar[i].GetComponent<Image>().color = darkness;
                else healthBar[i].GetComponent<Image>().color = Color.white;
            }

        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            Gizmos.DrawCube(spawnLocation, Vector3.one);
        }
    }

}
