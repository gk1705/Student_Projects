//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsWallhit", menuName = "HitEvaluationMetric/IsWallhit")]
	public class IsWallhit : HitEvaluationMetric
	{
		public override bool IsSatisfied(Hit hit)
		{
			return hit.WallHits > 0;
		}
	}
}
