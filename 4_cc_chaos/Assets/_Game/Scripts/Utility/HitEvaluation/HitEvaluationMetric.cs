//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using UnityEngine;

namespace CaravanCrashChaos
{
	/*--hit-metrics example

		 ----------------------

		 CAUSE:							|	METRIC:								  |		POSSIBLE TEXT EFFECT:
		--lots of damage				|	hit damage >= damage threshold		  |		???
		--little remaining health		|	remaining health <= RH threshold	  |		almost kill
		--killshot						|	killshot == true					  |		killshot
		--long distance					|	travel distance >= dist. threshold	  |		snipe
		--highspeed						|	speed >= speed threshold			  |		highspeed
		--long time travelled			|	etc..								  |		commuter
		--wallhits and kill				|										  |		band kill
		--multiple kills in one shot	|										  |		double kill, tripple kill, etc.

		----------------------

	*/

	/// <summary>
	/// --Abstract Scriptable Object--
	/// Metric to determine whether shot has been extraordinary.
	/// </summary>
	public abstract class HitEvaluationMetric : ScriptableObject
	{
		[Tooltip("Useful for mapping to effects.")]
		public string Expression;
		[Tooltip("When two hit metrics are satisfied, the one with the higher priority will be chosen.")]
		public int Priority;
		[Tooltip("For when metric is accompanied by a voice line.")]
		public bool HasVoiceLine;

		public virtual bool IsSatisfied(Hit hit)
		{
			return false;
		}
	}
}
