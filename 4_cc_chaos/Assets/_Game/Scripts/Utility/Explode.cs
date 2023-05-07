using System.Collections;
using System.Collections.Generic;
using CaravanCrashChaos;
using Exploder;
using Exploder.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

public class Explode : MonoBehaviour {

    private ExploderObject Exploder;

	[Tooltip("Tags that we want to collide with")]
	[SerializeField] private List<string> collideWithTags = new List<string>() {"Car", "Caravan"};
	[Required]
	[SerializeField] private ExploderProfile profile;

    void Start()
    {
        Exploder = ExploderSingleton.Instance;
		Assert.IsNotNull(profile);
    }

    void OnCollisionEnter(Collision collision)
    {
		if (!ValidCollision(collision.gameObject.tag)) return;
		ExplodeTheObject(collision.gameObject.GetComponent<Rigidbody>());
	}

    void OnTriggerEnter(Collider collision)
    {
	    if (!ValidCollision(collision.gameObject.tag)) return;
	    ExplodeTheObject(collision.gameObject.GetComponent<Rigidbody>());
	}

	/// <summary>
	/// Check if we collide with an object that has the right tag
	/// </summary>
	/// <param name="tag"></param>
	/// <returns></returns>
	private bool ValidCollision(string tag)
	{
		if (collideWithTags.Contains(tag))
			return true;
		return false;
	}

    void ExplodeTheObject(Rigidbody rigidBody)
	{
		// activate exploder and set position
		Exploder.gameObject.SetActive(true);
		var centroid = ExploderUtils.GetCentroid(gameObject);
		Exploder.transform.position = centroid;
		Exploder.ExplodeSelf = false;

		profile.SetExploderVariables(Exploder, profile, rigidBody.velocity.normalized);

		// explode
		Exploder.ExplodeObject(gameObject);
		ExploderUtils.SetActive(Exploder.gameObject, true);
	}
}
