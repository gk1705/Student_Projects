//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Simple pool for spawning particles
	/// If too many particles are spawned, the oldest particles are reused
	/// </summary>
	public class ParticlePoolSpawner
	{
		private readonly GameObject particlePrefab;
		private readonly Transform parentTransform;
		private readonly int particleCount;
		private readonly Queue<GameObject> particlePool;

		private Vector3 localParticleScale;

		public ParticlePoolSpawner(GameObject particlePrefab, Transform parentTransform, int particleCount)
		{
			this.particlePrefab = particlePrefab;
			this.parentTransform = parentTransform;
			this.particleCount = particleCount;
			this.localParticleScale = particlePrefab.transform.localScale;

			particlePool = new Queue<GameObject>(particleCount);
			for (int i = 0; i < particleCount; i++)
			{
				var dustParticles = GameObject.Instantiate(particlePrefab, parentTransform);
				particlePool.Enqueue(dustParticles);
			}
		}

		public void Spawn(Vector3 position)
		{
			var particleObj = GetAndRequeue();
			particleObj.transform.position = position;
			particleObj.transform.localScale = localParticleScale;

			var particleSys = particleObj.GetComponent<ParticleSystem>();
			particleSys.Stop(true);
			particleSys.Play(true);
		}

		public void SetLocalParticleScale(Vector3 localParticleScale)
		{
			this.localParticleScale = localParticleScale;
		}

		private GameObject GetAndRequeue()
		{
			var particleObj = particlePool.Dequeue();
			particlePool.Enqueue(particleObj);

			return particleObj;
		}
	}

}
