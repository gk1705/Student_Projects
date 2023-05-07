//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	[TypeInfoBox("Stabilizes the car when not upright.\nAlso applies additional gravity so car doesn't float too long while in air.")]
	public class CarStabilizer : MonoBehaviour
	{
		
		[TitleGroup("Stabilization")]
		[SerializeField] private float stability = 0.3f;
		[SerializeField] private float standardSpeed = 50f;
		[Tooltip("Speed if the car transform.up.y is under yUpThreshold")]
		[SerializeField] private float fastSpeed = 250f;
		[Tooltip("If transform.up.y is smaller than this then the car will stabilize")]
		[Range(-1, 1)]
		[SerializeField] private float startThreshold = 0.9f;
		[Tooltip("If transform.up.y is smaller than this then fastSpeed will be used")]
		[Range(-1, 1)]
		[SerializeField] private float fastThreshold = 0.2f;
		[TitleGroup("Gravity")]
		[SerializeField] private float gravityStrength = 100f;
		private CarController car;
		private float speed;

		private void Start()
		{
			car = GetComponent<CarController>();
		}

		void FixedUpdate()
		{
			if (car.transform.up.y > startThreshold) return; //don't stabilize when grounded or not sideways enough

			
			speed = car.transform.up.y < fastThreshold ? fastSpeed : standardSpeed;

			//tries to keep the upvector pointing up
			Vector3 predictedUp = Quaternion.AngleAxis
				(car.RigidBody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed, car.RigidBody.angularVelocity) * transform.up;
			Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
			car.RigidBody.AddTorque(torqueVector * speed * speed);

			car.RigidBody.AddForce(Vector3.down*gravityStrength, ForceMode.Acceleration);
			//Debug.Log($"stabilizing with {speed} speed and torque {torqueVector*speed*speed} {car.transform.up}");
		}
	}
}


