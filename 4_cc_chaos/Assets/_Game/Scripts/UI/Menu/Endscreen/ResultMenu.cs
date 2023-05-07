//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CaravanCrashChaos
{
	public class ResultMenu : MonoBehaviour
	{
		[SerializeField] private float lockedCooldown;

		private bool locked = false;
		private bool cooldownRunning = false;
		private float cooldown = 0f;

		public void Start()
		{
			LockMenu();
			UnlockAfterSeconds(lockedCooldown);
		}

		public void Update()
		{
			if (cooldownRunning)
			{
				cooldown -= Time.deltaTime;

				if (cooldown <= 0)
				{
					locked = false;
					cooldownRunning = false;
				}
			}
		}

		public void LoadLobby()
		{
			if (!locked)
				SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
		}

		public void Restart()
		{
			if (!locked)
			{
				// check if Tournament is finished
				if (Tournament.currentRound == Tournament.totalRounds)
				{
					// restart tournament
					Tournament.SetupTournament(Tournament.totalRounds);
					Tournament.AddRound();
					SceneManager.LoadScene(GameModes.Instance.CurrentGameMode.SceneToLoad);
				}
				else
				{
					// start next round of the tournament
					Tournament.AddRound();
					SceneManager.LoadScene(GameModes.Instance.CurrentGameMode.SceneToLoad);
				}
			}
				
		}

		public void LoadMainMenu()
		{
			if (!locked)
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
		}

		public void UnlockAfterSeconds(float amount)
		{
			cooldown = amount;
			cooldownRunning = true;
		}

		public void LockMenu()
		{
			locked = true;
		}
	}

	
}

