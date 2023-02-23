using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class Dialogue : MonoBehaviour
    {

        [SerializeField] private List<string> texts;
        [SerializeField] private List<UnityEvent> events; 
        [SerializeField] private Sprite portrait;
        [SerializeField] private GameObject dialogue;
		[SerializeField] private TMP_Text text;
		[SerializeField] private Image image;
        [SerializeField] private float nextTextCooldown = 0.5f;
        private bool canGoNext = true;
        [SerializeField] private float revealSpeed = 0.2f;
        public bool displayDialogue;

        [SerializeField] private int currentSellected;

        private string shownString;
        private IEnumerator RevealString()
        {
            int i = 0;
            while (i <= texts[currentSellected].Length)
            {
                if (!displayDialogue) break;
                
                    shownString = texts[currentSellected].Substring(0, i);
                text.text = shownString;

                yield return new WaitForSeconds(revealSpeed);
                i++;
            }
        }
        public void ShowDialogue(string text, Sprite portrait)
        {
            if (dialogue.activeInHierarchy != false) return;

            displayDialogue = true;
            this.texts[currentSellected] = text;
            image.sprite = portrait;
            dialogue.SetActive(true);

            StartCoroutine(RevealString());

        }

        public void ShowDialogue(string text, Sprite portrait, float displayTime)
        {
            if (dialogue.activeInHierarchy != false) return;

            displayDialogue = true;
            this.texts[currentSellected] = text;
            image.sprite = portrait;
            dialogue.SetActive(true);

            StartCoroutine(RevealString());
            StartCoroutine(HideIn(displayTime));
        }


        public void ShowDialogue(string text)
        {
            this.texts[currentSellected] = text;
            ShowDialogue();
        }

        public void ShowDialogue(string text, float displayTime)
        {
            this.texts[currentSellected] = text;
            ShowDialogue();
            StartCoroutine(HideIn(displayTime));
        }

        public void ShowDialogue()
        {
            if (dialogue.activeInHierarchy != false) return;

            displayDialogue = true;
            dialogue.SetActive(true);
			image.sprite = portrait;

            StartCoroutine(RevealString());
		}
        public void HideDialogue()
        {
            displayDialogue = false;
			dialogue.SetActive(false);
		}
		public void ShowNext()
        {
            if (!canGoNext) return;

			currentSellected++;

            print("Next Dialogue");

            if(currentSellected > texts.Count - 1)
                currentSellected = 0;
            if(texts.Count > currentSellected)
                StartCoroutine(RevealString());

            if (events.Count > currentSellected)
                events[currentSellected].Invoke();

            StartCoroutine(WaitForNext());
        }

        private IEnumerator WaitForNext()
        {
            canGoNext = false;

            yield return new WaitForSeconds(nextTextCooldown);

            canGoNext = true;



		}

        private IEnumerator HideIn(float time)
        {
            yield return new WaitForSeconds(time);
            HideDialogue();
        }
	}
}
