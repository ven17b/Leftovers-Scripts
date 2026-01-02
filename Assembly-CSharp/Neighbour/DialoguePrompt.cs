using System;

namespace Leftovers.Neighbour
{
	[Serializable]
	public class DialoguePrompt
	{
		public string name;

		public bool hasNod;
		public int nodSegmentIndex;

		public bool hasShake;
		public int shakeSegmentIndex;

		public bool hasShowFood;
		public int showFoodSegmentIndex;
	}
}
