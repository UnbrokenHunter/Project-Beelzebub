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
        [SerializeField] private float deathDelay = 1;

        [Title("Sounds")]
        [SerializeField] private string hitSound;
        [SerializeField] private string killSound;

        [Title("Scripts")]
        [SerializeField] private Inventory playerInventory;
        [SerializeField] private PigVisuals visuals;
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

            if (health <= 0)
                KillEntity();

            else
                visuals.SetHit();
        }

        private void KillEntity()
        {
			MasterAudio.PlaySound(hitSound);

            playerInventory = GameObject.Find("Player").GetComponentInChildren<Inventory>();
            foreach(InventoryItem item in drops)
            {
                playerInventory.AddItem(item);

            }

            // Death Animation
            visuals.SetDeath();

            print(visuals.name);

            AIPath path = GetComponent<AIPath>();
            path.canMove = false;

            wander.enabled = false;
            run.enabled = false;

            StartCoroutine(DeathDelay());


		}
        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(deathDelay);
            Destroy(transform.parent.gameObject);
        }

	}
}
