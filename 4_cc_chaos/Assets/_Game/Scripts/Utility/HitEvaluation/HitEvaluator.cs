//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Checks whether hit-metric has been satisfied,
	/// wherein the metric expression is displayed at the caravan's location.
	/// </summary>
	[System.Serializable]
	public class HitEvaluator
	{
		// Holds a list of metrics as scriptable objects
		[SerializeField] private List<HitEvaluationMetric> hitMetrics;

		/// <summary>
		/// Returns list of hit metrics that have been satisfied.
		/// </summary>
		/// <param name="hit"></param>
		public List<HitEvaluationMetric> Evaluate(Hit hit)
		{
			List<HitEvaluationMetric> satisfiedMetrics = new List<HitEvaluationMetric>();

			foreach (var hitMetric in hitMetrics)
			{
				if (hitMetric.IsSatisfied(hit))
				{
					satisfiedMetrics.Add(hitMetric);
				}
			}

			return satisfiedMetrics;
		}
	}
}
