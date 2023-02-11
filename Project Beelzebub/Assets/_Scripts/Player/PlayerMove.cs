using ProjectBeelzebub;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace ProjectBeelzebulb
{
    public class PlayerMove : MonoBehaviour
    {

        [Title("Stats")]
        [SerializeField] private PlayerStats stats;
        [SerializeField] private PlayerVisuals visuals;
		[SerializeField] private Transform attackOrigin;

        [Title("Debug")]
        [SerializeField] private GameObject deathMenu;
        [SerializeField] private Vector2 movement;
        [SerializeField] private Vector2 lastMovement;
        [SerializeField] private LayerMask mask;


        private Rigidbody2D rb;

        private void Awake() => rb = GetComponent<Rigidbody2D>();

        private void Start() => stats = GetComponent<PlayerStats>();

        private void FixedUpdate()
        {

            lastMovement = movement == Vector2.zero ? lastMovement : movement;

            // No movement if Inventory is open;
            
            if (GetComponent<Inventory>().IsInventoryOpen()) 
            { 
                rb.velocity = Vector2.zero;
                return;
            }

            else if(deathMenu.activeInHierarchy)
            {
                deathMenu.GetComponentInChildren<GameOverMenu>().Move(movement);

                rb.velocity = Vector2.zero;
                return;
            }

            // Add movement
            rb.velocity = Vector2.Lerp(rb.velocity, movement * (stats.maxSpeed * stats.speedMultiplier), 1f - stats.slipperyness);
			rb.velocity -= rb.velocity * stats.deceleration * Time.fixedDeltaTime;

		}

		public void Attack()
        {

            if (GetComponent<Inventory>().IsInventoryOpen())
                return;

            else if (deathMenu.activeInHierarchy)
            {
                deathMenu.GetComponentInChildren<GameOverMenu>().OnFire();
                return;
            }

            // Animation
            visuals.StartAttack();

			RaycastHit2D hit = Physics2D.Raycast(attackOrigin.position, lastMovement, 
                stats.attackRange, mask);

            print($"Attacking: {hit.collider}");

            if(hit.collider != null)
            {

                // Attackable
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    float weapon = stats.weapon == null ? 1 : stats.weapon.damage;

					hit.collider.GetComponent<Attackable>().RemoveHealth(stats.attackMultiplier * weapon);
                }

				// Material
				if (hit.collider.gameObject.tag == "Material")
				{

				}

			}

		}

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;

            Gizmos.DrawLine(attackOrigin.position, attackOrigin.position + 
                new Vector3(lastMovement.x * stats.attackRange, lastMovement.y * stats.attackRange, 0));

        }

        public void OnMove(InputValue value)
        {
            movement = value.Get<Vector2>();
        }

        public void OnFire(InputValue value) => Attack();
    }
}
