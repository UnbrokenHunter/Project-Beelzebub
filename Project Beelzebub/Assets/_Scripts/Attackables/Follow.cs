using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
    public class Follow : MonoBehaviour
    {

		[SerializeField] private GameObject toFollow;
		public GameObject location;
		[SerializeField] private AIDestinationSetter destinationSetter;
        [SerializeField] private float waitTime = 3;

        private void Awake()
        {
            destinationSetter = GetComponent<AIDestinationSetter>();
        }

        private void Update()
        {
            StartCoroutine(Wait());
            destinationSetter.target = toFollow.transform;
        }

        private IEnumerator Wait()
        {
            Vector3 loc = location.transform.position;

			yield return new WaitForSeconds(waitTime);

            toFollow.transform.position = loc;
        }
    }
}
