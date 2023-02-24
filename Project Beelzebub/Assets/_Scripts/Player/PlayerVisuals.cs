using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using ProjectBeelzebulb;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class PlayerVisuals : MonoBehaviour
    {

        [SerializeField] private float minMovement = 0.2f;


        [Title("Other")]
        [SerializeField] private ParticleSystem footstep;
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer rend;
        private Rigidbody2D rb;
        private PlayerMove move;

        private Vector2 lastDir;

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody2D>();
        }

        private void Start()
        {
            move = GetComponentInParent<PlayerMove>();
        }

        private void Update() => SetAnimations();

        public void PlayFootsteps()
        {
            MasterAudio.PlaySound("Footstep");
            footstep.Play();

		}

        private void SetAnimations()
        {
            // Direction
            Vector2 movement = move.movement;
            Vector2 velo = rb.velocity;

            // Set last movement
            if (movement != Vector2.zero) lastDir = movement;

            // Set State
            anim.SetBool("moving", velo.magnitude > minMovement);

            // Set Direction
            anim.SetFloat("moveX", lastDir.x);
            anim.SetFloat("moveY", lastDir.y);


            // Flix x
            rend.flipX = lastDir.x < 0;
        }


        public void StartAttack() => anim.SetTrigger("attack");

        public void StartDeath() => anim.SetTrigger("death");

        public IEnumerator StartRespawn()
        {
            GetComponentInParent<Inventory>().DropItems();

            anim.SetBool("respawn", true);

            print("Respawn");

            yield return new WaitForSeconds(1f);

            anim.SetBool("respawn", false);

            GetComponentInParent<Dialogue>().ShowDialogue("You items were dropped where you died. Go and pick them up!");

            yield return new WaitForSeconds(5);

           GetComponentInParent<Dialogue>().HideDialogue();
        }

    }
}
