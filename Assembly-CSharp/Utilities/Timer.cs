using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Utilities
{
	public class Timer : MonoBehaviour
	{
		public void StartTimer()
		{
            if (this.onTimerStart != null)
                this.onTimerStart.Invoke();

            var enumerator = new UpdateTimer_d__4();
            enumerator.__4__this = this;
            enumerator.__1__state = 0;

            StartCoroutine(enumerator);
        }

		private IEnumerator UpdateTimer()
		{
            var enumerator = new UpdateTimer_d__4();
            enumerator.__4__this = this;
            enumerator.__1__state = 0;
            return enumerator;
        }

		public Timer()
		{
            this.duration = 1f;
            this.onTimerStart = new UnityEvent();
            this.onTimerEnd = new UnityEvent();
            base..ctor();
        }

		[SerializeField]
		private float duration;

		[SerializeField]
		private UnityEvent onTimerStart;

		[SerializeField]
		private UnityEvent onTimerEnd;

		private sealed class <UpdateTimer>d__4 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <UpdateTimer>d__4(int <>1__state)
			{
			}

			private void Dispose()
			{
			}

			private bool MoveNext()
			{
				switch (this.__1__state)
				{
					case 0:
						this.__1__state = -1;
						if (this.__4__this == null)
							break;

						this.__2__current = new WaitForSeconds(this.__4__this.duration);
						this.__1__state = 1;
						return true;

					case 1:
						this.__1__state = -1;
						if (this.__4__this == null)
							break;

						if (this.__4__this.onTimerEnd != null)
							this.__4__this.onTimerEnd.Invoke();
						break;
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

			public Timer <>4__this;
		}
	}
}
