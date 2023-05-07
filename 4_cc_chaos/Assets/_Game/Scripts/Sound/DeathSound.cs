//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using Exploder;
using Exploder.Utils;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class DeathSound : MonoBehaviour
	{
		public AudioSource deathSound;
		private bool hasPlayed = false;

		private Health health;

		void Update()
		{
			if (health.IsDead && !hasPlayed)
			{
				deathSound.Play();
				hasPlayed = true;

			}
		}
	}
}
