using System;
using System.Collections;
using System.Collections.Generic;
using Leftovers.Neighbour;
using Leftovers.Player;
using Leftovers.UI;
using Leftovers.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.General
{
	public class GameState : MonoBehaviour
	{
		public static GameState Instance
		{
			get
			{
				return instance;
			}
			set
			{
				if (instance == null || value == null)
				{
					instance = value;
				}
				else
				{
					Destroy(value.gameObject);
				}
			}
		}

		public int NumberOfLeftOvers
		{
			get
			{
				return numberOfLeftovers;
			}
			set
			{
				numberOfLeftovers = value;
				if (onNumberOfLeftOversChanged != null)
				{
					onNumberOfLeftOversChanged.Invoke(numberOfLeftovers.ToString());
				}
				if (numberOfLeftovers == mistressThreshold)
				{
					if (onMistressThresholdReached != null)
					{
						onMistressThresholdReached.Invoke();
					}
				}
			}
		}

		public bool HasExtraLeftOver
		{
			set
			{
				hasExtraLeftover = value;
				UnityEvent eventToInvoke = value ? onReceivedExtraLeftover : onGaveAwayExtraLeftover;
				if (eventToInvoke != null)
				{
					eventToInvoke.Invoke();
				}
			}
		}

		public bool CanShowFood
		{
			get
			{
				return canShowFood;
			}
			set
			{
				canShowFood = value;
			}
		}

		private void Awake()
		{
			Instance = this;
			Debug.unityLogger.logEnabled = false;
			ResetGame();
		}

		private void OnDestroy()
		{
			Instance = null;
		}

		public void ResetGame()
		{
			NumberOfLeftOvers = startingNumberOfLeftovers;
			numberOfChances = startingNumberOfChances;
			hasExtraLeftover = false;

			if (neighbourStates == null)
			{
				neighbourStates = new List<NeighbourState>();
				NeighbourState[] foundStates = FindObjectsOfType<NeighbourState>();
				foreach (NeighbourState state in foundStates)
				{
					neighbourStates.Add(state);
				}
			}

			foreach (NeighbourState state in neighbourStates)
			{
				state.ResetState();
			}
		}

		private void Update()
		{
			if (Input.GetKey(KeyCode.L))
			{
				if (Input.GetKeyDown(KeyCode.K))
				{
					if (onMistressThresholdReached != null)
					{
						onMistressThresholdReached.Invoke();
					}
				}
				if (Input.GetKeyDown(KeyCode.M))
				{
					bully.SetActive(true);
				}
			}
		}

		[ContextMenu("Decrease Leftovers")]
		public void EditorDecreaseNumberOfLeftovers()
		{
			NumberOfLeftOvers = numberOfLeftovers - 1;
		}

		public void DecreaseNumberOfLeftovers(int number)
		{
			NumberOfLeftOvers = numberOfLeftovers - number;
		}

		[ContextMenu("Cry")]
		public void Cry()
		{
			int index = startingNumberOfChances - numberOfChances;
			numberOfChances--;

			PlayerController.Instance.handleKeyboardInput = false;
			PlayerController.Instance.handleMouseInput = false;
			Cursor.lockState = CursorLockMode.Locked;

			mumDialoguePlayer.OpeningSegmentIndex = chancesOpeningSegmentIndices[index];

			UIManager.Instance.SetInnerMessage("...", cryDuration);

			StartCoroutine(DelayCry());
		}

		private IEnumerator DelayCry()
		{
			yield return new WaitForSeconds(cryDuration);

			mumTeleportation.Teleport();

			yield return new WaitForSeconds(2.75f);

			PlayerController.Instance.handleKeyboardInput = false;
			PlayerController.Instance.handleMouseInput = true;
			Cursor.lockState = CursorLockMode.None;

			mumDialoguePlayer.StartCoroutine(mumDialoguePlayer.DelayLookAt());
		}

		[ContextMenu("Force Mistress Threshold")]
		public void ForceMistressThreshold()
		{
			if (onMistressThresholdReached != null)
			{
				onMistressThresholdReached.Invoke();
			}
		}

		public void RestartOldManConversation()
		{
			if (onRestartOldMan != null)
			{
				onRestartOldMan.Invoke();
			}
		}

		public void RestartMistressChase()
		{
			if (onRestartMistressChase != null)
			{
				onRestartMistressChase.Invoke();
			}
		}

		public void FoundCannedFood()
		{
			if (onFoundCannedFood != null)
			{
				onFoundCannedFood.Invoke();
			}
		}

		public GameState()
		{
			startingNumberOfLeftovers = 10;
			startingNumberOfChances = 3;
			mistressThreshold = 10;
			cryDuration = 4f;
			onNumberOfLeftOversChanged = new StringEvent();
			onMistressThresholdReached = new UnityEvent();
			onReceivedExtraLeftover = new UnityEvent();
			onGaveAwayExtraLeftover = new UnityEvent();
			onFoundCannedFood = new UnityEvent();
		}

		private static GameState instance;

		[Header("Settings")]
		[SerializeField]
		private int startingNumberOfLeftovers;

		[SerializeField]
		private int startingNumberOfChances;

		[SerializeField]
		private int mistressThreshold;

		[SerializeField]
		private int[] chancesOpeningSegmentIndices;

		[SerializeField]
		private float cryDuration;

		[SerializeField]
		private AudioSource cryAudioSource;

		[SerializeField]
		private AudioClip cryAudioClip;

		[Header("References")]
		[SerializeField]
		private NeighbourDialoguePlayer mumDialoguePlayer;

		[SerializeField]
		private Teleportation mumTeleportation;

		[SerializeField]
		private GameObject bully;

		[Header("Events - Restart")]
		[SerializeField]
		private UnityEvent onRestartOldMan;

		[Header("Events - Mistress Chase")]
		[SerializeField]
		private UnityEvent onRestartMistressChase;

		[Header("Events - Leftovers")]
		[SerializeField]
		private StringEvent onNumberOfLeftOversChanged;

		[SerializeField]
		private UnityEvent onMistressThresholdReached;

		[SerializeField]
		private UnityEvent onReceivedExtraLeftover;

		[SerializeField]
		private UnityEvent onGaveAwayExtraLeftover;

		[SerializeField]
		private UnityEvent onFoundCannedFood;

		private int numberOfLeftovers;

		private int numberOfChances;

		private bool canShowFood;

		private bool hasExtraLeftover;

		private List<NeighbourState> neighbourStates;

		[Serializable]
		private class StringEvent : UnityEvent<string>
		{
		}

		[Serializable]
		private class IntEvent : UnityEvent<int>
		{
		}
	}
}
