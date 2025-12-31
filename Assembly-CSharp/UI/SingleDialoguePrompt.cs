using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.UI
{
	public class SingleDialoguePrompt : MonoBehaviour
	{
		public void ShowDialogue()
		{
            var player = Leftovers_Player_PlayerController.instance;
            if (player != null)
                player.handleKeyboardInput = false;

            UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;

            onStartDialogue?.Invoke();

            StartCoroutine(ListenToPromptCoroutine());
        }

		private IEnumerator ListenToPrompt()
		{
            _this = instance;
            _state = 0;
        }

		public SingleDialoguePrompt()
		{
            onStartDialogue = new UnityEngine.Events.UnityEvent();
            onCloseDialogue = new UnityEngine.Events.UnityEvent();
        }

		[SerializeField]
		private string message;

		[SerializeField]
		private float delayMessage;

		[SerializeField]
		private UnityEvent onStartDialogue;

		[SerializeField]
		private UnityEvent onCloseDialogue;

		private sealed class <ListenToPrompt>d__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <ListenToPrompt>d__5(int <>1__state)
			{
              yield break;
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
						if (__4__this == null) return false;
						__2__current = new UnityEngine.WaitForSeconds(__4__this.delayMessage);
						__1__state = 1;
						return true;

					case 1:
						__1__state = -1;
						var uiManager = Leftovers_UI_UIManager.Instance;
						if (__4__this == null || uiManager == null) return false;
						uiManager.SetMessage(__4__this.message, false);
						__2__current = new UnityEngine.WaitForSeconds(1.25f);
						__1__state = 2;
						return true;

					case 2:
						__1__state = -1;
						var promptGO = Leftovers_UI_UIManager.Instance?.promptGameObject;
						if (promptGO != null)
							promptGO.SetActive(true);
						if (UnityEngine.Input.GetMouseButtonDown(0))
						{
							if (promptGO != null)
								promptGO.SetActive(false);

							Leftovers_UI_UIManager.Instance?.SetMessage(string.Empty, false);

							var player = Leftovers_Player_PlayerController.Instance;
							if (player != null)
							{
								player.lockInput = true;
								player.lockCursor = true;
							}

							__4__this?.onCloseDialogue?.Invoke();
							return false;
						}
						__2__current = null;
						__1__state = 3;
						return true;

					case 3:
						__1__state = 2;
						return true;

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

			public SingleDialoguePrompt <>4__this;
		}
	}
}
