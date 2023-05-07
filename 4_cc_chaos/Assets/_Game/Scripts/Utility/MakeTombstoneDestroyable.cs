//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CaravanCrashChaos
{
	public class MakeTombstoneDestroyable : MonoBehaviour
	{
		[SerializeField]
		private float waitTime;

		IEnumerator coroutine;

		// Start is called before the first frame update
		void Awake()
		{
			SetComponentsActive(false);
			coroutine = WaitAndSetActive();
			StartCoroutine(coroutine);
		}

		private IEnumerator WaitAndSetActive()
		{
			yield return new WaitForSeconds(waitTime);
			SetComponentsActive(true);
		}

		private void SetComponentsActive(bool isActive)
		{
			Debug.Log("SetComponents :" + isActive);

			gameObject.GetComponent<Destroyable>().enabled = isActive;

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				gameObject.transform.GetChild(i).GetComponent<Rigidbody>().detectCollisions = isActive;
				gameObject.transform.GetChild(i).GetComponent<Rigidbody>().useGravity = isActive;
				gameObject.transform.GetChild(i).GetComponent<BoxCollider>().enabled = isActive;
			}
		}
	}
}
