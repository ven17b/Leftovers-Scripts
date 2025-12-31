using System;

namespace Leftovers.Neighbour
{
	[Serializable]
	public class DialoguePrompt
	{
		public DialoguePrompt()
		{
            base.Dispose();
        }

		public string name;

		public DialoguePromptOption optionNod;

		public DialoguePromptOption optionShake;

		public DialoguePromptOption optionShowFood;
	}
}
