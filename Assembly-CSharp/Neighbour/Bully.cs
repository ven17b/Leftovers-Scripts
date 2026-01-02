using System;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class Bully : MonoBehaviour
	{
		[SerializeField]
		private float pushForce;

		[SerializeField]
		private float decaySpeed;

		[SerializeField]
		private Vector3 direction;

		private CharacterController playerController;
		private float currentForce;

		public Bully()
		{
			decaySpeed = 5.0f;
			direction = Vector3.zero;
		}

		private void OnTriggerEnter(Collider other)
		{
			GameObject gameObject = other.gameObject;
			playerController = gameObject.GetComponent<CharacterController>();

			if (playerController != null)
				currentForce = pushForce;
		}

		private void Update()
		{
			if (playerController != null)
			{
				if (currentForce > 0.0f)
				{
					float deltaTime = Time.deltaTime;
					Vector3 moveVector = new Vector3(
						direction.x * currentForce * deltaTime,
						direction.y * currentForce * deltaTime,
						direction.z * currentForce * deltaTime
					);
					playerController.Move(moveVector);
				}
				else
				{
					playerController = null;
				}

				currentForce = Mathf.Lerp(currentForce, 0.0f, Time.deltaTime * decaySpeed);
			}
		}
	}
}
