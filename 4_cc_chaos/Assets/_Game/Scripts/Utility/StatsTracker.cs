//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using UnityEngine;

namespace CaravanCrashChaos
{
	public class StatsTracker : MonoBehaviour
	{
		[Header("Score")]
		[SerializeField] private int RoundScore;
		[SerializeField] private int KillScore;
		[SerializeField] private int SurvivalScore;

		public bool locked { get; private set; }
		public int currentScore { get; private set; }
		public int roundCount { get; private set; }
		public int killCount { get; private set; }
		public int survivalScore { get; private set; }
		public float damageCount { get; private set; }
		public bool hasDied { get; private set; }
		public int goals { get; private set; }

		public void ResetAllStats()
		{
			currentScore = 0;
			roundCount = 0;
			killCount = 0;
			survivalScore = 0;
			damageCount = 0;
			hasDied = false;
			goals = 0;
		}

		public void SetLocked(bool value)
		{
			locked = !value;
		}

		public void AddScoreForDmg(float dmg)
		{
			if (!locked)
				currentScore += (int)dmg;
		}

		public void AddScoreForRound()
		{
			if (!locked)
				currentScore += RoundScore;
		}

		public void AddScoreForKill()
		{
			if (!locked)
				currentScore += KillScore;
		}

		public void AddScoreForSurvival()
		{
			if (!locked)
				currentScore += SurvivalScore;
		}

		public void AddRound()
		{
			if (!locked)
				roundCount++;
		}

		public void AddKill()
		{
			if (!locked)
			{
				killCount++;
			}
		}

		public void AddSurvivalScore( int value)
		{
			if (!locked)
				survivalScore += value;
		}

		public void AddDamageScore( float value)
		{
			damageCount += value;
		}

		public void AddGoal()
		{
			if (!locked)
				goals++;
		}

		/// <summary>
		/// Remove goal, clamps to 0
		/// </summary>
		public void RemoveGoal()
		{
			if (!locked && goals > 0)
				goals--;
		}

		public void HasDied()
		{
			hasDied = true;
		}

		public void Print()
		{
			Debug.Log($"Score {currentScore} - Rounds {roundCount} - Kills {killCount} - Survival {survivalScore} - Damage {damageCount}");
		}

	}
}


