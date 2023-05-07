//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class GameUIHandler : MonoBehaviour
	{
		[SerializeField] private PlayerField[] playerFields;
		[SerializeField] private bool hasScore = false;
		[SerializeField] private bool hasRounds = false;
		[SerializeField] private bool hasHealthBars = true;
		[SerializeField] private bool hasGoals = false;
		[SerializeField] private Sprite deathImage;

		private List<LobbyPlayer> lobbyPlayers;
		private List<GameObject> spawnedPlayers;
		private List<int> playerIds;

		IEnumerator coroutine;
		bool isFlashingOnHit = false;

		void Start()
		{
			lobbyPlayers = Lobby.Players;

		}

		void Update()
		{
			if (spawnedPlayers == null || spawnedPlayers.Count <= 0) return;

			if(hasHealthBars)
				UpdateHealth();
			if (hasScore)
				UpdateScore();
			if (hasRounds)
				UpdateRounds();
			if (hasGoals)
				UpdateGoals();

			UpdateImageOnPlayerDeath();
			
		}

		
		void UpdateHealth()
		{
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				Health playerHealth = spawnedPlayers[i].GetComponent<Health>();
				playerFields[playerIds[i]].SetHealth(playerHealth.CurrentHealth, playerHealth.MaxHealth);
			}
		}

		void BlinkOnHit(int id)
		{	
			if (!isFlashingOnHit)
			{
				isFlashingOnHit = true;
				playerFields[id].StartFlashing(isFlashingOnHit);
				coroutine = WaitAndSetActive(id);
				StartCoroutine(coroutine);
			}
		}

		private IEnumerator WaitAndSetActive(int id)
		{		
			yield return new WaitForSeconds(0.15f);
			isFlashingOnHit = false;
			playerFields[id].StartFlashing(isFlashingOnHit);
			playerFields[id].SetColor(Lobby.GetPlayer(id).Color);


		}

		//private void SetOnHitFlashing(bool value)
		//{
		//	isOnHitFlashing = value;
		//}

		public void UpdateImageOnPlayerDeath()
		{
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				Health playerHealth = spawnedPlayers[i].GetComponent<Health>();

				if (playerHealth.IsDead)
				{
					playerFields[playerIds[i]].SetImage(deathImage);
				}
			}
		}


		void UpdateScore()
		{
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				StatsTracker statsTracker = spawnedPlayers[i].GetComponent<StatsTracker>();
				playerFields[playerIds[i]].SetScore(statsTracker.currentScore);
			}
		}

		private void UpdateGoals()
		{
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				StatsTracker statsTracker = spawnedPlayers[i].GetComponent<StatsTracker>();
				playerFields[playerIds[i]].SetGoals(statsTracker.goals);
			}
		}


		void UpdateRounds()
		{
			TrackManager trackManager = FindObjectOfType<TrackManager>();
			Assert.IsNotNull(trackManager);

			int[] roundCount = trackManager.GetPlayerRoundCount();

			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				int playerID = spawnedPlayers[i].GetComponent<Player>().GetID;
				playerFields[playerIds[i]].SetRounds(roundCount[playerID]);
			}
		}

		public void SetPlayers(List<GameObject> players, List<int> playerIds)
		{			
			spawnedPlayers = players;
			this.playerIds = playerIds;

			Debug.Log($"spawned players {spawnedPlayers.Count}");


			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				playerFields[playerIds[i]].gameObject.SetActive(true);
				if (hasScore)
					playerFields[playerIds[i]].SetScoreDisplay(true);
				if (hasRounds)
					playerFields[playerIds[i]].SetRoundsDisplay(true);
				if(hasGoals)
					playerFields[playerIds[i]].SetGoalsDisplay(true);
			}

			SetColors();
			SetImages();
			if(Teams.HasGroups)
				SetTeamNames();

			if (!hasHealthBars)
			{
				HideAllHealthbars();
			}

			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				Health playerHealth = spawnedPlayers[i].GetComponent<Health>();
				playerHealth.OnLoseHealth += BlinkOnHit;
			}
		}


		private void SetTeamNames()
		{
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				int playerId = playerIds[i];
				int teamId = Lobby.GetPlayer(playerId).TeamId;
				playerFields[playerId].SetTeamName(Teams.CurrentGroup.GetNameAt(teamId));
			}
		}

		public void SetColors()
		{
			
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				Color playerColor = spawnedPlayers[i].gameObject.GetComponent<Player>().playerColor;
				playerFields[playerIds[i]].SetColor(playerColor);
			}
		}

		public void SetImages()
		{ 
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				Sprite playerImage = spawnedPlayers[i].gameObject.GetComponent<DecorationChanger>().GetDecorationImage();
				playerFields[playerIds[i]].SetImage(playerImage);
			}
		}

		public void HideAllHealthbars()
		{
			for (int i = 0; i < spawnedPlayers.Count; i++)
			{
				playerFields[playerIds[i]].DisableHealthbar();
			}
		}
	}
}
