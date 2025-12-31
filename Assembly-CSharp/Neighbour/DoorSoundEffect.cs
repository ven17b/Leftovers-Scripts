using System;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class DoorSoundEffect : MonoBehaviour
	{
		public void PlayDoorOpen()
		{
            if (!this->fields.audioSource)
                sub_180157B40(this);

            UnityEngine_AudioSource__PlayOneShot(
                this->fields.audioSource,
                this->fields.sfxDoorOpen,
                0
            );
        }

		public void PlayDoorClose()
		{
            if (!this->fields.audioSource)
                sub_180157B40(this);

            UnityEngine_AudioSource__PlayOneShot(this->fields.audioSource, this->fields.sfxDoorClose, 0);
        }

		public void PlayGrunt()
		{
            if (!this->fields.audioSource)
                sub_180157B40(this);

            UnityEngine_AudioSource__PlayOneShot(
                this->fields.audioSource,
                this->fields.sfxGrunt,
                0
            );
        }

		public DoorSoundEffect()
		{
		}

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip sfxDoorOpen;

		[SerializeField]
		private AudioClip sfxDoorClose;

		[SerializeField]
		private AudioClip sfxGrunt;
	}
}
