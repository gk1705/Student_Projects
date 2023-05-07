//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Encapsulates a list of player objects as well as decorations and colors.
	/// The latter two are in a ring buffer that reserves array indices based
	/// on who is currently using that color/decoration.
	///
	/// This classed is a fassade, used to iterate through those ringbuffer-like collections.
	/// </summary>
	public class PlayerCustomizationController : MonoBehaviour
	{
		[SerializeField] private List<GameObject> playerObjects;
		[SerializeField] private List<Decoration> decorations;
		[SerializeField] private List<GameObject> caravanModels;
		[SerializeField] [Required] private PlayerColors colors;

		private OccupiableRingBuffer<Decoration> playerDecorations;
		private OccupiableRingBuffer<Color> playerColors;
		private Dictionary<int, int> playerModelMapping;

		private List<bool> hasPlayerBeenActivated;

		void Awake()
		{
			hasPlayerBeenActivated = new List<bool>();
			for (int i = 0; i < playerObjects.Count; i++)
			{
				hasPlayerBeenActivated.Add(false);
			}

			playerDecorations = new OccupiableRingBuffer<Decoration>();
			playerColors = new OccupiableRingBuffer<Color>();
		}

		// Start is called before the first frame update
		void Start()
		{
			DeactivatePlayerObjects();
			SetupOccupiableRingBuffers();
		}

		public void SetPlayerActivated(int index)
		{
			hasPlayerBeenActivated[index] = true;
		}

		public void SetPlayerDeactivated(int index)
		{
			hasPlayerBeenActivated[index] = false;
		}

		public bool IsPlayerActivated(int index)
		{
			return hasPlayerBeenActivated[index];
		}

		public void TogglePlayerModel(int index)
		{
			var isActive = playerObjects[index].activeSelf;
			playerObjects[index].SetActive(!isActive);
		}

		public void ActivatePlayerModel(int index)
		{
			playerObjects[index].SetActive(true);
		}

		public void IterateDecorationUp(int playerIndex)
		{
			var go = playerObjects[playerIndex];
			var dec = playerDecorations.GetNextUp(playerIndex);

			Lobby.GetPlayer(playerIndex).SetDecoration(dec);
			ChangeDecoration(go, dec);
		}

		public void IterateDecorationDown(int playerIndex)
		{
			var go = playerObjects[playerIndex];
			var dec = playerDecorations.GetNextDown(playerIndex);

			Lobby.GetPlayer(playerIndex).SetDecoration(dec);
			ChangeDecoration(go, dec);
		}

		public void IterateColorUp(int playerIndex)
		{
			var go = playerObjects[playerIndex];
			var col = playerColors.GetNextUp(playerIndex);

			Lobby.GetPlayer(playerIndex).SetColor(col);
			ChangePlayerMeshColor(go, col);
		}

		public void IterateColorDown(int playerIndex)
		{
			var go = playerObjects[playerIndex];
			var col = playerColors.GetNextDown(playerIndex);

			Lobby.GetPlayer(playerIndex).SetColor(col);
			ChangePlayerMeshColor(go, col);
		}

		public void IterateModelUp(int playerIndex)
		{
			GameObject currentModel = Lobby.GetPlayer(playerIndex).caravanModel;
			int caravanModelIndex = caravanModels.IndexOf(currentModel);
			caravanModelIndex = (caravanModelIndex + 1) % caravanModels.Count;

			var go = playerObjects[playerIndex];
			var mod = caravanModels[caravanModelIndex];

			Lobby.GetPlayer(playerIndex).SetModel(mod);
			ChangeModel(go, mod);
		}

		public void IterateModelDown(int playerIndex)
		{
			GameObject currentModel = Lobby.GetPlayer(playerIndex).caravanModel;
			int caravanModelIndex = caravanModels.IndexOf(currentModel);
			caravanModelIndex = (caravanModelIndex + caravanModels.Count - 1) % caravanModels.Count;

			var go = playerObjects[playerIndex];
			var mod = caravanModels[caravanModelIndex];

			Lobby.GetPlayer(playerIndex).SetModel(mod);
			ChangeModel(go, mod);
		}

		private void SetupOccupiableRingBuffers()
		{
			foreach (var dec in decorations)
			{
				playerDecorations.Add(dec);
			}

			foreach (var col in colors.Colors)
			{
				playerColors.Add(col);
			}
		}

		private void DeactivatePlayerObjects()
		{
			foreach (var pObj in playerObjects)
			{
				pObj.SetActive(false);
			}
		}

		private static void ChangePlayerMeshColor(GameObject go, Color col)
		{
			var colChanger = go.GetComponent<ColorChanger>();
			colChanger.ChangeMeshColor(col);
		}

		private static void ChangeDecoration(GameObject go, Decoration decoration)
		{
			if (!GameModes.Instance.CurrentGameMode.AllowDecorations) return;
			var decChanger = go.GetComponent<DecorationChanger>();
			decChanger.ChangeDecoration(decoration);
		}

		private static void ChangeModel(GameObject go, GameObject model)
		{
			var modChanger = go.GetComponent<MeshChanger>();
			modChanger.ChangeModel(model);
		}
	}
}
