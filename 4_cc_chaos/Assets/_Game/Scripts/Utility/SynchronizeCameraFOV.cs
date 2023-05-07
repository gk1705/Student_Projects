using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Utility class to synchronize FOV between two cameras
	/// Add this script to a camera and set the taget from which the FOV-value is drawn
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class SynchronizeCameraFOV : MonoBehaviour
	{
		[SerializeField] private Camera targetCamera;
		private Camera goalCamera;

		void Awake()
		{
			goalCamera = GetComponent<Camera>();
		}

		// Update is called once per frame
		void Update()
		{
			goalCamera.fieldOfView = targetCamera.fieldOfView;
		}
	}
}
