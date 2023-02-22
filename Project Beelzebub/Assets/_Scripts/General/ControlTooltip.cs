using System.Collections;
using System.Collections.Generic;
using ProjectBeelzebulb;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;
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

        private bool hasSetMat = false;
        [SerializeField] private Material outlineMat;
        [SerializeField] private Material tempMat;
        [SerializeField] private SpriteRenderer changedSprite;

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

        [SerializeField] private float TalkSpacing;
        private float oldTalkSpacing;

        [Title("Move")]
        [SerializeField] private float movementSpacing;
        [SerializeField] private float navigationSpacing;

        [Title("Attack")]
        [SerializeField] private Vector2 attackoriginalSizeD;
        [SerializeField] private Vector2 attacksizeD;
        [SerializeField] private float oldSpace;
        [SerializeField] private float newSpace;

        [Title("Looking At")]
        [SerializeField] private FireScript currentFireScript;
        [SerializeField] private RockCircle currentRockCircle;
        [SerializeField] private Gate currentGate;

        private void Start()
        {
            oldTalkSpacing = inv.GetComponent<HorizontalLayoutGroup>().spacing;
        }

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
                attack.SetActive(true);
                if (hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>() != null)
                {
                    if (hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>().material != outlineMat && !hasSetMat)
                    {
                        changedSprite = hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>();
                        tempMat = changedSprite.material;
                        changedSprite.material = outlineMat;
                        hasSetMat = true;
                    }
                }

                // Attackable
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = oldSpace;
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attackoriginalSizeD.x, attackoriginalSizeD.y);

                    attack.GetComponentInChildren<TMP_Text>().text = "Attack:";
                }

                // Material
                if (hit.collider.gameObject.tag == "Material")
                {
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attackoriginalSizeD.x, attackoriginalSizeD.y);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = oldSpace;
                    attack.GetComponentInChildren<TMP_Text>().text = "Collect:";
                }

                if (hit.collider.gameObject.tag == "Fire")
                {

                    GameManager.Instance.playerLookingAtFire = true;

                    currentFireScript = hit.collider.gameObject.GetComponent<FireScript>();
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attacksizeD.x, attacksizeD.y);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = newSpace;

                    if (hit.collider.gameObject.GetComponent<FireScript>().fireActive)
                        attack.GetComponentInChildren<TMP_Text>().text = "Add Fuel:";
                    else
                        attack.GetComponentInChildren<TMP_Text>().text = "Start Fire:";

                    hit.collider.gameObject.GetComponent<FireScript>().ShowPopup();
                }

                if (hit.collider.gameObject.tag == "Rock Circle")
                {
                    GameManager.Instance.playerLookingAtRocks = true;
                    
                    attack.GetComponentInChildren<TMP_Text>().text = "Place Fire:";

                    currentRockCircle = hit.collider.gameObject.GetComponent<RockCircle>();
                    attack.GetComponent<RectTransform>().sizeDelta = new Vector2(attacksizeD.x, attacksizeD.y);
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = newSpace;

                    hit.collider.gameObject.GetComponent<RockCircle>().ShowPopup();
                }

                if (hit.collider.gameObject.tag == "Gate")
                {

                    attack.GetComponentInChildren<TMP_Text>().text = "Open Gate:";

                    currentGate = hit.collider.gameObject.GetComponent<Gate>();
                    currentGate.ShowPopup();
                }

                if (hit.collider.gameObject.tag == "NPC")
                {
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

                // The fact that this method was called means that we were looking at something
                // If it is is not null then we know we are not looking at it anyumore

                // So you can disable things afterwards
                if(currentFireScript != null)
                {
                    currentFireScript.HidePopup();
					GameManager.Instance.playerLookingAtFire = false;
				}

                if(currentRockCircle != null)
                {
                    currentRockCircle.HidePopup();
                    GameManager.Instance.playerLookingAtRocks = false;
                }

                if(currentGate != null)
                {
                    currentGate.HidePopup();
                }

                if (GameManager.Instance.dialogue != null)
                {
                   GameManager.Instance.dialogue.HideDialogue();

					GameManager.Instance.playerLookingAtDialogue = false;
                    GameManager.Instance.dialogue = null;
			    }

                if (changedSprite != null)
                {
                    changedSprite.material = tempMat;
                    changedSprite = null;

                    hasSetMat = false;
                }
			}
        }

        public void CanInv()
        {

            bool canOpen = stats.GetComponent<Inventory>().CanOpenInv();
            if (canOpen)
            {
                inv.SetActive(true);

                if (GameManager.Instance.playerLookingAtFire)
                {
                    inv.GetComponentInChildren<TMP_Text>().text = "Sleep:";
                    inv.GetComponent<HorizontalLayoutGroup>().spacing = oldTalkSpacing;
                }
				else if (GameManager.Instance.dialogue != null)
				{
					inv.GetComponentInChildren<TMP_Text>().text = "Talk:";
                    inv.GetComponent<HorizontalLayoutGroup>().spacing = TalkSpacing;

                }
				else
                {
                    inv.GetComponentInChildren<TMP_Text>().text = "Inventory:";
                    inv.GetComponent<HorizontalLayoutGroup>().spacing = oldTalkSpacing;

                }

            }
            else { inv.SetActive(false); }
        }

    }
}
