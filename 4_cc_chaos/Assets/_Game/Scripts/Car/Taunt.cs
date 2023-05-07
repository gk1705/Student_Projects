using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.Utils.Classes.Data;


namespace CaravanCrashChaos
{
	public class Taunt : MonoBehaviour
	{
		private CarController player;
		private AudioSource sound;

		private float nextTaunt = 0;
		private float cooldown = 2.0f;
		private float timer = 0.0f;

		bool timerRunning = false;

		void Start()
		{
			player = gameObject.GetComponentInParent<CarController>();
			sound = GetComponent<AudioSource>();

			for (int j = 0; j < transform.childCount; j++)
			{
				transform.GetChild(j).gameObject.SetActive(false);
			}
		}

		void Update()
		{
			bool isUp = player.RewiredPlayer.GetButtonUp("TurnLightsOn");
			bool isDown = player.RewiredPlayer.GetButtonDown("TurnLightsOn");
			bool isPressed = player.RewiredPlayer.GetButton("TurnLightsOn");

			if (isDown && Time.time > nextTaunt)
			{
				nextTaunt = Time.time + cooldown;
				sound.Play();
				timerRunning = true;
			}

			if (timerRunning)
			{
				timer += Time.deltaTime;

				if(timer <= cooldown){
					TurnLightsOn(true);
				}
				else
				{
					TurnLightsOn(false);
					timer -= cooldown;
					timerRunning = false;
				}
			}
		}

		void TurnLightsOn(bool isActive)
		{
			for (int j = 0; j < transform.childCount; j++)
			{
				transform.GetChild(j).gameObject.SetActive(isActive);
			}
		}
	}
}
