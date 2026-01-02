using System;
using System.Collections;
using UnityEngine;

namespace Leftovers.Neighbour
{
	public class NeighbourBendDown : MonoBehaviour
	{
		private void Start()
		{
			originalPosition = face.localPosition;
			facePosition = face.localPosition;
			neighbourHeadPosition = GameObject.Find("Head").transform;
		}

		private void Update()
		{
			UpdateUpperBody();

			float t = Mathf.Clamp01(offsetAmount);
			Vector3 interpolatedPosition = new Vector3(
				facePosition.x + (offsetPosition.x - facePosition.x) * t,
				facePosition.y + (offsetPosition.y - facePosition.y) * t,
				facePosition.z + (offsetPosition.z - facePosition.z) * t
			);
			face.localPosition = interpolatedPosition;
		}

		public void StartBendDown()
		{
			if (toBendDown)
			{
				StartCoroutine(BendDown());
			}
		}

		public void StartStraigten()
		{
			if (toBendDown)
			{
				StartCoroutine(Straighten());
			}
		}

		private IEnumerator BendDown()
		{
			Transform faceParent = face.parent;
			Vector3 endPosition = faceParent.InverseTransformPoint(neighbourHeadPosition.position);

			Vector3 offsetWorldPos = neighbourHeadPosition.position + neighbourHeadPosition.forward * offsetDistance;
			offsetPosition = faceParent.InverseTransformPoint(offsetWorldPos);

			timer = 0f;

			while (timer < bendDownDuration)
			{
				float t = Mathf.Clamp01(timer / bendDownDuration);
				facePosition = new Vector3(
					originalPosition.x + (endPosition.x - originalPosition.x) * t,
					originalPosition.y + (endPosition.y - originalPosition.y) * t,
					originalPosition.z + (endPosition.z - originalPosition.z) * t
				);
				face.localPosition = facePosition;
				timer += Time.deltaTime;
				yield return null;
			}

			facePosition = endPosition;
			face.localPosition = endPosition;
		}

		private IEnumerator Straighten()
		{
			Vector3 startPosition = facePosition;
			timer = 0f;
			offsetAmount = 0f;

			while (timer < straightenDuration)
			{
				float t = Mathf.Clamp01(timer / straightenDuration);
				facePosition = new Vector3(
					startPosition.x + (originalPosition.x - startPosition.x) * t,
					startPosition.y + (originalPosition.y - startPosition.y) * t,
					startPosition.z + (originalPosition.z - startPosition.z) * t
				);
				face.localPosition = facePosition;
				timer += Time.deltaTime;
				yield return null;
			}

			facePosition = originalPosition;
			face.localPosition = originalPosition;
		}

		private void UpdateUpperBody()
		{
			Vector3 topPos = upperBodyTop.position;
			Vector3 bottomPos = upperBodyBottom.position;
			Vector3 direction = (topPos - bottomPos).normalized;
			float angle = Vector3.Angle(Vector3.up, direction);

			Vector3 currentAngles = upperBody.localEulerAngles;
			upperBody.localEulerAngles = new Vector3(angle, currentAngles.y, currentAngles.z);
		}

		public void MoveFace(float duration)
		{
			StartCoroutine(MovingFace(duration, 1f));
		}

		public void MoveFaceBack(float duration)
		{
			StartCoroutine(MovingFace(duration, 0f));
		}

		private IEnumerator MovingFace(float duration, float amount)
		{
			float originalFaceDistance = offsetAmount;
			float localTimer = 0f;

			while (localTimer < duration)
			{
				offsetAmount = Mathf.Lerp(originalFaceDistance, amount, localTimer / duration);
				localTimer += Time.deltaTime;
				yield return null;
			}

			offsetAmount = amount;
		}

		public NeighbourBendDown()
		{
			toBendDown = true;
			bendDownDuration = 1f;
			straightenDuration = 1f;
			offsetDistance = 1f;
			originalPosition = Vector3.zero;
			offsetPosition = Vector3.zero;
			facePosition = Vector3.zero;
		}

		[Header("Settings")]
		[SerializeField]
		private bool toBendDown;

		[SerializeField]
		private float bendDownDuration;

		[SerializeField]
		private float straightenDuration;

		[SerializeField]
		private float offsetDistance;

		[Header("Face")]
		[SerializeField]
		private Transform face;

		[SerializeField]
		private Transform offsetReference;

		[Header("Upper Body")]
		[SerializeField]
		private Transform upperBodyBottom;

		[SerializeField]
		private Transform upperBodyTop;

		[SerializeField]
		private Transform upperBody;

		private Vector3 originalPosition;

		private Vector3 offsetPosition;

		private Transform neighbourHeadPosition;

		private Vector3 facePosition;

		private float offsetAmount;

		private float timer;
	}
}
