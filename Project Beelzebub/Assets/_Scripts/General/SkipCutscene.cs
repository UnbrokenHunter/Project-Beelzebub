using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ProjectBeelzebub
{
    public class SkipCutscene : MonoBehaviour
    {

        [SerializeField] private string sceneNameToTransition;
        [SerializeField] private string soundToPlay;

        [SerializeField] private float waitTime = 110;
        [SerializeField] private bool isPlaying = false;

		[SerializeField] private List<GameObject> objectsToEnable;
		[SerializeField] private List<GameObject> objectsToDisable;

		[SerializeField] private List<GameObject> objectsToEnableBefore;
		[SerializeField] private List<GameObject> objectsToDisableBefore;

        public void ButtonClick()
        {
            MasterAudio.PlaySound("OpenInventory");
        }

        public void StartCutscene()
        {
			if (isPlaying)
            {
                Skip();
                return;
            }

            print("Start");
            MasterAudio.FadeBusToVolume("Music", 0, 5);

			foreach (GameObject obj in objectsToDisableBefore)
			{
				obj.SetActive(false);
			}

			foreach (GameObject obj in objectsToEnableBefore)
			{
				obj.SetActive(true);
			}

            MasterAudio.PlaySound(soundToPlay);

			LevelManager.Instance.LoadSceneAsync(sceneNameToTransition);
			StartCoroutine(StartSkipCountdown());
		}


        private IEnumerator StartSkipCountdown()
        {
            isPlaying = true;

            yield return new WaitForSecondsRealtime(waitTime);
            print("End Wait");
            Skip();
        }
        [Button]
        public void Skip()
        {
            print("Skip");

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(true);
            }

            LevelManager.Instance.AllowSceneComplete();
        }

    }
}
