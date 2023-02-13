using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Title("Rescue")]
        [SerializeField] private float rescueTime = 100f;
        [SerializeField] public bool isFireRunning = false;
        [SerializeField] private float fireMultiplier = 5;

        [SerializeField, LabelText("Time until rescue")] private float islandTime = 0;

        [Title("Fire")]
        public bool playerLookingAtFire;

        [Title("Sleep")]
        [SerializeField] private float sleepCooldown = 10f;
        [SerializeField] private float sleepTimer = 0;

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
            sleepTimer += Time.deltaTime;

            if (islandTime > rescueTime)
            {
                Rescue();
            }
        }

        #region Sleep

        public void Sleep()
        {
            if(sleepTimer > sleepCooldown)
            {
                sleepTimer = 0;

                print("Sleep");
            }    
        }

        #endregion

        #region Fire

        public void CheckFire()
        {
            bool anyActive = false;
            for (int i = 0; i < transform.childCount; i++) {

                transform.GetChild(i).TryGetComponent(out FireScript fire);
                
                if(fire != null )
                {
                    if(fire.fireActive) anyActive = true;
                }

            }

            if(!anyActive)
                isFireRunning = false;
        }

        #endregion

        #region Rescue

        private void Rescue()
        {

        }

        #endregion

    }
}
