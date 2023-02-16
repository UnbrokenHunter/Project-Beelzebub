using System.Collections;
using System.Collections.Generic;
using ProjectBeelzebulb;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class ControlTooltip : MonoBehaviour
    {

        [SerializeField] private GameObject move;
        [SerializeField] private GameObject attack;
        [SerializeField] private GameObject inv;
        [SerializeField] private GameObject settings;

        [SerializeField] private bool leftie = false;

        [Title("Sprites")]
        [Title("Left")]
		[SerializeField] private Sprite arrowKeys;
		[SerializeField] private Sprite p;
		[SerializeField] private Sprite l;

		[Title("Right")]
		[SerializeField] private Sprite wasd;
		[SerializeField] private Sprite e;
		[SerializeField] private Sprite q;

		[Title("Attack")]
        [SerializeField] private PlayerMove playerMove;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask mask;

        [Title("Settings")]
        [SerializeField] private OpenSettings settingsContainer;

        [Title("Move")]
        [SerializeField] private float movementSpacing;
        [SerializeField] private float navigationSpacing;

        [Title("Attack")]
        [SerializeField] private Vector2 attackoriginalSizeD;
        [SerializeField] private Vector2 attacksizeD;
        [SerializeField] private float oldSpace;
        [SerializeField] private float newSpace;
        [SerializeField] private FireScript current;

        public void SwapLeftie()
        {
            // Set to left
            if(leftie)
            {
				move.GetComponentInChildren<Image>().sprite = arrowKeys;
				inv.GetComponentInChildren<Image>().sprite = l;
				settings.GetComponentInChildren<Image>().sprite = p;

				leftie = false;
            }
            // Set to right
            else
            {
				move.GetComponentInChildren<Image>().sprite = wasd;
				inv.GetComponentInChildren<Image>().sprite = e;
				settings.GetComponentInChildren<Image>().sprite = q;

				leftie = true;
            }
        }

        private void FixedUpdate()
        {
            CanAttack(playerMove.GetMovement());

            CanMove(playerMove.CanMove());

            CanSettings(settingsContainer.CanOpenSettings());

            CanInv();
        }

        public void CanSettings (bool canOpen)
        {
            if(canOpen)
            {
                settings.SetActive(true);
            }
            else { settings.SetActive(false); }
        }

        public void CanMove(bool canMove)
        {
            if(canMove)
            {
                move.SetActive(true);
                move.GetComponent<HorizontalLayoutGroup>().spacing = movementSpacing;
                move.GetComponentInChildren<TMP_Text>().text = "Move:";
            }
            else
            {
                move.GetComponent<HorizontalLayoutGroup>().spacing = navigationSpacing;
                move.SetActive(true);
                move.GetComponentInChildren<TMP_Text>().text = "Navigate:";
            }
        }

        public void CanAttack(Vector2 lastMovement)
        {
            RaycastHit2D hit = Physics2D.Raycast(attackOrigin.position, lastMovement, stats.attackRange, mask);

            if (hit.collider != null)
            {

                // Attackable
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    attack.SetActive(true);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = oldSpace;
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attackoriginalSizeD.x, attackoriginalSizeD.y);

                    attack.GetComponentInChildren<TMP_Text>().text = "Attack:";
                }

                // Material
                if (hit.collider.gameObject.tag == "Material")
                {
                    attack.SetActive(true);
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attackoriginalSizeD.x, attackoriginalSizeD.y);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = oldSpace;
                    attack.GetComponentInChildren<TMP_Text>().text = "Collect:";
                }

                if (hit.collider.gameObject.tag == "Fire")
                {

                    GameManager.Instance.playerLookingAtFire = true;

                    attack.SetActive(true);
                    current = hit.collider.gameObject.GetComponent<FireScript>();
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attacksizeD.x, attacksizeD.y);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = newSpace;

                    if (hit.collider.gameObject.GetComponent<FireScript>().fireActive)
                        attack.GetComponentInChildren<TMP_Text>().text = "Add Fuel:";
                    else
                        attack.GetComponentInChildren<TMP_Text>().text = "Start Fire:";

                    hit.collider.gameObject.GetComponent<FireScript>().ShowPopup();
                }

                if (hit.collider.gameObject.tag == "NPC")
                {
                    attack.SetActive(true);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = oldSpace;
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attackoriginalSizeD.x, attackoriginalSizeD.y);

                    attack.GetComponentInChildren<TMP_Text>().text = "Attack:";


                    GameManager.Instance.playerLookingAtDialogue = true;
					GameManager.Instance.dialogue = hit.collider.gameObject.GetComponent<Dialogue>();

                    GameManager.Instance.dialogue.ShowDialogue();
				}
			}
            else
            {
                attack.SetActive(false);

                // So you can disable things afterwards
                if(current != null)
                {
                    current.HidePopup();
					GameManager.Instance.playerLookingAtFire = false;
				}

                if(GameManager.Instance.dialogue != null)
                {
                   GameManager.Instance.dialogue.HideDialogue();

					GameManager.Instance.playerLookingAtDialogue = false;
                    GameManager.Instance.dialogue = null;
			    }
			}
        }

        public void CanInv()
        {

            bool canOpen = stats.GetComponent<Inventory>().CanOpenInv();
            if (canOpen)
            {
                inv.SetActive(true);
                
                if(GameManager.Instance.playerLookingAtFire)
                {
                    inv.GetComponentInChildren<TMP_Text>().text = "Sleep:";
                }
				else if (GameManager.Instance.dialogue != null)
				{
					inv.GetComponentInChildren<TMP_Text>().text = "Talk:";
				}
				else
                {
                    inv.GetComponentInChildren<TMP_Text>().text = "Inventory:";
                }

            }
            else { inv.SetActive(false); }
        }

    }
}
