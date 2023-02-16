using DarkTonic.MasterAudio;
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
        [SerializeField] private Inventory inv;
        [SerializeField] private PlayerVisuals visuals;
		[SerializeField] private Transform attackOrigin;
        private float attackTimer = 0;

        [Title("Debug")]
        [SerializeField] private GameObject deathMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] public Vector2 movement;
        [SerializeField] public Vector2 lastMovement;
        [SerializeField] private LayerMask mask;

        [Title("Fire")]
        [SerializeField] private InventoryItem fuel;
        [SerializeField] private InventoryItem starter;


        private Rigidbody2D rb;

        private void Awake() => rb = GetComponent<Rigidbody2D>();

        private void Start() => stats = GetComponent<PlayerStats>();

        private void FixedUpdate()
        {

            lastMovement = movement == Vector2.zero ? lastMovement : movement;

            // No movement if Inventory is open;
            if (!CanMove())
            {
                rb.velocity = Vector2.zero;
                return;
            }

            // Add movement
            rb.velocity = Vector2.Lerp(rb.velocity, movement * (stats.maxSpeed * stats.speedMultiplier), 1f - stats.slipperyness);
			rb.velocity -= rb.velocity * stats.deceleration * Time.fixedDeltaTime;

		}

        private void Update()
        {
            attackTimer += Time.deltaTime;
        }
        public void Attack()
        {
            if (GetComponent<Inventory>().IsInventoryOpen())
                return;

            else if (deathMenu.activeInHierarchy)
                return;

            if (attackTimer < stats.attackCooldown) return;
            attackTimer = 0;

            print(MasterAudio.PlaySound("attack"));

            // Animation
            visuals.StartAttack();

			RaycastHit2D hit = Physics2D.Raycast(attackOrigin.position, lastMovement, stats.attackRange, mask);

            if(hit.collider != null)
            {

                // Click previews in ControlTooltips.cs

                // Attackable
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    float weapon = stats.weapon == null ? 1 : stats.weapon.damage;

					hit.collider.GetComponent<Attackable>().RemoveHealth(stats.attackMultiplier * weapon);
                }

				// Material
				if (hit.collider.gameObject.tag == "Material")
				{
                    hit.collider.gameObject.GetComponent<FoodObject>().AddItem();
				}

                // Fire
                if (hit.collider.gameObject.tag == "Fire")
                {
                    print("Fire");

                    if(hit.collider.gameObject.GetComponent<FireScript>().fireActive && inv.CheckMaterial(fuel) > 0)
                    {
                        inv.RemoveItem(fuel, 1);
                        hit.collider.gameObject.GetComponent<FireScript>().AddFuel(1);
                    }
                    else if (inv.CheckMaterial(fuel) > 0 && inv.CheckMaterial(starter) > 0)
                    {
                        inv.RemoveItem(fuel, 1);

                        hit.collider.gameObject.GetComponent<FireScript>().StartFire(1);
                    }

                }

                if (hit.collider.gameObject.tag == "NPC")
                {
                    hit.collider.gameObject.TryGetComponent(out Littlun little);

                    if (little != null)
                    {
                        little.Attacked();
                    }

				}

            }

        }

        public Vector2 GetMovement()
        {
            return lastMovement = movement == Vector2.zero ? lastMovement : movement;
        }

        public bool CanMove()
        {
            if (GetComponent<Inventory>().IsInventoryOpen())
            {
                return false;
            }

            else if (deathMenu.activeInHierarchy)
            {
                return false;
            }

            else if (settingsMenu.activeInHierarchy)
            {
                return false;
            }

            return true;
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
    
        public void OnMenu(InputValue value)
        {
            settingsMenu.transform.parent.GetComponent<OpenSettings>().OnMenu(value);
        }
    }
}
