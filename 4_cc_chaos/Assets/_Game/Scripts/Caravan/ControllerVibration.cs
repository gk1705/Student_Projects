//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Vibrates the controller depending on how much damage we got
	/// </summary>
	public class ControllerVibration : MonoBehaviour
	{
		[SerializeField] private ControllerShakeProfile shakeProfile;
		[Tooltip("If damage is higher than this the max vibration (1) will be applied")]
		[SerializeField] private float maxDamage = 200;
		private CaravanProfile caravanProfile;
		private Rewired.Player rewiredPlayer;
		private CaravanDamage caravanDamage;

		private void Start()
		{
			rewiredPlayer = transform.parent.GetComponent<CaravanController>().RewiredPlayer;
			caravanDamage = transform.parent.GetComponent<CaravanDamage>();
			Assert.IsNotNull(rewiredPlayer);
			Assert.IsNotNull(caravanDamage);
			Assert.IsNotNull(shakeProfile);
			caravanProfile = GameModes.Instance.CurrentGameMode.CaravanProfile;
			caravanDamage.OnDealDamage += Vibrate;

		}

		private void OnDisable()
		{
			if(caravanDamage != null)
				caravanDamage.OnDealDamage -= Vibrate;
		}

		private void Vibrate(Hit hit)
		{
			if (hit.KillShot || hit.Deflect) return;

			if(caravanProfile.VibrateOnDealDamage && !hit.FromBot) //don't vibrate if we are bot and dealt damage
				VibrationOnDealDamage(hit);
			if(caravanProfile.VibrateOnReceiveDamage)
				VibrationForHitPlayer(hit);

		}

		/// <summary>
		/// Applies vibration to the player that dealt damage
		/// </summary>
		private void VibrationOnDealDamage(Hit hit)
		{
			if (rewiredPlayer.controllers.joystickCount <= 0)
				return;
			var playerJoystick = rewiredPlayer.controllers.Joysticks[0];
			if (!playerJoystick.supportsVibration) return;

			float ratio = Mathf.Min(hit.Damage, maxDamage) / maxDamage; //amount on a scale of 0 to 1

			rewiredPlayer.ApplyVibrationProfile(shakeProfile, ratio * 10, shakeProfile.Length * ratio / 2);
		}

		/// <summary>
		/// Applies vibration to the player that received damage
		/// </summary>
		private void VibrationForHitPlayer(Hit hit)
		{
			var otherCar = hit.Collision.gameObject.GetComponent<CarController>();
			var otherCaravan = hit.Collision.gameObject.GetComponent<CaravanController>();
			
			Joystick otherPlayerJoyStick = null;
			Rewired.Player otherRewiredPlayer = null;
			if (otherCar && otherCar.RewiredPlayer.controllers.joystickCount > 0 && !otherCar.IsAi)
			{
				otherRewiredPlayer = otherCar.RewiredPlayer;
				otherPlayerJoyStick = otherCar.RewiredPlayer.controllers.Joysticks[0];
			}						
			else if (otherCaravan && otherCaravan.RewiredPlayer.controllers.joystickCount > 0 && !otherCaravan.Car.IsAi)
			{
				otherRewiredPlayer = otherCaravan.RewiredPlayer;
				otherPlayerJoyStick = otherCaravan.RewiredPlayer.controllers.Joysticks[0];
			}
				
			else
				return;

			if (!otherPlayerJoyStick.supportsVibration)
				return;

			float otherRatio = Mathf.Min(hit.Damage, maxDamage) / maxDamage; //amount on a scale of 0 to 1

			otherRewiredPlayer.ApplyVibrationProfile(shakeProfile, otherRatio, shakeProfile.Length * otherRatio);
		}
	}
}


