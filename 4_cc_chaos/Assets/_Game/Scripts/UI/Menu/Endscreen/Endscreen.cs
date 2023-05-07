//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class Endscreen : MonoBehaviour
	{
		[SerializeField] private Image playerWonBackdrop;
		[SerializeField] private Text playerWonText;
		[SerializeField] private PlayerResults[] playerResults;
		[SerializeField] private Button restartButton;
		[SerializeField] public bool showDamage;
		[SerializeField] public bool showScore;
		[Space(10)] [SerializeField] private string lobbyScene = "Lobby";

		public void ShowEndScreen(Player winner)
		{
			this.GetComponent<Canvas>().enabled = true;
			ShowPlayerResults();
			FindObjectOfType<EventSystem>().SetSelectedGameObject(restartButton.gameObject);
			ShowWinText(winner);
		}

		private void ShowWinText(Player winner)
		{
			playerWonText.text = winner != null ? $"Player {winner.GetID+1} won!" : "Tie!";
			if (winner != null)
				playerWonBackdrop.color = winner.playerColor;
		}

		private void ShowPlayerResults()
		{
			foreach (var playerResult in playerResults)
			{
				playerResult.gameObject.SetActive(false);
			}

			foreach (var player in FindObjectsOfType<Player>())
			{
				playerResults[player.GetID].gameObject.SetActive(true);
				playerResults[player.GetID].ShowStats(player);
			}
		}

		public void HideEndScreen()
		{
			this.GetComponent<Canvas>().enabled = false;
		}

		public void Lobby()
		{
			SceneManager.LoadScene(lobbyScene);
		}

		public void Restart()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}


