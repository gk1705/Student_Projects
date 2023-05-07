//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

namespace CaravanCrashChaos
{
	public class RewiredPlayerProfile
	{
		public Rewired.Player RewiredPlayer { get; private set; }

		public int PlayerId
		{
			get => PlayerId;
			set
			{
				PlayerId = value;
				RewiredPlayer = ReInput.players.GetPlayer(PlayerId);
			}
		}
	}
}
