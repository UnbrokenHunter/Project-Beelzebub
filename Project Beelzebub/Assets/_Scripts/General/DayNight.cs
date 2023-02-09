using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectBeelzebub
{
    public class DayNight : MonoBehaviour
    {
		[Title("Day/Night")]
		[SerializeField] private float dayLength = 2;

		[SerializeField] private float daySpeed = 1;
		[SerializeField] private float nightSpeed = 1;

		[SerializeField] private bool isDay = true;

		[Space]

		[SerializeField] private float intensity;
		[SerializeField] private float lightLevel = 1;
		[SerializeField] private float min = 1;
		[SerializeField] private Light2D light;

		[SerializeField] private float timer = 0;

		private void Update()
		{
			if (isDay)
				timer += Time.deltaTime * daySpeed;

			else
				timer -= Time.deltaTime * nightSpeed;


			if (timer > dayLength)
				isDay = false;
			
			if(timer < 0)
				isDay = true;

			var amt = min + (timer / 10) * lightLevel;
			intensity = Mathf.InverseLerp(0, 1, amt);

			light.intensity = intensity;

		}

	}

}
