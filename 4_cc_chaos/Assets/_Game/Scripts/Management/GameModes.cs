//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Holds a reference to the current GameMode
	/// </summary>
	[CreateAssetMenu(fileName = "GameModesHolder", menuName = "Custom/GameModesHolder", order = 1)]
	public class GameModes : ScriptableSingleton<GameModes>
	{
		public GameMode CurrentGameMode;

		public void SetGameMode(GameMode gameMode)
		{
			CurrentGameMode = gameMode;
		}
	}
}

