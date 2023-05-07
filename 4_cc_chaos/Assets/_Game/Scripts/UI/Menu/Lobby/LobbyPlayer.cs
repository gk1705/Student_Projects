//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class LobbyPlayer
	{
		public int Id { get; private set; }
		public int TeamId { get; set; }
		public Color Color { get; private set; }

		public Decoration Decoration { get; private set; }
		public GameObject caravanModel { get; private set; }
		public bool IsBot { get; set; }

		public LobbyPlayer(int id)
		{
			Id = id;
			TeamId = id;
		}
		public LobbyPlayer(int id, Color color)
		{
			Id = id;
			Color = color;
			TeamId = id;
		}

		public void SetColor(Color color) => Color = color;
		public void SetDecoration(Decoration decoration) => Decoration = decoration;
		public void SetModel(GameObject model) => caravanModel = model;
	}
}


