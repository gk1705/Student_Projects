//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;


namespace CaravanCrashChaos
{
	public class SandboxController : MonoBehaviour
	{
		[SerializeField] private List<Transform> spawnPoints;
		[SerializeField] private List<GameObject> playerPrefabs;
		[SerializeField] private List<GameObject> cameras;
		[SerializeField] private SpawnBehaviour spawnBehaviour;
		[SerializeField] private List<GameObject> readyUiObjects;
		[SerializeField] private List<Text> readyTexts;
		[SerializeField] private List<Image> botInfo;

		private LobbyHandler lobbyHandler;
		private List<LobbyPlayer> lobbyPlayers;

		void Start()
		{
			lobbyPlayers = Lobby.Players;

			lobbyHandler = GameObject.FindObjectOfType<LobbyHandler>();
			lobbyHandler.OnSetReady += SpawnPlayer;
			lobbyHandler.OnSetNotReady += DespawnPlayer;
		}

		private void DespawnPlayer(int index)
		{
			int lobbyIndex = Lobby.GetPlayer(index).Id;

			cameras[lobbyIndex].gameObject.SetActive(false);
			readyUiObjects[lobbyIndex].gameObject.SetActive(false);

			DestroyPlayer(lobbyIndex);
		}

		private void SpawnPlayer(int index)
		{
			var lobbyPlayer = Lobby.GetPlayer(index);
			int lobbyIndex = lobbyPlayer.Id;

			var go = Instantiate(playerPrefabs[lobbyIndex], spawnPoints[lobbyIndex]);

			var carController = go.GetComponentInChildren<CarController>();
			var caravanController = go.GetComponentInChildren<CaravanController>();

			Debug.Log(Lobby.GetPlayer(index).Color);
			
			if(GameModes.Instance.CurrentGameMode.AllowDecorations) go.GetComponent<DecorationChanger>().ChangeDecoration(lobbyPlayer.Decoration);
			go.GetComponentInChildren<MeshChanger>().ChangeModel(lobbyPlayer.caravanModel);
			go.GetComponentInChildren<ChangeBodyColor>().ApplyColor(lobbyPlayer.Color);

			go.GetComponent<Player>().SetID(lobbyIndex);
			

			readyUiObjects[lobbyIndex].gameObject.SetActive(true);

			GameModes.Instance.CurrentGameMode.SpawnBehaviour.InvokeBehaviour(go, lobbyIndex);
			
			
			SetupCamera(lobbyIndex, carController, caravanController);

			SetReadyText(lobbyIndex);
			botInfo[lobbyIndex].gameObject.SetActive(lobbyPlayer.IsBot);
			if (lobbyPlayer.IsBot)
			{
				carController.IsAi = true;
				carController.GetComponent<CarAi>().enabled = true;				
			}
			//carController.GetComponent<TextMeshPro>()?.gameObject.SetActive(carController.IsAi);
		}


		private void SetReadyText(int id)
		{
			var player = Lobby.GetPlayer(id);
			if (!Teams.HasGroups)
			{
				readyTexts[id].text = player.IsBot ? $"Bot {id + 1} ready" : $"Player {id + 1} ready";
			}
			
			else
			{
				int teamId = Lobby.GetPlayer(id).TeamId;
				readyTexts[id].text = $"{Teams.CurrentGroup.GetNameAt(teamId)} ready";
			}
		}

		private void SetupCamera(int lobbyIndex, CarController carController, CaravanController caravanController)
		{
			cameras[lobbyIndex].gameObject.SetActive(true);
			CinemachineTargetGroup targetGroup = cameras[lobbyIndex].GetComponentInChildren<CinemachineTargetGroup>();

			if (caravanController && caravanController)
			{
				targetGroup.AddMember(carController.transform, 1, 2);
				targetGroup.AddMember(caravanController.transform, 1, 2);
			}
			else
			{
				Debug.Log("Player spawn behaviour can not be triggered, due to missing car- or caravan-controller.");
			}
		}

		private void DestroyPlayer(int lobbyIndex)
		{
			foreach (Transform child in spawnPoints[lobbyIndex].transform)
			{
				Destroy(child.gameObject);
			}
		}
	}
}
