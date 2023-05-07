//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Checks for collision with border, spawns particle on collision
	/// </summary>
	public class BorderCollisionParticles : MonoBehaviour
	{
		[SerializeField] private GameObject particlePrefab;
		[SerializeField] private int particleCount;

		[SerializeField] private Vector3 normalScale, caravanScale;

		private CaravanController caravanController;
		private ParticlePoolSpawner particlePoolSpawner;

		void Awake()
		{
			caravanController = GetComponent<CaravanController>();
			particlePoolSpawner = new ParticlePoolSpawner(particlePrefab, transform, particleCount);
		}

		private void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.CompareTag("Border"))
			{
				var contactPointPos = col.contacts[0].point;
				if (gameObject.CompareTag("Caravan") && caravanController.IsDetached)
				{
					particlePoolSpawner.SetLocalParticleScale(caravanScale);
				}
				else
				{
					particlePoolSpawner.SetLocalParticleScale(normalScale);
					var boxCollider = gameObject.GetComponent<BoxCollider>();
					contactPointPos.y = boxCollider.bounds.center.y - boxCollider.bounds.extents.y;
				}

				particlePoolSpawner.Spawn(contactPointPos);
			}
		}
	}
}
