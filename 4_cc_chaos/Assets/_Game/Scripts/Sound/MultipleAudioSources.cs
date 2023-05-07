//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
namespace CaravanCrashChaos
{
	public class MultipleAudioSources : MonoBehaviour
	{
		private AudioSource[] audioSources;
		void Start()
		{
			audioSources = GetComponents<AudioSource>();
		}

		public AudioSource GetAudioSource(int idx)
		{
			return audioSources?[idx];
		}
	}
}
