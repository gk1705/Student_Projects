//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "ControllerShakeProfile", menuName = "Custom/ControllerShakeProfile", order = 1)]
	public class ControllerShakeProfile : ScriptableObject
	{
		[InfoBox("Use custom length or strength if you set your own values via code")]
		public bool CustomLength = false;
		public bool CustomStrength = false;
		[Header("Values")]
		[HideIf("CustomLength")] public float Length = 0.5f;
		[HideIf("CustomStrength")] [Range(0f, 1f)] public float LeftStrength = 1f;
		[HideIf("CustomStrength")] [Range(0f, 1f)] public float RightStrength = 1f;
		[Header("Motors")]
		public bool RightMotor = true;
		public bool LeftMotor = true;
		public bool StopAllOtherMotors = false;
	}
}


