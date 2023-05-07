//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{

	public class HitShake : MonoBehaviour
	{
		[Tooltip("Shake occurs over this threshold")]
		[SerializeField] private float damageThreshold = 250f;
		[Required]
		[SerializeField] private CaravanDamage caravanDamage;
		[Required]
		[SerializeField] private ScreenShakeProfile shakeProfile;

		private CameraShake cameraShake;

		private void Start()
		{
			cameraShake = FindObjectOfType<CameraShake>();
			caravanDamage.OnDealDamage += HeavyHitShake;
		}


		private void HeavyHitShake(Hit hit)
		{
			if (hit.Damage < damageThreshold || hit.KillShot) return;
			StartCoroutine(cameraShake?.Shake(shakeProfile.Length, shakeProfile.Amplitude, shakeProfile.Frequency));
		}
	}
}


