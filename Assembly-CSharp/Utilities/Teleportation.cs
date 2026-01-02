using System;
using UnityEngine;
using UnityEngine.Events;
using Leftovers.Player;
using Leftovers.UI;

namespace Leftovers.Utilities
{
	public class Teleportation : MonoBehaviour
	{
		[SerializeField]
		private TransitionType type;

		[SerializeField]
		private Transform teleportationPoint;

		[SerializeField]
		private AudioClip startTeleportSound;

		[SerializeField]
		private AudioClip finishTeleportSound;

		[SerializeField]
		private AudioSource audioSource;

		public void Teleport()
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");

			PlayerController playerController = PlayerController.Instance;
			playerController.handleKeyboardInput = false;
			playerController.handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;

			switch (type)
			{
				case TransitionType.FadeInAndOut:
					UIManager.Instance.FadeInAndOut(
						() =>
						{
							CharacterController cc = player.GetComponent<CharacterController>();
							cc.enabled = false;

							Transform t = player.transform;
							t.position = teleportationPoint.position;
							t.eulerAngles = teleportationPoint.eulerAngles;

							cc.enabled = true;

							PlayerController.Instance.ResetRotationValues();

							if (audioSource && startTeleportSound)
								audioSource.PlayOneShot(startTeleportSound);
						},
						() =>
						{
							PlayerController pc = PlayerController.Instance;
							pc.handleKeyboardInput = true;
							pc.handleMouseInput = true;
							Cursor.lockState = CursorLockMode.Locked;

							if (audioSource && finishTeleportSound)
								audioSource.PlayOneShot(finishTeleportSound);
						}
					);
					break;

				case TransitionType.FadeOut:
					{
						CharacterController cc = player.GetComponent<CharacterController>();
						cc.enabled = false;

						Transform t = player.transform;
						t.position = teleportationPoint.position;
						t.eulerAngles = teleportationPoint.eulerAngles;

						cc.enabled = true;

						PlayerController.Instance.ResetRotationValues();

						if (audioSource && startTeleportSound)
							audioSource.PlayOneShot(startTeleportSound);

						UIManager.Instance.FadeOut(() =>
						{
							PlayerController pc = PlayerController.Instance;
							pc.handleKeyboardInput = true;
							pc.handleMouseInput = true;
							Cursor.lockState = CursorLockMode.Locked;

							if (audioSource && finishTeleportSound)
								audioSource.PlayOneShot(finishTeleportSound);
						});
					}
					break;

				case TransitionType.Instant:
					{
						CharacterController cc = player.GetComponent<CharacterController>();
						cc.enabled = false;

						Transform t = player.transform;
						t.position = teleportationPoint.position;
						t.eulerAngles = teleportationPoint.eulerAngles;

						cc.enabled = true;

						PlayerController.Instance.ResetRotationValues();
					}
					break;
			}
		}
	}
}
