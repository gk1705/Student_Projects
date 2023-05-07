//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class ScreenSwitcher : MonoBehaviour
	{
		[SerializeField] private float timeBetweenSwitch = 5f;
		[SerializeField] private List<GameObject> screens;
		[SerializeField] private List<GameObject> caravans;
		[SerializeField] private TournamentWinner tournamentWinner;
		[SerializeField] private Image progressBar;

		private bool countdownRunning = false;
		private float countdown = 0f;
		private int currentScreenIdx = 0;

		void Start()
		{
			countdown = timeBetweenSwitch;
			if (Tournament.totalRounds != 1)
				ActivateTimer();
			ActivateScreen();
		}

		void Update()
		{
			if (countdownRunning)
			{
				countdown -= Time.deltaTime;

				var percent = 100f / timeBetweenSwitch * countdown;
				progressBar.rectTransform.sizeDelta = new Vector2(percent, progressBar.rectTransform.sizeDelta.y);

				if (countdown <= 0)
				{
					TimerTriggered();
				}
			}
		}

		public void ActivateTimer()
		{
			countdownRunning = true;
		}

		private void TimerTriggered()
		{
			currentScreenIdx++;
			ActivateScreen();
			DeactivateCaravans();

			if (currentScreenIdx == 2)
			{
				tournamentWinner.Setup();
			}

			if (Tournament.currentRound != Tournament.totalRounds)
			{
				countdownRunning = false;
			}
			else if (currentScreenIdx == 1)
			{
				countdown = timeBetweenSwitch;
				progressBar.rectTransform.sizeDelta = new Vector2(100, progressBar.rectTransform.sizeDelta.y);
			}
			else
			{
				countdownRunning = false;
			}
		}

		public void ActivateScreen()
		{
			foreach (GameObject screen in screens)
			{
				screen.SetActive(false);
			}

			screens[currentScreenIdx].SetActive(true);
		}

		private void DeactivateCaravans()
		{
			foreach (GameObject caravan in caravans)
			{
				caravan.SetActive(false);
			}
		}
	}
}

