using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectBeelzebub
{
    public class OpenSettings : MonoBehaviour
    {
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject deathMenu;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private GameObject ui;

        public bool CanOpenSettings()
        {
            if (deathMenu.activeInHierarchy == false || ui.activeInHierarchy == false)
                return true; 
            
            else return false;
        }

        public void OnMenu(InputValue value)
        {
            if (CanOpenSettings())
            {
                if(GameManager.Instance.dialogue != null)
                {
                    GameManager.Instance.dialogue.HideDialogue();
				    GameManager.Instance.playerLookingAtDialogue = false;
				    GameManager.Instance.dialogue = null;
                }


				settings.SetActive(!settings.activeInHierarchy);
                playerUI.selectedMenu = 0;
                playerUI.CycleMenus();
            }
        }
    }
}
