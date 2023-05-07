//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class WheelRotation : MonoBehaviour
	{
		[SerializeField] private float rotationSpeed = 10f;
		[SerializeField] private string inputAxis = "Vertical0";
		private Transform wheelTransform;
		// Start is called before the first frame update
		void Start()
		{
			wheelTransform = transform;
		}

		void FixedUpdate()
		{
			float speed = Input.GetAxis(inputAxis);
			wheelTransform.Rotate(speed*rotationSpeed, 0, 0);
		}
	}
}


