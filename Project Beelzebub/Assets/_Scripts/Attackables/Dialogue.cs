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

		public void ShowDialogue()
        {
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

            if(currentSellected > texts.Count - 1)
                currentSellected = 0;

			text.text = texts[currentSellected];
            events[currentSellected].Invoke();

        }

	}
}
