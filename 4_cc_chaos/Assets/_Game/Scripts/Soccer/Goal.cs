//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kvant;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class Goal : MonoBehaviour
	{

		[SerializeField] private int goalId;
		[SerializeField] private bool reduceScoreOnGoalReceived = true;
		[SerializeField] private float waitUntilReset = 2f;
		[SerializeField] private ScreenShakeProfile goalShake;
		[SerializeField] private SlowMoProfile slowMoProfile;
		[Required] [SerializeField] private GameObject barricade;
		[SerializeField] private GameObject particles;
		[SerializeField] private AudioSource goalSound;
		[Header("UI")]
		[SerializeField] private Canvas worldSpaceCanvas;
		[SerializeField] private Image colorImage;
		[SerializeField] private Text goalsText;

		public Player GoalOwner { get; private set; } = null;
		private GameManager gameManager;
		private CameraShake cameraShake;
		private SlowMotion slowMotion;
		private List<Goal> goals = new List<Goal>();
		public Text GoalsText => goalsText;
		// Start is called before the first frame update
		void Start()
		{
			FindObjectOfType<GameManager>().OnStartGame += SetupGoal;
			gameManager = FindObjectOfType<GameManager>();
			cameraShake = FindObjectOfType<CameraShake>();
			slowMotion = FindObjectOfType<SlowMotion>();
			goals = FindObjectsOfType<Goal>().ToList();
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.gameObject.CompareTag("Ball"))
				return;

			SoccerBall ball = other.gameObject.GetComponent<SoccerBall>();

			if (!ball.CanScore) return;

			Debug.Log($"goal scored by {ball.LastTouched.gameObject.name} on {GoalOwner.gameObject.name}");

			bool ownGoal = ball.LastTouched == GoalOwner;

			if(!ownGoal)
				ball.LastTouched?.GetComponent<StatsTracker>().AddGoal();

			if(reduceScoreOnGoalReceived)
				GoalOwner?.GetComponent<StatsTracker>().RemoveGoal();

			UpdateWorldSpaceScores(ball.LastTouched, GoalOwner);

			slowMotion.StartSlowMo(slowMoProfile.Length, slowMoProfile.Speed);
			StartCoroutine(cameraShake.Shake(goalShake.Length, goalShake.Amplitude, goalShake.Frequency));
			StartCoroutine(ResetGame(ball));
			Announcer.Instance.ForceVoiceLine("Goal");
			goalSound.Play();
			
		}

		/// <summary>
		/// get goals from players and set the worldspace texts
		/// </summary>
		/// <param name="scorer"></param>
		/// <param name="receiver"></param>
		private void UpdateWorldSpaceScores(Player scorer, Player receiver)
		{
			foreach (var goal in goals)
			{
				if (goal.GoalOwner == scorer)
					goal.GoalsText.text = scorer.GetComponent<StatsTracker>().goals.ToString();
				else if(goal.GoalOwner == receiver)
					goal.GoalsText.text = receiver.GetComponent<StatsTracker>().goals.ToString();
			}
		}

		/// <summary>
		/// Reset the ball and activate goal effects
		/// </summary>
		/// <param name="ball"></param>
		/// <returns></returns>
		IEnumerator ResetGame(SoccerBall ball)
		{
			foreach (var spray in particles.GetComponentsInChildren<Spray>()) //assign particle color
			{
				spray.material.color = ball.LastTouched.playerColor;
			}
			particles.SetActive(true);
			ball.DisableScoring();
			yield return new WaitForSecondsRealtime(waitUntilReset);
			ball.ResetBall();
			particles.SetActive(false);
		}

	

		/// <summary>
		/// Configures the goal on start
		/// </summary>
		public void SetupGoal()
		{
			var players = FindObjectsOfType<Player>().ToList();

			foreach (var player in players) //find matching goal to player
			{
				if (player.GetID == goalId)
				{
					GoalOwner = player;
					barricade.SetActive(false);
					Debug.Log($"assigned goal {goalId} player {player.gameObject.name}");
					worldSpaceCanvas.enabled = true;
					colorImage.color = GoalOwner.playerColor;
					goalsText.text = $"0";
				}				
			}

			if (GoalOwner == null)
			{
				Debug.Log($"didn't find owner for goal {goalId}");
				barricade.SetActive(true);
				worldSpaceCanvas.enabled = false;
			}							
		}
	}
}


