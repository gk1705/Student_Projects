//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class PlayerResults : MonoBehaviour
	{
		[SerializeField] private Text damageText;
		[SerializeField] private Text scoreText;
		[SerializeField] private Text playerName;

		public void ShowStats(Player player)
		{
			var endscreen = FindObjectOfType<Endscreen>();

			if (endscreen.showDamage)
			{
				damageText.text = $"Damage: {player.GetComponent<DamageTracker>().GetTotalDamage():F0}";
				damageText.enabled = true;
			}

			if (endscreen.showScore)
			{
				scoreText.text = $"Score: {player.GetComponent<StatsTracker>().currentScore:F0}";
				scoreText.enabled = true;
			}

			playerName.text = $"Player{player.GetID}";
			playerName.color = player.playerColor;
		}
	}
}


