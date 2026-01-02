using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.General
{
	public class Interactable : Hoverable
	{
		[SerializeField]
		private UnityEvent onStartInteract;

		[SerializeField]
		private UnityEvent onStopInteract;

		[SerializeField]
		private InteractionAnimationType animationType;

		[SerializeField]
		private bool lockInteraction;

		public InteractionAnimationType AnimationType
		{
			get
			{
				return animationType;
			}
		}

		public Interactable()
		{
			onStartInteract = new UnityEvent();
			onStopInteract = new UnityEvent();
		}

		public void StartInteract()
		{
			if (lockInteraction)
			{
				Interactor instance = Interactor.Instance;
				if (instance != null)
				{
					instance.lockedInteraction = true;
					if (instance.currentDetectedObject != null)
					{
						instance.currentDetectedObject.StopHover();
						instance.currentDetectedObject = null;
					}
				}
			}
			if (onStartInteract != null)
				onStartInteract.Invoke();
		}

		public void StopInteract()
		{
			if (lockInteraction)
			{
				Interactor instance = Interactor.Instance;
				if (instance != null)
				{
					instance.lockedInteraction = false;
				}
			}
			if (onStopInteract != null)
				onStopInteract.Invoke();
		}
	}
}
