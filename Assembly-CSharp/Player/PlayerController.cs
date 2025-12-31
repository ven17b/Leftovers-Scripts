using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Player
{
	public class PlayerController : MonoBehaviour
	{
		public static PlayerController Instance
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
            Instance = this;
        }

		private void Start()
		{
            if (controlledCamera == null)
                sub_180157B40(controlledCamera);

            var cameraTransform = controlledCamera.transform;
            cameraTransform.SetParent(cameraContainer, false);

            cameraTransform.localPosition = Vector3.zero;
            cameraTransform.localEulerAngles = Vector3.zero;

            rotationX = 0f;

            var myTransform = transform;
            if (myTransform == null)
                sub_180157B40(controlledCamera);

            rotationY = myTransform.eulerAngles.y;
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

            if (handleKeyboardInput && characterController != null)
            {
                float Axis = Input.GetAxis("Horizontal");
                float v7 = Input.GetAxis("Vertical");
                Vector3 right = transform.right;
                Vector3 forward = transform.forward;
                Vector3 v132 = (right * Axis + forward * v7) * moveSpeed * deltaTime;
                characterController.Move(v132);
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
                    float v35 = Input.GetAxis("Mouse X");
                    float MouseSensitivity = Leftovers_Player_PlayerController_TypeInfo.MouseSensitivity;
                    float v39 = Input.GetAxis("Mouse Y");
                    float v41 = v39 * Leftovers_Player_PlayerController_TypeInfo.MouseSensitivity * cameraSpeed;
                    promptRotationY += MouseSensitivity * v35 * cameraSpeed;
                    promptRotationX = Mathf.Clamp(promptRotationX - v41, promptRotationXLimits.x, promptRotationXLimits.y);
                    promptRotationY = Mathf.Clamp(promptRotationY, promptRotationYLimits.x, promptRotationYLimits.y);

                    if (listenPrompt)
                    {
                        if (checkShake > 0)
                        {
                            int v45 = checkShake - 1;
                            if (v45 == 1 && shakingThreshold.x > promptRotationY)
                                checkShake = 1;
                            else if (v45 == 2 && promptRotationY > shakingThreshold.y)
                                checkShake = 2;
                            numberOfShakes--;
                        }
                        if (checkNod > 0)
                        {
                            int v45 = checkNod - 1;
                            if (v45 == 1 && noddingThreshold.x > promptRotationX)
                                checkNod = 1;
                            else if (v45 == 2 && promptRotationX <= noddingThreshold.y)
                                checkNod = 2;
                            numberOfNods--;
                        }
                        if (numberOfShakes <= 0)
                            OnShake?.Invoke();
                        if (numberOfNods > 0)
                            OnNod?.Invoke();
                    }
                }

                Vector3 position = lookAt.position;
                if (cameraContainer != null)
                {
                    Vector3 v51 = cameraContainer.position;
                    Vector3 direction = position - v51;
                    Quaternion v56 = Quaternion.LookRotation(direction);
                    Quaternion v57 = Quaternion.Euler(promptRotationX, promptRotationY, 0);
                    Quaternion v58 = v56 * v57;
                    cameraContainer.rotation = Quaternion.Slerp(cameraContainer.rotation, v58, promptLookSpeed * deltaTime);
                    Vector3 euler = cameraContainer.rotation.eulerAngles;
                    rotationY = euler.y;
                    float x = euler.x;
                    if (x < cameraRotationXLimits.x) x = cameraRotationXLimits.x;
                    else if (x > cameraRotationXLimits.y) x -= 360f;
                    rotationX = x;
                }
            }
            else if (handleMouseInput)
            {
                rotationX = Mathf.Clamp(rotationX - Input.GetAxis("Mouse Y") * Leftovers_Player_PlayerController_TypeInfo.MouseSensitivity * cameraSpeed, cameraRotationXLimits.x, cameraRotationXLimits.y);
                cameraContainer.localRotation = Quaternion.Euler(rotationX, 0, 0);
                rotationY += Input.GetAxis("Mouse X") * Leftovers_Player_PlayerController_TypeInfo.MouseSensitivity * cameraSpeed;
                Vector3 euler = transform.eulerAngles;
                transform.eulerAngles = new Vector3(euler.x, rotationY, euler.z);
            }

            Vector3 groundPos = transform.position;
            Vector3 v132 = groundPos;
            fallingVelocity.y = Physics.CheckSphere(groundPos, groundCheckRadius, groundCheckLayerMask) ? 0 : fallingVelocity.y + deltaTime * gravity;
            if (characterController != null)
                characterController.Move(fallingVelocity * deltaTime);

            if (listenPrompt && GameState.instance != null && GameState.instance.CanShowFood)
            {
                if (Input.GetMouseButtonDown(1))
                    animator?.SetBool(HashBoolShowFood, true);
                if (Input.GetMouseButtonUp(1))
                    animator?.SetBool(HashBoolShowFood, false);
                if (Input.GetKeyDown(KeyCode.Q))
                    OnShowFood?.Invoke();
            }

            if (Input.GetKey(KeyCode.L))
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                        OpenUIWithAnimation(i);
                }
            }

            if (zoomPhase != 0)
            {
                if (zoomPhase == 1)
                {
                    zoomTimer += deltaTime;
                    zoomAmount = Mathf.Lerp(zoomStartAmount, zoomIn, zoomTimer / zoomDuration);
                    if (zoomTimer > zoomDuration)
                    {
                        zoomPhase = 0;
                        zoomAmount = zoomIn;
                    }
                }
                else if (zoomPhase == 2)
                {
                    zoomTimer += deltaTime;
                    zoomAmount = Mathf.Lerp(zoomStartAmount, zoomOriginal, zoomTimer / zoomDuration);
                    if (zoomTimer > zoomDuration)
                    {
                        zoomPhase = 0;
                        zoomAmount = zoomOriginal;
                    }
                }
            }

            if (controlledCamera != null)
                controlledCamera.transform.localPosition = Vector3.forward * zoomAmount;

            if (canOpenEscape && Input.GetKeyDown(KeyCode.Escape))
            {
                paused = !paused;
                if (!paused) ResumeGame();
                else PauseGame();
            }

            if (cameraContainer != null && neighbourLookAt != null)
            {
                Vector3 v123 = cameraContainer.position;
                Vector3 v125 = neighbourLookAt.position;
                Vector3 forward = v123 - v125;
                neighbourLookAt.forward = forward;
            }
        }

		public void ResetRotationValues()
		{
            lookAt = lookAt;
            promptRotationX = 0f;

            if (lookAt != null)
            {
                if (cameraContainer != null)
                {
                    rotationY = cameraContainer.rotation.eulerAngles.y;
                    return;
                }
            }

            var t = this.transform;
            if (t == null)
                throw new NullReferenceException();

            rotationY = t.eulerAngles.y;
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
                if (shakingThreshold.x > promptRotationY || promptRotationY <= shakingThreshold.y)
                    checkShake = 1;
                --numberOfShakes;
            }
            else if (checkShake - 1 == 0)
            {
                if (promptRotationY > shakingThreshold.y)
                {
                    checkShake = 2;
                    --numberOfShakes;
                }
            }
            else if (checkShake - 1 == 1 && shakingThreshold.x > promptRotationY)
            {
                checkShake = 1;
                --numberOfShakes;
            }

            if (checkNod == 0)
            {
                if (noddingThreshold.x > promptRotationX || promptRotationX <= noddingThreshold.y)
                    checkNod = 1;
                --numberOfNods;
            }
            else if (checkNod - 1 == 0)
            {
                if (promptRotationX > noddingThreshold.y)
                {
                    checkNod = 2;
                    --numberOfNods;
                }
            }
            else if (checkNod - 1 == 1 && noddingThreshold.x > promptRotationX)
            {
                checkNod = 1;
                --numberOfNods;
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
            OnShowFood?.Invoke();
        }

		public void PutAwayFood()
		{
            if (animator == null)
                throw new NullReferenceException();

            UnityEngine.Animator animatorRef = animator;
            animatorRef.SetBool(PlayerController.HashBoolShowFood, false);
        }

		public void RemoveFood()
		{
            if (animator == null)
                throw new NullReferenceException();

            animator.SetBool(PlayerController.HashBoolShowFood, false);
            animator.Play("RemoveFood");

            var gameState = Leftovers.General.GameState.Instance;
            if (gameState == null)
                throw new NullReferenceException();

            gameState.NumberOfLeftOvers = gameState.NumberOfLeftOvers - 1;
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
            zoomPhase = 1;
        }

		public void StartZoomOut(float duration)
		{
            Debug.Log("Starting zoom out");
            zoomStartAmount = zoomAmount;
            zoomDuration = duration;
            zoomTimer = 0f;
            zoomPhase = 2;
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
            numberOfNods = 4;
            numberOfShakes = 4;
            handleMouseInput = true;
            Cursor.lockState = CursorLockMode.Locked;

            OnNod?.AddListener(nodListener);
            OnShake?.AddListener(shakeListener);
            OnShowFood?.AddListener(showFoodListener);
        }

		public void StopListeningToPrompt(UnityAction nodListener, UnityAction shakeListener, UnityAction showFoodListener)
		{
            listenPrompt = false;
            promptRotationX = 0f;
            handleMouseInput = false;
            Cursor.lockState = CursorLockMode.None;

            if (animator != null)
            {
                animator.SetBool(HashBoolShowFood, false);
            }

            OnNod?.RemoveListener(nodListener);
            OnShake?.RemoveListener(shakeListener);
            OnShowFood?.RemoveListener(showFoodListener);
        }

		public void CopyCameraTransform(Transform copier)
		{
            if (cameraContainer == null || copier == null)
            {
                return;
            }

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

            if (pauseMenu == null) throw new NullReferenceException();
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

            if (pauseMenu == null)
                throw new NullReferenceException();

            pauseMenu.SetActive(false);
        }

		public void CanOpenEscapeMenu()
		{
            canOpenEscape = true;
        }

		private IEnumerator PlayFootsteps()
		{
            return new PlayFootsteps_d__84(0) { __4__this = this };
        }

		public PlayerController()
		{
            HashBoolShowFood = Animator.StringToHash("ShowFood");
            instance = null;
            MouseSensitivity = 1.0f;

            handleKeyboardInput = true;
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

		private void <Update>b__64_1()
		{
            handleKeyboardInput = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

		private static readonly int HashBoolShowFood;

		private static PlayerController instance;

		public static float MouseSensitivity;

		private const float PlatformSensitivity = 1f;

		[SerializeField]
		private bool handleKeyboardInput;

		[SerializeField]
		private bool handleMouseInput;

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
		private Animator animator;

		[SerializeField]
		private Transform neighbourLookAt;

		[SerializeField]
		private GameObject promptIndicator;

		[SerializeField]
		private GameObject pauseMenu;

		[SerializeField]
		private Transform[] shortcutTeleportations;

		private bool listenPrompt;

		private Transform lookAt;

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

		private PlayerController.ZoomPhase zoomPhase;

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

		private sealed class <>c__DisplayClass64_0
		{
			public <>c__DisplayClass64_0()
			{
			}

			internal void <Update>b__0()
			{
                var _4__this = this.__4__this;
                if (_4__this == null || _4__this.characterController == null)
                    return;

                _4__this.characterController.enabled = false;

                var transform = _4__this.transform;
                if (transform == null)
                    return;

                var capturedIndex = this.captured;
                if ((uint)capturedIndex >= _4__this.someArray.Length) // assuming __4__this field points to an array length
                    throw new IndexOutOfRangeException();

                var targetTransform = _4__this.someArray[capturedIndex];
                if (targetTransform == null)
                    return;

                transform.position = targetTransform.position;
                transform.eulerAngles = targetTransform.eulerAngles;

                _4__this.rotationY = transform.eulerAngles.y;

                if (_4__this.monitor == null)
                    return;

                _4__this.monitor.enabled = true;
            }

			public int captured;

			public PlayerController <>4__this;
		}

		private sealed class <PlayFootsteps>d__84 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <PlayFootsteps>d__84(int <>1__state)
			{
		      __1__state = state;
			}

			private void Dispose()
			{
			}

			private bool MoveNext()
			{
                var __this = this.__4__this;
                if (this.__1__state > 1)
                    return false;
                this.__1__state = -1;

                if (__this == null || __this.footstepsAudioSource == null)
                    throw new NullReferenceException();

                __this.footstepsAudioSource.PlayOneShot(__this.footstepsAudioClip);

                this.__2__current = new UnityEngine.WaitForSeconds(__this.footstepsInterval);
                this.__1__state = 1;
                return true;
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

			public PlayerController <>4__this;
		}
	}
}
