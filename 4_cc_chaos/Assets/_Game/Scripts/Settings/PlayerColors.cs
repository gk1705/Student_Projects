//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "PlayerColors", menuName = "Custom/PlayerColors", order = 1)]
	public class PlayerColors : ScriptableObject
	{
		[SerializeField] private Color[] colors;
		[HideInInspector] public Color[] Colors => colors;

		public Color GetColor(int index)
		{
			if (index >= 0 && index < colors.Length)
			{
				Debug.Log($"returning {colors[index]}");
				return colors[index];
			}
				
			return Color.black;

		}

	}
}


