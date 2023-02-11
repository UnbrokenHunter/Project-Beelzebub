using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class PopupScript : MonoBehaviour
    {

        public Sprite sprite;
        public float toastLifespan = 5;

        [SerializeField] private Image image;

        private void Start()
        {
            image.sprite = sprite;
            StartCoroutine(lifespan());
        }

        private IEnumerator lifespan()
        {
            yield return new WaitForSeconds(toastLifespan);
            Destroy(gameObject);
        }


    }
}
