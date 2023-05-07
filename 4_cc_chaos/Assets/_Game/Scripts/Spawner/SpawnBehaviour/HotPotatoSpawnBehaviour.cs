//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "HotPotatoSpawnBehaviour", menuName = "SpawnBehaviour/HotPotato", order = 1)]
	public class HotPotatoSpawnBehaviour : SpawnBehaviour
	{
		public override void InvokeBehaviour(GameObject go, int objectId)
		{
			AssignPlayerName(go, objectId);
			AssignControllerId(go, objectId);
			go.GetComponentInChildren<CaravanController>().Deactivate();
		}

		private void AssignPlayerName(GameObject player, int spawnId)
		{
			player.name = player.name + spawnId.ToString();
		}

		private void AssignControllerId(GameObject player, int spawnId)
		{
			var carController = player.GetComponentInChildren<CarController>();
			var caravanController = player.GetComponentInChildren<CaravanController>();

			if (caravanController && caravanController)
			{
				carController.SetPlayerId(spawnId);
				caravanController.SetPlayerId(spawnId);
			}
			else
			{
				Debug.Log("Player spawn behaviour can not be triggered, due to missing car- or caravan-controller.");
			}
		}
	}
}

