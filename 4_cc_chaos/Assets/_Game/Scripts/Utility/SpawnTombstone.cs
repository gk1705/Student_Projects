//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class SpawnTombstone : MonoBehaviour
	{
		[SerializeField]
		private GameObject tombstone;
		[SerializeField]
		private GameObject car;
		
		private PlayerDeath playerDeath;
		void Start()
		{
			playerDeath = gameObject.GetComponent<PlayerDeath>();
			playerDeath.OnPlayerDies += OnPlayerDeath;
		}

		void OnPlayerDeath(int arg)
		{
			// Set y value to ground level
			Vector3 spawnPosition = new Vector3(car.transform.position.x, 5f, car.transform.position.z);
			float angle = -40.0f;
			GameObject o = Instantiate(tombstone,spawnPosition, Quaternion.Euler(new Vector3(0,angle,0)));
		}
	}
}
