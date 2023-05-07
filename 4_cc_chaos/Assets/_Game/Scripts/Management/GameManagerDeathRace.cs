//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



namespace CaravanCrashChaos
{
	public class GameManagerDeathRace : MonoBehaviour
	{

		[Header("UI Elements")]
		[SerializeField] private List<Transform> spawnPoints;
		[SerializeField] private List<GameObject> playerPrefabs;
		[SerializeField] private SpawnBehaviour spawnBehaviour;
		[SerializeField] private CinemachineTargetGroup targetGroup;

		[Header("UI Elements")]
		[SerializeField] private GameObject countdownUI;
		[SerializeField] private GameObject gameCountdown;
		[SerializeField] private GameUIHandler UIHandler;
		[SerializeField] private Text finishedText;

		[Header("Settings")]
		[SerializeField] private float timeUntilEndscreen = 5f;


		private List<LobbyPlayer> lobbyPlayers;
		private List<GameObject> scenePlayers = new List<GameObject>();
		private Dictionary<int, int> playerPlacement = new Dictionary<int, int>();
		private int livingPlayers = 0;

		void Start()
		{
			lobbyPlayers = Lobby.Players;

			SpawnPlayers();

			countdownUI.GetComponent<Countdown>().StartTimer();
			DisablePlayersControls();
			UIHandler.SetPlayers(scenePlayers, GetPlayerIds());


			countdownUI.GetComponent<Countdown>().OnTimerTrigger += StartGame;
			gameCountdown.GetComponent<Countdown>().OnTimerTrigger += GameOver;
		}

		void Update()
		{
			if (GetLivingPlayerCount() == 1)
			{
				GameOver();
			}
		}

		public List<GameObject> GetScenePlayers()
		{
			return scenePlayers;
		}

		private int GetLivingPlayerCount()
		{
			int countLiving = 0;
			foreach (GameObject player in scenePlayers)
			{
				if (!player.GetComponent<Health>().IsDead)
				{
					countLiving++;
				}
			}

			return countLiving;
		}

		private void SpawnPlayers()
		{
			foreach (LobbyPlayer lobbyPlayer in lobbyPlayers)
			{
				int playerId = lobbyPlayer.Id;
				SpawnPlayer(playerId);
			}

			livingPlayers = scenePlayers.Count;
		}

		private void SpawnPlayer(int playerId)
		{
			
			var go = Instantiate(playerPrefabs[playerId], spawnPoints[playerId]);

			var carController = go.GetComponentInChildren<CarController>();
			var caravanController = go.GetComponentInChildren<CaravanController>();

			go.GetComponent<Player>().playerColor = Lobby.GetPlayer(playerId).Color;
			go.GetComponent<DecorationChanger>().ChangeDecoration(Lobby.GetPlayer(playerId).Decoration);
			go.GetComponentInChildren<MeshChanger>().ChangeModel(Lobby.GetPlayer(playerId).caravanModel);
			go.GetComponentInChildren<ChangeBodyColor>().ApplyColor(Lobby.GetPlayer(playerId).Color);
			go.GetComponent<Player>().SetID(playerId);
			go.GetComponent<PlayerDeath>().OnPlayerDies += PlayerDied;

			if (carController && caravanController)
			{
				targetGroup.AddMember(carController.transform, 1, 2);
				targetGroup.AddMember(caravanController.transform, 1, 2);
			}
			else
			{
				Debug.Log("Player spawn behaviour can not be triggered, due to missing car- or caravan-controller.");
				return;
			}

			spawnBehaviour.InvokeBehaviour(go, playerId);

			scenePlayers.Add(go);
		}

		private void PlayerDied(int playerId)
		{
			playerPlacement[playerId] = livingPlayers;
			livingPlayers--;
		}

		private List<GameObject> GetBestPlayers()
		{
			int currentBest = 0;
			List<GameObject> bestPlayers = new List<GameObject>();

			foreach (GameObject player in scenePlayers)
			{
				int score = player.GetComponent<StatsTracker>().currentScore;
				if (score > currentBest)
				{
					currentBest = score;
					bestPlayers.Clear();
					bestPlayers.Add(player);
				} else if (score == currentBest)
				{
					currentBest = score;
					bestPlayers.Add(player);
				}
			}

			return bestPlayers;
		}

		private List<int> GetPlayerIds()
		{
			List<int> playerIds = new List<int>();
			foreach (LobbyPlayer lobbyPlayer in lobbyPlayers)
			{
				playerIds.Add(lobbyPlayer.Id);
			}
			return playerIds;
		}

		public GameObject GetPlayerById(int id)
		{
			foreach (GameObject player in scenePlayers)
			{
				Player playerClass = player.GetComponent<Player>();
				if (playerClass.GetID == id)
				{
					return player;
				}
			}
			return null;
		}

		private void StartGame()
		{
			countdownUI.gameObject.SetActive(false);
			EnablePlayersControls();

			gameCountdown.GetComponent<Countdown>().StartTimer();
		}

		private void GameOver()
		{
			AddSurvivalScore();
			LockScores();
			SetStats();

			gameCountdown.SetActive(false);
			var bestPlayers = GetBestPlayers();

			finishedText.gameObject.SetActive(true);

			if (bestPlayers.Count == 1)
			{
				finishedText.text = $"Player {bestPlayers[0].GetComponent<Player>().GetID + 1} Won!";
				finishedText.color = bestPlayers[0].GetComponent<Player>().playerColor;
			} else
			{
				finishedText.text = "Tie!";
			}

			StartCoroutine(EnableEndscreen(bestPlayers));
		}

		private void AddSurvivalScore()
		{
			foreach (GameObject player in scenePlayers)
			{
				if (!player.GetComponent<Health>().IsDead)
				{
					player.GetComponent<StatsTracker>().AddScoreForSurvival();
				}
			}
		}

		private void SetStats()
		{
			foreach (GameObject player in scenePlayers)
			{
				int playerId = player.GetComponent<Player>().GetID;
				StatsTracker statsTracker = player.GetComponent<StatsTracker>();

				StatsManager.SetScore(playerId, statsTracker.currentScore);
				StatsManager.SetKills(playerId, statsTracker.killCount);
				StatsManager.SetRounds(playerId, statsTracker.roundCount);
				StatsManager.SetSurvivalPlace(playerId, playerPlacement[playerId]);
				StatsManager.SetDamage(playerId, statsTracker.damageCount);
				StatsManager.SetHasDied(playerId, statsTracker.hasDied);
			}
		}

		private IEnumerator EnableEndscreen(List<GameObject> bestPlayers)
		{
			yield return new WaitForSecondsRealtime(timeUntilEndscreen);
			SceneManager.LoadScene("Results");
		}


		public void LockScores()
		{
			foreach (GameObject player in scenePlayers)
			{
				player.gameObject.GetComponent<StatsTracker>().SetLocked(false);
			}
		}

		public void DisablePlayersControls()
		{
			foreach (GameObject player in scenePlayers)
			{
				player.gameObject.GetComponentInChildren<CarController>().enabled = false;
				player.gameObject.GetComponentInChildren<CaravanController>().enabled = false;
			}
		}

		public void EnablePlayersControls()
		{
			foreach (GameObject player in scenePlayers)
			{
				player.gameObject.GetComponentInChildren<CarController>().enabled = true;
				player.gameObject.GetComponentInChildren<CaravanController>().enabled = true;
			}
		}
	}

}
