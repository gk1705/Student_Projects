//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "SlowMoProfile", menuName = "Custom/SlowMoProfile", order = 1)]
	public class SlowMoProfile : ScriptableObject
	{
		public float Length = 1f;
		public float Speed = 0.2f;
	}
}


