using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace Leftovers.UI
{
	public class LeftoverCounter : MonoBehaviour
	{
		public void SetCounter(string text)
		{
            if (textCounter == null) throw new Exception();

            textCounter.text = text;

            if (text == endCounter)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                    flashCoroutine = null;
                }

                var flashRoutine = new FlashTextCoroutine(this, flashColor, endColor);
                flashCoroutine = StartCoroutine(flashRoutine);
            }
            else if (text != startCounter)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                    flashCoroutine = null;
                }

                var flashRoutine = new FlashTextCoroutine(this, flashColor, originalColor);
                flashCoroutine = StartCoroutine(flashRoutine);
            }
        }

		private IEnumerator FlashText(Color startColor, Color endColor)
		{
            var coroutine = new FlashTextCoroutine(this, startColor, endColor);
            return coroutine;
        }

		public LeftoverCounter()
		{
            originalColor = LightmapperUtils.ExtractColorTemperature(Color.black);
            flashColor = LightmapperUtils.ExtractColorTemperature(Color.white);
            endColor = LightmapperUtils.ExtractColorTemperature(Color.gray);

            originalSize = 1f;
            flashSize = 1f;
            flashDuration = 2f;
            returnDuration = 1f;

            endCounter = "End";¡
            startCounter = "Start";¡

            base..ctor();
        }

		[SerializeField]
		private Color originalColor;

		[SerializeField]
		private Color flashColor;

		[SerializeField]
		private Color endColor;

		[SerializeField]
		private float originalSize;

		[SerializeField]
		private float flashSize;

		[SerializeField]
		private float flashDuration;

		[SerializeField]
		private float returnDuration;

		[SerializeField]
		private string endCounter;

		[SerializeField]
		private string startCounter;

		[SerializeField]
		private TMP_Text textCounter;

		private Coroutine flashCoroutine;

		private sealed class <FlashText>d__12 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <FlashText>d__12(int <>1__state)
			{
			}

			private void Dispose()
			{
			}

			private bool MoveNext()
			{
				if (__1__state == 0)
				{
					__1__state = -1;
					_timer = 0f;
					if (__4__this == null || __4__this.textCounter == null)
						throw new NullReferenceException();

					__4__this.textCounter.fontSize = __4__this.flashSize;
					__4__this.textCounter.color = startColor;
				}
				else if (__1__state == 1)
				{
					__1__state = -1;
				}

				if (__1__state == -1)
				{
					if (_timer < __4__this.flashDuration)
					{
						_timer += Time.deltaTime;
						__2__current = null;
						__1__state = 1;
						return true;
					}

					_timer = 0f;
					if (__4__this.textCounter == null)
						throw new NullReferenceException();

					float t = _timer / __4__this.returnDuration;
					__4__this.textCounter.fontSize = Mathf.Lerp(__4__this.flashSize, __4__this.originalSize, t);
					__4__this.textCounter.color = Color.Lerp(endColor, startColor, t);

					_timer += Time.deltaTime;
					__2__current = null;
					__1__state = 2;
					return true;
				}
				else if (__1__state == 2)
				{
					__1__state = -1;
					if (__4__this.textCounter == null)
						throw new NullReferenceException();

					__4__this.textCounter.fontSize = __4__this.originalSize;
					__4__this.textCounter.color = endColor;
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

			public LeftoverCounter <>4__this;

			public Color startColor;

			public Color endColor;

			private float <timer>5__2;
		}
	}
}
