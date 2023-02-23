using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private PlayerStats stats;

        [Title("Beast")]
        [SerializeField] private GameObject beast;
        [SerializeField] private MMF_Player spawnBeastFeedback;
        [SerializeField] private List<Vector2> spawnLocations;
        [SerializeField] private float isNightAt = 80f;
        [SerializeField] private float beastLifespan = 50;
        [SerializeField] private float beastTimer = 10;
        private float beastCount;

        // TEMP
        private Vector3 spawnPosition;

        [Title("Tutorial")]
        [SerializeField] private CinemachineVirtualCamera spawnCam;
        [SerializeField] private Sprite playerPortrait;
        [SerializeField] private string[] dialogues;
        [SerializeField] private float[] waitTimes;

        [Title("Rescue")]
        [SerializeField] private float rescueTime = 100f;

        [SerializeField, LabelText("Time until rescue")] private float islandTime = 0;
        [SerializeField] private GameObject officer;
        [SerializeField] private Vector3 officerOffset;
        [SerializeField] private bool officerSpawned = false;
        [SerializeField] private bool gameOver = false;
        [SerializeField] private GameObject EndGameMenu;
        [SerializeField] private GameObject Player;
        [SerializeField] private GameObject UI;
        [SerializeField] private GameObject dialogueObj;
        [SerializeField] private CinemachineVirtualCamera vcam;

        [Title("Fire")]
        [SerializeField] public string fireTimer;
        [SerializeField] private TMP_Text fireTimerUI;
        [SerializeField] public bool isFireRunning = false;
        [SerializeField] public bool hasFireRun = false;
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
                Instance = this;
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

            UpdateFireTimer();

            CheckBeast();
        }

        private void Start()
        {
            StartCoroutine(SpawnCam());
            StartCoroutine(StartTutorial());
        }

        #region Tutorial

        private IEnumerator SpawnCam()
        {
            spawnCam.Priority = 11;

            yield return new WaitForSeconds(1f);

            spawnCam.Priority = 9;

		}

        private IEnumerator StartTutorial()
        {

            for (int i = 0; i < dialogues.Length; i++)
            {
                GetComponent<Dialogue>().ShowDialogue(dialogues[i], playerPortrait);
				yield return new WaitForSeconds(waitTimes[i]);
                GetComponent<Dialogue>().HideDialogue();
            }



		}

        #endregion

        #region Sleep

        public void Sleep()
        {
            if (sleepTimer > sleepCooldown)
            {
                sleepTimer = 0;

                fade.SetTrigger("go");

                day.Sleep();
                stats.AddSleep();
                print("Sleep");
            }
        }

        #endregion

        #region Beast

        private void CheckBeast()
        {
            beastCount += Time.deltaTime;
            
            if (day.CurrentPercentage() >= isNightAt)
            {

                if (beastCount > beastTimer)
                {
                    StartCoroutine(SpawnBeast());
                    beastCount = 0;
                }
            }
        }

        [Button]
        public void InstaSpawnBeast() => StartCoroutine(SpawnBeast());

        private IEnumerator SpawnBeast()
        {
            MasterAudio.FadeAllPlaylistsToVolume(0, 10);

            yield return new WaitForSeconds(15);

            Vector2 playerPos = stats.transform.position;
            float minDistance = 1000;
            foreach (Vector2 beastPos in spawnLocations)
            {
                if(Vector2.Distance(playerPos, beastPos) < minDistance)
                {
                    minDistance = Vector2.Distance(playerPos, beastPos);
                    spawnPosition = beastPos;
                }
                print(minDistance);
            }

            spawnBeastFeedback?.PlayFeedbacks();

            yield return new WaitForSeconds(0.5f);

            GameObject beastObj = Instantiate(beast, spawnPosition, Quaternion.identity, transform);
            StartCoroutine(beastObj.GetComponent<BeastScript>().StartLifespan(beastLifespan));

        }

        #endregion

        #region Fire

        private void UpdateFireTimer()
        {
            if(isFireRunning)
            {
                fireTimerUI.text = fireTimer;
                fireTimerUI.gameObject.SetActive(true);
            }
            else if (hasFireRun)
            {
                fireTimerUI.text = "The fire was put out!";
                fireTimerUI.gameObject.SetActive(true);
            }
            else
            {
                fireTimerUI.gameObject.SetActive(false);
            }

        }

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

            GetComponent<Dialogue>().HideDialogue();
            GetComponent<Dialogue>().ShowDialogue("Rescue has arrived! Come over here!");
            vcam.Priority = 11;

            StartCoroutine(waitDialogue());

        }

        private IEnumerator waitDialogue()
        {
            yield return new WaitForSeconds(5);
            vcam.Priority = 9;
            GetComponent<Dialogue>().HideDialogue();
        }

        public void EndGame()
        {
            if(gameOver) return; 
                gameOver = true;


            fade.SetTrigger("go");

            StartCoroutine(waitForEnd());
        }

        private IEnumerator waitForEnd()
        {
            spawnCam.Priority = 11;

            yield return new WaitForSeconds(4);


            EndGameMenu.SetActive(true);
            dialogueObj.SetActive(false);
            Player.SetActive(false);
            UI.SetActive(false);


            yield return new WaitForSeconds(15);

            LevelManager.Instance.LoadScene("Title");

        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(officerOffset, Vector3.one);

            foreach (Vector2 spawn in spawnLocations)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(spawn, Vector3.one);
            }

        }
    }
}
