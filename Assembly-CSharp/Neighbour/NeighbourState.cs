using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Leftovers.Player;
using Leftovers.UI;
using Leftovers.Utilities;

namespace Leftovers.Neighbour
{
	public class NeighbourState : MonoBehaviour
	{
		private static readonly int HashBoolPartial = Animator.StringToHash("Partial");
		private static readonly int HashBoolFull = Animator.StringToHash("Full");
		private static readonly int HashBoolAlways = Animator.StringToHash("Always");
		private static readonly int HashBoolOpened = Animator.StringToHash("Opened");
		private static readonly int HashBoolForward = Animator.StringToHash("Forward");
		private static NeighbourState DebuggedNeighbour;

		[Header("Debug")]
		[Tooltip("Key to debug this neighbour")]
		[SerializeField]
		private KeyCode debugKeyCode;

		[Header("Controllers")]
		[SerializeField]
		private NeighbourAnimatorControllers animatorControllers;

		[SerializeField]
		private NeighbourDoorAnimationType doorAnimationType;

		[SerializeField]
		private bool isAvailable;

		[SerializeField]
		private float availabilityDelay;

		[SerializeField]
		private string unavailableMessage;

		[SerializeField]
		private float unavailableMessageDuration;

		[Header("Animators")]
		[SerializeField]
		private Animator animatorDoor;

		[SerializeField]
		private Animator animatorNeighbour;

		[SerializeField]
		private Animator animatorNeighbourFace;

		[SerializeField]
		private AudioSource audioSourceNeighbour;

		[SerializeField]
		private Transform lookAtFace;

		[Header("Events")]
		[SerializeField]
		private UnityEvent onReset;

		[SerializeField]
		private UnityEvent onUnavailable;

		[SerializeField]
		private UnityEvent onSpokenTo;

		[SerializeField]
		private UnityEvent onCompleted;

		[SerializeField]
		private UnityEvent onEndConversation;

		[Header("Dialogue")]
		[SerializeField]
		private List<DialogueSegment> segments;

		[SerializeField]
		private List<DialoguePrompt> prompts;

		[SerializeField]
		private int indexOpeningSegment;

		[SerializeField]
		private int indexSubsequentSegment;

		private bool spokenTo;
		private int currentSegment;
		private int currentPrompt;

		public NeighbourState()
		{
			doorAnimationType = (NeighbourDoorAnimationType)1;
			isAvailable = true;
			availabilityDelay = 1.0f;
			unavailableMessage = string.Empty;
			unavailableMessageDuration = 1.0f;
			onReset = new UnityEvent();
			onUnavailable = new UnityEvent();
			onSpokenTo = new UnityEvent();
			onCompleted = new UnityEvent();
			onEndConversation = new UnityEvent();
			segments = new List<DialogueSegment>();
			prompts = new List<DialoguePrompt>();
			currentSegment = -1;
			currentPrompt = -1;
		}

		private void Update()
		{
		}

		public void StartInteract()
		{
			Debug.Log("StartInteract");
			PlayerController.Instance.handleKeyboardInput = false;
			StartCoroutine(CheckAvailability());
		}

		private IEnumerator CheckAvailability()
		{
			Debug.Log("CheckAvailability");
			yield return new WaitForSeconds(availabilityDelay);

			if (!isAvailable)
			{
				Debug.Log("Not available");
				PlayerController.Instance.handleKeyboardInput = true;
				UIManager.Instance.SetInnerMessage(unavailableMessage, unavailableMessageDuration);
				if (onUnavailable != null)
					onUnavailable.Invoke();
				yield break;
			}

			SetNeighbourForwardState(true);
		}

		public void ResetState()
		{
			Debug.Log("ResetState");

			switch ((int)doorAnimationType)
			{
				case 0:
					animatorNeighbour.GetComponent<Billboard>().enabled = false;
					animatorNeighbour.GetComponent<NeighbourBendDown>().enabled = false;
					animatorNeighbour.runtimeAnimatorController = animatorControllers.controllerPartiallyOpened;
					break;
				case 1:
					animatorNeighbour.GetComponent<Billboard>().enabled = true;
					animatorNeighbour.GetComponent<NeighbourBendDown>().enabled = true;
					animatorNeighbour.runtimeAnimatorController = animatorControllers.controllerFullyOpened;
					break;
				case 2:
					animatorNeighbour.GetComponent<Billboard>().enabled = false;
					animatorNeighbour.GetComponent<NeighbourBendDown>().enabled = false;
					animatorNeighbour.runtimeAnimatorController = animatorControllers.controllerLegless;
					break;
				case 3:
					animatorNeighbour.GetComponent<Billboard>().enabled = true;
					animatorNeighbour.GetComponent<NeighbourBendDown>().enabled = false;
					animatorNeighbour.runtimeAnimatorController = animatorControllers.controllerNine;
					break;
			}

			ResetAnimation();
			spokenTo = false;
			if (onReset != null)
				onReset.Invoke();
		}

		public void SpokenTo()
		{
			Debug.Log("SpokenTo");
			spokenTo = true;
			if (onSpokenTo != null)
				onSpokenTo.Invoke();
		}

		public void Complete()
		{
			Debug.Log("Complete");
			if (onCompleted != null)
				onCompleted.Invoke();
		}

		public void PrepareConversation()
		{
			PlayerController.Instance.handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;
			PlayerController.Instance.lookAt = lookAtFace;
		}

		public void StartConversation()
		{
			Debug.Log("StartConversation");
			if (spokenTo)
			{
				GoToSegment(indexSubsequentSegment);
			}
			else
			{
				GoToSegment(indexOpeningSegment);
				Debug.Log("SpokenTo");
				spokenTo = true;
				if (onSpokenTo != null)
					onSpokenTo.Invoke();
			}
		}

		public void GoToSegment(int index)
		{
			Debug.Log("GoToSegment");
			currentSegment = index;
			StartCoroutine(PlaySegment());
		}

		private IEnumerator PlaySegment()
		{
			DialogueSegment segment = segments[currentSegment];
			int indexLine = 0;

			if (segment.onSegmentStart != null)
				segment.onSegmentStart.Invoke();

			PlayBodyAnimation(segment.animationBody);
			PlayFaceAnimation(segment.animationFace);
			PlayNeighbourSfx(segment.sfx);

			yield return new WaitForSeconds(segment.delay);

			while (indexLine < segment.lines.Count)
			{
				DialogueLine line = segment.lines[indexLine];

				yield return new WaitForSeconds(line.delay);

				UIManager.Instance.SetMessage(line.message, -1.0f);
				PlayBodyAnimation(line.animationBody);
				PlayFaceAnimation(line.animationFace);
				PlayNeighbourSfx(line.sfx);

				if (!string.IsNullOrWhiteSpace(line.message))
				{
					yield return new WaitForSeconds(1.25f);
					UIManager.Instance.SetDialogueClickPromptVisibility(true);

					yield return null;

					while (!Input.GetMouseButtonDown(0))
					{
						yield return null;
					}

					UIManager.Instance.SetDialogueClickPromptVisibility(false);
				}

				if (line.onLineEnd != null)
					line.onLineEnd.Invoke();

				indexLine++;
			}

			switch ((int)segment.type)
			{
				case 0:
					UIManager.Instance.SetMessage(string.Empty);
					SetNeighbourForwardState(false);
					break;
				case 1:
					GoToSegment(segment.indexType);
					break;
				case 2:
					GoToPrompt(segment.indexType);
					break;
				case 3:
					UIManager.Instance.SetMessage(string.Empty);
					SetNeighbourForwardState(false);
					Debug.Log("Complete");
					if (onCompleted != null)
						onCompleted.Invoke();
					break;
			}

			if (segment.onSegmentEnd != null)
				segment.onSegmentEnd.Invoke();
		}

		private void PlayBodyAnimation(string animation)
		{
			if (!string.IsNullOrWhiteSpace(animation))
			{
				if (animatorNeighbour != null)
				{
					animatorNeighbour.Play(animation);
				}
			}
		}

		private void PlayFaceAnimation(string animation)
		{
			if (!string.IsNullOrWhiteSpace(animation))
			{
				if (animatorNeighbourFace != null)
				{
					animatorNeighbourFace.Play(animation);
				}
			}
		}

		private void PlayNeighbourSfx(AudioClip clip)
		{
			if (clip != null)
			{
				if (audioSourceNeighbour != null)
				{
					audioSourceNeighbour.PlayOneShot(clip);
				}
			}
		}

		public void GoToPrompt(int index)
		{
			currentPrompt = index;
			PlayerController.Instance.StartListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		private void OnNod()
		{
			DialoguePrompt prompt = prompts[currentPrompt];
			if (!prompt.hasNod)
				return;

			Debug.Log("OnNod");
			GoToSegment(prompt.nodSegmentIndex);
			PlayerController.Instance.StopListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		private void OnShake()
		{
			DialoguePrompt prompt = prompts[currentPrompt];
			if (!prompt.hasShake)
				return;

			Debug.Log("OnShake");
			GoToSegment(prompt.shakeSegmentIndex);
			PlayerController.Instance.StopListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		private void OnShowFood()
		{
			DialoguePrompt prompt = prompts[currentPrompt];
			if (!prompt.hasShowFood)
				return;

			Debug.Log("OnShowFood");
			GoToSegment(prompt.showFoodSegmentIndex);
			PlayerController.Instance.StopListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		public void TakeFoodFromPlayer()
		{
			PlayerController.Instance.RemoveFood();
		}

		public void MakePlayerPutAwayFood()
		{
			PlayerController.Instance.animator.SetBool(PlayerController.HashBoolShowFood, false);
		}

		public void EndConversation()
		{
			Debug.Log("EndConversation");
			currentSegment = -1;
			currentPrompt = -1;
			PlayerController.Instance.lookAt = null;
			PlayerController.Instance.handleKeyboardInput = true;
			PlayerController.Instance.handleMouseInput = true;
			Cursor.lockState = CursorLockMode.Locked;
			if (onEndConversation != null)
				onEndConversation.Invoke();
		}

		public void SetDoorOpenedState(bool state)
		{
			Debug.Log("SetDoorOpenedState: " + state.ToString());
			animatorDoor.SetBool(HashBoolOpened, state);
		}

		public void SetNeighbourForwardState(bool state)
		{
			Debug.Log("SetNeighbourForwardState: " + state.ToString());
			animatorNeighbour.SetBool(HashBoolForward, state);
		}

		public void SetAvailability(bool state)
		{
			isAvailable = state;
		}

		public void SetOpeningSegmentIndex(int index)
		{
			indexOpeningSegment = index;
		}

		public void SetSubsequentSegmentIndex(int index)
		{
			indexSubsequentSegment = index;
		}

		[ContextMenu("Reset Animation")]
		private void ResetAnimation()
		{
			if (animatorDoor != null)
			{
				animatorDoor.Rebind();

				switch ((int)doorAnimationType)
				{
					case 0:
						animatorDoor.SetBool(HashBoolFull, false);
						animatorDoor.SetBool(HashBoolAlways, false);
						animatorDoor.SetBool(HashBoolPartial, true);
						break;
					case 1:
					case 3:
						animatorDoor.SetBool(HashBoolFull, true);
						animatorDoor.SetBool(HashBoolAlways, false);
						animatorDoor.SetBool(HashBoolPartial, false);
						break;
					case 2:
						animatorDoor.SetBool(HashBoolFull, false);
						animatorDoor.SetBool(HashBoolAlways, true);
						animatorDoor.SetBool(HashBoolPartial, true);
						break;
				}

				animatorDoor.SetBool(HashBoolOpened, false);
			}

			if (animatorNeighbour != null)
			{
				animatorNeighbour.Rebind();
			}
		}
	}
}
