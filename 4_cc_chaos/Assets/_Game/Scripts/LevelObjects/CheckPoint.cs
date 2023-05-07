//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher

using UnityEngine;
using UnityEngine.Assertions;

namespace CaravanCrashChaos
{
	public class CheckPoint : MonoBehaviour
	{
		[SerializeField] private int Idx;
		[SerializeField] private bool isFinal = false;

		private TrackManager trackManager;

		void Start()
		{
			trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
			Assert.IsNotNull(trackManager);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Car"))
			{
				Player player = other.gameObject.GetComponentInParent<Player>();
				trackManager.UpdatePlayerCheckPointIdx(Idx, player.GetID, isFinal);
			}
		}
	}
}

