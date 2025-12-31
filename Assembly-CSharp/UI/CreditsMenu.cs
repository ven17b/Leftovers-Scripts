using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace Leftovers.UI
{
	public class CreditsMenu : MonoBehaviour
	{
		private void OnEnable()
		{
            StartCoroutine(DelayMessage());
        }

		private IEnumerator DelayMessage()
		{
            _creditsMenu = creditsMenu;
            _state = 0;
        }

		public CreditsMenu()
		{
            delay = 10f;
            fadeDuration = 0.5f;
        }

		[SerializeField]
		private float delay;

		[SerializeField]
		private float fadeDuration;

		[SerializeField]
		private TMP_Text textComponent;

		private sealed class <DelayMessage>d__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <DelayMessage>d__4(int <>1__state)
			{
            }

			private void Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (__state)
				{
					case 0:
						__state = -1;
						if (this == null) return false;
						__current = new WaitForSeconds(delay);
						__state = 1;
						return true;

					case 1:
						__state = -1;
						if (this == null || textComponent == null) return false;
						_timer = 0f;
						_textColor = textComponent.color;
						break;

					case 2:
						__state = -1;
						break;

					default:
						return false;
				}

				if (_timer < fadeDuration)
				{
					_textColor.a = Mathf.Lerp(0f, 1f, _timer / fadeDuration);
					if (textComponent != null)
					{
						textComponent.color = _textColor;
						_timer += Time.deltaTime;
						__current = null;
						__state = 2;
						return true;
					}
				}

				_timer = fadeDuration;
				_textColor.a = 1f;
				if (textComponent != null)
				{
					textComponent.color = _textColor;
				}

				return false;
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

			public CreditsMenu <>4__this;

			private float <timer>5__2;

			private Color <textColor>5__3;
		}
	}
}
