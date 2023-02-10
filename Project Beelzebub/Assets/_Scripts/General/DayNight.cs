using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectBeelzebub
{
    public class DayNight : MonoBehaviour
    {
		[Title("Speed/Length")]
		[SerializeField] private float dayNightLength = 2;

		[SerializeField] private float sunRiseSpeed = 1;
		[SerializeField] private float sunSetSpeed = 1;
		[SerializeField] private AnimationCurve dayCurve;


		[Title("Day/Night")]
		[SerializeField] private bool isDay = true;

		[SerializeField] private float dayReq = .5f;

		[Space]


		[Title("Other")]
		[SerializeField] private float intensity = 1;
		[SerializeField] private float min = 1;
		[SerializeField] private Light2D light2D;

		[SerializeField] private float timer = 0;

		private void Update()
		{
			if (isDay)
				timer += Time.deltaTime * sunRiseSpeed * dayCurve.Evaluate(timer);

			else
				timer -= Time.deltaTime * sunSetSpeed * dayCurve.Evaluate(timer);


			if (timer > dayNightLength)
				isDay = false;
			
			if(timer < 0)
				isDay = true;

			var amt = (timer / 10);
			intensity = Mathf.InverseLerp(0, 1, amt);
			intensity = Mathf.Clamp(amt, min, 1);

            light2D.intensity = intensity;

		}

	}

}