//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Static class to get, add, remove players in the lobby
	/// </summary>
	public static class Lobby
	{
		public static List<LobbyPlayer> Players { get; private set; } = new List<LobbyPlayer>(4);

		public static void AddPlayer(LobbyPlayer player)
		{
			Players.Add(player);
		}

		public static void RemovePlayer(LobbyPlayer player)
		{
			Players.Remove(player);
		}

		public static void RemovePlayer(int id)
		{
			LobbyPlayer player = null;
			try
			{
				 player = Players.Find(p => p.Id == id);
			}
			catch (Exception e)
			{
				Debug.LogWarning($"could not remove player{id} {e.Message}");
				return;				
			}
			RemovePlayer(player);
		}

		public static LobbyPlayer GetPlayer(int id)
		{
			return Players.Find(p => p.Id == id);
		}
	}
}


