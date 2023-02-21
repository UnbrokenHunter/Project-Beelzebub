using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using Pathfinding;
using MoreMountains.Feedbacks;

namespace ProjectBeelzebub
{
    public class Attackable : MonoBehaviour
    {

        [Title("Stats")]
        [SerializeField] private float health = 5;
        [SerializeField] private float speedBoost = 1;
        [SerializeField] private float panicTimer = 20;
        [SerializeField] private float deathDelay = 1;

        [Title("Attacks")]
        [SerializeField] private bool doesAttack = false;
        [SerializeField] private float damage = 1;
        [SerializeField] private float attackCooldown = 1;
        private float cooldownCount = 0;

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

        [Title("Juice")]
        [SerializeField] private Color flickerColor;
        [SerializeField] private ParticleSystem blood;
        [SerializeField] private MMF_Player feedback;

        private bool isDead = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(!isDead && doesAttack)
            {
                if (collision.collider.gameObject.CompareTag("Player"))
                {
                    print(MasterAudio.PlaySound("PigAttack"));

                    visuals.SetAttack();

                    AttackPlayer(collision.collider.gameObject);
				}
			}
		}

        private void AttackPlayer(GameObject player)
        {
            if(cooldownCount > attackCooldown)
            {
				player.GetComponent<PlayerStats>().RemoveHealth(damage);
                cooldownCount = 0;
            }
		}

        private void Update() => cooldownCount += Time.deltaTime;

        private IEnumerator Panic()
        {

            AIPath path = GetComponent<AIPath>();
            blood.Play();
			wander.enabled = false;
			run.enabled = true;

			path.maxSpeed = speedBoost;

            yield return new WaitForSeconds(panicTimer);

            path.maxSpeed = speedBoost;
            blood.Pause();
			wander.enabled = true;
            run.enabled = false;
		}

		public void RemoveHealth(float amt)
        {
            health -= amt;

            MasterAudio.PlaySound(hitSound);


            StartCoroutine(Panic());

			feedback?.PlayFeedbacks();

            StartCoroutine(Flicker());

            print($"{gameObject.name} is now at {health}!");

            if (health <= 0)
                KillEntity();

            else
                visuals.SetHit();
        }

        private IEnumerator Flicker()
        {

            SpriteRenderer r = GetComponentInChildren<SpriteRenderer>();
            for(int i =0; i < 5; i++)
            {
                r.color = flickerColor;
                yield return new WaitForSeconds(0.05f);
                r.color = Color.white;
                yield return new WaitForSeconds(0.05f);

            }
        }

        private void KillEntity()
        {
            if (isDead) return;
            isDead = true;
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
