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
		private HorizontalLayoutGroup moveLayout;
		private TMP_Text movetext;

		[SerializeField] private GameObject attack;
        private RectTransform attackRect;
        private HorizontalLayoutGroup attackLayout;
        private TMP_Text attacktext;

        [SerializeField] private GameObject inv;
		private HorizontalLayoutGroup invLayout;
		private TMP_Text invtext;

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

		    attackRect = attack.GetComponent<RectTransform>();
		    attackLayout = attack.GetComponent<HorizontalLayoutGroup>();
		    attacktext = attack.GetComponentInChildren<TMP_Text>();

			invLayout = inv.GetComponent<HorizontalLayoutGroup>();
			invtext = inv.GetComponentInChildren<TMP_Text>();

			moveLayout = move.GetComponent<HorizontalLayoutGroup>();
			movetext = move.GetComponentInChildren<TMP_Text>();

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
                settings.SetActive(true);

            else 
                settings.SetActive(false);
        }

        public void CanMove(bool canMove)
        {
            if(canMove)
            {
                move.SetActive(true);
				moveLayout.spacing = movementSpacing;
				movetext.text = "Move: ";
            }
            else
            {
                move.SetActive(true);
				moveLayout.spacing = navigationSpacing;
				movetext.text = "Navigate: ";
            }
        }

        public void CanAttack(Vector2 lastMovement)
        {
            RaycastHit2D hit = Physics2D.Raycast(attackOrigin.position, lastMovement, stats.attackRange, mask);

            if (hit.collider != null)
            {
                attack.SetActive(true);

                // Set Sprite
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
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    attack.GetComponent<HorizontalLayoutGroup>().spacing = oldSpace;
                    attackRect.sizeDelta = attackoriginalSizeD;

					attacktext.text = "Attack:";
                }

                // Material
                else if (hit.collider.gameObject.CompareTag("Material"))
                {
                    attackRect.sizeDelta = attackoriginalSizeD;
					attackLayout.spacing = oldSpace;
					attacktext.text = "Collect:";
                }

                else if (hit.collider.gameObject.CompareTag("Fire"))
                {

                    GameManager.Instance.playerLookingAtFire = true;

                    currentFireScript = hit.collider.gameObject.GetComponent<FireScript>();
                    attackRect.sizeDelta = attacksizeD;
                    attackLayout.spacing = newSpace;

                    if (currentFireScript.fireActive)
						attacktext.text = "Add Fuel:";

                    else
						attacktext.text = "Start Fire:";

					currentFireScript.ShowPopup();
                }

                else if (hit.collider.gameObject.CompareTag("Rock Circle"))
                {
                    GameManager.Instance.playerLookingAtRocks = true;

					attacktext.text = "Place Fire:";

                    currentRockCircle = hit.collider.gameObject.GetComponent<RockCircle>();

					attackRect.sizeDelta = attacksizeD;
					attackLayout.spacing = newSpace;

                    hit.collider.gameObject.GetComponent<RockCircle>().ShowPopup();
                }

                else if (hit.collider.gameObject.CompareTag("Gate"))
                {

					attacktext.text = "Open Gate:";

                    currentGate = hit.collider.gameObject.GetComponent<Gate>();
                    currentGate.ShowPopup();
                }

                else if (hit.collider.gameObject.CompareTag("NPC"))
                {
					attackLayout.spacing = oldSpace;
					attackRect.sizeDelta = attackoriginalSizeD;

					attacktext.text = "Attack:";

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

                else if(currentRockCircle != null)
                {
                    currentRockCircle.HidePopup();
                    GameManager.Instance.playerLookingAtRocks = false;
                }

                else if(currentGate != null)
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
					invtext.text = "Sleep:";
					invLayout.spacing = oldTalkSpacing;
                }
				else if (GameManager.Instance.dialogue != null)
				{
					invtext.text = "Talk:";
					invLayout.spacing = TalkSpacing;

                }
				else 
                {
                    invtext.text = "Inventory:";
					invLayout.spacing = oldTalkSpacing;
                }

            }
            else 
                inv.SetActive(false); 
        }

    }
}
