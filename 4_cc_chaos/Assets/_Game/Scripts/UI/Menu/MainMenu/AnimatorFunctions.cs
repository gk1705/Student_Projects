using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	[SerializeField] private MenuButtonController menuButtonController;
	public bool disableOnce;

	void PlaySound(AudioClip whichSound)
	{
		if (!disableOnce)
		{
			menuButtonController.audioSource.PlayOneShot(whichSound);
		}
		else
		{
			disableOnce = false;
		}
	}

}
