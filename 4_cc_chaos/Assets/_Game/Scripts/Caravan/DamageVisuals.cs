//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class DamageVisuals : MonoBehaviour
	{
		[MinMaxSlider(0, 1000, true)]
		[SerializeField] private Vector2 healthRange;
		[SerializeField] private float updateRate = 0.1f;

		private Health health;

		private new ParticleSystem particleSystem;
		private ParticleSystem.MainModule particleMain;

		private WaitForSeconds updateWait;

		// Start is called before the first frame update
		void Start()
		{

			health = GetComponentInParent<Health>();

			particleSystem = GetComponent<ParticleSystem>();
			particleMain = particleSystem.main;

			Assert.IsNotNull(particleSystem);

			updateWait = new WaitForSeconds(updateRate);
			StartCoroutine(nameof(CheckSystems));
		}

		private IEnumerator CheckSystems()
		{
			while (true)
			{
				if (!particleSystem.isPlaying && InRange(health.CurrentHealth, healthRange.x, healthRange.y)) //only play when not already playing
					particleSystem.Play();
				else if (particleSystem.isPlaying && !InRange(health.CurrentHealth, healthRange.x, healthRange.y)) //stop when not in range and playing
					particleSystem.Stop();

				yield return updateWait; //check every x seconds
			}
		}

		bool InRange(int numberToCheck, float bottom, float top)
		{			
			return (numberToCheck >= bottom && numberToCheck <= top);
		}
	}
}
