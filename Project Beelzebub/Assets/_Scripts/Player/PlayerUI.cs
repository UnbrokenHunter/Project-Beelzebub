using DarkTonic.MasterAudio;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ProjectBeelzebub
{
    public class PlayerUI : MonoBehaviour
    {

		[Title("Settings")]
		[SerializeField, Range(0, 2)] public int selectedMenu = 0;

		[Title("Inputs")]
		[SerializeField] protected UnityEvent fire;
		[SerializeField] protected UnityEvent move;
		[SerializeField] protected Vector2 input;
        [SerializeField] private float showNextCooldown;
        private float timer;



        [Title("UI")]
		[SerializeField] protected GameObject playerMenu;
		[SerializeField] protected GameObject craftMenu;
		[SerializeField] protected GameObject inventoryMenu;
        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private GameObject settingsMenu;

        private void Update()
        {
            timer += Time.deltaTime;

        }


        #region Menu

        public bool CanOpenInv()
		{
			if (gameOverMenu.activeInHierarchy)
				return false;
			else if (settingsMenu.activeInHierarchy)
                return false;

			else return true;
        }

        public void CycleMenus()
		{

            if (gameOverMenu.activeInHierarchy)
				selectedMenu = 0;
			else if (settingsMenu.activeInHierarchy)
				selectedMenu = 0;

			if(GameManager.Instance.playerLookingAtFire)
			{
				selectedMenu = 0;

				GameManager.Instance.Sleep();
			}

			else if (GameManager.Instance.playerLookingAtDialogue)
			{
				selectedMenu = 0;

				GameManager.Instance.dialogue.ShowNext();
			}


			MasterAudio.PlaySound("OpenInventory");


			// Menu Off
			if (selectedMenu == 0)
			{
				playerMenu.SetActive(false);

				selectedMenu = 1;
			}

			// Inventory
			else if (selectedMenu == 1)
			{
				playerMenu.SetActive(true);
				
				inventoryMenu.SetActive(true);
				craftMenu.SetActive(false);

				selectedMenu = 2;
			}

			// Crafting
			else if(selectedMenu == 2)
			{
				playerMenu.SetActive(true);
				
				inventoryMenu.SetActive(false);
				craftMenu.SetActive(true);

				selectedMenu = 0;
			}

		}

		#endregion

		#region Inputs

		public void OnInventory(InputValue value) => CycleMenus();
		
		public void OnFire(InputValue value)
		{
			if (!IsInventoryOpen()) return;

				fire.Invoke();
		}

		public void OnMove(InputValue value)
		{
			if (!playerMenu.activeInHierarchy) return;

            if (timer < showNextCooldown) return;
            timer = 0;


            lastInput = curInput;
			curInput = input;

			// if (IsInputDifferecnce()) return;

			// Movement
			input = value.Get<Vector2>();
			move.Invoke();
			
		}

		#endregion

		#region Helpers

		private Vector2 curInput = Vector2.zero;
		private Vector2 lastInput = Vector2.zero;
		private bool IsInputDifferecnce()
		{
			if (curInput == Vector2.zero) return false;

			if (lastInput.x == curInput.x) return false;
			else return true;
		}


		#endregion

		#region Player Stats

		protected PlayerStats playerStats;

		private void Start() => playerStats = GetComponent<PlayerStats>();

		#endregion

		#region Getters/Setters

		public bool IsInventoryOpen() => playerMenu.activeInHierarchy;

		#endregion
	}
}
