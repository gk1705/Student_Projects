//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CaravanCrashChaos
{
	public static class Tournament
	{
		public static int currentRound { get; private set; }
		public static int totalRounds { get; private set; }

		public static List<RoundStats> roundStats = new List<RoundStats>();
		private static readonly int[] placementScores = new[] { 200, 150, 100, 50 };

		public static void SetupTournament(int rounds)
		{
			currentRound = 0;
			totalRounds = rounds;

			roundStats.Clear();
		}

		public static void SaveStats(RoundStats stats)
		{
			roundStats.Add(stats);
		}

		public static RoundStats GetStats(int round)
		{
			return roundStats[round];
		}

		public static void AddRound()
		{
			currentRound += 1;
		}

		public static int GetTournamentWinnerId()
		{
			Dictionary<int, int> playerPoints = new Dictionary<int, int>();

			// initialize player points
			foreach (LobbyPlayer lobbyPlayer in Lobby.Players)
			{
				playerPoints[lobbyPlayer.Id] = 0;
			}

			// calculate stats for all players
			foreach (RoundStats roundStat in roundStats)
			{
				foreach (KeyValuePair<int, int> keyValuePair in roundStat.playerSurvival)
				{
					int placementScore = placementScores[roundStat.playerSurvival[keyValuePair.Key] - 1];
					playerPoints[keyValuePair.Key] += placementScore;
				}
			}


			var sortedDict = from entry in playerPoints orderby entry.Value descending select entry;

			var lastValue = 0;
			bool duplicate = false;
			int count = 0;
			// check for duplicates
			foreach (KeyValuePair<int, int> keyValuePair in sortedDict)
			{
				if (lastValue == keyValuePair.Value && count < 2)
				{
					duplicate = true;
					return -1;
				}

				lastValue = keyValuePair.Value;
				count++;
			}

			// return best player
			foreach (KeyValuePair<int, int> keyValuePair in sortedDict)
			{
				return keyValuePair.Key;
			}

			return 0;
		}

		public static int GetTotalKills(int playerId)
		{
			int killSum = 0;
			foreach (RoundStats roundStat in roundStats)
			{
				killSum += roundStat.playerKills[playerId];
			}

			return killSum;
		}

		public static float GetTotalDamage(int playerId)
		{
			float damageSum = 0;
			foreach (RoundStats roundStat in roundStats)
			{
				damageSum += roundStat.playerDamages[playerId];
			}

			return damageSum;
		}

		public static float GetTotalPoints(int playerId)
		{
			int totalPoints = 0;
			foreach (RoundStats roundStat in roundStats)
			{
				int placementScore = placementScores[roundStat.playerSurvival[playerId] - 1];
				totalPoints += placementScore;
			}

			return totalPoints;
		}
	}
}
