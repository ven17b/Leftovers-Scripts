using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.General
{
	public class Hoverable : MonoBehaviour
	{
		public void StartHover()
		{
            var uiManager = Leftovers_UI_UIManager.Instance;
            if (uiManager == null || uiManager.Canvas == null)
                return;

            uiManager.Canvas.SetText(this.tooltip);

            onStartHover?.Invoke();
        }

		public void StopHover()
		{
            var uiManager = Leftovers_UI_UIManager.Instance;
            if (uiManager == null || uiManager.Canvas == null)
                return;

            uiManager.Canvas.SetText(string.Empty);

            onStopHover?.Invoke();
        }

		public Hoverable()
		{
            tooltip = string.Empty;
            onStartHover = new UnityEngine.Events.UnityEvent();
            onStopHover = new UnityEngine.Events.UnityEvent();
        }

		[SerializeField]
		private string tooltip;

		[SerializeField]
		private UnityEvent onStartHover;

		[SerializeField]
		private UnityEvent onStopHover;
	}
}
