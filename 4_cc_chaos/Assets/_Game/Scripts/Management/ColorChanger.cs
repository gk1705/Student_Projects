//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Changes the color of the assinged material.
	/// </summary>
	public class ColorChanger : MonoBehaviour
	{
		[SerializeField] private Material material;

		public delegate void colorChangeEvent(Color color);

		public event colorChangeEvent eventColor;
	
		public void ChangeMeshColor(Color color)
		{
			material.color = color;
			eventColor?.Invoke(color);
		}

		public Material GetMaterial()
		{
			return material;
		}
	}
}
