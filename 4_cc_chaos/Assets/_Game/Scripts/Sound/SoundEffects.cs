//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "SoundEffects", menuName = "Sound/SoundEffects")]
	public class SoundEffects : ScriptableObject
	{
		[SerializeField] private List<SoundClip> soundEffects;
		private Dictionary<string, SoundClip> expressionSoundEffectsMapping;

		public void Setup(GameObject soundManager)
		{
			SetupMapping();
			SetupSoundEffectAudioSources(soundManager);
		}

		private void SetupMapping()
		{
			expressionSoundEffectsMapping = new Dictionary<string, SoundClip>();
			foreach (var soundEffect in soundEffects)
			{
				expressionSoundEffectsMapping.Add(soundEffect.Expression, soundEffect);
			}
		}

		private void SetupSoundEffectAudioSources(GameObject soundManager)
		{
			foreach (var soundEffect in soundEffects)
			{
				var audioSource = soundManager.AddComponent<AudioSource>();
				soundEffect.SetAudioSource(audioSource);
			}
		}
		
		public SoundClip GetSoundEffect(string expression)
		{
			if (!expressionSoundEffectsMapping.ContainsKey(expression))
			{
				return null;
			}

			return expressionSoundEffectsMapping[expression];
		}
	}
}
