using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.General
{
	public class Interactable : Hoverable
	{
		public InteractionAnimationType AnimationType
		{
			get
			{
				return InteractionAnimationType.None;
			}
		}

		public void StartInteract()
		{
            if (lockInteraction)
            {
                var interactor = Leftovers_General_Interactor.instance;
                if (interactor != null)
                {
                    interactor.lockedInteraction = true;
                    if (interactor.currentDetectedObject != null)
                    {
                        interactor.currentDetectedObject.StopHover();
                        interactor.currentDetectedObject = null;
                    }
                }
            }

            onStartInteract?.Invoke();
        }

		public void StopInteract()
		{
            if (lockInteraction)
            {
                var interactor = Leftovers_General_Interactor.instance;
                if (interactor != null)
                {
                    interactor.lockedInteraction = false;
                }
            }

            onStopInteract?.Invoke();
        }

		public Interactable()
		{
            onStartInteract = new UnityEngine.Events.UnityEvent();
            onStopInteract = new UnityEngine.Events.UnityEvent();

            tooltip = string.Empty;

            onStartHover = new UnityEngine.Events.UnityEvent();
            onStopHover = new UnityEngine.Events.UnityEvent();
        }

		[SerializeField]
		private UnityEvent onStartInteract;

		[SerializeField]
		private UnityEvent onStopInteract;

		[SerializeField]
		private InteractionAnimationType animationType;

		[SerializeField]
		private bool lockInteraction;
	}
}
