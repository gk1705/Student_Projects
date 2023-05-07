//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class RotateAround : MonoBehaviour
	{
		[SerializeField] private Transform objectToRotateAround;

		[SerializeField] private float speed = 10f;


		// Update is called once per frame
		void Update()
		{
			transform.RotateAround(objectToRotateAround.position, Vector3.up, speed * Time.deltaTime);
		}
	}
}


