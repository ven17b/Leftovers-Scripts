using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Leftovers.UI
{
	public class ButtonEffect : MonoBehaviour
	{
		[SerializeField]
		private float hoveredScale;

		[SerializeField]
		private Color hoveredColor;

		[SerializeField]
		private float effectDuration;

		private TMP_Text textComponent;
		private Color originalColor;
		private float originalScale;
		private float timer;
		private Coroutine coroutine;
		private EventTrigger eventTrigger;

		public ButtonEffect()
		{
			hoveredScale = 1.0f;
			effectDuration = 1.0f;
			hoveredColor = Color.red;
			originalScale = 1.0f;
			originalColor = Color.white;
		}

		private void Awake()
		{
			textComponent = GetComponentInChildren<TMP_Text>();
			if (textComponent != null)
				originalColor = textComponent.color;

			originalScale = transform.localScale.x;
			timer = 0f;

			eventTrigger = gameObject.AddComponent<EventTrigger>();

			EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
			pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
			pointerEnterEntry.callback.AddListener(OnPointerEnter);
			eventTrigger.triggers.Add(pointerEnterEntry);

			EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
			pointerExitEntry.eventID = EventTriggerType.PointerExit;
			pointerExitEntry.callback.AddListener(OnPointerExit);
			eventTrigger.triggers.Add(pointerExitEntry);
		}

		private void OnEnable()
		{
			if (textComponent != null)
			{
				textComponent.color = originalColor;
				transform.localScale = Vector3.one * originalScale;
			}
		}

		private void OnDisable()
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		private void OnPointerEnter(BaseEventData data)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
			coroutine = StartCoroutine(PointerOverCoroutine());
		}

		private void OnPointerExit(BaseEventData data)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
			coroutine = StartCoroutine(PointerOffCoroutine());
		}

		private IEnumerator PointerOverCoroutine()
		{
			Vector3 originalScaleVec3 = Vector3.one * originalScale;
			Vector3 hoveredScaleVec3 = Vector3.one * hoveredScale;

			while (effectDuration > timer)
			{
				float t = Mathf.Clamp01(timer / effectDuration);
				transform.localScale = Vector3.Lerp(originalScaleVec3, hoveredScaleVec3, t);

				if (textComponent != null)
					textComponent.color = Color.Lerp(originalColor, hoveredColor, t);

				timer += Time.unscaledDeltaTime;
				yield return null;
			}

			timer = effectDuration;
			transform.localScale = hoveredScaleVec3;

			if (textComponent != null)
				textComponent.color = hoveredColor;

			coroutine = null;
		}

		private IEnumerator PointerOffCoroutine()
		{
			Vector3 originalScaleVec3 = Vector3.one * originalScale;
			Vector3 hoveredScaleVec3 = Vector3.one * hoveredScale;

			while (timer > 0f)
			{
				float t = Mathf.Clamp01(timer / effectDuration);
				transform.localScale = Vector3.Lerp(originalScaleVec3, hoveredScaleVec3, t);

				if (textComponent != null)
					textComponent.color = Color.Lerp(originalColor, hoveredColor, t);

				timer -= Time.unscaledDeltaTime;
				yield return null;
			}

			timer = 0f;
			transform.localScale = originalScaleVec3;

			if (textComponent != null)
				textComponent.color = originalColor;

			coroutine = null;
		}
	}
}