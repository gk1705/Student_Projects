//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	[RequireComponent(typeof(GameManagerDeathRace))]
	public class TrackManager : MonoBehaviour
	{
		[SerializeField] private GameManagerDeathRace gameManager;
		[SerializeField] private List<GameObject> checkPoints;


		private List<GameObject> scenePlayers;
		private int[] playerCheckPointIndices = new int[4];
		private int[] playerRoundCount = new int[4];

		void Start()
		{
			if (!gameManager)
			{
				Debug.Log("No GameManager in the Scene");
				return;
			}

			scenePlayers = gameManager.GetScenePlayers();

			foreach (GameObject player in scenePlayers)
			{
				Player playerClass = player.GetComponent<Player>();
				playerCheckPointIndices[playerClass.GetID] = -1;
				playerRoundCount[playerClass.GetID] = 0;
			}
		}

		public int[] GetPlayerRoundCount()
		{
			return playerRoundCount;
		}

		public void UpdatePlayerCheckPointIdx(int checkPointIdx, int playerID, bool isFinal)
		{
			int currentIdx = playerCheckPointIndices[playerID];

			// check for finish line condition
			if (isFinal && currentIdx == checkPoints.Count - 1){
				playerCheckPointIndices[playerID] = checkPointIdx;
				AddRound(playerID);
			}

			// general checkpoint check
			if (checkPointIdx == currentIdx + 1)
			{
				playerCheckPointIndices[playerID] = checkPointIdx;
			}
		}

		private void AddRound(int playerID)
		{
			playerRoundCount[playerID]++;
			GameObject player = gameManager.GetPlayerById(playerID);
			player.GetComponent<StatsTracker>().AddScoreForRound();
			player.GetComponent<StatsTracker>().AddRound();
		}
	}
}

