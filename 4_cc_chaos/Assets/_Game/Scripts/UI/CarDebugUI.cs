//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


namespace CaravanCrashChaos
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CarController))]
	[DisallowMultipleComponent]
	public class CarDebugUI : MonoBehaviour
	{
		[SerializeField] private bool show = true;
		private Rigidbody carRigidbody;
		private CarController carController;
		private bool init = false;

		void Start()
		{
			init = false;
			carRigidbody = GetComponent<Rigidbody>();
			carController = GetComponent<CarController>();
			init = true;
		}

		void Update()
		{
			if (!init) return;
			// DrawDebugUI();
		}


		private void DrawDebugUI()
		{
			DebugText.Instance.Print("Speed", $"{carRigidbody.velocity.magnitude:F2} m/s  {carRigidbody.velocity.magnitude * 3.6f:F2} km/h");
			DebugText.Instance.Print("Sidewaysspeed", $"{carController.GetLocalVelocity().x:F3}");
			DebugText.Instance.Print("Rotationspeed", $" {carRigidbody.angularVelocity.magnitude:F3}");
		}


		void OnDrawGizmos()
		{
			if (!Application.isPlaying || !show)
				return;

			List<WheelValues> wheelValues = carController.GetWheelValues();

			for (int i = 0; i < wheelValues.Count; i++)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(wheelValues[i].WheelPosition, wheelValues[i].ImpactPoint);
				Gizmos.DrawSphere(wheelValues[i].ImpactPoint, 0.1f);

				GUIStyle guiStyle = new GUIStyle();
				guiStyle.normal.textColor = Color.green;
				guiStyle.fontSize = 15;
				#if UNITY_EDITOR
				Handles.Label(wheelValues[i].WheelPosition, $" upforce: {wheelValues[i].UpForce.y:F3}", guiStyle);
				#endif
			}				
		}
	}
}


