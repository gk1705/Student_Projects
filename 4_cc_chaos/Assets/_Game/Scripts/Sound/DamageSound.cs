//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class DamageSound : MonoBehaviour
	{
		private MultipleAudioSources audioSources;
		private CaravanDamage caravanDamage;

		private void OnEnable()
		{
			audioSources = transform.parent.GetComponent<MultipleAudioSources>();
			caravanDamage = transform.parent.GetComponent<CaravanDamage>();
			Assert.IsNotNull(caravanDamage);
			Assert.IsNotNull(audioSources);
			caravanDamage.OnDealDamage += PlayDamageSound;
		}

		private void OnDisable()
		{
			caravanDamage.OnDealDamage -= PlayDamageSound;
		}

		private void PlayDamageSound(Hit hit) //todo scale audio with damage, maybe different sound for death
		{
			audioSources.GetAudioSource(1).Play();
		}
	}
}
