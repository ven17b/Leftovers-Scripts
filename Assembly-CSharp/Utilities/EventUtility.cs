using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leftovers.Player;

namespace Leftovers.Utilities
{
	public class EventUtility : MonoBehaviour
	{
		public void EnablePlayerControls()
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.handleKeyboardInput = true;
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void DisablePlayerControls()
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.handleKeyboardInput = false;
			Cursor.lockState = CursorLockMode.None;
		}

		public void EnablePlayerMovement()
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.handleKeyboardInput = true;
		}

		public void DisablePlayerMovement()
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.handleKeyboardInput = false;
		}

		public void SetPlayerLookAt(Transform lookAt)
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.SetLookAt(lookAt);
		}

		public void StartPlayerZoomIn(float duration)
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.StartZoomIn(duration);
		}

		public void StartPlayerZoomOut(float duration)
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.StartZoomOut(duration);
		}

		public void ClearPlayerLookAt()
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.SetLookAt(null);
		}

		public void CopyCameraTransform(Transform copier)
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null || copier == null)
				return;

			instance.CopyCameraTransform(copier);
		}

		public void ResumeGame()
		{
			PlayerController instance = PlayerController.Instance;
			if (instance == null)
				return;

			instance.ResumeGame();
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void RestartGame()
		{
			SceneManager.LoadScene(0);
		}

		public void SetMouseSensitivity(float value)
		{
			PlayerController.MouseSensitivity = value;
		}

		public void SetVolume(float value)
		{
			AudioListener.volume = value;
		}
	}
}
