//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsHighDamage", menuName = "HitEvaluationMetric/IsHighDamage")]
	public class IsHighDamage : HitEvaluationMetric
	{
		[SerializeField] private float damageThreshold;

		public override bool IsSatisfied(Hit hit)
		{
			return hit.Damage >= damageThreshold;
		}
	}
}
