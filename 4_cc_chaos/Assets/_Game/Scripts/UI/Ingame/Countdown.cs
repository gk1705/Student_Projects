using System;
using CaravanCrashChaos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI countdownText;
	[SerializeField] private float countdownAmount = 5;
	[SerializeField] private bool announceCountdown = false;

	[SerializeField] private string textPrefix = "";
	[SerializeField] private string textSuffix = "";

	private bool active = false;
	private float value = 0f;

	public delegate void TimerTrigger();
	public event TimerTrigger OnTimerTrigger;

	void Start()
    {
	    countdownText.text = countdownAmount.ToString();
	    value = countdownAmount;
    }

    void Update()
    {
	    if (active)
	    {
			ProcessTimer();
	    }
    }

	private void ProcessTimer()
	{
		value -= Time.deltaTime;

		// Print Suffix on last second
		if (value > 0 && value < 0.5f && textSuffix != String.Empty)
		{
			countdownText.text = textSuffix;
			countdownText.fontSize = 180;
		}
		else
		{
			countdownText.text = textPrefix + " " +  Mathf.RoundToInt(value).ToString();
		}


		if (value <= 0)
		{
			TriggerTimer();
		}
	}

	private void TriggerTimer()
	{
		value = 0;
		OnTimerTrigger.Invoke();
		countdownText.enabled = false;
		active = false;
	}

	public void StartTimer()
	{
		active = true;

		if (announceCountdown)
		{
			Announcer.Instance.ForceVoiceLine("Countdown");
		}
	}

	public void ResetTimer()
	{
		countdownText.text = countdownAmount.ToString();
		value = countdownAmount;
	}
}
