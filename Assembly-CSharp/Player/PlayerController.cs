using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Leftovers.General;
using Leftovers.UI;

namespace Leftovers.Player
{
	public class PlayerController : MonoBehaviour
	{
		public static readonly int HashBoolShowFood = Animator.StringToHash("ShowFood");
		private static PlayerController instance;
		public static float MouseSensitivity = 1.0f;

		[SerializeField]
		public bool handleKeyboardInput;

		[SerializeField]
		public bool handleMouseInput;

		[SerializeField]
		private float moveSpeed;

		[SerializeField]
		private float gravity;

		[SerializeField]
		private float groundCheckRadius;

		[SerializeField]
		private LayerMask groundCheckLayerMask;

		[SerializeField]
		private float footstepsInterval;

		[SerializeField]
		private AudioSource footstepsAudioSource;

		[SerializeField]
		private AudioClip footstepsAudioClip;

		[SerializeField]
		private float cameraSpeed;

		[SerializeField]
		private Vector2 cameraRotationXLimits;

		[SerializeField]
		private float lookAtSpeed;

		[SerializeField]
		private float neighbourLookAtOffsetHeight;

		[SerializeField]
		private float neighbourLookAtOffsetDistance;

		[SerializeField]
		private float zoomOriginal;

		[SerializeField]
		private float zoomIn;

		[SerializeField]
		private float promptLookSpeed;

		[SerializeField]
		private Vector2 promptRotationXLimits;

		[SerializeField]
		private Vector2 promptRotationYLimits;

		[SerializeField]
		private Vector2 noddingThreshold;

		[SerializeField]
		private Vector2 shakingThreshold;

		[SerializeField]
		private CharacterController characterController;

		[SerializeField]
		private Camera controlledCamera;

		[SerializeField]
		private Transform cameraContainer;

		[SerializeField]
		public Animator animator;

		[SerializeField]
		private Transform neighbourLookAt;

		[SerializeField]
		private GameObject promptIndicator;

		[SerializeField]
		private GameObject pauseMenu;

		[SerializeField]
		private Transform[] shortcutTeleportations;

		private bool listenPrompt;
		public Transform lookAt;
		private float promptRotationX;
		private float promptRotationY;
		private int checkShake;
		private int checkNod;
		private int numberOfShakes;
		private int numberOfNods;
		private UnityEvent OnNod;
		private UnityEvent OnShake;
		private UnityEvent OnShowFood;
		private Vector3 fallingVelocity;
		private float rotationX;
		private float rotationY;
		private ZoomPhase zoomPhase;
		private float zoomDuration;
		private float zoomTimer;
		private float zoomStartAmount;
		private float zoomAmount;
		private bool paused;
		private bool pausedMouse;
		private bool pausedKeyboard;
		private bool canOpenEscape;
		private Coroutine footstepsCoroutine;

		private enum ZoomPhase
		{
			None,
			In,
			Out
		}

		public static PlayerController Instance
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

		public PlayerController()
		{
			handleKeyboardInput = true;
			handleMouseInput = true;
			moveSpeed = 1.0f;
			gravity = -9.8f;
			groundCheckRadius = 1.0f;
			footstepsInterval = 0.5f;
			cameraSpeed = 1.0f;
			cameraRotationXLimits = Vector2.zero;
			lookAtSpeed = 1.0f;
			neighbourLookAtOffsetHeight = 0.1f;
			neighbourLookAtOffsetDistance = 0.5f;
			zoomIn = 2.0f;
			promptLookSpeed = 1.0f;
			promptRotationXLimits = Vector2.zero;
			promptRotationYLimits = Vector2.zero;
			noddingThreshold = Vector2.zero;
			shakingThreshold = Vector2.zero;
			OnNod = new UnityEvent();
			OnShake = new UnityEvent();
			OnShowFood = new UnityEvent();
			fallingVelocity = Vector3.zero;
		}

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			Transform cameraTransform = controlledCamera.transform;
			cameraTransform.SetParent(cameraContainer);

			cameraTransform.localPosition = Vector3.zero;
			cameraTransform.localEulerAngles = Vector3.zero;

			rotationX = 0f;
			rotationY = transform.eulerAngles.y;
		}

		private void OnDestroy()
		{
			handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;
			Instance = null;
		}

		private void Update()
		{
			if (promptIndicator != null)
				promptIndicator.SetActive(listenPrompt);

			float deltaTime = Time.deltaTime;

			if (handleKeyboardInput)
			{
				float horizontal = Input.GetAxis("Horizontal");
				float vertical = Input.GetAxis("Vertical");
				Vector3 right = transform.right;
				Vector3 forward = transform.forward;
				Vector3 moveVector = (right * horizontal + forward * vertical) * moveSpeed * deltaTime;
				if (characterController != null)
					characterController.Move(moveVector);
			}
			else if (footstepsCoroutine != null)
			{
				StopCoroutine(footstepsCoroutine);
				footstepsCoroutine = null;
			}

			if (lookAt != null)
			{
				if (handleMouseInput)
				{
					float mouseX = Input.GetAxis("Mouse X");
					float mouseY = Input.GetAxis("Mouse Y");
					float mouseYDelta = mouseY * MouseSensitivity * cameraSpeed;

					promptRotationY += MouseSensitivity * mouseX * cameraSpeed;
					promptRotationX = Mathf.Clamp(promptRotationX - mouseYDelta, promptRotationXLimits.x, promptRotationXLimits.y);
					promptRotationY = Mathf.Clamp(promptRotationY, promptRotationYLimits.x, promptRotationYLimits.y);

					if (listenPrompt)
					{
						CheckPrompt();
					}
				}

				Vector3 lookAtPosition = lookAt.position;
				Vector3 containerPosition = cameraContainer.position;
				Vector3 direction = lookAtPosition - containerPosition;
				Quaternion lookRotation = Quaternion.LookRotation(direction);
				Quaternion promptRotation = Quaternion.Euler(promptRotationX, promptRotationY, 0f);
				Quaternion targetRotation = lookRotation * promptRotation;
				cameraContainer.rotation = Quaternion.Slerp(cameraContainer.rotation, targetRotation, promptLookSpeed * deltaTime);

				Vector3 euler = cameraContainer.rotation.eulerAngles;
				rotationY = euler.y;
				float x = euler.x;
				if (cameraRotationXLimits.x > x)
					x = cameraRotationXLimits.x;
				else if (x > cameraRotationXLimits.y)
					x = x - 360f;
				rotationX = x;
			}
			else if (handleMouseInput)
			{
				float mouseX = Input.GetAxis("Mouse X");
				float mouseY = Input.GetAxis("Mouse Y");

				rotationX = Mathf.Clamp(rotationX - mouseY * MouseSensitivity * cameraSpeed, cameraRotationXLimits.x, cameraRotationXLimits.y);
				cameraContainer.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

				rotationY += mouseX * MouseSensitivity * cameraSpeed;
				Vector3 euler = transform.eulerAngles;
				transform.eulerAngles = new Vector3(euler.x, rotationY, euler.z);
			}

			Vector3 groundPos = transform.position;
			bool isGrounded = Physics.CheckSphere(groundPos, groundCheckRadius, groundCheckLayerMask);
			fallingVelocity.y = isGrounded ? 0f : fallingVelocity.y + deltaTime * gravity;
			if (characterController != null)
				characterController.Move(fallingVelocity * deltaTime);

			if (listenPrompt && GameState.Instance != null && GameState.Instance.CanShowFood)
			{
				if (Input.GetMouseButtonDown(1) && animator != null)
					animator.SetBool(HashBoolShowFood, true);
				if (Input.GetMouseButtonUp(1) && animator != null)
					animator.SetBool(HashBoolShowFood, false);
				if (Input.GetKeyDown(KeyCode.Q))
				{
					if (OnShowFood != null)
						OnShowFood.Invoke();
				}
			}

			if (Input.GetKey(KeyCode.L))
			{
				for (int i = 0; i < 9; i++)
				{
					if (Input.GetKeyDown(KeyCode.Alpha1 + i))
						OpenUIWithAnimation(i);
				}
			}

			if (zoomPhase != ZoomPhase.None)
			{
				if (zoomPhase == ZoomPhase.In)
				{
					zoomTimer += deltaTime;
					zoomAmount = Mathf.Lerp(zoomStartAmount, zoomIn, zoomTimer / zoomDuration);
					if (zoomTimer > zoomDuration)
					{
						zoomPhase = ZoomPhase.None;
						zoomAmount = zoomIn;
					}
				}
				else if (zoomPhase == ZoomPhase.Out)
				{
					zoomTimer += deltaTime;
					zoomAmount = Mathf.Lerp(zoomStartAmount, zoomOriginal, zoomTimer / zoomDuration);
					if (zoomTimer > zoomDuration)
					{
						zoomPhase = ZoomPhase.None;
						zoomAmount = zoomOriginal;
					}
				}
			}

			if (controlledCamera != null)
				controlledCamera.transform.localPosition = Vector3.forward * zoomAmount;

			if (canOpenEscape && Input.GetKeyDown(KeyCode.Escape))
			{
				paused = !paused;
				if (!paused)
					ResumeGame();
				else
					PauseGame();
			}

			if (cameraContainer != null && neighbourLookAt != null)
			{
				Vector3 forward = cameraContainer.forward;
				Vector3 camPos = cameraContainer.position;
				Vector3 up = Vector3.up;

				Vector3 neighbourPos = camPos + forward * neighbourLookAtOffsetDistance + up * neighbourLookAtOffsetHeight;
				neighbourLookAt.position = neighbourPos;

				Vector3 lookDirection = cameraContainer.position - neighbourLookAt.position;
				neighbourLookAt.forward = lookDirection;
			}
		}

		private void OpenUIWithAnimation(int index)
		{
			handleKeyboardInput = false;
			handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;

			int capturedIndex = index;
			UIManager.Instance.FadeInAndOut(
				() => {
					if (characterController != null)
						characterController.enabled = false;

					if (capturedIndex < shortcutTeleportations.Length && shortcutTeleportations[capturedIndex] != null)
					{
						transform.position = shortcutTeleportations[capturedIndex].position;
						transform.eulerAngles = shortcutTeleportations[capturedIndex].eulerAngles;
						rotationY = transform.eulerAngles.y;
					}

					if (characterController != null)
						characterController.enabled = true;
				},
				() => {
					handleKeyboardInput = true;
					handleMouseInput = true;
					Cursor.lockState = CursorLockMode.Locked;
				}
			);
		}

		public void ResetRotationValues()
		{
			promptRotationX = 0f;
			promptRotationY = 0f;

			if (lookAt != null && cameraContainer != null)
			{
				rotationY = cameraContainer.rotation.eulerAngles.y;
				return;
			}

			rotationY = transform.eulerAngles.y;
		}

		private float GetLimitedRotationX(float rotation)
		{
			if (cameraRotationXLimits.x > rotation)
				return cameraRotationXLimits.x;

			if (rotation > cameraRotationXLimits.y)
				return rotation - 360f;

			return rotation;
		}

		private void CheckPrompt()
		{
			if (checkShake == 0)
			{
				if (shakingThreshold.x > promptRotationY)
				{
					checkShake = 1;
					numberOfShakes--;
				}
				else if (promptRotationY > shakingThreshold.y)
				{
					checkShake = 2;
					numberOfShakes--;
				}
			}
			else if (checkShake == 1)
			{
				if (promptRotationY > shakingThreshold.y)
				{
					checkShake = 2;
					numberOfShakes--;
				}
			}
			else if (checkShake == 2)
			{
				if (shakingThreshold.x > promptRotationY)
				{
					checkShake = 1;
					numberOfShakes--;
				}
			}

			if (checkNod == 0)
			{
				if (noddingThreshold.x > promptRotationX)
				{
					checkNod = 1;
					numberOfNods--;
				}
				else if (promptRotationX > noddingThreshold.y)
				{
					checkNod = 2;
					numberOfNods--;
				}
			}
			else if (checkNod == 1)
			{
				if (promptRotationX > noddingThreshold.y)
				{
					checkNod = 2;
					numberOfNods--;
				}
			}
			else if (checkNod == 2)
			{
				if (noddingThreshold.x > promptRotationX)
				{
					checkNod = 1;
					numberOfNods--;
				}
			}

			if (numberOfShakes <= 0 && OnShake != null)
			{
				OnShake.Invoke();
			}
			else if (numberOfNods <= 0 && OnNod != null)
			{
				OnNod.Invoke();
			}
		}

		public void ShowFood()
		{
			if (OnShowFood != null)
				OnShowFood.Invoke();
		}

		public void PutAwayFood()
		{
			animator.SetBool(HashBoolShowFood, false);
		}

		public void RemoveFood()
		{
			animator.SetBool(HashBoolShowFood, false);
			animator.Play("RemoveFood");
			GameState.Instance.NumberOfLeftOvers = GameState.Instance.NumberOfLeftOvers - 1;
		}

		public void SetLookAt(Transform lookAtTransform)
		{
			lookAt = lookAtTransform;
		}

		public void StartZoomIn(float duration)
		{
			zoomStartAmount = zoomAmount;
			zoomDuration = duration;
			zoomTimer = 0f;
			zoomPhase = ZoomPhase.In;
		}

		public void StartZoomOut(float duration)
		{
			Debug.Log("Starting zoom out");
			zoomStartAmount = zoomAmount;
			zoomDuration = duration;
			zoomTimer = 0f;
			zoomPhase = ZoomPhase.Out;
		}

		public void StartHandlingKeyboardInput()
		{
			handleKeyboardInput = true;
		}

		public void StopHandlingKeyboardInput()
		{
			handleKeyboardInput = false;
		}

		public void StartHandlingMouseInput()
		{
			handleMouseInput = true;
			Cursor.lockState = CursorLockMode.Locked;
		}

		public void StopHandlingMouseInput()
		{
			handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;
		}

		public void StartListeningToPrompt(UnityAction nodListener, UnityAction shakeListener, UnityAction showFoodListener)
		{
			listenPrompt = true;
			promptRotationX = 0f;
			promptRotationY = 0f;
			checkNod = 0;
			checkShake = 0;
			numberOfNods = 4;
			numberOfShakes = 4;
			handleMouseInput = true;
			Cursor.lockState = CursorLockMode.Locked;

			OnNod.AddListener(nodListener);
			OnShake.AddListener(shakeListener);
			OnShowFood.AddListener(showFoodListener);
		}

		public void StopListeningToPrompt(UnityAction nodListener, UnityAction shakeListener, UnityAction showFoodListener)
		{
			listenPrompt = false;
			promptRotationX = 0f;
			promptRotationY = 0f;
			handleMouseInput = false;
			Cursor.lockState = CursorLockMode.None;

			if (animator != null)
				animator.SetBool(HashBoolShowFood, false);

			OnNod.RemoveListener(nodListener);
			OnShake.RemoveListener(shakeListener);
			OnShowFood.RemoveListener(showFoodListener);
		}

		public void CopyCameraTransform(Transform copier)
		{
			if (cameraContainer == null || copier == null)
				return;

			copier.position = cameraContainer.position;
			copier.rotation = cameraContainer.rotation;
		}

		public void PauseGame()
		{
			pausedMouse = handleMouseInput;
			pausedKeyboard = handleKeyboardInput;
			handleMouseInput = false;
			handleKeyboardInput = false;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0f;
			pauseMenu.SetActive(true);
		}

		public void ResumeGame()
		{
			if (pausedMouse)
			{
				handleMouseInput = true;
				Cursor.lockState = CursorLockMode.Locked;
			}

			if (pausedKeyboard)
				handleKeyboardInput = true;

			Time.timeScale = 1f;
			pauseMenu.SetActive(false);
		}

		public void CanOpenEscapeMenu()
		{
			canOpenEscape = true;
		}

		private IEnumerator PlayFootsteps()
		{
			while (true)
			{
				footstepsAudioSource.PlayOneShot(footstepsAudioClip);
				yield return new WaitForSeconds(footstepsInterval);
			}
		}
	}
}
