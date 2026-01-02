using System;
using UnityEngine;
using UnityEngine.Events;
using Leftovers.UI;

namespace Leftovers.General
{
	public class Hoverable : MonoBehaviour
	{
		[SerializeField]
		private string tooltip;

		[SerializeField]
		private UnityEvent onStartHover;

		[SerializeField]
		private UnityEvent onStopHover;

		public Hoverable()
		{
			tooltip = string.Empty;
			onStartHover = new UnityEvent();
			onStopHover = new UnityEvent();
		}

		public void StartHover()
		{
			UIManager.Instance.SetTooltip(tooltip);
			if (onStartHover != null)
				onStartHover.Invoke();
		}

		public void StopHover()
		{
			UIManager.Instance.SetTooltip(string.Empty);
			if (onStopHover != null)
				onStopHover.Invoke();
		}
	}
}