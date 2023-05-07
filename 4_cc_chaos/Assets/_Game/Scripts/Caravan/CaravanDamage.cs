//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{

	/// <summary>
	/// Handles collision of the caravan and how much damage is dealt.
	/// </summary>
	public class CaravanDamage : MonoBehaviour
	{
		[SerializeField] private CaravanProfile profile;
		private Player lastPlayer = null;
		private new Rigidbody rigidbody;
		private CaravanController caravanController;
		private bool onCooldown = false;

		/// <summary>
		/// Callback when damage is dealt by this caravan
		/// </summary>
		/// <param name="hit">the shot that made this collision</param>
		public delegate void DealDamage(Hit hit);
		public event DealDamage OnDealDamage;

		private void Start()
		{
			Assert.IsNotNull(profile);
			rigidbody = GetComponent<Rigidbody>();
			caravanController = GetComponent<CaravanController>();
			onCooldown = false;
		}

		private void OnCollisionEnter(Collision collision) 
		{
			if (!GameModes.Instance.CurrentGameMode.AllowDamage) return;

			if (GameModes.Instance.CurrentGameMode.GameModeIdentifier == GameModeIdentifier.HotPotato)
			{
				HotPotatoCollisionHandling(collision);
			}
			else
			{
				StandardCollisionHandling(collision);
			}
		}

		private void HotPotatoCollisionHandling(Collision collision)
		{
			if (!collision.gameObject.CompareTag("Car") && !collision.gameObject.CompareTag("Caravan"))
				return;
			if (collision.gameObject.GetComponent<CarController>() &&
			    collision.gameObject.GetComponent<CarController>() == caravanController.Car) //no damage if we collide with our own car
				return;
			
			var collisionPlayer = collision.transform.parent.GetComponent<Player>(); //get player we collided with

			Hit hit = new Hit(collision, -1f, -1, false, -1f, caravanController);
			OnDealDamage?.Invoke(hit);
		}

		private void StandardCollisionHandling(Collision collision)
		{
			if (!collision.gameObject.CompareTag("Car") && !collision.gameObject.CompareTag("Caravan"))
				return;
			if (collision.gameObject.GetComponent<CarController>() &&
			    collision.gameObject.GetComponent<CarController>() == caravanController.Car) //no damage if we collide with our own car
				return;
		
			var collisionPlayer = collision.transform.parent.GetComponent<Player>(); //get player we collided with
			if (onCooldown && lastPlayer == collisionPlayer) return; //if we are colliding with the same object in a short time span

			StartCooldown();

			var hit = DetermineHit(collision);
			caravanController.AddHit(hit);
			OnDealDamage?.Invoke(hit);
			lastPlayer = collisionPlayer;
		}

		/// <summary>
		/// Determines whether hit was an actual hit, or a deflection.
		/// </summary>
		/// <param name="collision"></param>
		/// <returns></returns>
		private Hit DetermineHit(Collision collision)
		{
			var otherCaravan = collision.gameObject.GetComponent<CaravanController>();						

			if (otherCaravan != null && otherCaravan.IsDetached) //caravan is detached							
			{																																	 
				var velocities = otherCaravan.Velocities;
				var otherVelocity = velocities[velocities.Size - 2];

				if (otherVelocity.sqrMagnitude > 10f) //caravan is moving, don't do damage	// caravan is moving -> is it's velocity higher than threshold
				{
					return CreateHit(collision, true);
				}

				return CreateHit(collision);
			}

			return CreateHit(collision); //caravan is attached
		}

		/// <summary>
		/// Creates a hit where other object receives damage.
		/// </summary>
		/// <param name="collision"></param>
		/// <returns></returns>
		private Hit CreateHit(Collision collision, bool deflect = false)
		{
			float speed = caravanController.Velocities.Back().magnitude;
			var damage = CalculateDamage(collision, speed);

			//if hit is a deflect, the collision object doesn't take any damage
			var resultingHealth = deflect ? collision.transform.parent.GetComponent<Health>().CurrentHealth : collision.transform.parent.GetComponent<Health>().TakeDamage((int)damage);

			return new Hit(collision, damage, resultingHealth, resultingHealth <= 0, speed, caravanController, deflect); //todo track multiple kills with 1 shot
		}

		/// <summary>
		/// Calculates damage 
		/// </summary>
		/// <param name="collision">Collision that happened</param>
		/// <returns>0 if we are not moving towards the collision, otherwise damage scaling with our velocity and the detachedMultiplier</returns>
		private float CalculateDamage(Collision collision, float speed)
		{						
			if (!IsMovingTowards(collision))
				return 0;			

			if(caravanController.IsDetached)
				return profile.BaseDamage * speed * profile.DetachMultiplier;

			return profile.BaseDamage * speed;
		}

		/// <summary>
		/// Sets onCooldown to true for the time of collisionCooldown
		/// </summary>
		private void StartCooldown()
		{
			StopCoroutine(nameof(Cooldown));
			StartCoroutine(nameof(Cooldown));
		}

		/// <summary>
		/// Sets onCooldown to true for the time of collisionCooldown
		/// </summary>
		private IEnumerator Cooldown()
		{
			onCooldown = true;
			yield return new WaitForSeconds(profile.CollisionCooldown);
			onCooldown = false;
		}

		/// <summary>
		/// Checks if we are moving towards the collision
		/// </summary>
		/// <param name="collision"></param>
		/// <returns>If the direction we are moving in is similar to the direction of our center to the collision</returns>
		private bool IsMovingTowards(Collision collision)
		{
			//get the average collision point
			var collisionContactAverage = new Vector3();
			int i = 0;
			foreach (var contact in collision.contacts)
			{
				collisionContactAverage += contact.point;
				i++;
			}
			collisionContactAverage /= i;
			collisionContactAverage += caravanController.Velocities.Back(); //not sure if necessary anymore

			var targetDirection = collisionContactAverage - transform.position;
			targetDirection.y = 0; //not sure if the y reset is necessary anymore either
			var velocity = caravanController.Velocities.Back();
			velocity.y = 0;

			//is the direction from the middle of the object to the collision point similar to the velocity?
			float dotProduct = Vector3.Dot(targetDirection.normalized, velocity.normalized);
			//if yes then it is moving towards the object
			return (dotProduct > 0);
		}

	}
}

