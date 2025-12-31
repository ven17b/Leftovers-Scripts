using System;
using UnityEngine;

namespace Leftovers.Utilities
{
	public class Teleportation : MonoBehaviour
	{
		public void Teleport()
		{
            var displayClass = new Leftovers_Utilities_Teleportation.__c__DisplayClass5_0();
            displayClass.@this = this;

            GameObject go = GameObject.FindGameObjectWithTag("Player");
            displayClass.go = go;

            var player = Leftovers_Player_PlayerController.instance;
            if (player == null)
                throw new NullReferenceException();

            player.handleKeyboardInput = false;
            player.handleMouseInput = false;
            Cursor.lockState = CursorLockMode.None;

            switch (type)
            {
                case 0:
                    {
                        var ui = Leftovers_UI_UIManager.Instance;
                        if (ui == null)
                            throw new NullReferenceException();

                        UnityAction a = displayClass.Teleport_b__0;
                        UnityAction b = displayClass.Teleport_b__1;
                        ui.FadeInAndOut(a, b);
                        return;
                    }

                case 1:
                    {
                        var cc = go.GetComponent<CharacterController>();
                        if (cc == null)
                            throw new NullReferenceException();

                        cc.enabled = false;

                        var t = go.transform;
                        var tp = teleportationPoint;
                        t.position = tp.position;
                        t.eulerAngles = tp.eulerAngles;

                        cc.enabled = true;

                        var pc = Leftovers_Player_PlayerController.instance;
                        pc.ResetRotationValues();

                        if (audioSource && startTeleportSound)
                            audioSource.PlayOneShot(startTeleportSound);

                        var ui = Leftovers_UI_UIManager.Instance;
                        if (ui == null)
                            throw new NullReferenceException();

                        UnityAction c = displayClass.Teleport_b__2;
                        ui.StartCoroutine(ui.FadingOut(c));
                        return;
                    }

                case 2:
                    {
                        var cc = go.GetComponent<CharacterController>();
                        if (cc == null)
                            throw new NullReferenceException();

                        cc.enabled = false;

                        var t = go.transform;
                        var tp = teleportationPoint;
                        t.position = tp.position;
                        t.eulerAngles = tp.eulerAngles;

                        cc.enabled = true;

                        var pc = Leftovers_Player_PlayerController.instance;
                        pc.ResetRotationValues();
                        return;
                    }
            }
        }

		public Teleportation()
		{
		}

		[SerializeField]
		private TransitionType type;

		[SerializeField]
		private Transform teleportationPoint;

		[SerializeField]
		private AudioClip startTeleportSound;

		[SerializeField]
		private AudioClip finishTeleportSound;

		[SerializeField]
		private AudioSource audioSource;

		private sealed class <>c__DisplayClass5_0
		{
			public <>c__DisplayClass5_0()
			{
			}

			internal void <Teleport>b__0()
			{
                GameObject player = this.player;
                if (player == null)
                    throw new NullReferenceException();

                var cc = player.GetComponent<CharacterController>();
                if (cc == null)
                    throw new NullReferenceException();

                cc.enabled = false;

                var t = player.transform;
                var tp = this.__4__this.teleportationPoint;
                if (tp == null)
                    throw new NullReferenceException();

                t.position = tp.position;
                t.eulerAngles = tp.eulerAngles;

                cc.enabled = true;

                var pc = Leftovers_Player_PlayerController.instance;
                if (pc == null)
                    throw new NullReferenceException();

                pc.ResetRotationValues();

                var audioSource = this.__4__this.audioSource;
                if (audioSource)
                {
                    var clip = this.__4__this.startTeleportSound;
                    if (clip)
                    {
                        audioSource.PlayOneShot(clip);
                    }
                }
            }

			internal void <Teleport>b__1()
			{
                var pc = Leftovers_Player_PlayerController.instance;
                if (pc == null)
                    throw new NullReferenceException();

                pc.handleKeyboardInput = true;
                pc.handleMouseInput = true;
                Cursor.lockState = CursorLockMode.Locked;

                var t = this.__4__this;
                if (t == null)
                    throw new NullReferenceException();

                var audioSource = t.audioSource;
                if (audioSource)
                {
                    var clip = t.finishTeleportSound;
                    if (clip)
                    {
                        audioSource.PlayOneShot(clip);
                    }
                }
            }

			internal void <Teleport>b__2()
			{
                var pc = Leftovers_Player_PlayerController.instance;
                if (pc == null)
                    throw new NullReferenceException();

                pc.handleKeyboardInput = true;
                pc.handleMouseInput = true;
                Cursor.lockState = CursorLockMode.Locked;

                var t = this.__4__this;
                if (t == null)
                    throw new NullReferenceException();

                var audioSource = t.audioSource;
                if (audioSource)
                {
                    var clip = t.finishTeleportSound;
                    if (clip)
                    {
                        audioSource.PlayOneShot(clip);
                    }
                }
            }

			public GameObject player;

			public Teleportation <>4__this;
		}
	}
}
