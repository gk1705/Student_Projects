//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaravanCrashChaos
{
	public class SoccerBall : MonoBehaviour
	{
		public Player LastTouched { get; private set; } = null;

		[SerializeField] private Transform startPosition;
		public bool CanScore { get; private set; }
		private new Rigidbody rigidbody;

		private new Renderer renderer;
		// Start is called before the first frame update
		void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			renderer = GetComponent<Renderer>();
			ResetBall();
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		/// <summary>
		/// resets ball to start position and state
		/// </summary>
		public void ResetBall()
		{
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.velocity = Vector3.zero;
			transform.position = startPosition.position;
			LastTouched = null;
			renderer.material.color = Color.white;
			gameObject.SetActive(true);
			GetComponent<Collider>().enabled = true;
			rigidbody.isKinematic = false;
			CanScore = true;
		}

		public void DisableScoring()
		{
			CanScore = false;
		}

		private void OnCollisionEnter(Collision other)
		{
			//assign last touched player
			if (!other.gameObject.CompareTag("Car") && !other.gameObject.CompareTag("Caravan"))
				return;

			CaravanController caravan = other.gameObject.GetComponent<CaravanController>();
			CarController car = other.gameObject.GetComponent<CarController>();
			Player otherPlayer = null;


			if (caravan)
				otherPlayer = caravan.GetComponentInParent<Player>();
			else if (car)
				otherPlayer = car.GetComponentInParent<Player>();

			if (otherPlayer)
			{
				LastTouched = otherPlayer;
				renderer.material.color = LastTouched.playerColor;
			}
				

		}
	}
}


