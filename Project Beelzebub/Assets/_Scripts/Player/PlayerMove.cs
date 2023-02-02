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

            // Add movement
            rb.AddForce(movement * acceleration * Time.deltaTime);

            // X movement cap
            if(Mathf.Abs(rb.velocity.x) > maxSpeed)
                rb.velocity = new Vector2(maxSpeed * rb.velocity.normalized.x, rb.velocity.y);

            // Y movement cap
            if (Mathf.Abs(rb.velocity.y) > maxSpeed)
                rb.velocity = new Vector2(rb.velocity.x, maxSpeed * rb.velocity.normalized.y);

            // Stop if going super slow
            if (rb.velocity.magnitude < 0.01f)
                rb.velocity = Vector2.zero;

            // Slow down when not pressing anything
            else if (movement.magnitude == 0 && rb.velocity.magnitude != 0)
                rb.velocity = new(rb.velocity.x / ((deceleration + 50) * Time.deltaTime), rb.velocity.y / ((deceleration + 50) * Time.deltaTime));
            
        }

        public void OnMove(InputValue value) => movement = value.Get<Vector2>();
    }
}
