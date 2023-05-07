//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Rewired;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Handles launching and reattaching the caravan.
	/// Tracks distance and wallhits.
	/// </summary>
	public class CaravanController : MonoBehaviour
	{
		
		[Space(10)]
		[SerializeField] private LayerMask wallLayer;

		private CaravanProfile profile;
		private new HingeJoint hingeJoint;
		private new Transform transform;
		private HingeJointValueHolder hingeJointCopy;
		private bool isDetached = false;
		private float reattachTimer = 0f;

		private int playerId = 0;
		private new BoxCollider collider;
		///keep track of last caravan positon for each frame to calc distance
		private Vector3 lastPosition;

		public delegate void DetachAction();
		public event DetachAction OnDetach;
		public event DetachAction OnAttach;

		#region Properties
		///Is the caravan detached? Also calls OnDetach and OnAttach when this property is set.
		public bool IsDetached 
		{
			get => isDetached;
			set
			{
				isDetached = value;
				if (isDetached)
					OnDetach?.Invoke();				
				else
					OnAttach?.Invoke();				
			}
		}
		public Vector3 GetLocalVelocity() => transform.InverseTransformDirection(Rigidbody.velocity);
		/// Can the caravan be attached, or is it still on cooldown?
		public bool Attachable { get; private set; }

		public CarController Car { get; private set; }

		public Rigidbody Rigidbody { get; private set; }
		public Rewired.Player RewiredPlayer { get; private set; }
		///Stores the last 10 velocities
		public CircularBuffer<Vector3> Velocities { get; private set; }


		public float DistanceMoved { get; private set; }
		/// The time since the caravan was last attached
		public float TimeTraveled { get; private set; }
		public int WallHits { get; private set; }
		///Information about the hits this caravan made while colliding with others (this info is reset when attached)
		public List<Hit> Hits { get; private set; } = new List<Hit>();
		public int KillsWithOneShot { get; private set; }
		public HingeJoint HingeJoint => hingeJoint;

		#endregion


		private void Awake()
		{
			SetValues();
			ReattachCaravan();
			lastPosition = transform.position;
		}


		private void OnEnable()
		{			
			OnDetach += ExecuteReattachDelay;
			OnAttach += ResetTrackingVariables;
		}

		private void OnDisable()
		{
			OnDetach -= ExecuteReattachDelay;
			OnAttach -= ResetTrackingVariables;
			StopAllCoroutines();
		}

		private void SetValues()
		{
			transform = GetComponent<Transform>();
			collider = GetComponent<BoxCollider>();
			Car = transform.parent.GetComponentInChildren<CarController>();
			profile = GameModes.Instance.CurrentGameMode.CaravanProfile;
			Assert.IsNotNull(profile);
			RewiredPlayer = ReInput.players.GetPlayer(playerId);
			Rigidbody = GetComponent<Rigidbody>();
			hingeJoint = GetComponent<HingeJoint>();
			hingeJointCopy = new HingeJointValueHolder();
			Utility.CopyHingeJoint(hingeJoint, hingeJointCopy);
			reattachTimer = 0f;
			Attachable = false;
			Velocities = new CircularBuffer<Vector3>(10);
		}

		public void SetPlayerId(int id)
		{
			playerId = id;
			RewiredPlayer = ReInput.players.GetPlayer(playerId);
		}

		public bool GetFireInput()
		{
			string button = "Fire";
			return RewiredPlayer.GetButtonDown(button);
		}

		public bool GetResetInput()
		{
			string button = "Reset";
			return RewiredPlayer.GetButtonDown(button);
		}

		private void FixedUpdate()
		{
			Velocities.PushBack(Rigidbody.velocity);
		}

		private void Update()
		{
			if(!Car.IsAi && (GetFireInput() || GetResetInput())) //todo maybe bind fire to both A and X 
				AttachOrShoot();

			if (IsDetached) //when detached track distance and time it is detached
			{
				TrackDistance();
				TimeTraveled += Time.deltaTime;
			}
		}

		/// <summary>
		/// Shoots the caravan if attached, tries to attach it if not
		/// </summary>
		public void AttachOrShoot()
		{
			if (IsDetached)
				TryAttach();
			else
				Launch();
		}

		/// <summary>
		/// Attaches caravan, checks distance if manualReattach is checked
		/// </summary>
		public void TryAttach()
		{
			if(!profile.ManualReattach)
				ReattachCaravan(); 
			else if(Vector3.Distance(Car.transform.position, this.transform.position) < profile.ReattachDistance) //only attach if in range
				ReattachCaravan();
		}

		/// <summary>
		/// Launches the caravan if attached
		/// </summary>
		public void Launch()
		{
			if (IsDetached) return;
			Detach();
			var launchForce = Rigidbody.velocity.normalized * Rigidbody.velocity.magnitude * profile.LaunchSpeed;
			if (launchForce.magnitude < profile.MinLaunchForce && Rigidbody.velocity.magnitude > profile.MinForceThreshold) //force min speed if we have enough velocity
				launchForce = launchForce.normalized * profile.MinLaunchForce;

			Rigidbody.AddForce(launchForce, ForceMode.Acceleration);
		}

		private void Detach()
		{
			IsDetached = true;
			Destroy(hingeJoint);
		}

		/// <summary>
		/// Detaches the caravan and deactivates the whole gameobject
		/// </summary>
		public void Deactivate()
		{		
			Detach();
			StopAllCoroutines();
			this.gameObject.SetActive(false);
		}

		/// <summary>
		/// Force attaches the caravan and activates the whole gameobject, doesn't check if caravan is in a wall
		/// </summary>
		public void ForceAttachAndActivate()
		{
			gameObject.SetActive(true);
			StopAllCoroutines();
			IsDetached = true;
			Attachable = true;
			//force attach without check if in collider
			var newPosition = Car.transform.TransformPoint(Car.GetTrailerAttachTransform().localPosition); 
			ResetPosition(newPosition);
			ConnectHinge();
			IsDetached = false;
		}

		/// <summary>
		/// Resets position of the caravan and reattaches the physics joint if detached and not on cooldown
		/// </summary>
		/// <returns></returns>
		public void ReattachCaravan()
		{
			if (!IsDetached || !Attachable)
			{
				return;
			}
			var newPosition = Car.transform.TransformPoint(Car.GetTrailerAttachTransform().localPosition);

			// collider is not centered on caravan
			var newColliderPosition = Car.transform.TransformPoint(Car.GetTrailerAttachTransform().localPosition + collider.center);
			if (IsInCollider(newColliderPosition))
			{
				Debug.Log($"new position is in collider cant attach");
				return;
			}

			ResetPosition(newPosition);
			ConnectHinge();
			IsDetached = false;
		}

		private void ResetPosition(Vector3 newPosition)
		{
			transform.position = newPosition;
			transform.rotation = Car.transform.rotation;
			Rigidbody.velocity = Vector3.zero;
			Rigidbody.angularVelocity = Vector3.zero;
			Physics.SyncTransforms();
		}

		/// <summary>
		/// Adds an hingejoint components and connects the car to it
		/// </summary>
		private void ConnectHinge()
		{
			if(hingeJoint) Destroy(hingeJoint);
			hingeJoint = gameObject.AddComponent<HingeJoint>();
			Utility.CopyHingeJoint(hingeJointCopy, hingeJoint);
			if (!hingeJoint.connectedBody)
				hingeJoint.connectedBody = Car.RigidBody;
		}

		/// Sets Attachable to false for the time of the reattachDelay
		private void ExecuteReattachDelay() => StartCoroutine(nameof(ReattachDelay));

		/// <summary>
		/// Sets Attachable to false for the time of the reattachDelay
		/// </summary>
		/// <returns></returns>
		private IEnumerator ReattachDelay()
		{
			Attachable = false;
			while (reattachTimer < profile.ReattachDelay)
			{
				reattachTimer += Time.deltaTime;
				yield return null;
			}
			reattachTimer = 0;
			Attachable = true;
		}

		public void ResetAttachTimer()
		{
			reattachTimer = 0;
			Attachable = true;
		}

		/// <summary>
		/// Checks if new caravan spawn position is in a collider
		/// </summary>
		/// <returns></returns>
		private bool IsInCollider(Vector3 newPosition) 
		{
			var isIn = Physics.CheckBox(newPosition, collider.bounds.extents/2, Quaternion.identity, wallLayer);
			return isIn;
		}


		private void TrackDistance()
		{
			DistanceMoved += Vector3.Distance(lastPosition, transform.position);
			lastPosition = transform.position;
		}

		private void ResetTrackingVariables()
		{
			DistanceMoved = 0;
			WallHits = 0;
			TimeTraveled = 0;
			Hits.Clear();
			KillsWithOneShot = 0;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (IsDetached && other.gameObject.CompareTag("Border"))
			{
				WallHits++;
			}				
		}

		public void AddHit(Hit hit)
		{
			Hits.Add(hit);
			if (hit.KillShot)
			{
				KillsWithOneShot++;
			}
		}
	}
}


