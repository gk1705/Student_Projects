//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	/// <summary>
	/// UI field in the lobby, initializes player when joining, and handles leaving (ready, unready)
	/// </summary>
	public class SelectionField : MonoBehaviour
	{
		[SerializeField] private Text pressToJoinText;
		[SerializeField] private GameObject readyButton;
		[SerializeField] private GameObject controls;
		[SerializeField] private Text teamName;
		[SerializeField] private Text isBotText;
		[SerializeField] private GameObject teamNameParent;
		[SerializeField] private GameObject teamSelectText;

		private int playerId; //id that controls this field
		private Rewired.Player rewiredPlayer;
		public bool ready;
		public bool IsBot;
		private Image backgroundImage;

		public PlayerCustomizationController PCustomizationController { get; set; }

		void Start()
		{
			playerId = -1;
			Assert.IsNotNull(pressToJoinText);
			backgroundImage = gameObject.GetComponent<Image>();
			teamNameParent.SetActive(false);

			if(!Teams.HasGroups) //deactivate ui text for selecting teams when no teams
				teamSelectText.SetActive(false);
		}

		public void SetTeamName(int teamIndex)
		{
			teamName.text = $"Team: {Teams.CurrentGroup.GetNameAt(teamIndex)}";
		}

		public void JoinPlayer(int id, Color color, bool isBot = false)
		{
			
			playerId = id;
			rewiredPlayer = !isBot ? ReInput.players.GetPlayer(playerId) : null;
			if(!isBot) pressToJoinText.enabled = false;
			Lobby.AddPlayer(new LobbyPlayer(id, color));
			backgroundImage.enabled = false;
			readyButton.gameObject.SetActive(true);
			controls.gameObject.SetActive(true);
			isBotText.enabled = false;
			Debug.Log($"{Teams.Groups.Count}");
			if (Teams.Groups.Count > 0)
			{
				teamNameParent.SetActive(true);
				SetTeamName(Lobby.GetPlayer(id).TeamId);
			}

			IsBot = isBot;
			if(isBot)
				SetAsBot();

			Debug.Log($"set id for {gameObject.name} to {id}");
		}

		public void SetAsBot()
		{
			var player = Lobby.GetPlayer(playerId);
			player.IsBot = true;
			isBotText.enabled = true;
			IsBot = true;
		}

		public void UnSetBot()
		{
			var player = Lobby.GetPlayer(playerId);
			player.IsBot = false;
			isBotText.enabled = false;
			IsBot = false;
		}

		public void Ready(Color color)
		{
			ready = true;
		}

		public void UnReady()
		{
			ready = false;
		}

		public void Leave()
		{
			PCustomizationController.TogglePlayerModel(playerId);
			pressToJoinText.enabled = true;
			if (IsBot) UnSetBot();
			Lobby.RemovePlayer(playerId);
			backgroundImage.enabled = true;
			readyButton.gameObject.SetActive(false);
			controls.gameObject.SetActive(false);
			teamNameParent.SetActive(false);
			
			playerId = -1;
			ready = false;
			
		}



		public bool HasId => playerId != -1;
	}
}


