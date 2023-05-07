//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	[CreateAssetMenu(fileName = "IsLongTimeTravelled", menuName = "HitEvaluationMetric/IsLongTimeTravelled")]
	public class IsLongTimeTravelled : HitEvaluationMetric
	{
		[SerializeField] private float travelTimeThreshold;

		public override bool IsSatisfied(Hit hit)
		{
			return hit.TravelTime >= travelTimeThreshold;
		}
	}
}
