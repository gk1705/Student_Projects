using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

public class DamageTracker : MonoBehaviour
{
	[SerializeField] private GameObject caravan;
	private CaravanDamage caravanDamage;

	private float totalDamage = 0;

    void Start()
    {
	    caravanDamage = caravan.GetComponent<CaravanDamage>();
	    caravanDamage.OnDealDamage += TrackDamage;
    }

	public float GetTotalDamage()
	{
		return totalDamage;
	}

	public void TrackDamage(Hit hit)
	{
		if (hit.Deflect) return;	// deflections don't count as damage dealt

		totalDamage += hit.Damage;

		StatsTracker statsTracker = gameObject.GetComponent<StatsTracker>();
		statsTracker.AddScoreForDmg(hit.Damage);
		statsTracker.AddDamageScore(hit.Damage);

		if (hit.KillShot)
		{
			statsTracker.AddScoreForKill();
			statsTracker.AddKill();
		}
	}

}
