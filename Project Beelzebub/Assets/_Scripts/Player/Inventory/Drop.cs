using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Drop : MonoBehaviour
    {
        public InventoryItem item;
        private bool triggerable = false;
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);
            triggerable = true;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!triggerable) return;

            if(collision.gameObject.tag == "Player")
            {
                collision.GetComponent<Inventory>().AddItem(item);

                Destroy(gameObject);
            }
        }
    }
}
