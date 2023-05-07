//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class CarAi : MonoBehaviour
	{

		private CarController carController;
		[Header("CollisionAvoidance")]
		[SerializeField] private float wallCheckDistance = 12f;
		[SerializeField] private float sideWallDistance = 8f;
		[SerializeField] private LayerMask layerMask;
		private BotDifficulty botDifficulty;
		
		// Start is called before the first frame update
		void Start()
		{
			carController = GetComponent<CarController>();
			botDifficulty = GameModes.Instance.CurrentGameMode.BotDifficulty;
		}


		// Update is called once per frame

		void FixedUpdate()
		{
			switch (GameModes.Instance.CurrentGameMode.GameModeIdentifier)
			{
				case GameModeIdentifier.LastVanStanding:
					LVSBehavior();
					break;
				case GameModeIdentifier.Hardcore:
					HardCoreBehavior();
					break;
				case GameModeIdentifier.HotPotato:
					LVSBehavior();
					break;
				case GameModeIdentifier.Soccer:
					LVSBehavior();
					break;
				default:
					LVSBehavior();
					break;
			}			
		}

		private void SoccerBehavior()
		{
			throw new System.NotImplementedException();
		}


		private void LVSBehavior()
		{
			var thrust = CalculateDriving().thrust;
			var turning = CalculateDriving().turning;


			carController.Drive(thrust, turning);
		}


		private void HardCoreBehavior()
		{
			var thrust = CalculateDriving().thrust;
			var turning = CalculateDriving().turning;

			if (carController.Caravan.IsDetached) //if caravan is detached drive towards it
			{
				var toCaravan = carController.Caravan.transform.position - carController.transform.position;
				var dotToCaravan = Vector2.Dot(toCaravan.normalized.ToVector2(),carController.transform.forward.normalized.ToVector2());

				if (dotToCaravan > 0.9f)
					turning = 0f;
				Debug.Log($"dottocaravan {dotToCaravan:F2}");
			}

			carController.Drive(thrust, turning);
		}


		private (float thrust, float turning) CalculateDriving()
		{
			//ping pong values for randomness
			float thrust = Utility.PingPong(Time.time, botDifficulty.MinThrust, botDifficulty.MaxThrust);
			float turning = 0;

			var turnMultiplier = Utility.PingPong(Time.time, botDifficulty.MinTurn, botDifficulty.MaxTurn);

			var additionalTurn = Mathf.Sin(Time.time) * turnMultiplier; //random sin offset
			turning += additionalTurn;

			if (WallLeft()) //if wall close sideways force turn
				turning = 1f;
			else if (WallRight())
				turning = -1f;
			else if (WallInRange()) //turn right when going straight into wall
				turning = 1f;

			return (thrust, turning);
		}


		private bool WallInRange()
		{
			return Physics.Raycast(carController.transform.position, carController.transform.forward, out var hit, wallCheckDistance, layerMask);
		}

		private bool WallLeft()
		{
			return Physics.Raycast(carController.transform.position, -carController.transform.right, out var hit, sideWallDistance, layerMask);
		}

		private bool WallRight()
		{
			return Physics.Raycast(carController.transform.position, carController.transform.right, out var hit, sideWallDistance, layerMask);
		}		
	}
}


