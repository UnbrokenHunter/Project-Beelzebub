using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class DealDamageTrigger : MonoBehaviour
    {

        [SerializeField] public bool isSpiky = true;
        [SerializeField] private float spikeDamage = 1;
        [SerializeField] private float spikeDamageCooldown = 1;
        [SerializeField] private string spikeSound = "Thorn";

        private float spikeTimer = 100;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player" && isSpiky)
            {
                spikeTimer += Time.deltaTime;
                if (spikeTimer > spikeDamageCooldown)
                {
                    MasterAudio.PlaySound(spikeSound);
                    collision.gameObject.GetComponent<PlayerStats>().RemoveHealth(spikeDamage);
                    spikeTimer = 0;
                }
            }
        }

    }
}
