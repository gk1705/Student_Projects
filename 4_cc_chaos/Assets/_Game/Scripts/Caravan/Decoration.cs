//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "Decoration", menuName = "Custom/Decoration", order = 1)]
	public class Decoration : ScriptableObject
	{
		[Required] public GameObject Model;
		[Required] public Sprite Icon;
	}
}


