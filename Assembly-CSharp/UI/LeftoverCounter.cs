using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Leftovers.UI
{
	public class LeftoverCounter : MonoBehaviour
	{
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

		public LeftoverCounter()
		{
			originalColor = Color.white;
			flashColor = Color.white;
			endColor = Color.white;
			originalSize = 1f;
			flashSize = 1f;
			flashDuration = 2f;
			returnDuration = 1f;
			endCounter = "0";
			startCounter = "5";
		}

		public void SetCounter(string text)
		{
			textCounter.text = text;

			if (text == endCounter)
			{
				if (flashCoroutine != null)
				{
					StopCoroutine(flashCoroutine);
					flashCoroutine = null;
				}
				flashCoroutine = StartCoroutine(FlashText(flashColor, endColor));
			}
			else if (text != startCounter)
			{
				if (flashCoroutine != null)
				{
					StopCoroutine(flashCoroutine);
					flashCoroutine = null;
				}
				flashCoroutine = StartCoroutine(FlashText(flashColor, originalColor));
			}
		}

		private IEnumerator FlashText(Color startColor, Color targetEndColor)
		{
			float timer = 0f;

			textCounter.fontSize = flashSize;
			textCounter.color = startColor;

			while (timer < flashDuration)
			{
				timer += Time.deltaTime;
				yield return null;
			}

			timer = 0f;

			while (timer < returnDuration)
			{
				float t = timer / returnDuration;
				textCounter.fontSize = Mathf.Lerp(flashSize, originalSize, t);
				textCounter.color = Color.Lerp(startColor, targetEndColor, t);
				timer += Time.deltaTime;
				yield return null;
			}

			textCounter.fontSize = originalSize;
			textCounter.color = targetEndColor;
		}
	}
}
