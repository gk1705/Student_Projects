using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnBehaviourPlayer", menuName = "SpawnBehaviour/Player", order = 1)]
public class PlayerSpawnBehaviour : SpawnBehaviour
{
	public override void InvokeBehaviour(GameObject go, int objectId)
	{
		AssignPlayerName(go, objectId);
		AssignControllerId(go, objectId);
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
