//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class HotPotato : MonoBehaviour
	{

		[SerializeField] private Decoration bomb;
		private List<Player> players = new List<Player>();
		private Bomb spawnedBomb;

		// Start is called before the first frame update
		void Start()
		{					
			FindObjectOfType<GameManager>().OnStartGame += SelectNextPlayer;
		}

		private List<Player> GetLivingPlayers()
		{
			players = FindObjectsOfType<Player>().ToList();
			return players.Where(p => p.GetComponent<Health>().IsDead == false).ToList();
		}

		/// <summary>
		/// Select a random living player to carry the bomb
		/// </summary>
		public void SelectNextPlayer()
		{
			var livingPlayers = GetLivingPlayers();
			if (livingPlayers.Count <= 1)
			{
				Debug.LogWarning($"cant select another player, all others are dead");
				return;
			}

			var randomIndex = Random.Range(0, livingPlayers.Count);
			var player = livingPlayers[randomIndex];
			player.GetComponent<DecorationChanger>().ChangeDecoration(bomb);
			player.CaravanController.ForceAttachAndActivate();
			if(player.CarController.IsAi)
				player.CaravanController.GetComponent<CaravanAi>().enabled = true;
			player.CaravanController.ResetAttachTimer();
			spawnedBomb = FindObjectOfType<Bomb>();
			spawnedBomb.StartBurning();
			spawnedBomb.Owner = livingPlayers[randomIndex];

		}


	}
}


