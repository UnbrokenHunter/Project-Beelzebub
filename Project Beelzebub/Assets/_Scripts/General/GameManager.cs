using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private PlayerStats stats;

        [Title("Rescue")]
        [SerializeField] private float rescueTime = 100f;

        [SerializeField, LabelText("Time until rescue")] private float islandTime = 0;
        [SerializeField] private GameObject officer;
        [SerializeField] private Vector3 officerOffset;
        [SerializeField] private bool officerSpawned = false;
        [SerializeField] private GameObject EndGameMenu;

        [Title("Fire")]
        [SerializeField] public bool isFireRunning = false;
        [SerializeField] private float fireMultiplier = 5;
        public bool playerLookingAtFire;

        public bool playerLookingAtRocks;

        [Title("Dialogue")]
        public bool playerLookingAtDialogue;
        public Dialogue dialogue;

        [Title("Sleep")]
        [SerializeField] public float sleepCooldown = 10f;
        [SerializeField] public float sleepTimer = 0;
        [SerializeField] private Animator fade;
        [SerializeField] private DayNight day;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            islandTime += Time.deltaTime * (isFireRunning ? fireMultiplier : 1);
            sleepTimer += Time.unscaledDeltaTime;

            if (islandTime > rescueTime)
            {
                if (!officerSpawned)
                    Rescue();
            }
        }

        #region Sleep

        public void Sleep()
        {
            if (sleepTimer > sleepCooldown)
            {
                sleepTimer = 0;

                fade.SetTrigger("go");

                day.Sleep();
                stats.sleep = stats.maxSleep;
                print("Sleep");
            }
        }

        #endregion

        #region Dialogue



        #endregion

        #region Fire

        public void CheckFire()
        {
            bool anyActive = false;
            for (int i = 0; i < transform.childCount; i++) {

                transform.GetChild(i).TryGetComponent(out FireScript fire);

                if (fire != null)
                {
                    if (fire.fireActive) anyActive = true;
                }

            }

            if (!anyActive)
                isFireRunning = false;
        }

        #endregion

        #region Rescue

        [Button]
        private void Rescue()
        {
            officer.transform.position = officerOffset;
            officerSpawned = true;

            GetComponent<Dialogue>().ShowDialogue();

            StartCoroutine(waitDialogue());

        }

        private IEnumerator waitDialogue()
        {
            yield return new WaitForSeconds(10);
            GetComponent<Dialogue>().HideDialogue();
        }

        public void EndGame()
        {
            fade.SetTrigger("go");

            StartCoroutine(waitForEnd());
        }

        private IEnumerator waitForEnd()
        {
            yield return new WaitForSeconds(4);
            EndGameMenu.SetActive(true);
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(officerOffset, Vector3.one);
        }
    }
}
