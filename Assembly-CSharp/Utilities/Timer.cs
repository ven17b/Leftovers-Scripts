using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Utilities
{
	public class Timer : MonoBehaviour
	{
		[SerializeField]
		private float duration;

		[SerializeField]
		private UnityEvent onTimerStart;

		[SerializeField]
		private UnityEvent onTimerEnd;

		public Timer()
		{
			duration = 1f;
			onTimerStart = new UnityEvent();
			onTimerEnd = new UnityEvent();
		}

		public void StartTimer()
		{
			if (onTimerStart != null)
				onTimerStart.Invoke();

			StartCoroutine(UpdateTimer());
		}

		private IEnumerator UpdateTimer()
		{
			yield return new WaitForSeconds(duration);

			if (onTimerEnd != null)
				onTimerEnd.Invoke();
		}
	}
}
