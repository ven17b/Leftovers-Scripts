using System;
using System.Collections;
using UnityEngine;
using Leftovers.Player;

namespace Leftovers.General
{
	public class Interactor : MonoBehaviour
	{
		private static readonly int HashTriggerKnock = Animator.StringToHash("Knock");
		private static Interactor instance;

		[SerializeField]
		private float raycastDistance;

		[SerializeField]
		private LayerMask raycastMask;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private Camera cameraRaycast;

		[SerializeField]
		private GameObject cursor;

		private Vector3 screenCenter;
		private RaycastHit hitInfo;
		public bool lockedInteraction;
		public Hoverable currentDetectedObject;

		public static Interactor Instance
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

		public Interactor()
		{
			raycastDistance = 1.0f;
			screenCenter = Vector3.zero;
		}

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
		}

		private void OnDestroy()
		{
			Instance = null;
		}

		private void Update()
		{
			if (cursor != null)
				cursor.SetActive(!lockedInteraction);

			if (lockedInteraction || cameraRaycast == null)
				return;

			Ray screenRay = cameraRaycast.ScreenPointToRay(screenCenter);
			Hoverable hoverableComponent = null;

			if (Physics.Raycast(screenRay, out hitInfo, raycastDistance, raycastMask))
			{
				Collider collider = hitInfo.collider;
				if (collider != null)
				{
					GameObject go = collider.gameObject;
					if (go != null)
						hoverableComponent = go.GetComponent<Hoverable>();
				}
			}

			HandleHoverable(hoverableComponent);

			if (Input.GetMouseButtonDown(0))
				HandleInteractable();
		}

		private void HandleHoverable(Hoverable hoverable)
		{
			if (currentDetectedObject != hoverable)
			{
				if (currentDetectedObject != null)
					currentDetectedObject.StopHover();
			}

			currentDetectedObject = hoverable;

			if (currentDetectedObject != null)
			{
				currentDetectedObject.StartHover();
			}
		}

		private void HandleInteractable()
		{
			Debug.Log("HandleInteractable");

			if (currentDetectedObject == null)
				return;

			Interactable interactable = currentDetectedObject as Interactable;
			if (interactable == null)
				return;

			interactable.StartInteract();

			if (animator != null && interactable.AnimationType == InteractionAnimationType.Knock)
			{
				PlayerController.Instance.handleKeyboardInput = false;
				Cursor.lockState = CursorLockMode.None;
				animator.SetTrigger(HashTriggerKnock);
				StartCoroutine(FinishAnimation(2.0f));
			}
		}

		private void PlayAnimation(InteractionAnimationType animationType)
		{
			if (animator != null && animationType == InteractionAnimationType.Knock)
			{
				PlayerController.Instance.handleKeyboardInput = false;
				Cursor.lockState = CursorLockMode.None;
				animator.SetTrigger(HashTriggerKnock);
				StartCoroutine(FinishAnimation(2.0f));
			}
		}

		private IEnumerator FinishAnimation(float duration)
		{
			yield return new WaitForSeconds(duration);
			PlayerController.Instance.handleKeyboardInput = true;
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void LockInteraction()
		{
			lockedInteraction = true;
			currentDetectedObject.StopHover();
			currentDetectedObject = null;
		}

		public void UnlockInteraction()
		{
			lockedInteraction = false;
		}
	}
}
