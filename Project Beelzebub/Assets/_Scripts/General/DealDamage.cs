using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class DealDamage : MonoBehaviour
    {

        [SerializeField] private bool isSpiky = true;
        [SerializeField] private float spikeDamage = 1;
        [SerializeField] private float spikeDamageCooldown = 1;
        [SerializeField] private string spikeSound = "Thorn";

        private float spikeTimer = 100;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && isSpiky)
            {
                spikeTimer += Time.deltaTime;
                if (spikeTimer > spikeDamageCooldown)
                {
                    print(MasterAudio.PlaySound(spikeSound));
                    collision.gameObject.GetComponent<PlayerStats>().RemoveHealth(spikeDamage);
                    spikeTimer = 0;
                }
            }
        }

    }
}
