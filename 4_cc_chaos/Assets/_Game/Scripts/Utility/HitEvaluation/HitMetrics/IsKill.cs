//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsKill", menuName = "HitEvaluationMetric/IsKill")]
	public class IsKill : HitEvaluationMetric
	{
		public override bool IsSatisfied(Hit hit)
		{
			return hit.KillShot;
		}
	}
}
