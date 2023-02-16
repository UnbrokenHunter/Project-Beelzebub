using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

namespace ProjectBeelzebub
{
    public class SkipCutscene : MonoBehaviour
    {

        [SerializeField] private float waitTime = 110;
        private bool hasSkipped = false;

        [SerializeField] private List<GameObject> objectsToEnable;
        [SerializeField] private List<GameObject> objectsToDisable;

        [SerializeField] private PlayerInput playerInput;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(110);
            Skip();
        }
        [Button]
        public void Skip()
        {
            if (hasSkipped) return;

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }

            playerInput.enabled = false;
            playerInput.enabled = true;

            hasSkipped = true;

            gameObject.SetActive(false);
        }

    }
}
