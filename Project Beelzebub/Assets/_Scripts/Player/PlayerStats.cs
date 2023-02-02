using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class PlayerStats : MonoBehaviour
    {

        [Title("Health")]
        [SerializeField] private float health = 10;
        [SerializeField] private float maxHealth = 10;
        [SerializeField] private float regenRate = 0.5f;
        [SerializeField] private Slider healthBar;

        [Title("Food")]
        [SerializeField] private float hunger = 10;
        [SerializeField] private float maxHunger = 10;
        [SerializeField] private float hungerTimer = 60;
        [SerializeField] private float hungerMultiplier = 1f;
        [SerializeField] private Slider hungerBar;


        // Utility
        private float time = 0;

        public void AddHunger(float amount)
        {
            hunger += amount;
            hunger = Mathf.Min(hunger, maxHunger);
            
            UpdateSliders();
        }

        private void HungerTimer()
        {
            time += Time.deltaTime;

            if (time > hungerTimer)
            {
                time = 0;
                hunger -= 1 * hungerMultiplier;

                UpdateSliders();
            }
        }


        // General

        private void Start() => UpdateSliders();

        private void Update()
        {
            HungerTimer();
        }

        private void UpdateSliders()
        {
            hungerBar.maxValue = maxHunger;
            hungerBar.value = hunger;

            healthBar.maxValue = maxHealth;
            healthBar.value = health;

        }
    }
}
