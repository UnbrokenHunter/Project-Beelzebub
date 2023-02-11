using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private Inventory inventory;
        [SerializeField] private PlayerStats stats;

        [SerializeField] private int selected;

        [SerializeField] private List<GameObject> buttons;

        [SerializeField] private Sprite spriteUnselected;
        [SerializeField] private Sprite spriteSelected;

        private Vector2 movement;


        private void ClickSelected()
        {
            if(selected == 0)
                Respawn();
            

            else if(selected == 1)
                Menu();
        }

        [Button]
        public void Respawn()
        {
            stats.Reset();

            menu.SetActive(false);
        }

        public void Menu()
        {

        
        }

        public void Move(Vector2 value)
        {
            movement = value;

            if(movement.x > 0)
            {
                selected = 1; 
            }
            else if (movement.x < 0)
            {
                selected = 0;
            }

            if (selected == 0)
            {
                buttons[0].GetComponent<Image>().sprite = spriteSelected;
                buttons[1].GetComponent<Image>().sprite = spriteUnselected;
            }
            else if (selected == 1)
            {
                buttons[1].GetComponent<Image>().sprite = spriteSelected;
                buttons[0].GetComponent<Image>().sprite = spriteUnselected;

            }

        }

        public void OnFire() => ClickSelected();


    }
}
