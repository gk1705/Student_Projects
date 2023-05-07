//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "DeathEffects", menuName = "Custom/DeathEffects", order = 1)]
	public class DeathEffects : ScriptableObject
	{
		[Header("Exploder")]
		public bool ExplodeCar = true, ExplodeCaravan = true;
		public ExploderProfile ExploderProfile;
		[Header("ScreenShake")]
		public bool ScreenShake = true;
		[ShowIf("ScreenShake")]
		public ScreenShakeProfile ScreenShakeProfile;
		[Header("SlowMo")]
		public bool SlowMo = true;
		[ShowIf("SlowMo")] public SlowMoProfile SlowMoProfile;
		[FormerlySerializedAs("GameOverSlowMo")] [ShowIf("SlowMo")] public bool PlayEndSlowMo = true;
		[ShowIf("SlowMo")] [ShowIf("PlayEndSlowMo")] public SlowMoProfile GameEndSlowMo;
		[Header("ControllerShake")]
		public bool ControllerShake = true;
		[ShowIf("ControllerShake")]
		public ControllerShakeProfile ControllerShakeProfile;

		private void Awake() //is called when 1: scriptable object is created, 2: a scene with a reference to it is loaded, 3: if selected in editor and awake wasn't called
		{
			if(ExplodeCar || ExplodeCaravan) Assert.IsNotNull(ExploderProfile);
			if(ScreenShake) Assert.IsNotNull(ScreenShakeProfile);
			if(SlowMo) Assert.IsNotNull(SlowMoProfile);
			if(PlayEndSlowMo) Assert.IsNotNull(GameEndSlowMo);
			if(ControllerShake) Assert.IsNotNull(ControllerShakeProfile);
		}
	}
}


