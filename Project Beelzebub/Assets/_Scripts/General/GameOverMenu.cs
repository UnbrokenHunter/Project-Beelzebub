using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class GameOverMenu : MonoBehaviour
    {
        [Title("Game Specific")]
        [SerializeField] private GameObject menu;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private CapsuleCollider2D col;

        [Title("General")]
        [SerializeField] private int elementCount;
        [SerializeField] private int selected;

        [SerializeField] private string clickSound = "Click";

        [SerializeField] private List<GameObject> buttons;
        [SerializeField] private List<UnityEvent> events;

        [SerializeField] private Sprite spriteUnselected;
        [SerializeField] private Sprite spriteSelected;

        [Title("Text Offset")]
        [SerializeField] private bool hasTextOffset = true;
        [SerializeField] private float textOffsetAmount = 10;

		[Title("Event Sys")]
		[SerializeField] private GameObject buttonGameObj;
		private GameObject currentSelGameObjTemp;

		private Vector2 movement;
		private bool canMove = true;

		private void OnEnable()
		{
			currentSelGameObjTemp = EventSystem.current.currentSelectedGameObject;
			EventSystem.current.SetSelectedGameObject(buttonGameObj);

		}

		private void OnDisable()
		{
			EventSystem.current.SetSelectedGameObject(currentSelGameObjTemp);

		}

		private void ClickSelected()
        {
            print(MasterAudio.PlaySound(clickSound));
            events[selected].Invoke();
        }

        #region Button Events

        public void Respawn()
        {
            stats.Reset();
            col.enabled = transform;
            menu.SetActive(false);
        }

        public void Menu()
        {
            LevelManager.Instance.LoadScene("Title");
        }

        public void Play()
        {
            LevelManager.Instance.LoadScene("Main");
        }

        public void Settings()
        {

        }

        #endregion

        public void Move(Vector2 value)
        {
            movement = value;

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].GetComponent<Image>().sprite = i == selected ? spriteSelected : spriteUnselected;
                if(hasTextOffset) buttons[i].transform.GetChild(0).gameObject.transform.localPosition = i == selected ? new Vector3(0, textOffsetAmount, 0) : Vector3.zero ;
            }

        }

        private void Update()
        {
            SetMovement();
        }

        private void SetMovement()
        {
            if(canMove && movement != Vector2.zero)
            {
                canMove = false;

                int dir = 0;
                if (movement.x > 0)
                    dir = 1;
                else if (movement.x < 0)
                    dir = -1;


                selected = Mathf.Clamp(selected + dir, 0, elementCount - 1);
            }
            else
            {
                if(movement == Vector2.zero)
                    canMove = true;
            }

        }

        public void OnMove(InputValue value) => Move(value.Get<Vector2>());

        public void OnFire(InputValue value) => ClickSelected();


    }
}
