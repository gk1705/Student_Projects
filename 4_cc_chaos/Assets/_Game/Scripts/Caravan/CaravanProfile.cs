//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "CaravanProfile", menuName = "Custom/Caravan/CaravanProfile", order = 1)]
	public class CaravanProfile : ScriptableObject
	{
		[Header("Caravan Control")]
		public float LaunchSpeed = 60f;
		public float ReattachDelay = 0.3f;
		public float MinLaunchForce = 1500f;
		[Tooltip("Under this speed the minLauncheForce won't be applied")]
		public float MinForceThreshold = 5f;
		[Space(10)]
		public bool ManualReattach = false;
		[ShowIf("ManualReattach")]
		public float ReattachDistance = 10f;

		[Header("Caravan Damage")]
		public float BaseDamage = 0.75f;
		public float DetachMultiplier = 2f;
		[Tooltip("The minimum velocity we have to have to do damage")]
		public float MinVelocity = 3f;
		[Tooltip("The time after a collision when no other collision with the same object will register")]
		public float CollisionCooldown = 0.2f;

		[Header("Vibration")]
		public bool VibrateOnDealDamage = true;
		public bool VibrateOnReceiveDamage = true;
	}
}


