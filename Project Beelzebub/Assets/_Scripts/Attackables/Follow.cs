using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Follow : MonoBehaviour
    {

		public GameObject location;
        [SerializeField] private float waitTime = 3;
        [SerializeField] private float noMoveArea = 3;

        private void Update()
        {
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            Vector3 loc = location.transform.position;
			yield return new WaitForSeconds(waitTime);
		    location.transform.position = loc;
        }
    }
}
