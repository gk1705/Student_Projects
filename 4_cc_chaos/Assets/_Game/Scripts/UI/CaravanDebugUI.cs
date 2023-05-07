//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class CaravanDebugUI : MonoBehaviour
	{
		[SerializeField] private bool show = true;
		private new Rigidbody rigidbody;
		private new HingeJoint hingeJoint;
		private CaravanController caravanController;

		void Start()
		{
			if (!show) return;
			SetValues();
		}

		private void SetValues()
		{
			rigidbody = GetComponent<Rigidbody>();
			hingeJoint = GetComponent<HingeJoint>();
			caravanController = GetComponent<CaravanController>();
		}

		void Update()
		{
			if (!show) return;
			DrawDebugUI();
		}

		private void DrawDebugUI()
		{
			DebugText.Instance.Print("Caravan Speed", $"{rigidbody.velocity.magnitude:F2} m/s {rigidbody.velocity.magnitude * 3.6f:F2} km/h");
			DebugText.Instance.Print("Caravan Velocity", $"{rigidbody.velocity}");
			DebugText.Instance.Print("Caravan sideways velocity", $"{caravanController.GetLocalVelocity().x:F2}");
		}
	}
}


