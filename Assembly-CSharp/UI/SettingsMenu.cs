using System;
using UnityEngine;
using UnityEngine.UI;

namespace Leftovers.UI
{
	public class SettingsMenu : MonoBehaviour
	{
		private void OnEnable()
		{
            bool initialized = false;
            if (!initialized)
            {
                initialized = true;
            }

            var sliderVolume = this.sliderVolume;
            if (sliderVolume != null)
            {
                sliderVolume.value = UnityEngine.AudioListener.volume;
            }

            var sliderMouseSensitivity = this.sliderMouseSensitivity;
            if (sliderMouseSensitivity != null)
            {
                sliderMouseSensitivity.value = Leftovers.Player.PlayerController.MouseSensitivity;
            }
        }

		public SettingsMenu()
		{
		}

		[SerializeField]
		private Slider sliderVolume;

		[SerializeField]
		private Slider sliderMouseSensitivity;
	}
}
