//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Stores info about a shot made with a caravan
	/// </summary>
	public class Hit
	{
		public Collision Collision;
		public float Damage, Distance, Speed, TravelTime; //todo make sure long distance with low velocity doesn't do much damage
		public int ResultingHealth, WallHits;
		public bool KillShot, Deflect, FromBot;
		/// The caravan that produced the hit
		public CaravanController Caravan;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="col"></param>
		/// <param name="damage"></param>
		/// <param name="resultingHealth"></param>
		/// <param name="kill">did the player that was hit die?</param>
		/// <param name="speed"></param>
		/// <param name="caravan">The caravan that produced the hit</param>
		public Hit(Collision col, float damage, int resultingHealth, bool kill, float speed, CaravanController caravan, bool deflect = false)
		{
			Collision = col;
			Damage = damage;
			ResultingHealth = resultingHealth;
			KillShot = kill;
			Distance = caravan.DistanceMoved;
			Speed = speed;
			TravelTime = caravan.TimeTraveled;
			WallHits = caravan.WallHits;
			Caravan = caravan;
			Deflect = deflect;
			FromBot = caravan.Car.IsAi;
		}

		public override string ToString()
		{
			return $"hit {Collision.gameObject.name} for {Damage:F3} damage over {Distance:F2}m with {Speed:F2}m/s, Result: {ResultingHealth} health, kill: {KillShot} Caravan traveled {TravelTime:F3}s and hit the walls {WallHits} times";
		}
	}
}
