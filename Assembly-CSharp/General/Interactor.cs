using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Leftovers.General
{
	public class Interactor : MonoBehaviour
	{
		public static Interactor Instance
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		private void Awake()
		{
            Leftovers_General_Interactor.Instance = this;
        }

		private void Start()
		{
            screenCenter = new UnityEngine.Vector3(UnityEngine.Screen.width * 0.5f, UnityEngine.Screen.height * 0.5f, 0f);
        }

		private void OnDestroy()
		{
            Leftovers_General_Interactor.Instance = null;
        }

		private void Update()
		{
            if (cursor != null)
                cursor.SetActive(!lockedInteraction);

            if (lockedInteraction || cameraRaycast == null)
                return;

            Ray screenRay = cameraRaycast.ScreenPointToRay(screenCenter);
            RaycastHit hit;
            LayerMask mask = raycastMask;

            Hoverable hoverableComponent = null;

            if (Physics.Raycast(screenRay, out hit, raycastDistance, mask))
            {
                var collider = hit.collider;
                if (collider != null)
                {
                    var go = collider.gameObject;
                    if (go != null)
                        hoverableComponent = go.GetComponent<Hoverable>();
                }
            }

            HandleHoverable(hoverableComponent, false);

            if (Input.GetMouseButtonDown(0))
                HandleInteractable(false);
        }

		private void HandleHoverable(Hoverable hoverable)
		{
            if (this.currentDetectedObject != hoverable)
            {
                if (this.currentDetectedObject != null)
                    Leftovers_General_Hoverable.StopHover(this.currentDetectedObject);
            }

            this.currentDetectedObject = hoverable;

            if (this.currentDetectedObject != null)
            {
                Leftovers_General_Hoverable newHoverable = this.currentDetectedObject;
                Leftovers_UI_UIManager uiManager = Leftovers_UI_UIManager.Instance;

                if (uiManager != null && uiManager.onStopHover != null)
                {
                    TMPro_TMP_Text.SetText(uiManager.onStopHover, newHoverable.tooltip, true);
                    if (newHoverable.onStartHover != null)
                        newHoverable.onStartHover.Invoke();
                }
            }
        }

		private void HandleInteractable()
		{
            UnityEngine.Debug.Log("HandleInteractable called");

            if (currentDetectedObject == null) return;

            Leftovers_General_Interactable interactable = currentDetectedObject as Leftovers_General_Interactable;
            if (interactable != null)
            {
                if (Leftovers_General_Interactor.Instance != null)
                {
                    var instance = Leftovers_General_Interactor.Instance;
                    instance.lockedInteraction = true;

                    if (instance.currentDetectedObject != null)
                    {
                        instance.currentDetectedObject.StopHover();
                        instance.currentDetectedObject = null;
                    }
                }

                interactable.onInteract?.Invoke();

                if (animator != null && interactable.m_CachedPtr == 1)
                {
                    var playerController = Leftovers_Player_PlayerController.Instance;
                    var playerInstance = playerController?.Instance;
                    if (playerInstance != null)
                    {
                        playerInstance.handleKeyboardInput = false;
                        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
                    }

                    animator.SetTrigger(Leftovers_General_Interactor.HashTriggerKnock);

                    var finishAnimCoroutine = new Leftovers_General_Interactor.FinishAnimation_d__21(0);
                    StartCoroutine(finishAnimCoroutine);
                }
            }
        }

		private void PlayAnimation(InteractionAnimationType animationType)
		{
            if (animator != null && animationType == 1)
            {
                var player = Leftovers_Player_PlayerController.Instance;
                if (player != null)
                {
                    player.fields.handleKeyboardInput = false;
                    UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
                }

                if (animator == null)
                    throw new System.NullReferenceException();

                animator.SetTrigger(HashTriggerKnock);

                var finishAnim = Leftovers_General_Interactor.FinishAnimation();
                UnityEngine.MonoBehaviour.StartCoroutine(finishAnim);
            }
        }

		private IEnumerator FinishAnimation(float duration)
		{
            var enumerator = new Leftovers_General_Interactor_FinishAnimation_d__21();
            enumerator.Dispose();
            enumerator.duration = duration;
            enumerator.__1__state = 0;
            return enumerator;
        }

		public void LockInteraction()
		{
            lockedInteraction = true;
            if (currentDetectedObject == null) throw new NullReferenceException();
            currentDetectedObject.StopHover();
            currentDetectedObject = null;
        }

		public void UnlockInteraction()
		{
            lockedInteraction = false;
        }

		public Interactor()
		{
            get
            {
                return Leftovers_General_Interactor_TypeInfo.static_fields.instance;
            }
        }

		private static readonly int HashTriggerKnock;

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

		private bool lockedInteraction;

		private Hoverable currentDetectedObject;

		private sealed class <FinishAnimation>d__21 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <FinishAnimation>d__21(int <>1__state)
			{
			}

			private void Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (__1__state)
				{
					case 0:
						__1__state = -1;
						__2__current = new UnityEngine.WaitForSeconds(duration);
						__1__state = 1;
						return true;
					case 1:
						__1__state = -1;
						var playerController = Leftovers_Player_PlayerController.Instance;
						if (playerController != null)
						{
							playerController.fields.lockedInteraction = true;
							UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Locked;
						}
						return false;
					default:
						return false;
				}
			}

			private object Current
			{
				get
				{
					return null;
				}
			}

			private void Reset()
			{
			}

			private object Current
			{
				get
				{
					return null;
				}
			}

			private int <>1__state;

			private object <>2__current;

			public float duration;
		}
	}
}
