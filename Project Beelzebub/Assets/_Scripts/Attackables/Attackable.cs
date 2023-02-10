using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using Pathfinding;

namespace ProjectBeelzebub
{
    public class Attackable : MonoBehaviour
    {

        [Title("Stats")]
        [SerializeField] private float health = 5;
        [SerializeField] private float speedBoost = 1;
        [SerializeField] private float panicTimer = 20;

        [Title("Sounds")]
        [SerializeField] private string hitSound;
        [SerializeField] private string killSound;

        [Title("Scripts")]
        [SerializeField] private Inventory playerInventory;
		[SerializeField] private RunAway run;
		[SerializeField] private Wander wander;

        [Title("Drops")]
		[SerializeField] private List<InventoryItem> drops;

        private IEnumerator Panic()
        {

            AIPath path = GetComponent<AIPath>();

			wander.enabled = false;
			run.enabled = true;

			path.maxSpeed += speedBoost;

            yield return new WaitForSeconds(panicTimer);

            path.maxSpeed -= speedBoost;

			wander.enabled = true;
            run.enabled = false;
		}

		public void RemoveHealth(float amt)
        {
            health -= amt;

            MasterAudio.PlaySound(hitSound);

            StartCoroutine(Panic());

            print($"{gameObject.name} is now at {health}!");

            if(health < 0)
                KillEntity();

        }

        private void KillEntity()
        {
			MasterAudio.PlaySound(hitSound);

            foreach(InventoryItem item in drops)
            {

                playerInventory.AddItem(item);

            }



            // Death Animation
            
            Destroy(gameObject);

		}

	}
}