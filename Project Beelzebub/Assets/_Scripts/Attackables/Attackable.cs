using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

namespace ProjectBeelzebub
{
    public class Attackable : MonoBehaviour
    {

        [SerializeField] private float health = 5;

        [SerializeField] private string hitSound;
        [SerializeField] private string killSound;

        [SerializeField] private List<InventoryItem> drops;

        public void RemoveHealth(float amt)
        {
            health -= amt;

            MasterAudio.PlaySound(hitSound);

            print($"{gameObject.name} is now at {health}!");

            if(health < 0)
            {
                KillEntity();


            }
        }

        private void KillEntity()
        {
			MasterAudio.PlaySound(hitSound);

            // Death Animation
            
            Destroy(gameObject);

		}

	}
}
