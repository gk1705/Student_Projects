//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "ScreenShakeProfile", menuName = "Custom/ScreenShakeProfile", order = 1)]
	public class ScreenShakeProfile : ScriptableObject
	{
		public float Length = 1f;
		public float Amplitude = 1f;
		public float Frequency = 1.2f;
	}
}


