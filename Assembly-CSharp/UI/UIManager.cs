using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Leftovers.UI
{
	public class UIManager : MonoBehaviour
	{
		public static UIManager Instance
		{
			get
			{
				return instance;
			}
			set
			{
				if (instance == null || value == null)
				{
					instance = value;
				}
				else
				{
					Destroy(value.gameObject);
				}
			}
		}

		private void Awake()
		{
			Instance = this;
		}

		private void OnDestroy()
		{
			Instance = null;
		}

		public void SetMessage(string message, float duration = -1f)
		{
			if (clearMessageCoroutine != null)
			{
				StopCoroutine(clearMessageCoroutine);
				clearMessageCoroutine = null;
			}

			if (duration < 0f)
			{
				messageText.SetText(message);
			}
			else
			{
				StartCoroutine(ShowAndClearMessage(messageText, message, duration));
			}
		}

		public void SetMessage(string message)
		{
			if (clearMessageCoroutine != null)
			{
				StopCoroutine(clearMessageCoroutine);
				clearMessageCoroutine = null;
			}

			messageText.SetText(message);
		}

		public void ClearMessage(float delay)
		{
			if (clearMessageCoroutine != null)
			{
				StopCoroutine(clearMessageCoroutine);
				clearMessageCoroutine = null;
			}

			clearMessageCoroutine = StartCoroutine(DelayedClearMessage(messageText, delay));
		}

		public void SetInnerMessage(string message, float duration = -1f)
		{
			if (clearInnerMessageCoroutine != null)
			{
				StopCoroutine(clearInnerMessageCoroutine);
				clearInnerMessageCoroutine = null;
			}

			if (duration < 0f)
			{
				innerMessageText.SetText(message);
			}
			else
			{
				StartCoroutine(ShowAndClearMessage(innerMessageText, message, duration));
			}
		}

		public void SetInnerMessage(string message)
		{
			if (clearInnerMessageCoroutine != null)
			{
				StopCoroutine(clearInnerMessageCoroutine);
				clearInnerMessageCoroutine = null;
			}

			innerMessageText.SetText(message);
		}

		public void ClearInnerMessage(float delay)
		{
			if (clearInnerMessageCoroutine != null)
			{
				StopCoroutine(clearInnerMessageCoroutine);
				clearInnerMessageCoroutine = null;
			}

			clearInnerMessageCoroutine = StartCoroutine(DelayedClearInnerMessage(innerMessageText, delay));
		}

		public void SetDialogueClickPromptVisibility(bool visibility)
		{
			dialogueClickPrompt.SetActive(visibility);
		}

		public void SetTooltip(string message)
		{
			tooltipText.SetText(message);
		}

		public void FadeIn(UnityAction callback)
		{
			StartCoroutine(FadingIn(callback));
		}

		public void FadeIn()
		{
			StartCoroutine(FadingIn(null));
		}

		public void FadeOut(UnityAction callback)
		{
			StartCoroutine(FadingOut(callback));
		}

		public void FadeOut()
		{
			StartCoroutine(FadingOut(null));
		}

		public void FadeInAndOut(UnityAction callbackIn, UnityAction callbackOut)
		{
			StartCoroutine(FadingInAndOut(callbackIn, callbackOut));
		}

		private IEnumerator FadingIn(UnityAction callback)
		{
			float timer = 0f;
			transitionImage.color = originalColor;

			while (timer < fadeInDuration)
			{
				transitionImage.color = Color.Lerp(originalColor, fadedColor, timer / fadeInDuration);
				timer += Time.deltaTime;
				yield return null;
			}

			transitionImage.color = fadedColor;
			callback?.Invoke();
		}

		private IEnumerator FadingOut(UnityAction callback)
		{
			float timer = 0f;
			transitionImage.color = fadedColor;

			while (timer < fadeOutDuration)
			{
				transitionImage.color = Color.Lerp(fadedColor, originalColor, timer / fadeOutDuration);
				timer += Time.deltaTime;
				yield return null;
			}

			transitionImage.color = originalColor;
			callback?.Invoke();
		}

		private IEnumerator FadingInAndOut(UnityAction callbackIn, UnityAction callbackOut)
		{
			float timer = 0f;
			transitionImage.color = originalColor;

			while (timer < fadeInDuration)
			{
				transitionImage.color = Color.Lerp(originalColor, fadedColor, timer / fadeInDuration);
				timer += Time.deltaTime;
				yield return null;
			}

			transitionImage.color = fadedColor;
			callbackIn?.Invoke();

			yield return new WaitForSeconds(fadeInAndOutDelay);

			timer = 0f;
			while (timer < fadeOutDuration)
			{
				transitionImage.color = Color.Lerp(fadedColor, originalColor, timer / fadeOutDuration);
				timer += Time.deltaTime;
				yield return null;
			}

			transitionImage.color = originalColor;
			callbackOut?.Invoke();
		}

		private IEnumerator ShowAndClearMessage(TMP_Text textComponent, string message, float duration)
		{
			textComponent.SetText(message);
			yield return new WaitForSeconds(duration);
			textComponent.SetText(string.Empty);
		}

		private IEnumerator DelayedClearMessage(TMP_Text textComponent, float delay)
		{
			yield return new WaitForSeconds(delay);
			textComponent.SetText(string.Empty);
			clearMessageCoroutine = null;
		}

		private IEnumerator DelayedClearInnerMessage(TMP_Text textComponent, float delay)
		{
			yield return new WaitForSeconds(delay);
			textComponent.SetText(string.Empty);
			clearInnerMessageCoroutine = null;
		}

		public UIManager()
		{
			fadeInDuration = 1f;
			fadeOutDuration = 1f;
			fadeInAndOutDelay = 1f;
			originalColor = Color.black;
			fadedColor = Color.black;
		}

		private static UIManager instance;

		[Header("Fade Settings")]
		[SerializeField]
		private float fadeInDuration;

		[SerializeField]
		private float fadeOutDuration;

		[SerializeField]
		private float fadeInAndOutDelay;

		[SerializeField]
		private Color originalColor;

		[SerializeField]
		private Color fadedColor;

		[Header("UI Elements")]
		[SerializeField]
		private TMP_Text messageText;

		[SerializeField]
		private TMP_Text innerMessageText;

		[SerializeField]
		private TMP_Text tooltipText;

		[SerializeField]
		private Image transitionImage;

		[SerializeField]
		private GameObject dialogueClickPrompt;

		private Coroutine clearMessageCoroutine;

		private Coroutine clearInnerMessageCoroutine;
	}
}