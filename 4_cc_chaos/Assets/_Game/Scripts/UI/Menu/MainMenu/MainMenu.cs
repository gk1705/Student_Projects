//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaravanCrashChaos
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private string lobbyLevel;

		private void Start()
		{
			GameModes.Instance.CurrentGameMode = null;
		}

		public void LoadLobby()
		{
			SceneManager.LoadScene(lobbyLevel, LoadSceneMode.Single);
		}

		public void StartSingleGame()
		{
			Tournament.SetupTournament(1);
			LoadLobby();
		}

		public void StartBestOfThree()
		{
			Tournament.SetupTournament(3);
			LoadLobby();
		}

		public void StartBestOfFive()
		{
			Tournament.SetupTournament(5);
			LoadLobby();
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}


