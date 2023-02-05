using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Raycast : MonoBehaviour
    {

        public
            SpriteRenderer renderer;


        private void Awake()
        {

        }
        private void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

            if (hit.collider != null)
            {
                renderer.enabled = false;
            }

        }
    }
}
