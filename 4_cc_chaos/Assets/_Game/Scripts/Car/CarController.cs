//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Rewired;
using Rewired.Utils.Classes.Data;
using Sirenix.OdinInspector;

namespace CaravanCrashChaos
{
	[RequireComponent(typeof(Rigidbody))]
	[DisallowMultipleComponent]
	public class CarController : MonoBehaviour
	{
		#region SerializedFields
		[Header("Wheels")]
		[SerializeField] [Required] private GameObject[] hoverPoints = null;

		[Header("Transforms")]
		[SerializeField] [Required] private Transform centerOfMass = null;
		[SerializeField] [Required] private Transform airCenterOfMass = null;
		[SerializeField] [Required] private Transform thrustApplyTransform = null;
		[SerializeField] [Required] private Transform trailerAttachPoint = null;

		[Header("Other")]
		[SerializeField] private LayerMask wheelLayerMask = new LayerMask();
		#endregion

		#region private Variables
		private DrivingProfile profile = null;
		private new Transform transform;
		private readonly List<WheelValues> wheelValues = new List<WheelValues>(4);
		private bool grounded;
		private float currentMaxSpeed = 1;
		private int playerId = 0;
		#endregion

		#region Properties
		public Rigidbody RigidBody { get; private set; }
		public bool Grounded() => grounded;
		public Vector3 GetLocalVelocity() => transform.InverseTransformDirection(RigidBody.velocity);
		public float GetMaxVelocity() => profile.VelocityLimit;
		public List<WheelValues> GetWheelValues() => wheelValues;
		public Transform GetTrailerAttachTransform() => trailerAttachPoint;
		public CaravanController Caravan { get; private set; }
		public Rewired.Player RewiredPlayer { get; private set; }
		public bool IsAi { get; set; } = false;
		#endregion

		void Awake()
		{
			profile = GameModes.Instance.CurrentGameMode.DrivingProfile;
			CheckFields();
			SetValues();
		}

		private void SetValues()
		{
		
			RewiredPlayer = ReInput.players.GetPlayer(playerId);

			RigidBody = this.GetComponent<Rigidbody>();
			transform = this.GetComponent<Transform>();

			RigidBody.centerOfMass = centerOfMass.localPosition;
			RigidBody.maxAngularVelocity = profile.AngularVelocityLimit;

			for (int i = 0; i < hoverPoints.Length; i++)
			{
				wheelValues.Add(new WheelValues());
				wheelValues[i].WheelPosition = hoverPoints[i].transform.position;
			}
			Caravan = transform.parent.GetComponentInChildren<CaravanController>();
		}

		public void SetPlayerId(int id)
		{
			playerId = id;
			RewiredPlayer = ReInput.players.GetPlayer(playerId);
		}

		private void CheckFields()
		{
			Assert.IsNotNull(profile);

			for (int i = 0; i < hoverPoints.Length; i++)
			{
				if(hoverPoints[i] == null)
					Debug.LogError("a wheel is null");
			}
			if(!centerOfMass) Debug.LogError("no center of mass set on car");
			if(!airCenterOfMass) Debug.LogError("no air center of mass set on car");
			if(!thrustApplyTransform) Debug.LogError("no thrustApplyTransform set on car");
			if(!trailerAttachPoint) Debug.LogError("no trailerAttachPoint set on car");
		}

		/// <summary>
		/// Drives the car with a given input.
		/// Clamps any given input to between -1 and 1.
		/// </summary>
		/// <param name="thrust">Vertical input</param>
		/// <param name="steering">Horizontal input</param>
		public void Drive(float thrust, float steering)
		{
			//Debug.Log("Drive with: thrust, turn"+thrust + " "+turnValue);
			if (RigidBody.velocity.magnitude > currentMaxSpeed) //update current max speed
				currentMaxSpeed = RigidBody.velocity.magnitude;

			thrust = Mathf.Clamp(thrust, -1, 1);
			steering = Mathf.Clamp(steering, -1, 1);

			CalculateWheels();
			SetInAirValues();
			ApplyForces(thrust, steering);
			ApplyTraction();
		}

		private void FixedUpdate()
		{
			if(!IsAi)
				Drive(GetAccelerationInput(), GetSteeringInput());
		}

		/// <summary>
		/// Polls the acceleration axis for input
		/// </summary>
		/// <returns>the input value for the acceleration axis on a scale of -1 to 1</returns>
		public float GetAccelerationInput()
		{
			//main movement/thrust/acceleration
			string axis = "Acceleration";
			return RewiredPlayer.GetAxisRaw(axis); //forward/backward input
		}

		/// <summary>
		/// Polls the steering axis for input.
		/// Also applies inverted backwards steering if the setting is checked
		/// </summary>
		/// <returns>the input value for the steering axis on a scale of -1 to 1</returns>
		public float GetSteeringInput()
		{
			//turning

			string axis = "Steering";

			float multiplier = 1; 
			if (profile.InvertBackwardsSteering && GetAccelerationInput() < 0) //inverted backwards steering multiplier
				multiplier = -1;

			var steering = RewiredPlayer.GetAxisRaw(axis) * multiplier;

			return steering;

		}

		/// <summary>
		/// Inverts backwardssteering if enabled in the profile
		/// </summary>
		/// <returns>1 if no inverted backwardssteering, -1 if inverted backwardssteering</returns>
		public float BackwardsSteeringMultiplier(float accelerationInput)
		{
			if (profile.InvertBackwardsSteering && accelerationInput < 0 && GetLocalVelocity().z < 0)
				return -1f;
			else
				return 1f;
		}

		public bool AllWheelsGrounded()
		{
			for (int i = 0; i < wheelValues.Count; i++)
			{
				if (!wheelValues[i].Grounded) //car is not grounded if 1 wheel doesn't touch the ground
					return false;
			}
			return true;
		}

		/// <summary>
		/// Calculates the hover force for each wheel and if they are grounded
		/// </summary>
		private void CalculateWheels()
		{
			//hovering   
			for (int i = 0; i < hoverPoints.Length; i++)
			{
				wheelValues[i].Grounded = false;
				if (Physics.Raycast(hoverPoints[i].transform.position, -transform.up, out var hit, profile.HoverHeight, wheelLayerMask)) //if raycast hits something under the car within the distance
				{
					float hoverDelta = profile.HoverHeight - hit.distance;
					//Damping
					float newHoverDelta = hoverDelta - wheelValues[i].HoverDelta; //the old hoverdelta, only update hoverdelta after this was assigned
					newHoverDelta /= Time.fixedDeltaTime;
					float damping = newHoverDelta * profile.DampingCoefficient;
					//spring
					float spring = hoverDelta * profile.SpringCoefficient;
					float force = damping + spring;
					Vector3 forceToApply = Vector3.up * force * profile.HoverForce;
					RigidBody.AddForceAtPosition(forceToApply, hoverPoints[i].transform.position, ForceMode.Acceleration); //the closer to ground the smaller hit.distance is, therefore more strength to push it back

					wheelValues[i].WheelPosition = hoverPoints[i].transform.position;
					wheelValues[i].HoverDelta = hoverDelta;
					wheelValues[i].ImpactPoint = hit.point;
					wheelValues[i].ImpactNormal = hit.normal;
					wheelValues[i].UpForce = forceToApply;
					wheelValues[i].Grounded = true;
				}
			}
			grounded = IsGrounded(); //todo no steering when frontwheels in air
			//Debug.Log($"car grounded {grounded}");
		}

		/// <summary>
		/// Get the normal of the nearest ground under the vehicle
		/// </summary>
		/// <returns>Normal of the raycast hit ground, Vector3.down if no ground hit</returns>
		private Vector3 GetGroundNormal()
		{
			if (Physics.Raycast(transform.position, -transform.up, out var hit, wheelLayerMask))
			{
				return hit.normal;
			}
			return Vector3.down; //if nothing hit return upsidedown ground
		}

		/// <summary>
		/// Is the vehicle upside down?
		/// </summary>
		/// <returns>true if the y of transform.up is smaller than -0.75f</returns>
		private bool IsUpsideDown()
		{
			return transform.up.y < -0.75f;
		}

		/// <summary>
		/// Checks if vehicle is grounded.
		/// </summary>
		/// <returns>if at least 1 wheel touches the ground.</returns>
		private bool IsGrounded()
		{
			for (int i = 0; i < wheelValues.Count; i++)
			{
				if (wheelValues[i].Grounded) //car is grounded as long as 1 wheel touches the ground
					return true;
			}
			return false;
		}

		/// <summary>
		/// Adds up impactnormal of all wheels and averages them
		/// </summary>
		/// <returns>Vector3.up if no wheel is grounded, else the average ground/impact normal</returns>
		private Vector3 GetAverageImpactNormal()
		{
			if (!grounded) return Vector3.up; //if no wheel grounded, return standard ground normal
			var normal = Vector3.zero;
			int count = 0;

			for (int i = 0; i < wheelValues.Count; i++)
			{
				if (wheelValues[i].Grounded)
				{
					normal += wheelValues[i].ImpactNormal;
				}
				else
				{
					normal += Vector3.up; //if wheel does not touch ground, assume normal ground normal				
				}
				count++;
			}
			return normal / count;
		}

		private void SetInAirValues()
		{			
			RigidBody.centerOfMass = grounded ? centerOfMass.localPosition : airCenterOfMass.localPosition;
			RigidBody.drag = grounded ? profile.GroundedDrag : profile.AirDrag;
			if(!grounded)
				RigidBody.angularVelocity /= profile.AngularReduction; //todo adapt so it can turn left right but not fly upwards because of torque
		}

		/// <summary>
		/// Applies acceleration, steering, traction if the surface is not too steep. Also limits velocity.
		/// </summary>
		private void ApplyForces(float thrust, float turning)
		{
			var groundNormal = GetAverageImpactNormal();
			Vector3 forward = Vector3.ProjectOnPlane(transform.forward, groundNormal); //apply forces relative to the ground we are driving on

			if (Mathf.Abs(thrust) > 0)
			{
				thrust = thrust > 0 ? thrust * profile.ForwardAcceleration : thrust * profile.BackwardAcceleration;
				thrust = grounded ? thrust : thrust / profile.AirAccelerationReduction;
				//float groundMultiplier = profile.GroundTractionCurve.Evaluate(transform.up.y); //no thrust if we are driving up a steep angle/upsidedown
				if(!Single.IsNaN(thrust))
					RigidBody.AddForceAtPosition(forward * thrust , thrustApplyTransform.position, ForceMode.VelocityChange);
			}


			if (Mathf.Abs(turning) > 0)
			{
				turning = turning * profile.TurnSpeed * LimitTurning(); //todo maybe replace turning with curve depending on speed
				turning = grounded ? turning : turning / profile.AirTurningReduction;
				if(Mathf.Approximately(Mathf.Sign(RigidBody.angularVelocity.y), Mathf.Sign(turning))) //if we are steering in the same direction as the car is turning
					turning *= profile.SteeringCurve.Evaluate(RigidBody.angularVelocity.magnitude); //more steering when we have low angular velocity
				RigidBody.AddTorque(Vector3.up * turning, ForceMode.VelocityChange);
			}
			else if(grounded) //only limit when not in air
			{
				RigidBody.angularVelocity /= profile.AngularReduction; //reduce angular speed when no steering occurs so car doesn't steer too much 
			}
				

			//limit velocity
			if (RigidBody.velocity.magnitude > (RigidBody.velocity.normalized * profile.VelocityLimit).magnitude)
				RigidBody.velocity = RigidBody.velocity.normalized * profile.VelocityLimit;
		}

		/// <summary>
		/// limits the turning speed if under the threshold
		/// </summary>
		/// <returns>a value between 0 and 1 depending on the speed</returns>
		private float LimitTurning()
		{
			if (Mathf.Abs(RigidBody.velocity.magnitude) > profile.TurningThresholdVelocity) //if velocity is over the threshold don't limit
				return 1;

			float velocityRatio = RigidBody.velocity.magnitude / profile.TurningThresholdVelocity; //a value from 0 to 1 depending on how fast we go
			return velocityRatio;
		}

		/// <summary>
		/// Applies a counterforce in the opposite direction of the sideways velocity to reduce sliding
		/// </summary>
		private void ApplyTraction()
		{
			//rocket league traction model
			var forwardVelocity = Math.Abs(GetLocalVelocity().z);
			var sidewaysVelocity = Mathf.Abs(GetLocalVelocity().x);

			var frictionRatio = sidewaysVelocity / (sidewaysVelocity + forwardVelocity); //ratio of forwardspeed relative to sidewaysspeed (0 straight forward, 1 fully sideways)

			if (Single.IsNaN(frictionRatio)) return;
			float slideFriction = profile.TractionCurve.Evaluate(frictionRatio);
			
			var counterLocalVelocity = new Vector3(GetLocalVelocity().x * -1, 0, 0); //opposite of our sideways velocity
			var counterVelocity = transform.TransformDirection(counterLocalVelocity); //transform back to worldspace velocity
			float groundMultiplier = profile.GroundTractionCurve.Evaluate(transform.up.y);
			var tractionForce = profile.TractionForce * slideFriction *groundMultiplier;
			RigidBody.AddForce(counterVelocity * tractionForce, ForceMode.Acceleration);

			//DebugText.Instance.Print("ratio", $"{frictionRatio:F3}");
			//DebugText.Instance.Print("slideFriction", $"{slideFriction:F3}");
			//DebugText.Instance.Print("tractionForce", $"{tractionForce:F3}");
			//DebugText.Instance.Print("groundMultiplier", $"{groundMultiplier:F3}");


			//var localVelocity = GetLocalVelocity();
			//localVelocity.x *= -1; //get opposite direction of sliding velocity
			//localVelocity.x = Mathf.Clamp(localVelocity.x, -profile.MaxLocalCounterVelocity, profile.MaxLocalCounterVelocity); //todo maybe replace this with cutoff point for traction
			//var counterLocalVelocity = new Vector3(localVelocity.x, 0, 0);
			//var counterVelocity = transform.TransformDirection(counterLocalVelocity); //transform back to worldspace velocity
			//float velocityRatio = RigidBody.velocity.magnitude / currentMaxSpeed; //how fast on a scale of 0 to 1 are we?
			//velocityRatio = 1 - velocityRatio; //if we are at maxvelocity apply almost no counterforce (less traction the faster you go)

			//RigidBody.AddForce(counterVelocity * profile.TractionForce * velocityRatio, ForceMode.Acceleration);
		}
	}

	public class WheelValues
	{
		public Vector3 WheelPosition { get; set; }
		public float HoverDelta { get; set; }
		public Vector3 ImpactPoint { get; set; }
		public Vector3 ImpactNormal { get; set; }
		public Vector3 UpForce { get; set; }
		public bool Grounded { get; set; }
	}
}
