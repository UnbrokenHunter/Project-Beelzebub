using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DarkTonic.MasterAudio;
using Sirenix.Serialization;

namespace ProjectBeelzebub
{
    public class FoodObject : MonoBehaviour
    {
        [Title("General")]
        [SerializeField, LabelWidth(140)] private int maxFood = 3;
        [SerializeField, LabelWidth(140)] private int foodCount = 1;
        [SerializeField, LabelWidth(140)] private float regrowTime = 10;
        [SerializeField, LabelWidth(140)] private string pickupSound;
        [SerializeField, LabelWidth(140)] private bool doesRegrow = true;

        [Title("Destroy")]
        [SerializeField] private bool destroyWhenExausted = false;
        [SerializeField] private string soundOnDestroy = "Destroy";

        [Title("Item to Give")]
        [SerializeField, TableList] private List<DropChance> drops;

        [Title("Sprites")]
        [SerializeField] private bool changeSprite;
        [SerializeField] private List<Sprite> images;

        [Title("Objects")]
        private SpriteRenderer rend;
        private Inventory inventory;


        // Utility
        private float time = 0;


        private void Awake() => rend = GetComponentInChildren<SpriteRenderer>();
        private void Start()
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            InvokeRepeating(nameof(UpdateFood), 0, regrowTime);
        }

        public void AddItem()
        {
            if (foodCount < 0) return;

            // Add drops 
            foreach(DropChance drop in drops)
            {
                var chance = Random.Range(0, 100);
                if (chance < drop.dropChance)
                    AddItemToInventory(drop);

            }

            // Sound
            MasterAudio.PlaySound(pickupSound);

            // Reset regrow timer when you collect to prevent insta respawn
            time = 0;

            // Destroy when out of resources
            if(foodCount <= 0 && destroyWhenExausted)
            {
                MasterAudio.PlaySound(soundOnDestroy);
                Destroy(gameObject);
            }
        }

        private void AddItemToInventory(DropChance drop)
        {
            bool addItem = inventory.AddItem(drop.item);
            foodCount = addItem ? foodCount - 1 : foodCount;
        }


        private void UpdateFood()
        {
            if (foodCount < maxFood && doesRegrow)
            {
                time = 0;
                foodCount += 1;
            }

            if(changeSprite && images.Count > foodCount)
                rend.sprite = images[foodCount];

        }

        [System.Serializable]
        private class DropChance 
        {
            [SerializeField] public InventoryItem item;
            [SerializeField, Range(0, 100)] public float dropChance = 100;
        }
    }
    
}
