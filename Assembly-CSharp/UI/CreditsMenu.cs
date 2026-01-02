using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Leftovers.UI
{
	public class CreditsMenu : MonoBehaviour
	{
		[SerializeField]
		private float delay;

		[SerializeField]
		private float fadeDuration;

		[SerializeField]
		private TMP_Text textComponent;

		public CreditsMenu()
		{
			delay = 10f;
			fadeDuration = 0.5f;
		}

		private void OnEnable()
		{
			StartCoroutine(DelayMessage());
		}

		private IEnumerator DelayMessage()
		{
			yield return new WaitForSeconds(delay);

			float timer = 0f;
			Color textColor = textComponent.color;

			while (timer < fadeDuration)
			{
				textColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
				textComponent.color = textColor;
				timer += Time.deltaTime;
				yield return null;
			}

			timer = fadeDuration;
			textColor.a = 1f;
			textComponent.color = textColor;
		}
	}
}
