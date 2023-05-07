//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsHighSpeed", menuName = "HitEvaluationMetric/IsHighSpeed")]
	public class IsHighSpeed : HitEvaluationMetric
	{
		[SerializeField] private float speedThreshold;

		public override bool IsSatisfied(Hit hit)
		{
			return hit.Speed >= speedThreshold;
		}
	}
}
