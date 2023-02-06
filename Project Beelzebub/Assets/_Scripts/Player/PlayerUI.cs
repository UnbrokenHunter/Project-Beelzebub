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
		[SerializeField, Range(0, 1)] protected int selectedMenu = 0;

		[Title("Inputs")]
		[SerializeField] protected UnityEvent fire;
		[SerializeField] protected UnityEvent move;
		[SerializeField] protected Vector2 input;

		[Title("UI")]
		[SerializeField] protected GameObject craftMenu;
		[SerializeField] protected GameObject playerMenu;
		[SerializeField] protected GameObject inventoryMenu;

		#region Menu

		private void CycleMenus()
		{

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

			lastInput = curInput;
			curInput = input;

			if (!CheckInputDifferecnce()) return;

			// Movement
			move.Invoke();
			input = value.Get<Vector2>();
			
		}

		#endregion

		#region Helpers

		private Vector2 curInput = Vector2.zero;
		private Vector2 lastInput = Vector2.zero;
		private bool CheckInputDifferecnce()
		{
			if (curInput == Vector2.zero) return false;

			if (lastInput.x == curInput.x) return false;
			else return true;
		}


		#endregion

		#region Player Stats

		protected PlayerStats playerStats;

		private void Awake()
		{
			fire = new UnityEvent();
			move = new UnityEvent();
		}


		private void Start() => playerStats = GetComponent<PlayerStats>();

		#endregion

		#region Getters/Setters

		public bool IsInventoryOpen() => playerMenu.activeInHierarchy;

		#endregion
	}
}
