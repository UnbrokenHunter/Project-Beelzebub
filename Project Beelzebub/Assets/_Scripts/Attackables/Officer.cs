using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Officer : MonoBehaviour
    {

        public void StartEnd()
        {
            print("Game Over");

            GameManager.Instance.EndGame();
        }


    }
}
