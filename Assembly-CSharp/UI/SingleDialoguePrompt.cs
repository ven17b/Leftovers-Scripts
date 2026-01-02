using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Leftovers.Player;

namespace Leftovers.UI
{
	public class SingleDialoguePrompt : MonoBehaviour
	{
		[SerializeField]
		private string message;

		[SerializeField]
		private float delayMessage;

		[SerializeField]
		private UnityEvent onStartDialogue;

		[SerializeField]
		private UnityEvent onCloseDialogue;

		public SingleDialoguePrompt()
		{
			onStartDialogue = new UnityEvent();
			onCloseDialogue = new UnityEvent();
		}

		public void ShowDialogue()
		{
			PlayerController player = PlayerController.Instance;
			if (player != null)
			{
				player.handleKeyboardInput = false;
				player.handleMouseInput = false;
			}

			Cursor.lockState = CursorLockMode.None;

			if (onStartDialogue != null)
				onStartDialogue.Invoke();

			StartCoroutine(ListenToPrompt());
		}

		private IEnumerator ListenToPrompt()
		{
			yield return new WaitForSeconds(delayMessage);

			UIManager.Instance.SetMessage(message);

			yield return new WaitForSeconds(1.25f);

			UIManager.Instance.SetDialogueClickPromptVisibility(true);

			while (!Input.GetMouseButtonDown(0))
			{
				yield return null;
			}

			UIManager.Instance.SetDialogueClickPromptVisibility(false);
			UIManager.Instance.SetMessage(string.Empty);

			PlayerController player = PlayerController.Instance;
			if (player != null)
			{
				player.handleKeyboardInput = true;
				player.handleMouseInput = true;
			}

			Cursor.lockState = CursorLockMode.Locked;

			if (onCloseDialogue != null)
				onCloseDialogue.Invoke();
		}
	}
}
