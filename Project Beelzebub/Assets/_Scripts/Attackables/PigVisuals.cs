using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class PigVisuals : MonoBehaviour
    {

        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer rend;

        [SerializeField] private float minMovement = 0.2f;
        
        [SerializeField] private Vector2 lastPos = Vector2.zero;
        [SerializeField] private Vector2 curPos = Vector2.zero;

        [SerializeField] private Vector2 direction;

        private Vector2 lastDir;

        private void Update()
        {
            
            GetDirection();

            SetAnimation();

        }

        private void SetAnimation()
        {
            // Direction
            Vector2 velo = direction;

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

        private void GetDirection()
        {
            lastPos = curPos;
            curPos = transform.position;
        
            direction = curPos - lastPos;

        }

        public void SetHit() => anim.SetTrigger("hit");
      
        public void SetDeath() => anim.SetTrigger("death");

    }
}
