//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Information about a GameMode, can be accessed via GameModes.Instance.CurrentGameMode (if this is the current one)
	/// </summary>
	[CreateAssetMenu(fileName = "GameMode", menuName = "Custom/GameMode", order = 1)]
	public class GameMode : ScriptableObject
	{
		[Header("Info")]
		public string Name;
		public GameModeIdentifier GameModeIdentifier;
		[Required] public string SceneToLoad;
		[Header("ModeSettings")]
		[Required] public DrivingProfile DrivingProfile;
		[Required] public CaravanProfile CaravanProfile;
		public bool AllowDecorations = true;
		public bool AllowDamage = true;
		public int playerHealth;		
		[Required] public SpawnBehaviour SpawnBehaviour;
		public RankingMode rankingMode;	
		[Required] public DeathEffects DeathEffects;
		[Header("Bots")]
		public bool AllowBots = true;
		public BotDifficulty BotDifficulty;
		public enum RankingMode { Score, Damage, Survival, Kills, Goals };

		private void Awake()
		{
			Assert.IsNotNull(DrivingProfile);
			Assert.IsNotNull(CaravanProfile);
			Assert.IsNotNull(SpawnBehaviour);
			Assert.IsNotNull(DeathEffects);
			if(SceneToLoad.IsNullOrWhitespace()) Debug.LogError($"gamemode scene is not set");
		}
	}

	public enum GameModeIdentifier
	{
		LastVanStanding = 0,
		Hardcore = 1,
		HotPotato = 2,
		DeathRace = 3,
		Soccer = 4
	}
}


