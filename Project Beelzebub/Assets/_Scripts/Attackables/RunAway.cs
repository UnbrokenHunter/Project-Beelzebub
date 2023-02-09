using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

namespace ProjectBeelzebub
{
    public class RunAway : MonoBehaviour
    {

        [SerializeField] GameObject player;
        [SerializeField] private Vector2 dir;
        [SerializeField] private Transform target;

        [SerializeField] private float guessDistance = 1;

        private void Update()
        {
            SetDestination(player.transform.position);
        }

        public void SetDestination(Vector3 runFrom)
        {
			AIDestinationSetter setter = GetComponent<AIDestinationSetter>();

            dir = (transform.position - runFrom);

            Vector3 location = dir.normalized * guessDistance;

            target.position = location;

            setter.target = target;

		}

		private void OnDrawGizmos()
		{
			if (!Application.isPlaying) return;

			Gizmos.color = Color.red;

            Gizmos.DrawRay(transform.position, dir);
        }


	}
}
