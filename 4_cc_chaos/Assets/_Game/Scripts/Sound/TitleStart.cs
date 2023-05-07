//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class TitleStart : MonoBehaviour
	{
		[SerializeField] private float announcerTitleDelay = 0.5f;
		[SerializeField] private float musicDelay = 1f;
		[SerializeField] private AudioSource musicSource;

		// Start is called before the first frame update
		void Start()
		{
			Announcer.Instance.ForceVoiceLine("Title", announcerTitleDelay);
			StartCoroutine(PlayMusicAfterDelay(musicDelay));
		}

		IEnumerator PlayMusicAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			musicSource.Play();
		}
	}
}
