using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class PopupScript : MonoBehaviour
    {
        public ToastManager manager;
        public Sprite sprite;
        [SerializeField] private TMP_Text counter; 
        public float toastLifespan = 5;
        private float timer = 0;
        private int count = 1;

        [SerializeField] private Image image;

        private void Start()
        {
            image.sprite = sprite;
        }

        public void AddCount()
        {
            count++;
            counter.text = count.ToString();
        }

        public void ResetTimer()
        {
            timer = 0;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= toastLifespan)
            {
                manager.RemoveItem(gameObject);
                Destroy(gameObject);
            }
        }

    }
}
