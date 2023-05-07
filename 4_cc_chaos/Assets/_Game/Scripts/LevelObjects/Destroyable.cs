//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CaravanCrashChaos
{
	/// <summary>
	/// Applies an explosion force to all Rigidbodies located in the children of this object
	/// </summary>
	[RequireComponent(typeof(Collider))]
	[TypeInfoBox("Applies an explosion force to all Rigidbodies located in the children of this object")]
	public class Destroyable : MonoBehaviour
	{
		[Tooltip("Use a placeholder object and activate the extra exploding object when it should")]
		[SerializeField] private bool useSecondObject;
		[Tooltip("Placeholder object that can't explode")]
		[ShowIf("useSecondObject")] [SerializeField] private GameObject firstObject;
		[Tooltip("The object that can explode")]
		[ShowIf("useSecondObject")] [SerializeField] private GameObject secondObject;
		[Header("Profile")]
		[Required] [SerializeField] private DestroyableProfile profile;
		[SerializeField] private UnityEvent onDestroyed;
		[SerializeField] private bool vanishObjectsAfter = true;
		[ShowIf("vanishObjectsAfter")] [SerializeField] private float timeUntilVanish = 5f;
		private List<Rigidbody> rigidbodies = new List<Rigidbody>();

		private void Start()
		{

			Assert.IsNotNull(profile);
			GetComponent<Collider>().enabled = true;
			GetComponent<Collider>().isTrigger = true;

			ConfigureRigidbodies(transform);

			if (useSecondObject)
			{
				firstObject.SetActive(true);
				secondObject.SetActive(false);
			}		
		}

		/// <summary>
		/// Applies initial settings for each rigidbody before it explodes
		/// </summary>
		/// <param name="transformToSearch"></param>
		private void ConfigureRigidbodies(Transform transformToSearch)
		{
			//configure rigidbodies
			for (int i = 0; i < transformToSearch.childCount; i++)
			{
				var rigidbody = transformToSearch.GetChild(i).GetComponent<Rigidbody>();
				if (rigidbody == null) continue;
				rigidbodies.Add(rigidbody);
				rigidbody.isKinematic = true;
				rigidbody.GetComponent<Collider>().enabled = false;
				profile.ApplyRigidbodySettings(rigidbody);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.CompareTag("Car") && !other.gameObject.CompareTag("Caravan") && !other.gameObject.CompareTag("Ball"))
				return;
		
			if (useSecondObject)
			{
				firstObject.SetActive(false);
				secondObject.SetActive(true);
				ConfigureRigidbodies(secondObject.transform); //apply here because object is inactive before this
			}
			
			GetComponent<Collider>().enabled = false;
			Explode(other.GetComponent<Rigidbody>().velocity.normalized);
		}

		private void Explode(Vector3 direction)
		{
			onDestroyed?.Invoke();
			for (int i = 0; i < rigidbodies.Count; i++)
			{
				rigidbodies[i].isKinematic = false;
				rigidbodies[i].GetComponent<Collider>().enabled = true;

				var randomDegree = Random.Range(-profile.MaxRandomAngle, profile.MaxRandomAngle);
				var randomRotation = Quaternion.Euler(randomDegree, randomDegree/4, randomDegree);
				direction = Quaternion.Inverse(randomRotation) * direction; //rotate vector by random rotation
				rigidbodies[i].AddForce(direction * profile.DirectionStrength);
				
				if(vanishObjectsAfter)
					rigidbodies[i].GetComponent<ScaleOverTime>()?.StartScaling(timeUntilVanish);
			}
		}
	}
}


