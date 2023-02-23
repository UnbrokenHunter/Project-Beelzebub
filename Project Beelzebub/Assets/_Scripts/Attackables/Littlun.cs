using DarkTonic.MasterAudio;
using Pathfinding;
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

		[SerializeField] private SpriteRenderer rend;
		[SerializeField] private Animator anim;

		[SerializeField] private bool follow;
		[SerializeField] private GameObject followObj;

		[SerializeField] private Vector2 lastPos = Vector2.zero;
		[SerializeField] private Vector2 curPos = Vector2.zero;

		[SerializeField] private Vector2 direction;
		[SerializeField] private float minMovement = 0.2f;
		private Vector2 lastDir;


		private void Start()
		{
			if (follow)
			{
				GetComponent<AIDestinationSetter>().target = followObj.transform;
			}

		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.layer == 9)
            {
				GetComponent<AIPath>().canMove = false;
			}
		}

        private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.layer == 9)
			{
				GetComponent<AIPath>().canMove = true;
			}
		}

		private void Update()
		{

			GetDirection();

			// Direction
			Vector2 velo = direction;

			if (Mathf.Abs(velo.x) < minMovement) velo = new Vector2(0, velo.y);
			if (Mathf.Abs(velo.y) < minMovement) velo = new Vector2(velo.x, 0);

			velo.Normalize();

			if (velo != Vector2.zero) lastDir = velo;

			// Set State
			anim.SetBool("moving", velo.magnitude > 0);

			// Set Direction
			anim.SetFloat("moveX", lastDir.x);
			anim.SetFloat("moveY", lastDir.y);

			// Flix x
			rend.flipX = lastDir.x < 0;


		}

		private void GetDirection()
		{
			lastPos = curPos;
			curPos = transform.position;

			direction = curPos - lastPos;

		}

		public void Attacked()
		{
			ouch.SetActive(true);
			MasterAudio.PlaySound(hitSound);
			StartCoroutine(timer());

		}

		private IEnumerator timer()
		{
			yield return new WaitForSeconds(ouchTime);

			ouch.SetActive(false);
		}

	}
}
