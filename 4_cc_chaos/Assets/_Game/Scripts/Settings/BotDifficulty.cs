//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{

	[CreateAssetMenu(fileName = "BotDifficulty", menuName = "Custom/Bots/BotDifficulty", order = 1)]
	public class BotDifficulty : ScriptableObject
	{
		[Header("Car")]
		public float MinThrust = 0.8f;
		public float MaxThrust = 1f;
		public float MinTurn = 0f;
		public float MaxTurn = 0.2f;
		[Header("Caravan")]
		public float DotThreshold = 0.99f;
		public float DelayBeforeShot = 0f;
	}
}


