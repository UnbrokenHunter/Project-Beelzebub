using ProjectBeelzebub;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectBeelzebulb
{
    public class PlayerMove : MonoBehaviour
    {

        [Title("Speed")]
        [SerializeField] private float maxSpeed = 5;
        [SerializeField] private float acceleration = 90;
        [SerializeField] private float deceleration = 60;

        [SerializeField]
        private Vector2 movement;

        private Rigidbody2D rb;

        private void Awake() => rb = GetComponent<Rigidbody2D>();

        private void FixedUpdate()
        {

            // Stop if going super slow
            if (rb.velocity.magnitude < 0.01f)
                rb.velocity = Vector2.zero;

            // Slow down X
            if (movement.x < 0.01f)
                rb.velocity = new(rb.velocity.x / ((deceleration + 50) * Time.deltaTime), rb.velocity.y);

            // Slow down Y
            if (movement.y < 0.01f)
                rb.velocity = new(rb.velocity.x, rb.velocity.y / ((deceleration + 50) * Time.deltaTime));

            // No movement if Inventory is open;
            if (GetComponent<Inventory>().IsInventoryOpen()) return;

            // Add movement
            rb.velocity += (movement * acceleration * Time.deltaTime);

            // X movement cap
            if(Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(maxSpeed * rb.velocity.normalized.x, rb.velocity.y);

            // Y movement cap
            if (Mathf.Abs(rb.velocity.y) > maxSpeed)
                rb.velocity = new Vector2(rb.velocity.x, maxSpeed * rb.velocity.normalized.y);

            
        }

        public void OnMove(InputValue value) => movement = value.Get<Vector2>();
    }
}
