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

        private AsyncOperation scene;

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

        public void AllowSceneComplete()
        {
            if(scene != null)
            {
                scene.allowSceneActivation = true;
            }
        }

		public void LoadSceneAsync(string sceneName)
		{
            print("Start Loading Scene");
			scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
		}

    }
}