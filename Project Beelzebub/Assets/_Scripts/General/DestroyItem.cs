using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class DestroyItem : MonoBehaviour
    {

        public void DestroyItemNow()
        {
            Destroy(gameObject);
        }

    }
}
