//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// AudioClip wrapper
	/// Encapsulates properties regarding playback 
	/// </summary>
	[System.Serializable]
	public class SoundClip
	{
		public string Expression;
		[SerializeField] private AudioClip audioClip;
		[SerializeField, Range(0f, 1f)] private float volume;
		[SerializeField, Range(0.1f, 3f)] private float pitch;

		private AudioSource audioSource;

		public void SetAudioSource(AudioSource audioSource)
		{
			this.audioSource = audioSource;
		}

		public bool IsPlaying()
		{
			return audioSource.isPlaying;
		}

		public void Stop()
		{
			audioSource.Stop();
		}

		/// <summary>
		/// Plays the audio clip stored in this voice line
		/// </summary>
		/// <param name="delay">Delay is specified in seconds</param>
		public void Play(float delay = 0)
		{
			if (!audioSource)
			{
				Debug.Log("AudioSource mustn't be null.");
				return;
			}

			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.pitch = pitch;

			if (delay > 0)
				audioSource.PlayDelayed(delay);
			else
				audioSource.Play();
		}
	}
}
