using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Littlun : MonoBehaviour
    {
		[SerializeField] private string hitSound;
		[SerializeField] private GameObject ouch;
		[SerializeField] private float ouchTime;

		public void Attacked()
		{
			ouch.SetActive(true);
			StartCoroutine(timer());

		}

		private IEnumerator timer()
		{
			yield return new WaitForSeconds(ouchTime);

			ouch.SetActive(false);
		}

	}
}
