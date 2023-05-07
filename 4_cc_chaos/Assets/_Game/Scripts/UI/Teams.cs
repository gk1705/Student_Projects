//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public static class Teams
	{
		public static List<Group> Groups = new List<Group>();
		public static Group CurrentGroup = null;
		public static bool HasGroups => Groups.Count > 0;
	}

	[System.Serializable]
	public class Group
	{
		public List<string> TeamNames = new List<string>();
		public int Nr;

		/// <summary>
		/// Get name of a team in this group
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetNameAt(int index)
		{
			if (index >= 0 && index < TeamNames.Count)
			{
				return TeamNames[index];
			}

			return string.Empty;
		}

		/// <summary>
		/// Get all teamnames as a single string
		/// </summary>
		/// <returns>All team names as a string, separated by a space</returns>
		public string GetAllNames()
		{
			string all = String.Empty;
			foreach (string teamName in TeamNames)
			{
				all += $"{teamName} ";
			}
			return all;
		}
	}
}


