//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Checks for inputs, adds players to the fields and starts the game
	/// </summary>
	public class LobbyHandler : MonoBehaviour
	{
		[SerializeField] private string menuScene = "MainMenu";
		[Space(10)]
		[SerializeField] [Required] private SelectionField[] selectionFields;
		[SerializeField] [Required] private Camera[] cameras;
		[SerializeField] [Required] private PlayerColors playerColors;
		[SerializeField] private int minPlayers;

		[SerializeField] [Required] private PlayerCustomizationController playerCustomizationController;
		[SerializeField] private Text minPlayerWarningText;

		[SerializeField] private GameObject countdownUI;

		public delegate void SetReady(int index);
		public event SetReady OnSetReady;
		public delegate void SetNotReady(int index);
		public event SetNotReady OnSetNotReady;

		private List<Rewired.Player> rewiredPlayers;
		private bool countdownRunning = false;

		void Awake()
		{
			Tournament.SetupTournament(Tournament.totalRounds);
		}

		void Start()
		{
			Lobby.Players.Clear(); //clear all player if we load back in this scene
			rewiredPlayers = ReInput.players.GetPlayers().ToList();

			foreach (var sf in selectionFields)
			{
				sf.PCustomizationController = playerCustomizationController;
			}

			countdownUI.GetComponent<Countdown>().OnTimerTrigger += StartGame;

			SceneManager.LoadScene("Sandbox", LoadSceneMode.Additive);
		}

		void Update()
		{
			CheckInput();
		}

		/// <summary>
		/// Checks if a player pressed the join button and initializes this player field.
		/// Also checks for the start button and calls StartGame if enough players are ready.
		/// </summary>
		private void CheckInput()
		{
			for (int i = 0; i < rewiredPlayers.Count; i++)
			{
				HandleUISubmit(i);
				HandleUICancel(i);
				HandleUIStart(i);
				if(GameModes.Instance.CurrentGameMode.AllowBots)
					HandleBots(i);

				if (!selectionFields[i].ready && selectionFields[i].HasId)
				{
					CheckCustomizationInput(i);				
				}

				if (countdownRunning)
				{
					if (!AllPlayersReady())
					{
						countdownRunning = false;
						countdownUI.gameObject.SetActive(false);
						countdownUI.GetComponent<Countdown>().ResetTimer();
					}
				}
			}
		}

		private void ReadyPlayer(int i)
		{
			if (!selectionFields[i].ready)
			{
				selectionFields[i].Ready(Lobby.GetPlayer(i).Color);
				OnSetReady?.Invoke(i);
				cameras[i].gameObject.SetActive(false);
			}
		}

		private void UnReadyPlayer(int i)
		{
			if (selectionFields[i].ready)
			{
				selectionFields[i].UnReady();
				OnSetNotReady?.Invoke(i);
				cameras[i].gameObject.SetActive(true);
			}
		}

		private void LeaveBotAndJoin(int i)
		{
			UnReadyPlayer(i);
			selectionFields[i].Leave();
			playerCustomizationController.SetPlayerDeactivated(i);
			selectionFields[i].JoinPlayer(i, playerColors.GetColor(i));
			SetAndActivateFirstDeco(i);
		}

		private void HandleUISubmit(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("UISubmit") && !selectionFields[i].HasId) // join player
			{
				selectionFields[i].JoinPlayer(i, playerColors.GetColor(i));

				SetAndActivateFirstDeco(i);
			}
			else if (rewiredPlayers[i].GetButtonDown("UISubmit") && selectionFields[i].HasId) // set player ready
			{
				if (selectionFields[i].IsBot) //if bot has field then override it with real player
					LeaveBotAndJoin(i);
				else
					ReadyPlayer(i);
			}							
		}

		private void HandleUICancel(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("UICancel") && CanGoBackToMenu()) // go back to MainMenu
			{
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
			}
			else if (rewiredPlayers[i].GetButtonDown("UICancel") && !selectionFields[i].ready && selectionFields[i].HasId) // leave player
			{
				selectionFields[i].Leave();
				playerCustomizationController.SetPlayerDeactivated(i);
			}
			else if (rewiredPlayers[i].GetButtonDown("UICancel") && selectionFields[i].HasId) // set player unready
			{
				if(selectionFields[i].IsBot)
					LeaveBotAndJoin(i);
				else
					UnReadyPlayer(i);
			}
		}

		/// <summary>
		/// checks if all fields are clear so we can go back to the menu
		/// </summary>
		/// <returns></returns>
		private bool CanGoBackToMenu()
		{
			for (int i = 0; i < selectionFields.Length; i++)
			{
				if (selectionFields[i].ready || selectionFields[i].HasId)
					return false;
			}
			return true;
		}

		private void HandleUIStart(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("UIStart")) // press start
			{
				if (Lobby.Players.Count >= minPlayers)
				{
					if (AllPlayersReady())
					{
						countdownRunning = true;
						countdownUI.gameObject.SetActive(true);
						countdownUI.GetComponent<Countdown>().StartTimer();
					}
					else
					{
						// TODO: implement UI message
						Debug.Log("not all players are ready");
					}
				}
				else
				{
					// TODO: implement UI message
					Debug.Log("not enough players");
				}
			}
		}

		private void HandleBots(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("AddBot")) 
			{
				Debug.Log($"add bot pressed");
				for (int j = 0; j < selectionFields.Length; j++)
				{
					if (!selectionFields[j].HasId)
					{
						selectionFields[j].JoinPlayer(j, playerColors.GetColor(j), true);
						SetAndActivateFirstDeco(j);
						ReadyPlayer(j);
						break;
					}						
				}
			}

			if (rewiredPlayers[i].GetButtonDown("RemoveBot"))
			{
				for (int j = selectionFields.Length - 1; j >= 0; j--)
				{
					if (selectionFields[j].IsBot)
					{
						Debug.Log($"leaving field {j}");
						UnReadyPlayer(j);
						selectionFields[j].Leave();
						playerCustomizationController.SetPlayerDeactivated(j);
						break;
					}
				}
			}
		}
		
		private void CheckCustomizationInput(int i)
		{
			CheckColorIteration(i);
			CheckDecorationIteration(i);
			CheckModelIteration(i);
			if(Teams.HasGroups)
				CheckTeamNameIteration(i);
		}

		private void CheckTeamNameIteration(int i)
		{
			
			if (rewiredPlayers[i].GetButtonDown("IterateTeamNameUp"))
			{
				int teamId = Lobby.GetPlayer(i).TeamId;
				int teamCount = Teams.CurrentGroup.TeamNames.Count;
				if (teamId > teamCount - 1) teamId = teamCount;
				Lobby.GetPlayer(i).TeamId = (teamId + 1) % teamCount;

				selectionFields[i].SetTeamName(Lobby.GetPlayer(i).TeamId);
			}
			else if (rewiredPlayers[i].GetButtonDown("IterateTeamNameDown"))
			{
				int teamId = Lobby.GetPlayer(i).TeamId;
				int teamCount = Teams.CurrentGroup.TeamNames.Count;
				if (teamId > teamCount - 1) teamId = teamCount;
				Lobby.GetPlayer(i).TeamId = (teamId - 1) % teamCount;

				selectionFields[i].SetTeamName(Lobby.GetPlayer(i).TeamId);
			}
		}


		private void CheckColorIteration(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("IterateColorUp"))
			{
				playerCustomizationController.IterateColorUp(i);
			}
			else if (rewiredPlayers[i].GetButtonDown("IterateColorDown"))
			{
				playerCustomizationController.IterateColorDown(i);
			}
		}

		private void CheckDecorationIteration(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("IterateDecorationUp"))
			{
				playerCustomizationController.IterateDecorationUp(i);
			}
			else if (rewiredPlayers[i].GetButtonDown("IterateDecorationDown"))
			{
				playerCustomizationController.IterateDecorationDown(i);
			}
		}

		private void CheckModelIteration(int i)
		{
			if (rewiredPlayers[i].GetButtonDown("IterateModelUp"))
			{
				playerCustomizationController.IterateModelUp(i);
			}
			else if (rewiredPlayers[i].GetButtonDown("IterateModelDown"))
			{
				playerCustomizationController.IterateModelDown(i);
			}
		}

		private void SetAndActivateFirstDeco(int index)
		{
			// set first decoration
			if (!playerCustomizationController.IsPlayerActivated(index))
			{
				playerCustomizationController.IterateColorUp(index);
				playerCustomizationController.IterateDecorationUp(index);
				playerCustomizationController.IterateModelUp(index);

				playerCustomizationController.SetPlayerActivated(index);
			}

			playerCustomizationController.TogglePlayerModel(index);
		}

		private void ShowMinPlayerWarning()
		{
			minPlayerWarningText.text = $"You need at least {minPlayers} players to start!";
			minPlayerWarningText.enabled = true;
		}

		private bool AllPlayersReady()
		{
			foreach (LobbyPlayer lobbyPlayer in Lobby.Players)
			{
				if (!selectionFields[lobbyPlayer.Id].ready)
				{
					return false;
				}
			}

			return true;
		}

		private void StartGame()
		{
			Tournament.AddRound();
			SceneManager.LoadScene(GameModes.Instance.CurrentGameMode.SceneToLoad);
		}

		private void LoadMenu()
		{
			SceneManager.LoadScene(menuScene);
		}
	}
}


