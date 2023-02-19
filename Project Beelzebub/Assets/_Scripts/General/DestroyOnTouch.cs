using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class DestroyOnTouch : MonoBehaviour
    {
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag != "Beast")
            {
                print("Destroy Object");
                Destroy(gameObject.transform.parent.gameObject);

            }
        }
    }
}
