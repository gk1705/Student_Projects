//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsDeflect", menuName = "HitEvaluationMetric/IsDeflect")]
	public class IsDeflect : HitEvaluationMetric
	{
		public override bool IsSatisfied(Hit hit)
		{
			return hit.Deflect;
		}
	}
}
