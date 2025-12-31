using System;
using Il2CppDummyDll;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class NeighbourLineRenderers : MonoBehaviour
	{
		private void Start()
		{
            UnityEngine_LineRenderer_o* upperToFaceLine = this->fields.upperToFaceLine;

            if (!upperToFaceLine)
                sub_180157B40(0);

            UnityEngine_LineRenderer__set_positionCount(upperToFaceLine, 2, 0);
        }

		private void Update()
		{
            UnityEngine_Transform_o* upperTop = this->fields.upperTop;
            UnityEngine_LineRenderer_o* upperToFaceLine = this->fields.upperToFaceLine;
            UnityEngine_Vector3_o positionVec;
            UnityEngine_Vector3_o tempVec[2];

            if (!upperTop || !upperToFaceLine)
                sub_180157B40(this);

            UnityEngine_Transform__get_position(tempVec, upperTop, 0);
            positionVec = *tempVec;
            UnityEngine_LineRenderer__SetPosition(upperToFaceLine, 0, &positionVec, 0);

            UnityEngine_Transform_o* faceBottom = this->fields.faceBottom;
            UnityEngine_LineRenderer_o* line = this->fields.upperToFaceLine;

            if (!faceBottom || !line)
                sub_180157B40(this);

            UnityEngine_Transform__get_position(tempVec, faceBottom, 0);
            positionVec = *tempVec;
            UnityEngine_LineRenderer__SetPosition(line, 1, &positionVec, 0);
        }

		public NeighbourLineRenderers()
		{
            UnityEngine_Terrain___ctor((UnityEngine_Terrain_o*)this, 0);
        }

		[SerializeField]
		private LineRenderer upperToFaceLine;

		[SerializeField]
		private Transform upperTop;

		[SerializeField]
		private Transform faceBottom;
	}
}
