using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectBeelzebub
{
    public class OpenSettings : MonoBehaviour
    {
        [SerializeField] private GameObject settings;

        public void OnMenu(InputValue value)
        {
            settings.SetActive(!settings.activeInHierarchy);
        }


    }
}
