using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Utilities
{
	public class Tween : MonoBehaviour
	{
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

		public Tween()
		{
			enableOnStart = true;
			disableOnEnd = true;
			onTweenStart = new UnityEvent();
			onTweenEnd = new UnityEvent();
		}

		public void StartTween()
		{
			StartCoroutine(Tweening());
		}

		private IEnumerator Tweening()
		{
			Debug.Log(gameObject.name + " Tween start");

			if (onTweenStart != null)
				onTweenStart.Invoke();

			if (enableOnStart)
				objectToTween.gameObject.SetActive(true);

			float timer = 0f;

			while (timer < tweenDuration)
			{
				Vector3 startPos = positionFrom.position;
				Vector3 endPos = positionTo.position;
				float t = Mathf.Clamp01(timer / tweenDuration);
				objectToTween.position = Vector3.Lerp(startPos, endPos, t);

				timer += Time.deltaTime;
				yield return null;
			}

			objectToTween.position = positionTo.position;

			if (disableOnEnd)
				objectToTween.gameObject.SetActive(false);

			if (onTweenEnd != null)
				onTweenEnd.Invoke();

			Debug.Log(gameObject.name + " Tween end");
		}
	}
}
