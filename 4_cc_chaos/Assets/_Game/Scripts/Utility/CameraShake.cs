//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class CameraShake : MonoBehaviour
	{
		[SerializeField] private float shakeDuration = 0.3f;          // Time the Camera Shake effect will last
		[SerializeField] private float shakeAmplitude = 1f;         // Cinemachine Noise Profile Parameter
		[SerializeField] private float shakeFrequency = 1.2f;         // Cinemachine Noise Profile Parameter

		// Cinemachine Shake
		public CinemachineVirtualCamera VirtualCamera;
		private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

		// Use this for initialization
		void Start()
		{
			// Get Virtual Camera Noise Profile
			if (VirtualCamera != null)
				virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
		}

		public IEnumerator Shake()
		{
			if (!VirtualCamera || !virtualCameraNoise) yield break;			
			StartShake(shakeAmplitude, shakeFrequency);
			yield return new WaitForSecondsRealtime(shakeDuration);
			StopShake();
		}

		public IEnumerator Shake(float duration)
		{
			if (!VirtualCamera || !virtualCameraNoise) yield break;
			StartShake(shakeAmplitude, shakeFrequency);
			yield return new WaitForSecondsRealtime(duration);
			StopShake();
		}

		public IEnumerator Shake(float duration, float amplitude, float frequency)
		{
			if (!VirtualCamera || !virtualCameraNoise) yield break;
			StartShake(amplitude, frequency);
			yield return new WaitForSecondsRealtime(duration);
			StopShake();
		}

		private void StartShake(float amplitude, float frequency)
		{
			virtualCameraNoise.m_AmplitudeGain = amplitude;
			virtualCameraNoise.m_FrequencyGain = frequency;
		}

		private void StopShake()
		{
			virtualCameraNoise.m_AmplitudeGain = 0f;
		}
	}
}


