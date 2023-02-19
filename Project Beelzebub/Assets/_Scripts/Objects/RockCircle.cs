using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class RockCircle : MonoBehaviour
    {
        
        [SerializeField] private bool hasFire = false;
        [SerializeField] private GameObject fire;
        [SerializeField] private GameObject firePrefab;
        [SerializeField] private Vector3 spawnLocation;
        [SerializeField] private GameObject popup;

        public void ShowPopup()
        {
            if(!hasFire)
            {
                popup.SetActive(true);

            }
        }

        public void HidePopup()
        {
            popup.SetActive(false);
        }

        public void PlaceFire()
        {
            if (!hasFire)
            {
                print("Place Fire");
                fire = Instantiate(firePrefab, spawnLocation + transform.position, Quaternion.identity, transform);
                hasFire = true;

                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(spawnLocation + transform.position, .1f);
        }
    }
}
