//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class FollowTransform : MonoBehaviour
	{

		[SerializeField] private Transform target;


		private Vector3 velocity;
		// Start is called before the first frame update
		void Start()
		{
			Assert.IsNotNull(target);
		}

		// Update is called once per frame
		void Update()
		{
			transform.position = target.position;
			transform.rotation = target.rotation;
		}

	}
}


