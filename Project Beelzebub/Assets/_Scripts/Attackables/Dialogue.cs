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

        [SerializeField] private int currentSellected;

        public void ShowDialogue(string text)
        {
            this.texts[currentSellected] = text;
            ShowDialogue();
        }


        public void ShowDialogue()
        {
            if (dialogue.activeInHierarchy != false) return;

            dialogue.SetActive(true);
			image.sprite = portrait;

			text.text = texts[currentSellected];
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
			    text.text = texts[currentSellected];
            
            if (events.Count > currentSellected)
                events[currentSellected].Invoke();

        }

	}
}
