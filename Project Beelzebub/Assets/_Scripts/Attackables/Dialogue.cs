using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class Dialogue : MonoBehaviour
    {

        [SerializeField] private List<string> texts;
        [SerializeField] private Sprite portrait;
        [SerializeField] private GameObject dialogue;
		[SerializeField] private TMP_Text text;
		[SerializeField] private Image image;
		[SerializeField] private float showNextCooldown;
        private float timer;

        [SerializeField] private int currentSellected;

        private void Update()
        {
            timer += Time.deltaTime;

        }

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
            if (timer < showNextCooldown) return;
				timer = 0;

			currentSellected++;

            print("Next Dia");

            if(currentSellected > texts.Count - 1)
                currentSellected = 0;

			text.text = texts[currentSellected];

		}

	}
}
