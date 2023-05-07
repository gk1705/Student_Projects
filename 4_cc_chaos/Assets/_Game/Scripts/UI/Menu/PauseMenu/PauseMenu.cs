//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace CaravanCrashChaos
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField] private string lobbyScene = "Lobby";
		[SerializeField] private string mainMenuScene = "MainMenu";
		[SerializeField] private GameObject resumeButton;
		public bool IsPaused { get; private set; }
		private Canvas canvas;
		private GameManager gameManager;

		// Start is called before the first frame update
		void Start()
		{
			canvas = this.GetComponent<Canvas>();
			canvas.enabled = false;
			IsPaused = false;
			gameManager = FindObjectOfType<GameManager>();
		}

		// Update is called once per frame
		void Update()
		{
			if (gameManager && gameManager.IsGameOver) return;

			foreach (var player in ReInput.players.GetPlayers())
			{
				if(player.GetButtonDown("UIStart") && !IsPaused)
					EnterPauseMenu();

				if (player.GetButtonDown("UICancel") && IsPaused)
					ClosePauseMenu();
			}
		}

		public void EnterPauseMenu()
		{
			IsPaused = true;
			canvas.enabled = true;
			FindObjectOfType<EventSystem>().SetSelectedGameObject(resumeButton);
			Time.timeScale = 0;			
		}

		public void ClosePauseMenu()
		{
			IsPaused = false;
			canvas.enabled = false;
			FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
			Time.timeScale = 1;
		}


		public void Lobby()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(lobbyScene);
		}

		public void MainMenu()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(mainMenuScene);
		}
	}
}


