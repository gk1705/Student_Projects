//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "AnnouncerVoiceLines", menuName = "Sound/AnnouncerVoiceLines")]
	public class AnnouncerVoiceLines : ScriptableObject
	{
		[SerializeField] private List<SoundClip> voiceLines;

		public void Setup(GameObject announcer)
		{
			SetupVoiceLineAudioSources(announcer);
		}

		private void SetupVoiceLineAudioSources(GameObject announcer)
		{
			foreach (var voiceLine in voiceLines)
			{
				var audioSource = announcer.AddComponent<AudioSource>();
				AudioMixer mixer = Resources.Load("AudioWixer") as AudioMixer;
				audioSource.outputAudioMixerGroup = mixer?.FindMatchingGroups("Voicelines")[0];
				voiceLine.SetAudioSource(audioSource);
			}
		}

		public SoundClip GetVoiceLine(string expression)
		{
			return voiceLines[Random.Range(0, voiceLines.Count - 1)];
		}
	}
}
