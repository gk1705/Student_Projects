//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsAlmostKill", menuName = "HitEvaluationMetric/IsAlmostKill")]
	public class IsAlmostKill : HitEvaluationMetric
	{
		[SerializeField] private int remainingHealthThreshold;

		public override bool IsSatisfied(Hit hit)
		{
			return !hit.KillShot && hit.ResultingHealth <= remainingHealthThreshold;
		}
	}
}
