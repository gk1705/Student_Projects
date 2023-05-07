//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RectTransform = UnityEngine.RectTransform;

namespace CaravanCrashChaos
{

	public class PlayerField : MonoBehaviour
	{
		[SerializeField] private Text scoreText;
		[SerializeField] private Text roundsText;
		[SerializeField] private Text goalsText;
		[SerializeField] private Image healthBar;
		[SerializeField] private Image playerItem;
		[SerializeField] private Image goalsBackground;
		[SerializeField] private Text teamName;
		[SerializeField] private GameObject teamNameBackground;
		[SerializeField] private UIColorFlasher uiColorFlasher;
		[Tooltip("Bar will flash under this %")]
		[SerializeField] private float healthThresholdPercentage = 0;

		private float health = 0;
		private int score = 0;
		private int rounds = 0;
		private int goals = 0;

		private bool showScore = false;
		private bool showRounds = false;
		private bool showGoals = false;

		void Update()
		{
			if (showScore)
				scoreText.text = score.ToString();
			if (showRounds)
				roundsText.text = rounds.ToString();
			if (showGoals)
				goalsText.text = $"Goals: {goals}";
		}

		public void SetHealth(float amount, int maxHealth)
		{
			health = amount;

			float healthPercent = 100.0f / maxHealth * health;

			float barWidth = 200 / 100.0f * healthPercent;

			if (healthBar != null)
			{
				healthBar.rectTransform.sizeDelta = new Vector2(barWidth, healthBar.rectTransform.sizeDelta.y);
			}

			CheckHealthBarFlash(healthPercent);
		}

		public void SetTeamName(string teamName)
		{
			teamNameBackground.SetActive(true);
			this.teamName.text = teamName;
		}

		public void SetScore(int amount)
		{
			score = amount;
		}

		public void SetRounds(int amount)
		{
			rounds = amount;
		}

		public void SetGoals(int amount)
		{
			goals = amount;
		}

		public void SetScoreDisplay(bool value)
		{
			scoreText.gameObject.SetActive(value);
			showScore = value;
		}

		public void SetRoundsDisplay(bool value)
		{
			scoreText.gameObject.SetActive(value);
			showRounds = value;
		}

		public void SetGoalsDisplay(bool value)
		{
			goalsBackground.gameObject.SetActive(value);
			showGoals = value;
		}

		void CheckHealthBarFlash(float healthPercentage)
		{
			if (healthPercentage < healthThresholdPercentage && !uiColorFlasher.IsFlashing && healthPercentage > 0.01f) //only start when not already flashing
			{
				uiColorFlasher.SetFlashing(true);
			}
			else if(uiColorFlasher.IsFlashing && healthPercentage >= healthThresholdPercentage)
			{
				uiColorFlasher.SetFlashing(false);
			}
		}

		public void StartFlashing(bool value)
		{
			uiColorFlasher.SetFlashingOnHit(value);
		}

		public void SetColor(Color color)
		{
			playerItem.color = color;
			healthBar.color = color;
		}

		public void SetImage(Sprite sprite)
		{
			if (sprite == null)
				playerItem.enabled = false;
			else
			{
				playerItem.sprite = sprite;
				playerItem.enabled = true;
			}
			
		}

		public void DisableHealthbar()
		{
			healthBar.transform.parent.gameObject.SetActive(false);
		}
	}

}
