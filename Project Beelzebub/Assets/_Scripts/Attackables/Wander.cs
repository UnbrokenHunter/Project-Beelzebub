using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
	public class Wander : MonoBehaviour
	{
		private AIDestinationSetter setter;
		[SerializeField] Vector2 dir;
		[SerializeField] private Transform target;
		[SerializeField] private float guessDistance = 5;
		[SerializeField] private float newLocationTimer = 20;

		private void Awake() => setter = GetComponent<AIDestinationSetter>();

		private void Start()
		{
			InvokeRepeating("PickNew", 0, newLocationTimer);
		}

		private void PickNew()
		{
			Vector3 random = new Vector3(Random.Range(-guessDistance, guessDistance), Random.Range(-guessDistance, guessDistance));

			SetDestination(random);
		}

		public void SetDestination(Vector3 random)
		{
			AIDestinationSetter setter = GetComponent<AIDestinationSetter>();

			dir = (transform.position + random);

			Vector3 location = dir.normalized * guessDistance;

			target.position = location;

			setter.target = target;

		}
	}
}
