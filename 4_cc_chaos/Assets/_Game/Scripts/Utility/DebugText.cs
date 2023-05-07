//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  CaravanCrashChaos
{
	public class DebugText : MonoBehaviour
	{
		public static DebugText Instance = null;
		[SerializeField] private GUIStyle messageStyle = null;

		private readonly Dictionary<string, string> messageDictionary = new Dictionary<string, string>();

		void Awake()
		{
			if (Instance == null)
				Instance = this;
		}

		public void Print(string title, string message)
		{
			messageDictionary[title] = message;
		}

		void OnGUI()
		{
			int yPos = 0;
			foreach (KeyValuePair<string, string> keyValuePair in messageDictionary)
			{
				if (keyValuePair.Value != String.Empty)
				{
					var textDimension = messageStyle.CalcSize(new GUIContent(keyValuePair.Key));
					GUI.Label(new Rect(10, yPos, textDimension.x, 20), keyValuePair.Key + ": ", messageStyle);
					GUI.Label(new Rect(textDimension.x+35, yPos, 50, 20), keyValuePair.Value, messageStyle);				
					yPos += (int)textDimension.y;
				}
			}
		}

		//todo make a useable printfunction for worldspace debug text
		//todo make sure to parent it correctly, spawn and destroy it 
		//private IEnumerator PrintDebugText(string text, float duration)
		//{
		//	if (!textObject) textObject = GameObject.Instantiate(textMesh, transform.position, Quaternion.identity, transform);
		//	var lookAtComponent = textObject.AddComponent<LookAt>();
		//	lookAtComponent.cameraTarget = GameObject.Find("MainCamera" + caravanController.GetPlayer().GetID).transform;
		//	var textMeshMesh = textObject.GetComponent<TextMesh>();
		//	textMeshMesh.text = text;
		//	textMeshMesh.fontSize = 20;
		//	yield return new WaitForSeconds(duration);
		//	textMeshMesh.text = "";
		//}
	}
}


