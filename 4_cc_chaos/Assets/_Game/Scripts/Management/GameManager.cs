using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CaravanController = CaravanCrashChaos.CaravanController;

public class GameManager : MonoBehaviour
{

	[SerializeField] private Text finishedText;
	[SerializeField] private GameObject countdownUI;
	[SerializeField] private float timeUntilEndscreen = 1.5f;

	private List<GameObject> players;
	public bool IsGameOver { get; private set; }

	private Dictionary<int, int> playerPlacement = new Dictionary<int, int>();
	private int playerRanking = 0;

	public delegate void StartedGame();
	public event StartedGame OnStartGame;

	void Start()
	{
		countdownUI.GetComponent<Countdown>().OnTimerTrigger += StartGame;		
	}

	void Update()
	{
		if(GameModes.Instance.CurrentGameMode.GameModeIdentifier != GameModeIdentifier.Soccer)
			CheckWinningCondition();
	}

	public void CheckWinningCondition()
	{

		if (players != null && !IsGameOver)
		{
			int countLiving = GetLivingPlayerCount();

			switch (countLiving)
			{
				case 1:
				{
					GameOver(false);
					break;
				}
				case 0:
				{
					GameOver(true);
					break;
				}
			}
		}
	}

	private void StartGame()
	{
		SetPlayerDeathEvents();
		countdownUI.gameObject.SetActive(false);
		EnablePlayersControls();
		OnStartGame?.Invoke();
	}


	public void SoccerGameOver(Player winner, bool tie)
	{
		IsGameOver = true;
		var lastPlayer = winner;
		if(!tie)
			playerPlacement[lastPlayer.GetID] = 1;

		if (!Teams.HasGroups)
			finishedText.text = tie ? "Tie!" : $"Player {lastPlayer.GetID + 1} Won!";
		else
		{
			int teamId = 0;
			if(!tie) teamId = Lobby.GetPlayer(lastPlayer.GetID).TeamId;
			finishedText.text = tie ? "Tie!" : $"{Teams.CurrentGroup.GetNameAt(teamId)} Won!";
		}

		finishedText.gameObject.SetActive(true);
		if(!tie) lastPlayer.CarCrown.SetActive(true);
		StartCoroutine(nameof(EnableEndscreen));

		LockScores();
		SetStats();

		if (!tie)
			finishedText.color = lastPlayer.playerColor;

		Announcer.Instance.ForceVoiceLine("Win");
	}

	private void GameOver(bool tie)
	{
		IsGameOver = true;
		var lastPlayer = GetLivingPlayer().GetComponent<Player>();
		playerPlacement[lastPlayer.GetID] = 1;

		if (!Teams.HasGroups)
			finishedText.text = tie ? "Tie!" : $"Player {lastPlayer.GetID + 1} Won!";
		else
		{
			int teamId = Lobby.GetPlayer(lastPlayer.GetID).TeamId;
			finishedText.text = tie ? "Tie!" : $"{Teams.CurrentGroup.GetNameAt(teamId)} Won!";
		}

		finishedText.gameObject.SetActive(true);
		lastPlayer.CarCrown.SetActive(true);
		StartCoroutine(nameof(EnableEndscreen));

		LockScores();
		SetStats();

		if (!tie)
			finishedText.color = lastPlayer.playerColor;

		Announcer.Instance.ForceVoiceLine("Win");
	}

	private IEnumerator EnableEndscreen()
	{
		yield return new WaitForSecondsRealtime(timeUntilEndscreen);
		SceneManager.LoadScene("Results");

		//Announcer.Instance.EnqueueVoiceLine("GameEnd", .2f);
	}

	public void SetPlayers(List<GameObject> players)
	{
		this.players = players;
		playerRanking = players.Count;
		countdownUI.GetComponent<Countdown>().StartTimer();
		DisablePlayersControls();
	}

	public void LockScores()
	{
		foreach (GameObject player in players)
		{
			player.gameObject.GetComponent<StatsTracker>().SetLocked(false);
		}
	}

	private void SetPlayerDeathEvents()
	{
		foreach (GameObject player in players)
		{
			player.GetComponent<PlayerDeath>().OnPlayerDies += PlayerDied;
		}
	}

	private void PlayerDied(int playerId)
	{
		playerPlacement[playerId] = playerRanking;
		playerRanking--;
	}

	private void SetStats()
	{
		StatsManager.ResetStats();
		foreach (GameObject player in players)
		{
			int playerId = player.GetComponent<Player>().GetID;

			// manually set 2nd placed player, because Method gets executed before PlayerDied-Event
			if (!playerPlacement.ContainsKey(playerId))
			{
				playerPlacement[playerId] = 2;
			}

			
			StatsTracker statsTracker = player.GetComponent<StatsTracker>();

			StatsManager.SetScore(playerId, statsTracker.currentScore);
			StatsManager.SetKills(playerId, statsTracker.killCount);
			StatsManager.SetRounds(playerId, statsTracker.roundCount);
			StatsManager.SetSurvivalPlace(playerId, playerPlacement[playerId]);
			StatsManager.SetDamage(playerId, statsTracker.damageCount);
			StatsManager.SetHasDied(playerId, statsTracker.hasDied);
			StatsManager.SetGoals(playerId, statsTracker.goals);
		}
	}

	private void DisableLastPlayerControls()
	{
		GameObject lastPlayer = GetLivingPlayer();
		if (lastPlayer != null)
		{
			var carController = lastPlayer.gameObject.GetComponentInChildren<CarController>();
			if (carController != null)
				carController.enabled = false;
			var caravanController = lastPlayer.gameObject.GetComponentInChildren<CaravanController>();
			if(caravanController != null)
				caravanController.enabled = false;
		}
	}

	public void DisablePlayersControls()
	{
		foreach (GameObject player in players)
		{
			var carController = player.gameObject.GetComponentInChildren<CarController>();
			if (carController != null)
				carController.enabled = false;
			var caravanController = player.gameObject.GetComponentInChildren<CaravanController>();
			if (caravanController != null)
				caravanController.enabled = false;

			if (carController.IsAi)
			{
				player.GetComponentInChildren<CarAi>().enabled = false;
				var caravanAi = player.GetComponentInChildren<CaravanAi>();
				if(caravanAi)
					caravanAi.enabled = false;
			}
		}
	}

	public void EnablePlayersControls()
	{
		foreach (GameObject player in players)
		{
			var carController = player.gameObject.GetComponentInChildren<CarController>();
			if (carController != null)
				carController.enabled = true;
			var caravanController = player.gameObject.GetComponentInChildren<CaravanController>();
			if (caravanController != null)
				caravanController.enabled = true;

			if (carController.IsAi)
			{
				player.GetComponentInChildren<CarAi>().enabled = true;
				var caravanAi = player.GetComponentInChildren<CaravanAi>();
				if (caravanAi)
					caravanAi.enabled = true;
			}
				
		}		
	}

	public int GetLivingPlayerCount()
	{
		int countLiving = 0;
		foreach (GameObject player in players)
		{
			if (!player.GetComponent<Health>().IsDead)
			{
				countLiving++;
			}
		}

		return countLiving;
	}

	public GameObject GetLivingPlayer()
	{
		foreach (GameObject player in players)
		{
			if (!player.GetComponent<Health>().IsDead)
			{
				return player;
			}
		}

		return null;
	}
}
