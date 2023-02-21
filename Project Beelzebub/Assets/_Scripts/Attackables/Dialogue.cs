using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private float revealSpeed = 0.2f;

        [SerializeField] private int currentSellected;

        public void ShowDialogue(string text, Sprite portrait)
        {
            if (dialogue.activeInHierarchy != false) return;
            
            this.texts[currentSellected] = text;
            image.sprite = portrait;
            dialogue.SetActive(true);

            StartCoroutine(RevealString());

        }


        public void ShowDialogue(string text)
        {
            this.texts[currentSellected] = text;
            ShowDialogue();
        }

        private string shownString;
        private IEnumerator RevealString()
        {
            int i = 0;
            while (i <= texts[currentSellected].Length)
            {
                shownString = texts[currentSellected].Substring(0, i);
                text.text = shownString;

                yield return new WaitForSeconds(revealSpeed);
                i++;
            }
        }



        public void ShowDialogue()
        {
            if (dialogue.activeInHierarchy != false) return;

            dialogue.SetActive(true);
			image.sprite = portrait;

            StartCoroutine(RevealString());
		}

        public void HideDialogue()
        {
			dialogue.SetActive(false);
		}

		public void ShowNext()
        {
			currentSellected++;

            print("Next Dia");

            if(currentSellected > texts.Count)
                currentSellected = 0;
            if(texts.Count > currentSellected)
                StartCoroutine(RevealString());

            if (events.Count > currentSellected)
                events[currentSellected].Invoke();

        }

	}
}
