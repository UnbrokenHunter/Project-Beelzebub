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

        private Vector2 lastDir;

        private void Awake()
        {
            rb = GetComponentInParent<Rigidbody2D>();
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
            Vector2 velo = rb.velocity;

            if (Mathf.Abs(velo.x) < minMovement) velo = new Vector2(0, velo.y);
            if (Mathf.Abs(velo.y) < minMovement) velo = new Vector2(velo.x, 0);

            velo.Normalize();

            if (velo != Vector2.zero) lastDir = velo;   


            // Set State
            anim.SetBool("moving", velo.magnitude > 0);


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
            anim.SetBool("respawn", true);

            print("Respawn");

            yield return new WaitForSeconds(1f);

            anim.SetBool("respawn", false);
        }

    }
}
