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

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene (sceneName);
        }

    }
}