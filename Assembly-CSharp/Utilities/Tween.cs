using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Utilities
{
	public class Tween : MonoBehaviour
	{
		public void StartTween()
		{
            var enumerator = new _Tweening_d();
            enumerator.__4__this = this;
            enumerator.__1__state = 0;
            StartCoroutine(enumerator);
        }

		private IEnumerator Tweening()
		{
            return new Tweening_d { __1__state = 0, __4__this = this };
        }

		public Tween()
		{
            onTweenStart = new UnityEvent();
            onTweenEnd = new UnityEvent();
        }

		[SerializeField]
		private float tweenDuration;

		[SerializeField]
		private bool enableOnStart;

		[SerializeField]
		private bool disableOnEnd;

		[SerializeField]
		private Transform objectToTween;

		[SerializeField]
		private Transform positionFrom;

		[SerializeField]
		private Transform positionTo;

		[SerializeField]
		private UnityEvent onTweenStart;

		[SerializeField]
		private UnityEvent onTweenEnd;

		private sealed class <Tweening>d__9 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <Tweening>d__9(int <>1__state)
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
                    if (__4__this == null) return false;

                    Debug.Log(__4__this.name + " Tween start");
                    __4__this.onTweenStart?.Invoke();

                    if (__4__this.enableOnStart && __4__this.objectToTween != null)
                        __4__this.objectToTween.gameObject.SetActive(true);

                    _timer_5__2 = 0f;
                }
                else if (__1__state == 1)
                {
                    __1__state = -1;
                }
                else
                {
                    return false;
                }

                if (__4__this.objectToTween != null && _timer_5__2 < __4__this.tweenDuration)
                {
                    Vector3 startPos = __4__this.positionFrom?.position ?? __4__this.objectToTween.position;
                    Vector3 endPos = __4__this.positionTo?.position ?? startPos;
                    float t = Mathf.Clamp01(_timer_5__2 / __4__this.tweenDuration);
                    __4__this.objectToTween.position = Vector3.Lerp(startPos, endPos, t);

                    _timer_5__2 += Time.deltaTime;
                    __2__current = null;
                    __1__state = 1;
                    return true;
                }

                if (__4__this.objectToTween != null && __4__this.positionTo != null)
                    __4__this.objectToTween.position = __4__this.positionTo.position;

                if (__4__this.disableOnEnd && __4__this.objectToTween != null)
                    __4__this.objectToTween.gameObject.SetActive(false);

                __4__this.onTweenEnd?.Invoke();

                Debug.Log(__4__this.name + " Tween end");
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

			public Tween <>4__this;

			private float <timer>5__2;
		}
	}
}
