using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Utilities
{
	public class TriggerBox : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
            if (this.onTriggerEnter != null)
                this.onTriggerEnter.Invoke();
            this.inContact++;
        }

		private void OnTriggerExit(Collider other)
		{
            if (this.onTriggerExit != null)
                this.onTriggerExit.Invoke();
            this.inContact--;
        }

		public void TriggerIfInContact()
		{
            if (this.inContact > 0)
            {
                if (this.ifInContact != null)
                    this.ifInContact.Invoke();
            }
        }

		public TriggerBox()
		{
            onTriggerEnter = new UnityEngine.Events.UnityEvent();
            onTriggerExit = new UnityEngine.Events.UnityEvent();
            ifInContact = new UnityEngine.Events.UnityEvent();
        }

		[SerializeField]
		private UnityEvent onTriggerEnter;

		[SerializeField]
		private UnityEvent onTriggerExit;

		[SerializeField]
		private UnityEvent ifInContact;

		private int inContact;
	}
}
