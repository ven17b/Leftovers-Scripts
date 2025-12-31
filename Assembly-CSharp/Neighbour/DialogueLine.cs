using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Neighbour
{
	[Serializable]
	public struct DialogueLine
	{
		public string message;

		public string animationBody;

		public string animationFace;

		public float delay;

		public float durationMessage;

		public float durationLine;

		public AudioClip sfx;

		public UnityEvent onLineEnd;
	}
}
