using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditor.Sequences;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectBeelzebub
{

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [SerializeField] private GameObject _loaderCanvas;
        [SerializeField] private Image _progressBar;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async void LoadScene(string sceneName)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
        
            _loaderCanvas.SetActive(true);

            do
            {
                _progressBar.fillAmount = scene.progress;
            } while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
            _loaderCanvas.SetActive(false);
        }

    }
}