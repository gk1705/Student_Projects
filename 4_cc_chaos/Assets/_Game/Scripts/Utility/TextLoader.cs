//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace CaravanCrashChaos
{
	public class TextLoader : MonoBehaviour
	{
		[SerializeField] private DropDownList dropDownList;
		// Start is called before the first frame update
		void Awake()
		{
			string path = Application.dataPath;
			if(Application.platform == RuntimePlatform.WindowsPlayer)
				path += "/../";
			else if (Application.isEditor)
				path = Path.Combine(path, "_Game");

			string[] textFiles = Directory.GetFiles(path, "*.txt");

			if (textFiles.Length <= 0)
			{
				Debug.LogWarning($"can't find a text file to read, aborting the mission");
				dropDownList.gameObject.SetActive(false);
				return;
			}

			Debug.Log($"{path}");
			using (StreamReader reader = new StreamReader(textFiles[0]))
			{
				Teams.Groups.Clear();
				Teams.CurrentGroup = null;
				int counter = 0;
				Group group = null;
				while (reader.Peek() >= 0)
				{
					if (counter % 4 == 0) //every 4 teams make a new group
					{
						group = new Group();
						Teams.Groups.Add(group);
						group.Nr = Teams.Groups.Count;
					}
					string line = reader.ReadLine();
					group.TeamNames.Add(line);
					Debug.Log($"read line {line}");
					counter++;
				}
			}

			PopulateDropdown();
		}


		private void PopulateDropdown()
		{
			dropDownList.Items.Clear();
			foreach (var group in Teams.Groups)
			{
				dropDownList.Items.Add(new DropDownListItem($"{group.Nr}: {group.GetAllNames()}", $"{group.Nr}"));
			}
			dropDownList.gameObject.SetActive(true);
		}

		public void SetSelectedGroup(int index)
		{
			Teams.CurrentGroup = Teams.Groups[index];
			Debug.Log($"selected group {Teams.CurrentGroup.Nr}");
		}

		// Update is called once per frame
		void Update()
		{
			
		}
	}
}


