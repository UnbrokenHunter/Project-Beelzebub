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
        [SerializeField] private TMP_Dropdown resolutionDropdown;

        Resolution[] resolutions;

        private void Start()
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> list = new List<string>();

            int currentResIndex = 0;
            int count = 0;
            foreach (var resolution in resolutions)
            {
                string option = $"{resolution.width} x {resolution.height}";
                list.Add(option);

                if (resolution.height == Screen.currentResolution.height && resolution.width == Screen.currentResolution.width)
                    currentResIndex = count;
            
                count++;
            }

            resolutionDropdown.AddOptions(list);
            resolutionDropdown.value = currentResIndex;
            resolutionDropdown.RefreshShownValue();
        }

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

    }
}
