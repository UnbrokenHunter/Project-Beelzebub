using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class BeastScript : MonoBehaviour
    {
        [SerializeField] private string spawnSound = "Creepy";
        [SerializeField] private string deathSound = "Creepy";

        private void Start()
        {
            MasterAudio.PlaySound(spawnSound);
        }

        private void DestroyObject()
        {
            MasterAudio.PlaySound(deathSound);
            GetComponent<MMF_Player>().PlayFeedbacks();
        }

        public IEnumerator StartLifespan(float beastLifespan)
        {
            yield return new WaitForSeconds(beastLifespan);
            DestroyObject();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
                DestroyObject();
        }
    }
}
