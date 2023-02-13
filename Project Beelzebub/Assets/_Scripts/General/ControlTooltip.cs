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

        [Title("Attack")]
        [SerializeField] private PlayerMove playerMove;
        [SerializeField] private PlayerStats stats;
        [SerializeField] private Transform attackOrigin;
        [SerializeField] private LayerMask mask;

        [Title("Settings")]
        [SerializeField] private OpenSettings settingsContainer;

        [Title("Move")]
        [SerializeField] private Vector2 originalSizeD;
        [SerializeField] private Vector2 sizeD;

        [Title("Attack")]
        [SerializeField] private Vector2 attackoriginalSizeD;
        [SerializeField] private Vector2 attacksizeD;
        [SerializeField] private float oldSpace;
        [SerializeField] private float newSpace;
        [SerializeField] private FireScript current;

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
                move.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSizeD.x, originalSizeD.y);
                move.GetComponentInChildren<TMP_Text>().text = "Move:";
            }
            else
            {
                move.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeD.x, sizeD.y);
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

            }
            else
            {
                attack.SetActive(false);
                if(current!= null) 
                    current.HidePopup();
            }
        }

        public void CanInv()
        {
            bool canOpen = stats.GetComponent<Inventory>().CanOpenInv();
            if (canOpen)
            {
                inv.SetActive(true);
            }
            else { inv.SetActive(false); }
        }

    }
}
