using System.Collections;
using System.Collections.Generic;
using ProjectBeelzebulb;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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

        [SerializeField] private Vector2 originalSizeD;
        [SerializeField] private Vector2 sizeD;

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

                print(hit.collider.gameObject.name);

                // Attackable
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    attack.SetActive(true);
                    attack.GetComponentInChildren<TMP_Text>().text = "Attack:";
                }

                // Material
                if (hit.collider.gameObject.tag == "Material")
                {
                    attack.SetActive(true);
                    attack.GetComponentInChildren<TMP_Text>().text = "Collect";
                }

            }
            else
            {
                attack.SetActive(false);
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
