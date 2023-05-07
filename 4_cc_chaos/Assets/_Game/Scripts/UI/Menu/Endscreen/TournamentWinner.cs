//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class TournamentWinner : MonoBehaviour
	{
		[SerializeField] private Text teamNameText;
		[SerializeField] private Text pointsText;
		[SerializeField] private Text killsText;
		[SerializeField] private Text damageText;
		[SerializeField] private Image teamColor;
		[SerializeField] private GameObject trophy;

		public void Setup()
		{
			int winnerId = Tournament.GetTournamentWinnerId();

			if (winnerId == -1)
			{
				teamNameText.text = "It's a Draw!";
				killsText.text = "";
				damageText.text = "";
				pointsText.text = "";
				return;
			}

			var currentTeam = Teams.CurrentGroup?.GetNameAt(Lobby.GetPlayer(winnerId).TeamId);

			teamNameText.text = Teams.HasGroups ? currentTeam + " has won the Tournament!" : $"Player {winnerId + 1} has won the Tournament!";
			killsText.text = $"{Tournament.GetTotalKills(winnerId)} Kills";
			damageText.text = $"{Math.Round(Tournament.GetTotalDamage(winnerId))} Damage";
			pointsText.text = $"{Tournament.GetTotalPoints(winnerId)} Points";

			trophy.SetActive(true);

			teamColor.color = Lobby.GetPlayer(winnerId).Color;
		}
	}
}
