//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class LookAt : MonoBehaviour
	{
		public Transform cameraTarget;
		// Update is called once per frame

		private void Start()
		{
			if (cameraTarget == null)
				cameraTarget = Camera.main.transform;
		}

		void Update()
		{
			transform.rotation = Quaternion.LookRotation(transform.position - cameraTarget.transform.position);
		}
	}
}


