//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Instantiates the passed game object at the given mount transform.
	/// Delets the current object, insofar it is not null.
	/// </summary>
	public class MeshChanger : MonoBehaviour
	{
		[SerializeField]
		private GameObject currentModel;

		public void ChangeModel(GameObject newModel)
		{
			if (currentModel)
			{
				DestroyImmediate(currentModel);
			}

			currentModel = Instantiate(newModel, transform);

			// Additional check if the model is in the lobby scene
			// Because the first shown model should not have the default white color

			if (GetComponent<ColorChanger>() != null)
			{
				Material material = GetComponent<ColorChanger>().GetMaterial();
				if(material!=null)
					currentModel.GetComponentInChildren<ChangeBodyColor>().ApplyColor(material.color);
			}
		}
	}
}
