//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class ScaleOverTime : MonoBehaviour
	{
		[SerializeField] private float scaleFactor = 0.1f;
		[SerializeField] private float scaleTime = 3f;
		[SerializeField] private bool deactivateAfter = true;
		private Vector3 startScale;
		private Vector3 endScale;

		public void StartScaling(float waitBeforeScale)
		{		
			startScale = transform.localScale;
			endScale = startScale * scaleFactor;
			StartCoroutine(Scale(waitBeforeScale));
		}

		private IEnumerator Scale(float waitBeforeScale)
		{
			yield return new WaitForSeconds(waitBeforeScale);
			var time = 0f;
			while (time <= scaleTime)
			{
				transform.localScale = Vector3.Lerp(startScale, endScale, time / scaleTime);
				time += Time.deltaTime;
				yield return null;
			}

			if(deactivateAfter)
				gameObject.SetActive(false);
		}

	}
}


