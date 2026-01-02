using System;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class DoorSoundEffect : MonoBehaviour
	{
		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip sfxDoorOpen;

		[SerializeField]
		private AudioClip sfxDoorClose;

		[SerializeField]
		private AudioClip sfxGrunt;

		public void PlayDoorOpen()
		{
			if (audioSource != null && sfxDoorOpen != null)
				audioSource.PlayOneShot(sfxDoorOpen);
		}

		public void PlayDoorClose()
		{
			if (audioSource != null && sfxDoorClose != null)
				audioSource.PlayOneShot(sfxDoorClose);
		}

		public void PlayGrunt()
		{
			if (audioSource != null && sfxGrunt != null)
				audioSource.PlayOneShot(sfxGrunt);
		}
	}
}
