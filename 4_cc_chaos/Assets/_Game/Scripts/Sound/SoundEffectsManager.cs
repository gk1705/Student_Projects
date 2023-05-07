//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using Exploder;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class SoundEffectsManager : Singleton<SoundEffectsManager>
	{
		private SoundEffects soundEffects;

		private void Awake()
		{
			soundEffects = Resources.Load<SoundEffects>("SoundEffects");
			soundEffects.Setup(gameObject);
		}

		public void PlaySoundEffect(string expression, float delay = 0)
		{
			var soundEffect = soundEffects.GetSoundEffect(expression);
			soundEffect?.Play(delay);
		}
	}
}
