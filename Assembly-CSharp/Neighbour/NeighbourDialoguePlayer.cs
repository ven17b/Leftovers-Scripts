using System;
using System.Collections;
using System.Collections.Generic;
using Leftovers.Player;
using Leftovers.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Neighbour
{
	public class NeighbourDialoguePlayer : MonoBehaviour
	{
		public int OpeningSegmentIndex
		{
			get { return indexOpeningSegment; }
			set { indexOpeningSegment = value; }
		}

		private void Start()
		{
			originalOpeningSegment = indexOpeningSegment;
		}

		public void StartInteract()
		{
			PlayerController.Instance.handleKeyboardInput = false;
			PlayerController.Instance.handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;
			PlayerController.Instance.lookAt = lookAtFace;
			StartCoroutine(DelayLookAt());
		}

		public void ResetState()
		{
			Debug.Log("ResetState");
			indexOpeningSegment = originalOpeningSegment;
			if (onReset != null)
			{
				onReset.Invoke();
			}
		}

		public void Complete()
		{
			Debug.Log("Complete");
			if (onCompleted != null)
			{
				onCompleted.Invoke();
			}
		}

		public void StartConversation()
		{
			GoToSegment(indexOpeningSegment);
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

			if (segment.onSegmentStart != null)
			{
				segment.onSegmentStart.Invoke();
			}

			PlayBodyAnimation(segment.animationBody);
			PlayFaceAnimation(segment.animationFace);
			PlayNeighbourSfx(segment.sfx);

			yield return new WaitForSeconds(segment.delay);

			int indexLine = 0;
			while (indexLine < segment.lines.Count)
			{
				DialogueLine line = segment.lines[indexLine];

				yield return new WaitForSeconds(line.delay);

				UIManager.Instance.SetMessage(line.message);
				PlayBodyAnimation(line.animationBody);
				PlayFaceAnimation(line.animationFace);
				PlayNeighbourSfx(line.sfx);

				yield return new WaitForSeconds(1.25f);

				UIManager.Instance.SetDialogueClickPromptVisibility(true);

				while (!Input.GetMouseButtonDown(0))
				{
					yield return null;
				}

				UIManager.Instance.SetDialogueClickPromptVisibility(false);

				if (line.onLineEnd != null)
				{
					line.onLineEnd.Invoke();
				}

				indexLine++;
			}

			switch (segment.type)
			{
				case DialogueSegmentType.End:
					EndConversation();
					break;
				case DialogueSegmentType.GoToSegment:
					GoToSegment(segment.indexType);
					break;
				case DialogueSegmentType.Prompt:
					GoToPrompt(segment.indexType);
					break;
				case DialogueSegmentType.Complete:
					Complete();
					EndConversation();
					break;
			}

			if (segment.onSegmentEnd != null)
			{
				segment.onSegmentEnd.Invoke();
			}
		}

		private void PlayBodyAnimation(string animation)
		{
			if (!string.IsNullOrWhiteSpace(animation) && animatorNeighbour != null)
			{
				animatorNeighbour.Play(animation);
			}
		}

		private void PlayFaceAnimation(string animation)
		{
			if (!string.IsNullOrWhiteSpace(animation) && animatorNeighbourFace != null)
			{
				animatorNeighbourFace.Play(animation);
			}
		}

		private void PlayNeighbourSfx(AudioClip clip)
		{
			if (clip != null && audioSourceNeighbour != null)
			{
				audioSourceNeighbour.PlayOneShot(clip);
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
			{
				return;
			}

			Debug.Log("Nod");
			GoToSegment(prompt.nodSegmentIndex);
			PlayerController.Instance.StopListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		private void OnShake()
		{
			DialoguePrompt prompt = prompts[currentPrompt];
			if (!prompt.hasShake)
			{
				return;
			}

			Debug.Log("Shake");
			GoToSegment(prompt.shakeSegmentIndex);
			PlayerController.Instance.StopListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		private void OnShowFood()
		{
			DialoguePrompt prompt = prompts[currentPrompt];
			if (!prompt.hasShowFood)
			{
				return;
			}

			Debug.Log("ShowFood");
			GoToSegment(prompt.showFoodSegmentIndex);
			PlayerController.Instance.StopListeningToPrompt(OnNod, OnShake, OnShowFood);
		}

		public void EndConversation()
		{
			currentSegment = -1;
			currentPrompt = -1;

			UIManager.Instance.SetMessage(string.Empty);

			PlayerController.Instance.lookAt = null;
			PlayerController.Instance.handleKeyboardInput = true;
			PlayerController.Instance.handleMouseInput = true;
			Cursor.lockState = CursorLockMode.Locked;

			if (onEndConversation != null)
			{
				onEndConversation.Invoke();
			}
		}

		public void SetOpeningSegmentIndex(int index)
		{
			indexOpeningSegment = index;
		}

		public IEnumerator DelayLookAt()
		{
			yield return new WaitForSeconds(lookAtDelay);
			GoToSegment(indexOpeningSegment);
		}

		public void TakeFoodFromPlayer()
		{
			PlayerController.Instance.RemoveFood();
		}

		public NeighbourDialoguePlayer()
		{
			lookAtDelay = 1f;
			onReset = new UnityEvent();
			onCompleted = new UnityEvent();
			onEndConversation = new UnityEvent();
			segments = new List<DialogueSegment>();
			prompts = new List<DialoguePrompt>();
			originalOpeningSegment = -1;
			currentSegment = -1;
			currentPrompt = -1;
		}

		[Header("Settings")]
		[SerializeField]
		private float lookAtDelay;

		[Header("References")]
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

		private int originalOpeningSegment;

		private int currentSegment;

		private int currentPrompt;
	}
}