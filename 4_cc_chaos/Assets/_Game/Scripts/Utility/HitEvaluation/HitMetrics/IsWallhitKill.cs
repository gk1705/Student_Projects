using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

[CreateAssetMenu(fileName = "IsWallhitKill", menuName = "HitEvaluationMetric/IsWallhitKill")]
public class IsWallhitKill : HitEvaluationMetric
{
	public override bool IsSatisfied(Hit hit)
	{
		return hit.KillShot && hit.WallHits > 0;
	}
}
