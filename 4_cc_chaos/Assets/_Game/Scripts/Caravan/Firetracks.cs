//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Fire particle system behind caravan that adapts to speed
	/// </summary>
	public class Firetracks : MonoBehaviour
	{
		private AudioSource audioSource;

		[SerializeField] [Required] private Rigidbody caravanRigidbody;
		[SerializeField] [MinMaxSlider(0, 1, true)] private Vector2 lifeTime;
		[SerializeField] [MinMaxSlider(0, 10, true)] private Vector2 size;
		[Tooltip("The speed threshold under which no particles will be shown")]
		[SerializeField] private float speedThreshold;
		[Tooltip("Speed over which particles will stay at max size and not grow anymore")]
		[SerializeField] private float maxSpeed;

		private new ParticleSystem particleSystem;
		private ParticleSystem.MainModule particleMain;

		// Start is called before the first frame update
		void Start()
		{
			//audioSource = transform.parent.GetComponent<AudioSource>();
			//audioSource.Play();
			particleSystem = GetComponent<ParticleSystem>();
			particleMain = particleSystem.main;
			Assert.IsNotNull(particleSystem);
			Assert.IsNotNull(caravanRigidbody);
		}

		void Awake()
		{
			
		}
		private void FixedUpdate()
		{
			var caravanSpeed = caravanRigidbody.velocity.magnitude;
			var speedRatio = Mathf.Min(caravanSpeed, maxSpeed) / maxSpeed; //how fast on a scale of 0 to 1? clamped with maxSpeed
			if (caravanSpeed < speedThreshold)
				speedRatio = 0;

			//audioSource.volume = speedRatio*100;
			Vector2 newSize = size * speedRatio;
			Vector2 newLifeTime = lifeTime * speedRatio;
			particleMain.startSize = new ParticleSystem.MinMaxCurve(newSize.x, newSize.y);
			particleMain.startLifetime = new ParticleSystem.MinMaxCurve(newLifeTime.x, newLifeTime.y);


			//DebugText.Instance.Print("caravan speed", $"{caravanRigidbody.velocity.magnitude:F2}");
			//DebugText.Instance.Print("caravan ratio", $"{speedRatio:F2}");
		}
	}
}


