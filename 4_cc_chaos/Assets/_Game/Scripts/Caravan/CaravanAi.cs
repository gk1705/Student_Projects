//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired.Utils.Classes.Data;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Random = UnityEngine.Random;

namespace CaravanCrashChaos
{
	public class CaravanAi : MonoBehaviour
	{

		List<CarController> cars = new List<CarController>();
		private CaravanController caravan;
		[SerializeField] private float shotCheckInterval = 0.1f;
		[SerializeField] private float minJointAngle = 35f;
		[Tooltip("When the distance to the closest car is closer than this after ~1 seconds then the caravan will wait additional time before attaching again")]
		[SerializeField] private float firstCheckDistance = 50f;
		private WaitForSeconds shotWait;
		private WaitForSeconds hardCoreAttachCheck;
		private BotDifficulty botDifficulty;

		// Start is called before the first frame update
		void Start()
		{
				
		}

		private void OnEnable()
		{
			StopAllCoroutines();
			cars = FindObjectsOfType<CarController>().ToList();
			caravan = GetComponent<CaravanController>();
			botDifficulty = GameModes.Instance.CurrentGameMode.BotDifficulty;
			shotWait = new WaitForSeconds(botDifficulty.DelayBeforeShot);
			hardCoreAttachCheck = new WaitForSeconds(0.2f);
			caravan.OnDetach += AttachRoutine;


			switch (GameModes.Instance.CurrentGameMode.GameModeIdentifier)
			{

				case GameModeIdentifier.Soccer:
					StartCoroutine(CheckForShotSoccer());
					break;
				default:
					StartCoroutine(CheckForShot());
					break;
			}

			
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}

		private IEnumerator CheckForShotSoccer()
		{
			yield return new WaitForSeconds(0.1f); //fix for goal not assigned when this is executed
			SoccerBall soccerBall = FindObjectOfType<SoccerBall>();
			var goals = FindObjectsOfType<Goal>();

			var ownGoal = goals.First(g => g.GoalOwner == caravan.GetComponentInParent<Player>());

			while (true)
			{
				cars = cars.Where(c => c.transform.parent.GetComponent<Health>().IsDead == false).ToList(); //get targets that are not dead
				if (caravan.IsDetached)
				{
					Debug.Log($"detached");
					yield return null;
					continue;
				}

				var targetBall = soccerBall.transform.position - caravan.transform.position;
				var ownGoalVector = ownGoal.transform.position - caravan.transform.position;
				var velocityVector = caravan.Rigidbody.velocity;

				var dotBall = Vector2.Dot(targetBall.normalized.ToVector2(), velocityVector.normalized.ToVector2());
				var dotOwnGoal = Vector2.Dot(ownGoalVector.normalized.ToVector2(),velocityVector.normalized.ToVector2());


				if (dotBall >= botDifficulty.DotThreshold && dotOwnGoal < 0.8f) //if we are shooting at the ball and not at our own goal
				{
					if (caravan.Rigidbody.velocity.magnitude > 10f && !CarInTheWay(caravan.Rigidbody.velocity, caravan.Car.transform.forward)) //if caravan swings enough
					{
						yield return shotWait; ; //extra delay to hit object
						Shoot();
					}
				}

				yield return new WaitForSeconds(shotCheckInterval);
			}
		}

		private IEnumerator CheckForShot()
		{
			while (true)
			{
				cars = cars.Where(c => c.transform.parent.GetComponent<Health>().IsDead == false).ToList(); //get targets that are not dead

				if (caravan.IsDetached)
				{
					yield return null;
					continue;
				}


				foreach (var carController in cars) //check all players
				{
					if (carController == caravan.Car) //dont check for own car
						continue;

					var targetCar = carController.transform.position - caravan.transform.position;
					var targetCaravan = carController.Caravan.transform.position - caravan.transform.position;
					var velocityVector = caravan.Rigidbody.velocity;

					//Debug.DrawRay(transform.position, targetCar, Color.red, shotCheckInterval);
					//Debug.DrawRay(transform.position, targetCaravan, Color.blue, shotCheckInterval);
					//Debug.DrawRay(transform.position, velocityVector, Color.green, shotCheckInterval);

					var dotCar = Vector2.Dot(targetCar.normalized.ToVector2(), velocityVector.normalized.ToVector2()); //get dot product of velocity and vector to target
					var dotCaravan = Vector2.Dot(targetCaravan.normalized.ToVector2(), velocityVector.normalized.ToVector2());

					if (!carController.Caravan.gameObject.activeSelf) //if hotpotato, don't shoot at other deactivated caravans
						dotCaravan = 0;

					if (dotCar >= botDifficulty.DotThreshold || dotCaravan >= botDifficulty.DotThreshold) //if the velocity points to a target
					{
						if (caravan.Rigidbody.velocity.magnitude > 10f && !CarInTheWay(caravan.Rigidbody.velocity, caravan.Car.transform.forward)) //if caravan swings enough
						{
							yield return shotWait; ; //extra delay to hit object
							Shoot();
							break;
						}					
					}						
				}
			
				yield return new WaitForSeconds(shotCheckInterval);
			}
		}

		/// <summary>
		/// Checks if the own car would block the shot
		/// </summary>
		/// <returns></returns>
		private bool CarInTheWay(Vector3 caravanVelocity, Vector3 carForward)
		{
			var dot = Vector2.Dot(caravanVelocity.normalized.ToVector2(), carForward.normalized.ToVector2()); 
			return dot > 0.85f && Mathf.Abs(caravan.HingeJoint.angle) < minJointAngle; //check if velocity would go into car and caravan doesn't swing enough
		}

		private void AttachRoutine()
		{
			switch (GameModes.Instance.CurrentGameMode.GameModeIdentifier)
			{
				case GameModeIdentifier.LastVanStanding:
					StartCoroutine(LVSAttach());
					break;
				case GameModeIdentifier.Hardcore:
					StartCoroutine(HardCoreAttach());
					break;
				case GameModeIdentifier.HotPotato:
					StartCoroutine(LVSAttach());
					break;
				default:
					StartCoroutine(LVSAttach());
					break;
			}
			
		}

		/// <summary>
		/// Waits a random time, tries to attach again if caravan not close to other cars, otherwise waits a bit before trying again
		/// </summary>
		/// <returns></returns>
		private IEnumerator LVSAttach()
		{
			while (caravan.IsDetached)
			{
				var randomWait = Random.Range(0.8f, 1.4f);
				yield return new WaitForSeconds(randomWait);
				var distance = GetClosestCarDistance();
				if(distance < firstCheckDistance)
					yield return new WaitForSeconds(1f); //if we could hit another car then wait a bit
				Attach();
			}			
		}

		private IEnumerator HardCoreAttach()
		{
			while (caravan.IsDetached)
			{
				Attach();
				yield return hardCoreAttachCheck;
			}
		}

		private float GetClosestCarDistance()
		{
			float closest = float.MaxValue;
			foreach (var carController in cars)
			{
				if (carController == caravan.Car)
					continue;

				var distance = Vector3.Distance(caravan.transform.position, carController.transform.position);
				if (distance < closest)
					closest = distance;
			}
			return closest;
		}

		private void Shoot()
		{
			caravan.Launch();
		}

		private void Attach()
		{
			caravan.TryAttach();
		}
		
	}
}


