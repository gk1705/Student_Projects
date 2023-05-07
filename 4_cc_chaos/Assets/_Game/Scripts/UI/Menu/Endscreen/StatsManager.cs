//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CaravanCrashChaos
{
	public static class StatsManager
	{
		public static Dictionary<int, int> playerScores = new Dictionary<int, int>();
		public static Dictionary<int, int> playerKills = new Dictionary<int, int>();
		public static Dictionary<int, int> playerRounds = new Dictionary<int, int>();
		public static Dictionary<int, int> playerSurvival = new Dictionary<int, int>();
		public static Dictionary<int, float> playerDamages = new Dictionary<int, float>();
		public static Dictionary<int, bool> playerHasDied = new Dictionary<int, bool>();
		public static Dictionary<int, int> playerGoals = new Dictionary<int, int>();

		public static void ResetStats()
		{
			playerScores.Clear();
			playerKills.Clear();
			playerRounds.Clear();
			playerSurvival.Clear();
			playerDamages.Clear();
			playerHasDied.Clear();
			playerGoals.Clear();
		}

		public static void SetScore(int playerId, int score)
		{
			playerScores[playerId] = score;
		}

		public static void SetKills(int playerId, int kills)
		{
			playerKills[playerId] = kills;
		}

		public static void SetRounds(int playerId, int rounds)
		{
			playerRounds[playerId] = rounds;
		}

		public static void SetSurvivalPlace(int playerId, int place)
		{
			playerSurvival[playerId] = place;
		}

		public static void SetDamage(int playerId, float damage)
		{
			playerDamages[playerId] = damage;
		}

		public static void SetHasDied(int playerId, bool value)
		{
			playerHasDied[playerId] = value;
		}

		public static void SetGoals(int playerId, int goals)
		{
			playerGoals[playerId] = goals;
		}

		public static int GetScore(int playerId)
		{
			return playerScores[playerId];
		}

		public static int GetKills(int playerId)
		{
			return playerKills[playerId];
		}

		public static int GetRounds(int playerId)
		{
			return playerRounds[playerId];
		}

		public static int GetSurvivalPlace(int playerId)
		{
			return playerSurvival[playerId];
		}

		public static float GetDamage(int playerId)
		{
			return playerDamages[playerId];
		}

		public static bool GetHasDied(int playerId)
		{
			return playerHasDied[playerId];
		}

		public static int GetGoals(int playerId)
		{
			return playerGoals[playerId];
		}

		public static int GetMaxScore()
		{
			return playerScores.Values.Max();
		}

		public static int GetMaxRounds()
		{
			return playerRounds.Values.Max();
		}

		public static int GetMaxSurvivalScore()
		{
			return playerSurvival.Values.Max();
		}

		public static int GetMaxKills()
		{
			return playerKills.Values.Max();
		}

		public static float GetMaxDamage()
		{
			return playerDamages.Values.Max();
		}

		public static Dictionary<int, int> GetPlayerScores()
		{
			return playerScores;
		}

		public static Dictionary<int, int> GetPlayerKills()
		{
			return playerKills;
		}

		public static Dictionary<int, int> GetPlayerRounds()
		{
			return playerRounds;
		}

		public static Dictionary<int, int> GetPlayerSurvivalPlaces()
		{
			return playerSurvival;
		}

		public static Dictionary<int, float> GetPlayerDamages()
		{
			return playerDamages;
		}

		public static Dictionary<int, bool> GetPlayerHasDied()
		{
			return playerHasDied;
		}

		public static Dictionary<int, int> GetPlayersGoals()
		{
			return playerGoals;
		}
	}
}

