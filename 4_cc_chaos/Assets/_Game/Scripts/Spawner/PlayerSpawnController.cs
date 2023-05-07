//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Rewired;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Spawns given prefab; either by what order the spawn points were given,
	/// or randomly. Holds an instance of all spawned objects, to later deactivate/delete them.
	/// </summary>
	public class PlayerSpawnController : MonoBehaviour
	{
		[SerializeField]
		enum HowToSpawn
		{
			Sequentially,
			Shuffled
		};
		
		[SerializeField] private List<Transform> spawnPoints;
		[SerializeField] private List<GameObject> playerPrefabs;
		[SerializeField] private SpawnBehaviour spawnBehaviour;
		[SerializeField] private HowToSpawn spawnOrder;
		[SerializeField] private CinemachineTargetGroup targetGroup;
		[SerializeField] private GameUIHandler UIHandler;
		[SerializeField] private GameManager gameManager;

		private List<LobbyPlayer> players;

		private List<GameObject> spawnedPlayers;
		private List<int> playerIds;

		private void Awake()
		{
			spawnedPlayers = new List<GameObject>();
			playerIds = new List<int>();
			players = Lobby.Players;
		}

		private void Start()
		{
			SpawnPlayersSequentially();
		}

		public void SpawnObjects()
		{
			switch (spawnOrder)
			{
				case HowToSpawn.Sequentially:
					SpawnPlayersSequentially();
					break;
				case HowToSpawn.Shuffled:
					//NOTE: not implemented
					break;
			}
		}

		private GameObject InstantiatePlayer(int index)
		{
			var playerId = players[index].Id;
			var go = Instantiate(playerPrefabs[playerId], spawnPoints[playerId]);

			var carController = go.GetComponentInChildren<CarController>();
			var caravanController = go.GetComponentInChildren<CaravanController>();

			go.GetComponent<Player>().playerColor = players[index].Color;
			if (GameModes.Instance.CurrentGameMode.AllowDecorations) go.GetComponent<DecorationChanger>().ChangeDecoration(players[index].Decoration);
			go.GetComponentInChildren<MeshChanger>().ChangeModel(players[index].caravanModel);
			go.GetComponentInChildren<ChangeBodyColor>().ApplyColor(players[index].Color);

			if (caravanController && caravanController)
			{
				targetGroup.AddMember(carController.transform, 1, 2);
				targetGroup.AddMember(caravanController.transform, 1, 2);
			}
			else
			{
				Debug.Log("Player spawn behaviour can not be triggered, due to missing car- or caravan-controller.");
				return null;
			}

			spawnedPlayers.Add(go);
			playerIds.Add(playerId);

			return go;
		}

		private void ExecutePlayerSpawnBehaviour(GameObject go, int playerId)
		{
			GameModes.Instance.CurrentGameMode.SpawnBehaviour.InvokeBehaviour(go, playerId);
		}

		private void SpawnPlayersSequentially()
		{
			Debug.Assert(spawnPoints.Count >= players.Count);

			for (int i = 0; i < players.Count; i++)
			{
				var go = InstantiatePlayer(i);
				ExecutePlayerSpawnBehaviour(go, players[i].Id);
				go.transform.LookAt(transform);
				var player = go.GetComponent<Player>();
				player.SetID(players[i].Id);
				if (players[i].IsBot)
				{
					player.CarController.IsAi = true;				
				}
				player.CarController.GetComponentInChildren<TextMeshPro>().enabled = player.CarController.IsAi;
			}

			UIHandler.SetPlayers(spawnedPlayers, playerIds);
			gameManager.SetPlayers(spawnedPlayers);
		}

		private void DespawnObjects()
		{
			foreach (var so in spawnedPlayers)
			{
				Destroy(so);
			}
		}
	}
}
