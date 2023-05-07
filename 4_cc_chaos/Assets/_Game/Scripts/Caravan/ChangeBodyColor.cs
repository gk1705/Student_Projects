using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CaravanCrashChaos
{
	public class ChangeBodyColor : MonoBehaviour
	{
		/// <summary>
		/// This script is attached on the caravan model and listens to the ChangeColor event from the ColorChanger Script
		/// When the color is changed in menu: ApplyColor(Color color) looks over every children of the model and changes every changeable color to the players color
		/// </summary>
		/// 
		private Material material;

		private ColorChanger colorChanger;

		private void Subscribe(Color color)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform t = transform.GetChild(i);

				if (t.GetComponent<MeshRenderer>().material.name.Contains("changeable"))
				{
					Debug.Log($"apply color {gameObject.name} for {t.gameObject.name}");
					t.GetComponent<MeshRenderer>().material.color = color;
				}
			}
		}

		void Start()
		{
			colorChanger = transform.parent.GetComponentInParent<ColorChanger>();
			//material = colorChanger.GetMaterial();
			if(colorChanger != null)
				colorChanger.eventColor += Subscribe;
		}

		void OnDisable()
		{
			if(colorChanger!=null)
				colorChanger.eventColor -= Subscribe;
		}

		public Material GetMaterial()
		{
			return material;
		}

		public void SetMaterial(Material material)
		{
			this.material = material;
		}

		public void ApplyColor(Color color)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform t = transform.GetChild(i);

				if (t.GetComponent<MeshRenderer>().material.name.Contains("changeable"))
				{
					t.GetComponent<MeshRenderer>().material.color = color;
				}
			}
		}
	}
}
