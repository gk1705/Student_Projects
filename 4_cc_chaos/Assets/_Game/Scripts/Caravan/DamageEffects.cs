//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Exploder;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using GameObject = UnityEngine.GameObject;
using Hit = CaravanCrashChaos.Hit;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Handles the particle system that is spawned when damage is dealt
	/// </summary>
	public class DamageEffects : MonoBehaviour
	{
		[Header("Explosions")]
		[SerializeField] [Required] private GameObject defaultExplosion; //scriptable objects for scale?
		[SerializeField] [Required] private GameObject deathExplosion;

		// List of explosion prefabs
		[SerializeField] private List<GameObject> metricExplosionEffects;
		// At object start a mapping between a metric's expression and the explosion's name minus "Explosion"
		// Example: "AlmostExplosion" (becomes) -> "Almost"
		// When querying for an explosion, the metric's expression property is used
		// Example: One of our's has "Almost" as property expression
		private Dictionary<string, GameObject> explosionEffectExpressionMapping;

		[Header("Settings")]
		[Tooltip("If the damage is higher than this, the text for the explosion is shown")]
		[SerializeField] private float damageThreshold;
		[Tooltip("If the damage is higher than this, the explosion will not get any bigger")]
		[SerializeField] private float maxDamage = 200f;
		[SerializeField] private float scalingFactor = 5f;

		// Evaluates hits upon calling the ShowDamageEffects and returns what metrics have been met
		[SerializeField] private HitEvaluator hitEvaluator;

		private CaravanDamage caravanDamage;

		private void Awake()
		{
			explosionEffectExpressionMapping = new Dictionary<string, GameObject>();
		}

		private void Start()
		{
			// mapping the hit expression (explosion name minus "Explosion") to the explosion game object
			foreach (var metricExplosionEffect in metricExplosionEffects)
			{
				string effectExpression = metricExplosionEffect.name.Replace("Explosion", "");
				explosionEffectExpressionMapping.Add(effectExpression, metricExplosionEffect);
			}
		}

		private void OnEnable()
		{
			caravanDamage = transform.parent.GetComponent<CaravanDamage>();
			Assert.IsNotNull(caravanDamage);
			Assert.IsNotNull(defaultExplosion);
			caravanDamage.OnDealDamage += ShowDamageEffects;
		}

		private void OnDisable()
		{
			caravanDamage.OnDealDamage -= ShowDamageEffects;
		}

		/// <summary>
		/// Spawns the explosion and modifies its size and the size of the child particlesystems according to the damage dealt
		/// </summary>
		/// <param name="collision">the collision from when the damage occurred</param>
		/// <param name="damage">How much damage was dealt</param>
		private void ShowDamageEffects(Hit hit)
		{
			// Get which metrics have been satisfied
			HitEvaluationMetric metric = GetHighestMetric(hitEvaluator.Evaluate(hit));

			// Always display killshot metric
			// Otherwise if metric has been met and damage threshold has been reached
			if (metric != null && (hit.KillShot || hit.Damage >= damageThreshold))
			{
				SpawnMetricExplosion(metric, hit);
				SoundEffectsManager.Instance.PlaySoundEffect(metric.Expression);

				if (metric.HasVoiceLine)
				{
					Announcer.Instance.ForceVoiceLine(metric.Expression);
				}
			}
			// If none of that holds, display either default, or "BANG" explosion
			else
			{
				SpawnExplosion(hit);
			}
		}

		private void SpawnExplosion(Hit hit)
		{
			var selectedExplosion = SelectExplosion(hit);
			var spawnedExplosion = Instantiate(selectedExplosion.explosion, hit.Collision.contacts[0].point, Quaternion.identity, transform);
			ScaleExplosion(spawnedExplosion, selectedExplosion.scaleWithDmg, hit);

			spawnedExplosion.GetComponent<ParticleSystem>().Play(true); //play particlesystem with all children
		}

		private void SpawnMetricExplosion(HitEvaluationMetric metric, Hit hit)
		{
			var spawnedExplosion = Instantiate(explosionEffectExpressionMapping[metric.Expression], hit.Collision.contacts[0].point, Quaternion.identity, transform);
			ScaleExplosion(spawnedExplosion, false, hit);

			spawnedExplosion.GetComponent<ParticleSystem>().Play(true); //play particlesystem with all children
		}

		/// <summary>
		/// Highest metric is sought
		/// If two metrics share the same priority, the first in the list of metrics is used
		/// Returns null if no metric has been met
		/// </summary>
		/// <param name="hitMetrics"></param>
		/// <returns></returns>
		private HitEvaluationMetric GetHighestMetric(List<HitEvaluationMetric> hitMetrics)
		{
			if (hitMetrics.Count == 0) return null;

			var highestPriority = hitMetrics.Max(x => x.Priority);
			var highestPriorityElement = hitMetrics.Where(y => y.Priority == highestPriority);
			var highestPriorityIndex = hitMetrics.IndexOf(highestPriorityElement.ToList()[0]); // if two elements share the same priority, take the first one
			
			return hitMetrics[highestPriorityIndex];
		}

		//Multiple return values
		private (GameObject explosion, bool scaleWithDmg) SelectExplosion(Hit hit)
		{
			if (hit.KillShot)
			{
				return (deathExplosion, false);
			}
				
			return (defaultExplosion, true);
		}

		private void ScaleExplosion(GameObject spawnedExplosion, bool scaleWithDamage, Hit hit)
		{
			var damageMultiplier = Mathf.Min(hit.Damage, maxDamage) / maxDamage; //damage on a scale of 0 to 1
			if (!scaleWithDamage) damageMultiplier = 1f;

			if (scaleWithDamage && hit.Damage < damageThreshold)
				spawnedExplosion.GetComponent<ParticleSystemRenderer>().enabled = false; //don't show the text when damage is too low

			ScaleParticleSystemsByMultiplier(spawnedExplosion, damageMultiplier);
		}

		private void ScaleParticleSystemsByMultiplier(GameObject spawnedExplosion, float sizeMultiplier)
		{
			var systems = spawnedExplosion.GetComponentsInChildren<ParticleSystem>();
			foreach (var system in systems)
			{
				var multiplier = sizeMultiplier * scalingFactor;
				var newMin = system.main.startSize.constantMin * multiplier;
				var newMax = system.main.startSize.constantMax * multiplier;
				var newConstant = system.main.startSize.constant * multiplier;
				var mainModule = system.main;

				//select scaling mode for each particle variant, startSizeMultiplier is broken btw
				if (mainModule.startSize.mode == ParticleSystemCurveMode.Constant)
					mainModule.startSize = newConstant;
				else if (mainModule.startSize.mode == ParticleSystemCurveMode.TwoConstants)
					mainModule.startSize = new ParticleSystem.MinMaxCurve(newMin, newMax);
				else if (mainModule.startSize.mode == ParticleSystemCurveMode.Curve || mainModule.startSize.mode == ParticleSystemCurveMode.TwoCurves)
				{
					var startSize = mainModule.startSize;
					startSize.curveMultiplier = multiplier;
				}
			}
		}
	}
}


