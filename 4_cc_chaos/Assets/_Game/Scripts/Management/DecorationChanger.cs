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
	public class DecorationChanger : MonoBehaviour
	{
		[SerializeField] private Transform mountTransform;

		private GameObject currentDecoration;
		private Sprite currentDecorationSprite;
		public GameObject CurrentDecoration => currentDecoration;

		public void ChangeDecoration(Decoration decoration)
		{
			if (decoration == null)
			{
				Debug.LogError($"cant change decoration is null");
				return;
			}
			if (currentDecoration)
			{
				Destroy(currentDecoration);
			}

			currentDecoration = Instantiate(decoration.Model, mountTransform.position, mountTransform.rotation, mountTransform);
			currentDecorationSprite = decoration.Icon;
		}

		public void AssignDecoration(GameObject decoration, Sprite decoSprite)
		{
			if (decoration == null)
			{
				Debug.LogWarning($"deco is null");
				return;
			}
			decoration.transform.position = mountTransform.position;
			decoration.transform.rotation = mountTransform.rotation;
			decoration.transform.parent = mountTransform;
			decoration.SetActive(true);
			currentDecoration = decoration;
			currentDecorationSprite = decoSprite;
		}

		public void SetSprite(Sprite sprite)
		{
			currentDecorationSprite = sprite;
		}

		public Sprite GetDecorationImage()
		{
			return currentDecorationSprite;
		}
	}
}
