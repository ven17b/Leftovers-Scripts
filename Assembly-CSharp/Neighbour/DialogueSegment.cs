using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Leftovers.Neighbour
{
	[Serializable]
	public struct DialogueSegment
	{
		public string name;

		public List<DialogueLine> lines;

		public float delay;

		public string animationBody;

		public string animationFace;

		public AudioClip sfx;

		public DialogueSegmentType type;

		public int indexType;

		public UnityEvent onSegmentStart;

		public UnityEvent onSegmentEnd;
	}
}
