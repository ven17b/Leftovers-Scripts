using System;
using System.Collections;
using UnityEngine;

namespace Leftovers.Utilities
{
	public class AudioUtility : MonoBehaviour
	{
		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private float volume;

		public AudioUtility()
		{
			volume = 1.0f;
		}

		public void StartFadeIn(float duration)
		{
			audioSource.volume = 0f;
			audioSource.Play();
			StartCoroutine(FadeIn(duration));
		}

		public void StartFadeOut(float duration)
		{
			audioSource.volume = volume;
			StartCoroutine(FadeOut(duration));
		}

		private IEnumerator FadeIn(float duration)
		{
			float timer = 0f;

			while (timer < duration)
			{
				timer += Time.deltaTime;
				audioSource.volume = Mathf.Lerp(0f, volume, timer / duration);
				yield return null;
			}

			audioSource.volume = volume;
		}

		private IEnumerator FadeOut(float duration)
		{
			float timer = 0f;

			while (timer < duration)
			{
				timer += Time.deltaTime;
				audioSource.volume = Mathf.Lerp(volume, 0f, timer / duration);
				yield return null;
			}

			audioSource.volume = 0f;
			audioSource.Stop();
		}
	}
}
