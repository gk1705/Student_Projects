//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class DetachSound : MonoBehaviour
	{
		private MultipleAudioSources audioSources;
		private CaravanController caravanController;

		private void OnEnable()
		{
			audioSources = transform.parent.GetComponent<MultipleAudioSources>();
			caravanController = transform.parent.GetComponent<CaravanController>();
			Assert.IsNotNull(caravanController);
			Assert.IsNotNull(audioSources);
			caravanController.OnDetach += PlayDetachSound;
		}

		private void OnDisable()
		{
			if(caravanController != null)
				caravanController.OnDetach -= PlayDetachSound;
		}

		private void PlayDetachSound()
		{
			audioSources.GetAudioSource(0)?.Play();
		}
	}
}
