using System;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class NeighbourAnimatorControllers : ScriptableObject
	{
		public NeighbourAnimatorControllers()
		{
            UnityEngine_ScriptableObject___ctor((UnityEngine_ScriptableObject_o*)this, 0);
        }

		public RuntimeAnimatorController controllerFullyOpened;

		public RuntimeAnimatorController controllerPartiallyOpened;

		public RuntimeAnimatorController controllerLegless;

		public RuntimeAnimatorController controllerNine;
	}
}
