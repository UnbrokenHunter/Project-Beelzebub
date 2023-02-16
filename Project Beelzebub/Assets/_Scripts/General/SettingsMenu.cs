using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ProjectBeelzebub
{
    public class SettingsMenu : MonoBehaviour
    {

        [SerializeField] private AudioMixer mixer;


        public void SetMainVolume (float volume)
        {
            mixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            mixer.SetFloat("Sound Effects", Mathf.Log10(volume) * 20);
        }



        public void SetQuality (int quailtyIndex)
        {
            QualitySettings.SetQualityLevel(quailtyIndex);
        }

        public void SetFullScreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

		public void SetLeftie(bool isLeftie)
		{
			Screen.fullScreen = isLeftie;
		}


	}
}
