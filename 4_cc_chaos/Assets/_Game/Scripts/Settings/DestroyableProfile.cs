//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Settings for the explosion force and the child rigidbodies
	/// </summary>
	[CreateAssetMenu(fileName = "DestroyableProfile", menuName = "Custom/DestroyableProfile", order = 1)]
	public class DestroyableProfile : ScriptableObject
	{
		[Header("Explosion")]
		public float DirectionStrength = 800;
		public float MaxRandomAngle = 25f;
		[Header("Rigidbody")]
		public float Mass = 1f;
		public float Drag = 0.25f;
		public float AngularDrag = 0.25f;
		public bool Gravity = true;
		public RigidbodyInterpolation Interpolation = RigidbodyInterpolation.Interpolate;
		public CollisionDetectionMode CollisionDetection = CollisionDetectionMode.Discrete;
		public RigidbodyConstraints Constraints = RigidbodyConstraints.None;


		/// <summary>
		/// Applies the settings of the profile to the passed rigidbody
		/// </summary>
		/// <param name="rigidbody"></param>
		public void ApplyRigidbodySettings(Rigidbody rigidbody)
		{
			rigidbody.mass = Mass;
			rigidbody.drag = Drag;
			rigidbody.angularDrag = AngularDrag;
			rigidbody.useGravity = Gravity;
			rigidbody.interpolation = Interpolation;
			rigidbody.collisionDetectionMode = CollisionDetection;
			rigidbody.constraints = Constraints;
		}
	}
}


