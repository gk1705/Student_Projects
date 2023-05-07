//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class WheelSteeringRotation : MonoBehaviour
	{
		[SerializeField] private float maxRotationAngle = 0f;
		[SerializeField] private string inputAxis = "Horizontal0";
		private Transform wheelTransform;

		void Start()
		{
			wheelTransform = transform;
		}
		void FixedUpdate()
		{
			var localEulerAngles = wheelTransform.localEulerAngles;
			float turning = Input.GetAxis(inputAxis);
			float turningAngle = turning * maxRotationAngle;
			localEulerAngles.y = turningAngle > 0 ? 0 + turningAngle : 360 + turningAngle;
			wheelTransform.localEulerAngles = localEulerAngles;
		}
	}
}


