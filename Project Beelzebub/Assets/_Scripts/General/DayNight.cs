using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectBeelzebub
{
    [RequireComponent(typeof(Light2D))]
    public class DayNight : MonoBehaviour
    {
        [Title("Day Night")]
        [SerializeField] private float duration = .5f;
        [SerializeField] private float minimumLight = 0.5f;
        [SerializeField] private float startOffset = 0f;

        [Title("Color")]
        [SerializeField] private Gradient gradient;

        [Title("Display")]
        [SerializeField] private Color currentColor;
        [SerializeField] private float currentPercentage;
        [HideInInspector] public float CurrentPercentage() => currentPercentage;

        // Variables
        private Light2D _light;
        private float _startTime;
        private float _skippedTime = 0;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _startTime = Time.time;
        }

        private void Update()
        {
            // Calculate the time elapsed since the start time
            var timeElapsed = (Time.time + _skippedTime + startOffset) - _startTime;

            // Calculate the percentage based on the since of the time elapsed
            var percentage = Mathf.Sin(timeElapsed / duration * Mathf.PI * 2) * 0.5f + minimumLight;
            
            // Clamp the percentage to be between 0 and 1
            percentage = Mathf.Clamp01(percentage);

            // Display the percentage
            currentPercentage = (percentage * 100);

            // Set color
            currentColor = gradient.Evaluate(percentage);
            _light.color = currentColor;
        }

        public void Sleep(float sleepLength)
        {
            _skippedTime += sleepLength;
        }

        [Button]
        public void Sleep()
        {
            _skippedTime += duration / 2;
        }


    }

}
