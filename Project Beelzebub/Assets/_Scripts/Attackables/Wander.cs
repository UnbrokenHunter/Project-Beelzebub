using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectBeelzebub
{
	public class Wander : MonoBehaviour
	{
		private AIDestinationSetter setter;
		private Vector3 spawn;
		[SerializeField] Vector2 dir;
		[SerializeField] private Transform target;
		[SerializeField] private float wanderRange = 5;
		[SerializeField] private float newLocationTimer = 20;

		private void Awake() => setter = GetComponent<AIDestinationSetter>();

		private void Start()
		{
			spawn = transform.position;
			AIDestinationSetter setter = GetComponent<AIDestinationSetter>();
			InvokeRepeating("PickNew", 0, newLocationTimer);
		}

		private void PickNew()
		{
			Vector3 random = new Vector3(Random.Range(-wanderRange, wanderRange), Random.Range(-wanderRange, wanderRange));
			random = random + spawn;
			SetDestination(random);
		}

		public void SetDestination(Vector3 random)
		{
			target.position = random;
			setter.target = target;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			if(Application.isPlaying)
				Gizmos.DrawWireSphere(spawn, wanderRange);
		
			else
				Gizmos.DrawWireSphere(transform.position, wanderRange);

		}
	}
}
