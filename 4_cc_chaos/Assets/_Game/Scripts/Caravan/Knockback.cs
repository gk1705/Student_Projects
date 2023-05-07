//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class Knockback : MonoBehaviour
	{
		[SerializeField] private float knockBackStrength = 0f;
		[Tooltip("Knockback to the car when the caravan is hit")]
		[SerializeField] private float carKnockBackStrength = 2f;

		private void OnEnable()
		{
			transform.parent.GetComponent<CaravanDamage>().OnDealDamage += ApplyKnockback;
		}

		private void OnDisable()
		{
			transform.parent.GetComponent<CaravanDamage>().OnDealDamage -= ApplyKnockback;
		}

		private void ApplyKnockback(Hit hit)
		{
			var player = hit.Collision.transform.parent.GetComponent<Player>();

			var knockBack = 1 + hit.Damage * knockBackStrength;
			var carKnockBack = 1 + hit.Damage * carKnockBackStrength; //knockback to car when caravan is hit

			var caravan = hit.Collision.gameObject.GetComponent<CaravanController>();

			Vector3 forceDirection = hit.Collision.relativeVelocity * -1;
			forceDirection.y = 0;

			if (
				(caravan != null && caravan.IsDetached) // caravan hit
				||
				(caravan == null && player.CaravanController.IsDetached) // car hit
			) //case_1: hit while detached
			{
				hit.Collision.rigidbody.AddForce(forceDirection * knockBack, ForceMode.Acceleration);
			}
			else //case_2: hit while attached
			{
				player.CarController.RigidBody.AddForce(forceDirection * carKnockBack, ForceMode.Acceleration);
				hit.Collision.rigidbody.AddForce(forceDirection * knockBack, ForceMode.Acceleration);
			}
		}
	}
}
