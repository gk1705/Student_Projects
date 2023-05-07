//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class TournamentResults : MonoBehaviour
	{
		[Header("Text")]
		[SerializeField] private List<Text> round1Text;
		[SerializeField] private List<Text> round2Text;
		[SerializeField] private List<Text> round3Text;
		[SerializeField] private List<Text> round4Text;
		[SerializeField] private List<Text> round5Text;

		[Header("Circles")]
		[SerializeField] private List<Image> round1Image;
		[SerializeField] private List<Image> round2Image;
		[SerializeField] private List<Image> round3Image;
		[SerializeField] private List<Image> round4Image;
		[SerializeField] private List<Image> round5Image;
		[SerializeField] private List<Sprite> placementImages;

		[Header("Gameobjects")]
		[SerializeField] private List<GameObject> roundObjectsP1;
		[SerializeField] private List<GameObject> roundObjectsP2;
		[SerializeField] private List<GameObject> roundObjectsP3;
		[SerializeField] private List<GameObject> roundObjectsP4;

		[Header("Other")]
		[SerializeField] private List<Text> teamNames;
		[SerializeField] private List<Image> teamColors;
		[SerializeField] private List<Text> teamPoints;
		[SerializeField] private List<GameObject> playerResults;

		private List<List<Text>> roundListText;
		private List<List<Image>> roundListImage;
		private List<List<GameObject>> roundObjectList;
		private Dictionary<int, int> playerMapping = new Dictionary<int, int>();
		private readonly int[] placementScores = new[] { 200, 150, 100, 50 };

		void Start()
		{
			Setup();
			SetupUI();
			SetupTeamNames();
			SetupTeamColors();
			SetupPlacementsAndPoints();
		}

		public void Setup()
		{
			// create mapping for players
			for (int i = 0; i < Lobby.Players.Count; i++)
			{
				playerMapping[Lobby.Players[i].Id] = i;
			}

			// setup lists
			roundListText = new List<List<Text>>();
			roundListText.Add(round1Text);
			roundListText.Add(round2Text);
			roundListText.Add(round3Text);
			roundListText.Add(round4Text);
			roundListText.Add(round5Text);

			roundListImage = new List<List<Image>>();
			roundListImage.Add(round1Image);
			roundListImage.Add(round2Image);
			roundListImage.Add(round3Image);
			roundListImage.Add(round4Image);
			roundListImage.Add(round5Image);

			roundObjectList = new List<List<GameObject>>();
			roundObjectList.Add(roundObjectsP1);
			roundObjectList.Add(roundObjectsP2);
			roundObjectList.Add(roundObjectsP3);
			roundObjectList.Add(roundObjectsP4);
		}

		public void SetupUI()
		{
			// deactivate player UI
			foreach (GameObject playerResult in playerResults)
			{
				playerResult.SetActive(false);
			}

			// reactivate UI for players in lobby
			for (int i = 0; i < Lobby.Players.Count; i++)
			{
				playerResults[i].SetActive(true);
			}

			// deactivate all placement boxes
			foreach (List<GameObject> gameObjects in roundObjectList)
			{
				foreach (GameObject o in gameObjects)
				{
					o.SetActive(false);
				}
			}

			// reactivate all current boxes
			foreach (List<GameObject> gameObjects in roundObjectList)
			{
				for (int i = 0; i < Tournament.currentRound; i++)
				{
					gameObjects[i].SetActive(true);
				}
			}
		}

		public void SetupTeamNames()
		{
			foreach (KeyValuePair<int, int> keyValuePair in playerMapping)
			{
				int playerId = keyValuePair.Key;
				var currentTeam = Teams.CurrentGroup?.GetNameAt(Lobby.GetPlayer(playerId).TeamId);
				int playerMappingIndex = playerMapping[playerId];
				teamNames[playerMappingIndex].text = Teams.HasGroups ? currentTeam : $"Player {playerId + 1}";
			}
		}

		public void SetupTeamColors()
		{
			foreach (KeyValuePair<int, int> keyValuePair in playerMapping)
			{
				int playerId = keyValuePair.Key;
				Color teamColor = Lobby.GetPlayer(playerId).Color;
				int playerMappingIndex = playerMapping[playerId];
				teamColors[playerMappingIndex].color = teamColor;
			}
		}

		public void SetupPlacementsAndPoints()
		{
			Dictionary<int, int > playerPoints = new Dictionary<int, int>();

			for (int i = 0; i < Lobby.Players.Count; i++)
			{
				playerPoints[Lobby.Players[i].Id] = 0;
			}

			for (int i = 0; i < Tournament.currentRound; i++)
			{
				foreach (KeyValuePair<int, int> keyValuePair in playerMapping)
				{
					// get stats and playerId
					int playerId = keyValuePair.Key;
					RoundStats roundStats = Tournament.GetStats(i);
					int playerMappingIndex = playerMapping[playerId];

					int playerPlacement = roundStats.playerSurvival[playerId];
					
					roundListText[i][playerMappingIndex].text = "#" + playerPlacement;
					roundListImage[i][playerMappingIndex].sprite = placementImages[playerPlacement-1];
					int placementScore = placementScores[roundStats.playerSurvival[playerId] - 1];
					playerPoints[playerId] = playerPoints[playerId] + placementScore;
					teamPoints[playerMappingIndex].text = playerPoints[playerId] + " Points";
				}
			}
		}
	}
}

