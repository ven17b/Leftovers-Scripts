using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Utilities
{
	public class TriggerBox : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent onTriggerEnter;

		[SerializeField]
		private UnityEvent onTriggerExit;

		[SerializeField]
		private UnityEvent ifInContact;

		private int inContact;

		public TriggerBox()
		{
			onTriggerEnter = new UnityEvent();
			onTriggerExit = new UnityEvent();
			ifInContact = new UnityEvent();
		}

		private void OnTriggerEnter(Collider other)
		{
			if (onTriggerEnter != null)
				onTriggerEnter.Invoke();
			inContact++;
		}

		private void OnTriggerExit(Collider other)
		{
			if (onTriggerExit != null)
				onTriggerExit.Invoke();
			inContact--;
		}

		public void TriggerIfInContact()
		{
			if (inContact > 0)
			{
				if (ifInContact != null)
					ifInContact.Invoke();
			}
		}
	}
}
