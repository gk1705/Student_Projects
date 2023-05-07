//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class ResultsManager : MonoBehaviour
	{
		[Header("ResultScreens")]
		[SerializeField] private GameObject roundResult;
		[SerializeField] private GameObject tournamentResult;
		[SerializeField] private GameObject tournamentWinner;

		[Header("Text UI")]
		[SerializeField] private Text winnerTeamText;
		[SerializeField] private List<Text> teamNames;
		[SerializeField] private List<Text> scoreText;
		[SerializeField] private List<Text> killText;
		[SerializeField] private List<Text> damageText;
		[SerializeField] private List<GameObject> playerBoxes;

		[Header("Other")]
		[SerializeField] private Text playAgainButton;
		[SerializeField] private List<GameObject> caravans;

		private List<LobbyPlayer> lobbyPlayers;
		private bool countdownRunning = false;

		void Awake()
		{
			roundResult.SetActive(true);
			tournamentResult.SetActive(false);

			lobbyPlayers = Lobby.Players;
			SetupUI();
			SetupResults();
			SetButtonText();
		}

		private void SetupUI()
		{
			for (int i = 0; i < lobbyPlayers.Count; i++)
			{
				playerBoxes[i].SetActive(true);
				caravans[i].SetActive(true);
			}
		}

		private void SetButtonText()
		{
			if (Tournament.currentRound == Tournament.totalRounds)
			{
				playAgainButton.text = "Play Again";
			} else
			{
				playAgainButton.text = "Next Round";
			}
		}

		private void SetupResults()
		{
			GameMode.RankingMode rankingMode = GameModes.Instance.CurrentGameMode.rankingMode;

			// TODO: Implement other Rankings than Survival

			switch (rankingMode)
			{
				case GameMode.RankingMode.Score:
					SetupScoreRanking();
					break;
				case GameMode.RankingMode.Kills:
					break;
				case GameMode.RankingMode.Survival:
					SetupSurvivalRanking();
					break;
				case GameMode.RankingMode.Damage:
					break;
				case GameMode.RankingMode.Goals:
					SetupSoccerRanking();
					break;
			}

			countdownRunning = true;
		}

		private void SetupSoccerRanking()
		{
			Dictionary<int, int> playerPlaces = StatsManager.GetPlayersGoals();

			var sortedDict = from entry in playerPlaces orderby entry.Value ascending select entry;
			int index = 0;

			RoundStats roundStats = new RoundStats();
			roundStats.SaveStatsFromStatsManager();
			Tournament.SaveStats(roundStats);

			foreach (KeyValuePair<int, int> keyValuePair in sortedDict)
			{
				var currentTeam = Teams.CurrentGroup?.GetNameAt(Lobby.GetPlayer(keyValuePair.Key).TeamId);

				if (index == 0)
				{
					winnerTeamText.text = Teams.HasGroups ? $"{currentTeam} Won!" : $"Player {keyValuePair.Key + 1} Won!";
				}

				int playerId = keyValuePair.Key;

				// setup UI text
				teamNames[index].text = Teams.HasGroups ? currentTeam : $"Player {keyValuePair.Key + 1}";
				scoreText[index].text = StatsManager.GetGoals(playerId).ToString();
				killText[index].text = "";
				damageText[index].text = "";

				// setup caravan
				caravans[index].GetComponent<DecorationChanger>().ChangeDecoration(Lobby.GetPlayer(playerId).Decoration);
				caravans[index].GetComponentInChildren<MeshChanger>().ChangeModel(Lobby.GetPlayer(playerId).caravanModel);
				caravans[index].GetComponentInChildren<ChangeBodyColor>().ApplyColor(Lobby.GetPlayer(playerId).Color);
				caravans[index].GetComponent<RotateAround>().enabled = false;
				index++;
			}
		}

		private void SetupScoreRanking()
		{
			// TODO
		}

		private void SetupSurvivalRanking()
		{
			Dictionary<int, int> playerPlaces = StatsManager.GetPlayerSurvivalPlaces();

			var sortedDict = from entry in playerPlaces orderby entry.Value ascending select entry;
			int index = 0;

			RoundStats roundStats = new RoundStats();
			roundStats.SaveStatsFromStatsManager();
			Tournament.SaveStats(roundStats);

			foreach (KeyValuePair<int, int> keyValuePair in sortedDict)
			{
				var currentTeam = Teams.CurrentGroup?.GetNameAt(Lobby.GetPlayer(keyValuePair.Key).TeamId);

				if (index == 0)
				{
					winnerTeamText.text = Teams.HasGroups ? $"{currentTeam} Won!" : $"Player {keyValuePair.Key+1} Won!";
				}

				int playerId = keyValuePair.Key;

				// setup UI text
				teamNames[index].text = Teams.HasGroups ? currentTeam : $"Player {keyValuePair.Key+1}";
				scoreText[index].text = StatsManager.GetScore(playerId).ToString();
				killText[index].text = StatsManager.GetKills(playerId).ToString();
				damageText[index].text = Math.Round(StatsManager.GetDamage(playerId)).ToString();

				// setup caravan
				caravans[index].GetComponent<DecorationChanger>().ChangeDecoration(Lobby.GetPlayer(playerId).Decoration);
				caravans[index].GetComponentInChildren<MeshChanger>().ChangeModel(Lobby.GetPlayer(playerId).caravanModel);
				caravans[index].GetComponentInChildren<ChangeBodyColor>().ApplyColor(Lobby.GetPlayer(playerId).Color);
				caravans[index].GetComponent<RotateAround>().enabled = false;
				index++;
			}
		}
	}
}


