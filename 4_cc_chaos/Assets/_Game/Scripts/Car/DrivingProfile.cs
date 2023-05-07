using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "DrivingProfile", menuName = "Custom/Car/DrivingProfile", order = 1)]
	public class DrivingProfile : ScriptableObject
	{
		[TitleGroup("Acceleration")]
		public float VelocityLimit = 30;
		public float ForwardAcceleration = 1;
		public float BackwardAcceleration = 0.75f;
		[TitleGroup("Steering")]
		public float AngularVelocityLimit = 2;
		public float TurnSpeed = 0.2f;
		[Tooltip("When no steering occurs, the angularvelocity is divided by this factor each fixed update, so car doesn't steer too much if no steering is applied")]
		public float AngularReduction = 1.15f;
		[Tooltip("If the speed is under this threshold the turning speed will be lowered proportionally to the velocity")]
		public float TurningThresholdVelocity = 20;
		public bool InvertBackwardsSteering = true;
		[Tooltip("x axis is angularVelocity, y is multiplier applied to steering input")]
		public AnimationCurve SteeringCurve;

		[TitleGroup("Traction")]
		public float TractionForce = 20;
		[Tooltip("The maximum opposite of the sideways velocity that is used to apply traction(see sideways velocity in the debug gui), for more see documentation")]
		[HideInInspector]
		public float MaxLocalCounterVelocity = 10; //todo remove, rework this
		public float GroundedDrag = 1;
		[Tooltip("x axis is ratio of forwardspeed to sidewaysspeed (0 forward, 1 fully sideways), y is multiplier applied to tractionforce")]
		public AnimationCurve TractionCurve;
		[Tooltip("x axis is Y of transform.up (1 upright, -1 upside down), y is multiplier applied to tractionforce \n Use this so that a car can't drive upsidedown and instead falls off")]
		public AnimationCurve GroundTractionCurve;

		[TitleGroup("Hovering")]
		public float HoverForce = 3;
		public float HoverHeight = 0.6f; //how much above the ground the vehicle should hover
		[Tooltip("Stiffness of the spring | higher = more stiffness")]
		public float SpringCoefficient = 4;
		[Tooltip("Damping factor, reduces the up-down bouncing of the spring | higher values reduce bouncing velocity faster (also making the car somewhat more stiff)")]
		public float DampingCoefficient = 0.8f;

		[TitleGroup("In Air")]
		[Tooltip("Accelerationspeed in air = acceleration / reduction")]
		public float AirAccelerationReduction = 10000;
		[Tooltip("Turningspeed in air = turnspeed / reduction")]
		public float AirTurningReduction = 10;
		public float AirDrag = 0;
	}
}


