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

        [SerializeField] private string pickupSound;

        // Utility
        private float time = 0;

        private BoxCollider2D box;

        private void Awake() => box = GetComponent<BoxCollider2D>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (foodCount > 0)
                {
                    bool addItem = collision.GetComponent<Inventory>().AddItem(food);
                    foodCount = addItem ? foodCount - 1 : foodCount;
                    print(MasterAudio.PlaySound(pickupSound));
                }
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
        }
    }
}
