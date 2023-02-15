using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DarkTonic.MasterAudio;

namespace ProjectBeelzebub
{
    public class FoodObject : MonoBehaviour
    {

        [Title("General")]
        [SerializeField] private int maxFood = 3;
        [SerializeField] private int foodCount = 1;
        [SerializeField] private float regrowTime = 10;
        [SerializeField] private string pickupSound;
        [SerializeField] private bool doesRegrow = true;

        [Title("Destroy")]
        [SerializeField] private bool destroyWhenExausted = false;
        [SerializeField] private string soundOnDestroy = "Destroy";

        [Title("Item to Give")]
        [SerializeField] private InventoryItem food;

        [Title("Sprites")]
        [SerializeField] private bool changeSprite;
        [SerializeField] private List<Sprite> images;

        [Title("Objects")]
        private SpriteRenderer rend;
        [SerializeField] private Inventory inventory;


        // Utility
        private float time = 0;


        private void Awake() => rend = GetComponentInChildren<SpriteRenderer>();

        public void AddFood()
        {
            if (foodCount > 0)
            {
                bool addItem = inventory.AddItem(food);
                foodCount = addItem ? foodCount - 1 : foodCount;

                print(MasterAudio.PlaySound(pickupSound));

                time = 0;

                if(foodCount <= 0 && destroyWhenExausted)
                {
                    MasterAudio.PlaySound(soundOnDestroy);
                    Destroy(gameObject);
                }
            }
        }


        private void Update()
        {
            time += Time.deltaTime;

            if (time > regrowTime && foodCount < maxFood && doesRegrow)
            {
                time = 0;
                foodCount += 1;
            }

            if(changeSprite)
                rend.sprite = images[foodCount];

        }
    }
}
