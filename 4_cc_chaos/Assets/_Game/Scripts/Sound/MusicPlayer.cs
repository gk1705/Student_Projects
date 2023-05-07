//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class MusicPlayer : MonoBehaviour
	{

		[SerializeField] private List<AudioClip> music;

		[Required] [SerializeField] private AudioSource audioSource;

		private AudioClip currentAudioClip = null;
		// Start is called before the first frame update
		void Start()
		{
			StartCoroutine(SelectNextClip());
		}

		IEnumerator SelectNextClip()
		{
			var foundSong = music.Find(a => a != currentAudioClip && a != null);
			audioSource.clip = foundSong;
			currentAudioClip = foundSong;
			audioSource.Play();
			yield return new WaitForSecondsRealtime(currentAudioClip.length);
			StartCoroutine(SelectNextClip());
		}
	}
}


