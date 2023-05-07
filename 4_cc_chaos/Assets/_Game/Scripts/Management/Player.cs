//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// manages references to both car and caravan, should be placed on a separate parent object
	/// </summary>
	public class Player : MonoBehaviour
	{
		[SerializeField] int id = 0;
		public int GetID => id;
		public GameObject playerItem { get; set; }
		public Color playerColor;
		public CarController CarController { get; private set; }
		public CaravanController CaravanController { get; private set; }
		public CaravanDamage CaravanDamage { get; private set; }
		public GameObject CarCrown;


		void Awake()
		{
			CarController = GetComponentInChildren<CarController>();
			CaravanController = GetComponentInChildren<CaravanController>();
			CaravanDamage = GetComponentInChildren<CaravanDamage>();
			CarCrown.SetActive(false);
		}

		public void SetID(int id)
		{
			this.id = id;
		}
	}
}
