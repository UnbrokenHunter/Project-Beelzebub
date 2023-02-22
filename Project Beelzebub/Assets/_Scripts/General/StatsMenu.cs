using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class StatsMenu : MonoBehaviour
    {
        [Title("Health")]
        [SerializeField] private GameObject healthObj;
        [SerializeField] private List<GameObject> health;
        [SerializeField] private GameObject healthPrefab;

        [Title("Hunger")]
        [SerializeField] private GameObject hungerObj;
        [SerializeField] private List<GameObject> hunger;
        [SerializeField] private GameObject hungerPrefab;

        [Title("Thirst")]
        [SerializeField] private GameObject thirstObj;
        [SerializeField] private List<GameObject> thirst;
        [SerializeField] private GameObject thirstPrefab;

        [Title("Sleep")]
        [SerializeField] private GameObject sleepObj;
        [SerializeField] private List<GameObject> sleep;
        [SerializeField] private GameObject sleepPrefab;

        [Title("Items")]
        [SerializeField] private GameObject equpt;
        [SerializeField] private Image equptIcon;
        [SerializeField] private TMP_Text equptName;
        [SerializeField] private TMP_Text equptDamage;

        [SerializeField] private PlayerStats stats;

        public void UpdateUI() => ResetUI();

        private void OnEnable()
        {
            ResetUI();
        }

        private void ResetUI()
        {
            foreach (GameObject g in health)
                Destroy(g);

            foreach (GameObject g in hunger)
                Destroy(g);

            foreach (GameObject g in thirst)
                Destroy(g);

            foreach (GameObject g in sleep)
                Destroy(g);

            health.Clear();
            hunger.Clear();
            thirst.Clear();
            sleep.Clear();

            // Health
            for (int i = 0; i < stats.maxHealth; i++)
            {
                GameObject healthTemp = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity, healthObj.transform);
                health.Add(healthTemp);
            }


            // Hunger
            for (int i = 0; i < stats.maxHunger; i++)
            {
                GameObject hungerTemp = Instantiate(hungerPrefab, Vector3.zero, Quaternion.identity, hungerObj.transform);
                hunger.Add(hungerTemp);
            }


            // Thirst
            for (int i = 0; i < stats.maxThirst; i++)
            {
                GameObject thirstTemp = Instantiate(thirstPrefab, Vector3.zero, Quaternion.identity, thirstObj.transform);
                thirst.Add(thirstTemp);
            }


            // Sleep
            for (int i = 0; i < stats.maxSleep; i++)
            {
                GameObject thirstTemp = Instantiate(sleepPrefab, Vector3.zero, Quaternion.identity, sleepObj.transform);
                sleep.Add(thirstTemp);
            }


            // Item
            if(stats.weapon != null)
            {
                equptIcon.sprite = stats.weapon.sprite;
                equptName.text = stats.weapon.name;
                equptDamage.text = "Damage: " + stats.weapon.damage.ToString();
                equptIcon.enabled = true;
            }
            else
                equptIcon.enabled = false;

            // Grey Out
            for (int i = 0; i < stats.maxHealth; i++)
            {
                if (i >= stats.health) health[i].GetComponent<Image>().color = stats.darkness;
                else health[i].GetComponent<Image>().color = Color.white;
            }

            for (int i = 0; i < stats.maxHunger; i++)
            {
                if (i >= stats.hunger) hunger[i].GetComponent<Image>().color = stats.darkness;
                else hunger[i].GetComponent<Image>().color = Color.white;
            }

            for (int i = 0; i < stats.maxThirst; i++)
            {
                if (i >= stats.thirst) thirst[i].GetComponent<Image>().color = stats.darkness;
                else thirst[i].GetComponent<Image>().color = Color.white;
            }

            for (int i = 0; i < stats.maxSleep; i++)
            {
                if (i >= stats.sleep) sleep[i].GetComponent<Image>().color = stats.darkness;
                else sleep[i].GetComponent<Image>().color = Color.white;
            }


        }


    }
}
