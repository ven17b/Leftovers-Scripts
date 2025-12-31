using System;
using UnityEngine;

namespace Leftovers.UI
{
	public class PauseMenu : MonoBehaviour
	{
		private void OnEnable()
		{
            var pageMain = this.pageMain;
            if (pageMain != null)
            {
                pageMain.SetActive(true);
            }

            var pageSettings = this.pageSettings;
            if (pageSettings != null)
            {
                pageSettings.SetActive(false);
            }
        }

		public PauseMenu()
		{
		}

		[SerializeField]
		private GameObject pageMain;

		[SerializeField]
		private GameObject pageSettings;
	}
}
