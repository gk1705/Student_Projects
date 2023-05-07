//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Manages the soccer gamemode
	/// </summary>
	public class Soccer : MonoBehaviour
	{
		[SerializeField] private float gameLength = 120f;
		[SerializeField] private GameObject countdown;
		[SerializeField] private Text countdownText;
		private GameManager gameManager;
		private float timeRemaining;
		private bool gameEnded = false;
		private float timerInterval = 0.1f;
		private WaitForSecondsRealtime timerWait;
		private void Start()
		{
			//add ball to camera targetgroup
			FindObjectOfType<CinemachineTargetGroup>().AddMember(FindObjectOfType<SoccerBall>().transform, 2, 2);
			gameManager = FindObjectOfType<GameManager>();
			timeRemaining = gameLength;
			gameEnded = false;
			gameManager.OnStartGame += StartCountdown;
			countdownText.text = $"{gameLength:F1}";
			timerWait = new WaitForSecondsRealtime(timerInterval);
		}

		private void StartCountdown()
		{
			StartCoroutine(Countdown());
		}

		IEnumerator Countdown()
		{
			while (timeRemaining >= 0)
			{
				timeRemaining -= timerInterval;
				countdownText.text = $"{timeRemaining:F1}";
				yield return timerWait;
			}
			EndGame();
			
		}

		private void EndGame()
		{
			Debug.Log($"game ended");
			gameEnded = true;
			FindObjectOfType<SoccerBall>().gameObject.SetActive(false);

			var winners = GetWinners();
			bool tie = winners.Count > 1;

			Player winner = tie ? null : winners[0]; //if we have one winner assign him, otherwise null
			gameManager.SoccerGameOver(winner, tie);
		}


		private List<Player> GetWinners()
		{
			var players = FindObjectsOfType<Player>();
			int mostGoals = -1;
			List<Player> bestPlayers = new List<Player>();
			foreach (var player in players) //find players with the highest score
			{
				var tracker = player.GetComponent<StatsTracker>();
				var goals = tracker.goals;

				if (goals > mostGoals)
				{
					bestPlayers.Clear();
					bestPlayers.Add(player);
					mostGoals = goals;
				}
				else if (goals == mostGoals)
				{
					bestPlayers.Add(player);
				}
			}

			Debug.Log($"found {bestPlayers.Count} winners");
			return bestPlayers;
		}
	}
}


