using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Leftovers.UI
{
	public class ButtonEffect : MonoBehaviour
	{
		private void Awake()
		{
            textComponent = GetComponentInChildren<TMPro.TMP_Text>();
            if (textComponent != null)
                originalColor = textComponent.color;
            originalScale = transform.localScale.x;
            timer = 0f;
            var gameObjectRef = gameObject;
            if (gameObjectRef == null) return;
            eventTrigger = gameObjectRef.AddComponent<UnityEngine.EventSystems.EventTrigger>();

            var entry0 = new UnityEngine.EventSystems.EventTrigger.Entry();
            entry0.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
            entry0.callback.AddListener((data) => Awake_b__8_0());
            eventTrigger.triggers.Add(entry0);

            var entry1 = new UnityEngine.EventSystems.EventTrigger.Entry();
            entry1.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
            entry1.callback.AddListener((data) => Awake_b__8_1());
            eventTrigger.triggers.Add(entry1);
        }

		private void OnEnable()
		{
            if (textComponent != null)
            {
                textComponent.color = originalColor;
                Transform t = this.transform;
                Vector3 one = Vector3.one;
                t.localScale = one * originalScale;
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

		private void OnPointerOverDelegate(PointerEventData eventData)
		{
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            coroutine = StartCoroutine(PointerOverCoroutine());
        }

		private void OnPointerOffDelegate(PointerEventData eventData)
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
            yield return FinishAnimation();
        }

		private IEnumerator PointerOffCoroutine()
		{
            var coroutineInstance = new PointerOffCoroutine
            {
                __4__this = this,
                __1__state = 0
            };
            return coroutineInstance;
        }

		public ButtonEffect()
		{
            this.hoveredScale = 1.0f;
            this.effectDuration = 1.0f;
            this.hoveredColor = UnityEngine.Color.red;
            this.originalScale = 1.0f;
            this.originalColor = UnityEngine.Experimental.GlobalIllumination.LightmapperUtils.ExtractColorTemperature();
        }

		private void <Awake>b__8_0(BaseEventData data)
		{
            if (data != null && !(data is UnityEngine.EventSystems.PointerEventData))
            {
                data = data as UnityEngine.EventSystems.PointerEventData;
            }

            if (this.coroutine != null)
            {
                this.StopCoroutine(this.coroutine);
                this.coroutine = null;
            }

            this.coroutine = this.StartCoroutine(this.PointerOverCoroutine());
        }

		private void <Awake>b__8_1(BaseEventData data)
		{
            if (data != null && !(data is UnityEngine.EventSystems.PointerEventData))
            {
                data = data as UnityEngine.EventSystems.PointerEventData;
            }

            if (this.coroutine != null)
            {
                this.StopCoroutine(this.coroutine);
                this.coroutine = null;
            }

            this.coroutine = this.StartCoroutine(this.PointerOffCoroutine());
        }

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

		private sealed class <PointerOverCoroutine>d__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <PointerOverCoroutine>d__13(int <>1__state)
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

                    _originalScaleVec3 = Vector3.one * _4__this.originalScale;
                    _hoveredScaleVec3 = Vector3.one * _4__this.hoveredScale;
                }
                else if (__1__state == 1)
                {
                    __1__state = -1;
                    if (_4__this == null) return false;
                }
                else
                {
                    return false;
                }

                if (_4__this.timer > 0f)
                {
                    var t = Mathf.Clamp01(_4__this.timer / _4__this.effectDuration);
                    var newScale = Vector3.Lerp(_originalScaleVec3, _hoveredScaleVec3, t);
                    var transform = _4__this.transform;
                    if (transform != null)
                    {
                        transform.localScale = newScale;

                        var text = _4__this.textComponent;
                        if (text != null)
                        {
                            text.color = Color.Lerp(_4__this.originalColor, _4__this.hoveredColor, t);
                        }

                        _4__this.timer -= Time.unscaledDeltaTime;
                        __2__current = null;
                        __1__state = 1;
                        return true;
                    }
                }

                _4__this.timer = 0f;

                var finalTransform = _4__this.transform;
                if (finalTransform != null)
                {
                    finalTransform.localScale = _originalScaleVec3;
                    var textComp = _4__this.textComponent;
                    if (textComp != null)
                    {
                        textComp.color = _4__this.originalColor;
                    }
                }

                _4__this.coroutine = null;
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

			public ButtonEffect <>4__this;

			private Vector3 <originalScaleVec3>5__2;

			private Vector3 <hoveredScaleVec3>5__3;
		}

		private sealed class <PointerOffCoroutine>d__14 : IEnumerator<object>, IEnumerator, IDisposable
		{
			public <PointerOffCoroutine>d__14(int <>1__state)
			{
			}

			private void Dispose()
			{
			}

			private bool MoveNext()
			{
                switch (_state)
                {
                    case 0:
                        _state = -1;
                        _originalScaleVec3 = Vector3.one * _uiButtonEffect.originalScale;
                        _hoveredScaleVec3 = Vector3.one * _uiButtonEffect.hoveredScale;
                        break;
                    case 1:
                        _state = -1;
                        break;
                    default:
                        return false;
                }

                if (_uiButtonEffect.effectDuration > _uiButtonEffect.timer)
                {
                    var transform = _uiButtonEffect.transform;
                    float t = Mathf.Clamp01(_uiButtonEffect.timer / _uiButtonEffect.effectDuration);
                    if (transform != null)
                    {
                        transform.localScale = Vector3.Lerp(_originalScaleVec3, _hoveredScaleVec3, t);
                        if (_uiButtonEffect.textComponent != null)
                        {
                            _uiButtonEffect.textComponent.color =
                                Color.Lerp(_uiButtonEffect.originalColor, _uiButtonEffect.hoveredColor, t);
                        }
                    }
                    _uiButtonEffect.timer += Time.unscaledDeltaTime;
                    _current = null;
                    _state = 1;
                    return true;
                }

                _uiButtonEffect.timer = _uiButtonEffect.effectDuration;
                var finalTransform = _uiButtonEffect.transform;
                if (finalTransform != null)
                {
                    finalTransform.localScale = _hoveredScaleVec3;
                    if (_uiButtonEffect.textComponent != null)
                    {
                        _uiButtonEffect.textComponent.color = _uiButtonEffect.hoveredColor;
                    }
                }

                _uiButtonEffect.coroutine = null;
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

			public ButtonEffect <>4__this;

			private Vector3 <originalScaleVec3>5__2;

			private Vector3 <hoveredScaleVec3>5__3;
		}
	}
}
