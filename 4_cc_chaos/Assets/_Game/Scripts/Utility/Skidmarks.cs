//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class Skidmarks : MonoBehaviour
	{
		[Tooltip("Speed over which intensity is always 1")]
		[SerializeField] private float maxSpeed = 30f;
		[SerializeField] private float speedThreshold = 10f;
		[Range(0, 1)]
		[SerializeField] private float maxIntensity = 1f;
		[SerializeField] private LayerMask groundLayer;

		private SkidmarksManager skidmarksManager;
		private CarController carController;
		private CaravanController caravanController;

		private int lastIndex = -1;
		// Start is called before the first frame update
		void Start()
		{
			skidmarksManager = FindObjectOfType<SkidmarksManager>();
			Assert.IsNotNull(skidmarksManager, "Please put a skidmarksmanager in the scene");
			carController = GetComponentInParent<CarController>();
			caravanController = GetComponentInParent<CaravanController>();
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			if(caravanController != null)
				DrawCaravanSkidmarks();
			if(carController != null)
				DrawCarSkidmarks();
		}

		/// <summary>
		/// Draws skidmarks when caravan slides sideways
		/// </summary>
		private void DrawCaravanSkidmarks()
		{
			if (!caravanController.isActiveAndEnabled) return;

			var sidewaysVelocity = Mathf.Abs(caravanController.GetLocalVelocity().x);

			if (sidewaysVelocity < speedThreshold) //if not enough speed don't draw
			{
				lastIndex = -1;
				return;
			}

			if (!Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayer)) //if in air also don't draw
			{
				lastIndex = -1;
				return;
			}

			var intensity = sidewaysVelocity / maxSpeed;
			intensity = Mathf.Clamp(intensity, 0, maxIntensity);

			lastIndex = skidmarksManager.AddSkidMark(transform.position, Vector3.up, intensity, lastIndex);
		}
		private void DrawCarSkidmarks()
		{

		}


	}
}


