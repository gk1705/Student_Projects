//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections.Generic;
using System.Linq;

namespace CaravanCrashChaos
{
	public class RoundStats
	{
		public Dictionary<int, int> playerScores = new Dictionary<int, int>();
		public Dictionary<int, int> playerKills = new Dictionary<int, int>();
		public Dictionary<int, int> playerRounds = new Dictionary<int, int>();
		public Dictionary<int, int> playerSurvival = new Dictionary<int, int>();
		public Dictionary<int, float> playerDamages = new Dictionary<int, float>();
		public Dictionary<int, bool> playerHasDied = new Dictionary<int, bool>();

		public void SaveStatsFromStatsManager()
		{
			// copy values, otherwise reference would be used (would overwrite everything)
			playerScores = StatsManager.GetPlayerScores().ToDictionary(entry => entry.Key, entry => entry.Value);
			playerKills = StatsManager.GetPlayerKills().ToDictionary(entry => entry.Key, entry => entry.Value);
			playerRounds = StatsManager.GetPlayerRounds().ToDictionary(entry => entry.Key, entry => entry.Value);
			playerSurvival = StatsManager.GetPlayerSurvivalPlaces().ToDictionary(entry => entry.Key, entry => entry.Value);
			playerDamages = StatsManager.GetPlayerDamages().ToDictionary(entry => entry.Key, entry => entry.Value);
			playerHasDied = StatsManager.GetPlayerHasDied().ToDictionary(entry => entry.Key, entry => entry.Value);
		}
	}
}


