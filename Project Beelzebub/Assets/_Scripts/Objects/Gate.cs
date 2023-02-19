using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private InventoryItem keyType;
        [SerializeField] private string OpenSound;

        public InventoryItem GetKeyType() => keyType;

        public void ShowPopup()
        {
            popup.SetActive(true);
        }

        public void HidePopup()
        {
            popup.SetActive(false);
        }

        public void OpenGate()
        {
            print(MasterAudio.PlaySound(OpenSound));
            GetComponent<MMF_Player>().PlayFeedbacks();
            Destroy(gameObject);
        }
    }
}
