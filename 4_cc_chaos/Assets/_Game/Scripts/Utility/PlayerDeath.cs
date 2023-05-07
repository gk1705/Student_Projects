using System;
using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using Cinemachine;
using Exploder;
using Exploder.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerDeath : MonoBehaviour
{
	[Required] [SerializeField] private Health health;
	[Required] [SerializeField] private GameObject car;
	[Required] [SerializeField] private GameObject caravan;

	public delegate void PlayerDies(int playerId);
	public event PlayerDies OnPlayerDies;

	private ExploderObject Exploder;
	private bool destroyed = false;
	private SlowMotion slowMotion;
	private CameraShake cameraShake;

	void Start()
	{
		Exploder = ExploderSingleton.Instance;
		slowMotion = FindObjectOfType<SlowMotion>();
		cameraShake = FindObjectOfType<CameraShake>();
	}

	void Update()
    {
	    if (health.IsDead && !destroyed)
	    {
			HandleDeath();
		    destroyed = true;
	    }
    }

	/// <summary>
	/// Explode car, caravan, do slowmo and screenshake
	/// </summary>
	private void HandleDeath()
	{
		OnPlayerDies?.Invoke(gameObject.GetComponent<Player>().GetID);
		gameObject.GetComponent<StatsTracker>().HasDied();

		var deathEffects = GameModes.Instance.CurrentGameMode.DeathEffects;
		var shakeProfile = deathEffects.ScreenShakeProfile;
		var slowMoProfile = deathEffects.SlowMoProfile;
		var gameOverSlowMo = deathEffects.GameEndSlowMo;
		var rewiredPlayer = car.GetComponent<CarController>().RewiredPlayer;
		bool gameOver = FindObjectOfType<GameManager>().IsGameOver;
		bool wasBot = car.GetComponent<CarController>().IsAi;

		//remove dead player from target group
		var cineTargetGroup = FindObjectOfType<CinemachineTargetGroup>();

		cineTargetGroup?.RemoveMember(car.transform);
		cineTargetGroup?.RemoveMember(caravan.transform);


		//death effects
		if (deathEffects.ControllerShake && rewiredPlayer.controllers.Joysticks.Count > 0 && rewiredPlayer.controllers.Joysticks[0].supportsVibration && !wasBot)
		{
			rewiredPlayer.ApplyVibrationProfile(deathEffects.ControllerShakeProfile);
		}
		
		if(deathEffects.ExplodeCar)
			ExplodeTheObject(car, deathEffects.ExploderProfile);

		if (deathEffects.ExplodeCaravan)
			ExplodeTheObject(caravan, deathEffects.ExploderProfile);

		if(deathEffects.ScreenShake)
			StartCoroutine(cameraShake?.Shake(shakeProfile.Length, shakeProfile.Amplitude, shakeProfile.Frequency));

		if(gameOver && deathEffects.PlayEndSlowMo) //play other slowmo on game end
			slowMotion?.StartSlowMo(gameOverSlowMo.Length, gameOverSlowMo.Speed);
		else if (deathEffects.SlowMo)
			slowMotion?.StartSlowMo(slowMoProfile.Length, slowMoProfile.Speed);
	}


	void ExplodeTheObject(GameObject target, ExploderProfile profile)
	{
		// activate exploder and set position
		Exploder.gameObject.SetActive(true);
		var centroid = ExploderUtils.GetCentroid(target);
		Exploder.transform.position = centroid;
		Exploder.ExplodeSelf = false;

		profile.SetExploderVariables(Exploder, profile, Vector3.zero);	

		// explode
		Exploder.ExplodeObject(target);
		ExploderUtils.SetActive(Exploder.gameObject, true);
	}
}
