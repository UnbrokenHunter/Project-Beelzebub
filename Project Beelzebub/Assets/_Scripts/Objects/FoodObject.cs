using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DarkTonic.MasterAudio;

namespace ProjectBeelzebub
{
    public class FoodObject : MonoBehaviour
    {

        [Title("Food Settings")]
        [SerializeField] private int maxFood = 3;
        [SerializeField] private int foodCount = 1;
        [SerializeField] private float regrowTime = 10; 
        [SerializeField] private InventoryItem food;

        [SerializeField] private bool isSpiky = true;
        [SerializeField] private float spikeDamage = 1;
        [SerializeField] private float spikeDamageCooldown = 1;
        [SerializeField] private string spikeSound = "Thorn+";

        private float spikeTimer = 100;

        [SerializeField] private bool changeSprite;
        [SerializeField] private List<Sprite> images;


        [SerializeField] private SpriteRenderer rend;
        [SerializeField] private Inventory inventory;

        [SerializeField] private string pickupSound;

        // Utility
        private float time = 0;


        private void Awake() => rend = GetComponentInChildren<SpriteRenderer>();

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                spikeTimer += Time.deltaTime;
                if (spikeTimer > spikeDamageCooldown)
                {
                    print(MasterAudio.PlaySound(spikeSound)); 
                    collision.gameObject.GetComponent<PlayerStats>().RemoveHealth(spikeDamage);
                    spikeTimer = 0;
                }
            }
        }

        public void AddFood()
        {
            if (foodCount > 0)
            {
                bool addItem = inventory.AddItem(food);
                foodCount = addItem ? foodCount - 1 : foodCount;
                print(MasterAudio.PlaySound(pickupSound));

                time = 0;
            }
        }


        private void Update()
        {
            time += Time.deltaTime;

            if (time > regrowTime && foodCount < maxFood)
            {
                time = 0;
                foodCount += 1;
            }

            if(changeSprite)
                rend.sprite = images[foodCount];

        }
    }
}
