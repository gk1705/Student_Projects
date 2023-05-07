//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class Health : MonoBehaviour
	{
		private int maxHealth = 0;
		public bool IsDead { get; private set; }
		public int CurrentHealth { get; private set; }

		public int MaxHealth => maxHealth;

		public delegate void LoseHealth(int idx);
		public event LoseHealth OnLoseHealth;


		private void Awake()
		{
			maxHealth = GameModes.Instance.CurrentGameMode.playerHealth;

			RefillHealth();
		}

		/// <summary>
		/// Fills the health back to the maxHealth
		/// </summary>
		public void RefillHealth()
		{
			IsDead = false;
			CurrentHealth = maxHealth;
		}

		/// <summary>
		/// Subtracts a specified amount from the current health if character is alive. Sets IsDead to true if health gets under 0.
		/// </summary>
		/// <param name="amount">amount of damage to take</param>
		/// <returns>Resulting health amount</returns>
		public int TakeDamage(int amount)
		{
			if (IsDead) return 0;

			CurrentHealth -= amount;
			OnLoseHealth?.Invoke(gameObject.GetComponent<Player>().GetID);
			if (CurrentHealth <= 0)
			{
				IsDead = true;
				CurrentHealth = 0;
			}
			return CurrentHealth;
		}

		/// <summary>
		/// Adds a specified amount of health if alive, clamps total health between 0 and maxHealth
		/// </summary>
		/// <param name="amount">amount of health to add</param>
		public void AddHealth(int amount)
		{
			if(IsDead) return;

			CurrentHealth += amount;
			Mathf.Clamp(CurrentHealth, 0, maxHealth);
		}

		/// <summary>
		/// Sets health to a specified amount if alive, also checks if health is under 0 and acts accordingly
		/// </summary>
		/// <param name="health">Amount the current health should be set to</param>
		public void SetHealth(int health)
		{
			if (IsDead) return;

			CurrentHealth = health;
			if (CurrentHealth <= 0)
			{
				IsDead = true;
				CurrentHealth = 0;
			}
		}

		

	}
}


