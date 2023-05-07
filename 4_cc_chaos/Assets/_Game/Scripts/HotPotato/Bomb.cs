//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class Bomb : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] private float fuseTime = 10f;
		[Tooltip("The speed the the bomb starts growing with and the max the it will gradually reach over time")]
		[MinMaxSlider(0.1f, 10, true)] [SerializeField] private Vector2 speed;
		[Tooltip("The maximum grow size it will reach (x = when timer is full, y = timer is finished)")]
		[MinMaxSlider(1, 5, true)] [SerializeField] private Vector2 maxSize;
		[Tooltip("min max audio interval over time")]
		[MinMaxSlider(0.01f, 1f, true)] [SerializeField] private Vector2 audioWait;
		[Tooltip("Time after a hit that the owner cannot be switched")]
		[SerializeField] private float coolDown = 0.3f;
		[SerializeField] private Color flashColor = Color.red;
		[SerializeField] private float colorFlashSpeedMultiplier = 10f;
		[Tooltip("Percentage (0 to 1) under which the color will begin to flash")]
		[SerializeField] [Range(0f, 1f)] private float colorFlashThreshold = 0.2f;
		[Space(10)]
		[SerializeField] private float shakeTime;
		[SerializeField] private float shakeAmplitude;
		[SerializeField] private float shakeFrequency;
		[Space(10)]
		[SerializeField] private ControllerShakeProfile shakeProfile;
		[SerializeField] private ControllerShakeProfile beepShakeProfile;


		[Header("References")]
		[SerializeField] [Required] private GameObject body;
		[SerializeField] [Required] private GameObject explosion; //explosion also has audio because this bomb gets destroyed every time it explodes
		[SerializeField] [Required] private Decoration bombDecoration;
		[SerializeField] [Required] private AudioSource audioSource;

		private bool exploded, isBaseColor;
		private float timer, timeSinceLastSwitch, timerRatio;

		private delegate void Exploded();

		private event Exploded OnExplosion;
		private Vector3 originalSize;
		private Player owner = null;

		private Material bombMaterial;
		private Color currentColor, baseColor;
		private HotPotato hotPotato;
		private Coroutine beepAudio = null;
		private float pingPongTimer = 0f;

		public Player Owner
		{
			get => owner;
			set
			{
				bool same = owner == value;
				owner = value;
				if (!same)
					OwnerChanged();

			}
		}

		private void OnDisable()
		{
			if(beepAudio != null) StopCoroutine(beepAudio);
			beepAudio = null;
		}

		public bool IsBurning { get; private set; }

		// Start is called before the first frame update
		void Start()
		{
			timer = fuseTime;
			originalSize = transform.localScale;
			bombMaterial = body.GetComponent<Renderer>().material;
			currentColor = bombMaterial.color;
			baseColor = currentColor;
			hotPotato = FindObjectOfType<HotPotato>();
			pingPongTimer = 0f;
			if(beepAudio == null)
				beepAudio = StartCoroutine(PlayAudio());
		}

		private void OwnerChanged()
		{
			if (Owner != null)
			{
				Owner.GetComponentInChildren<CaravanDamage>().OnDealDamage += OtherCaravanHit;
				var gameUiHandler = FindObjectOfType<GameUIHandler>();
				gameUiHandler.SetImages();
				gameUiHandler.SetColors();
				if(!Owner.CarController.IsAi)
					Owner.CarController.RewiredPlayer.ApplyVibrationProfile(shakeProfile);
				timeSinceLastSwitch = 0;
			}
		}


		/// <summary>
		/// Deactivtes the old owner, activates and sets up the new one when hit with caravan
		/// </summary>
		/// <param name="hit"></param>
		private void OtherCaravanHit(Hit hit)
		{
			if (exploded) return;

			var oldOwner = Owner;
			var hitPlayer = hit.Collision.gameObject.GetComponentInParent<Player>();
			if (hitPlayer == Owner || timeSinceLastSwitch < coolDown)
			{
				Debug.Log($"cant proceed with switching {hitPlayer == Owner}"); //don't proceed if we are already the owner or the cooldown hasn't passed
				return; 
			}
				
			//activate new caravan, switch deco, and deactivate old one
			var oldDecoChanger = oldOwner.GetComponentInChildren<DecorationChanger>();
			var oldDeco = oldDecoChanger.CurrentDecoration;
			oldDecoChanger.SetSprite(null);
			oldOwner.GetComponentInChildren<CaravanDamage>().OnDealDamage -= OtherCaravanHit;
			if (oldOwner.CarController.IsAi)
				oldOwner.CaravanController.GetComponent<CaravanAi>().enabled = false; //deactivate CaravanAi if bot
			oldOwner.CaravanController.Deactivate();

			hitPlayer.CaravanController.ForceAttachAndActivate();			
			hitPlayer.GetComponentInChildren<DecorationChanger>().AssignDecoration(oldDeco, bombDecoration.Icon);
			hitPlayer.CaravanController.ResetAttachTimer();
			Owner = hitPlayer;
			if(hitPlayer.CarController.IsAi)
				hitPlayer.CaravanController.GetComponent<CaravanAi>().enabled = true; //enable on new player if bot

			if (beepAudio == null) beepAudio = StartCoroutine(PlayAudio());
		}

		// Update is called once per frame
		void Update()
		{
			if (!IsBurning || !Owner) return;

			timer -= Time.deltaTime;
			pingPongTimer += Time.deltaTime;
			timeSinceLastSwitch += Time.deltaTime;
			timerRatio = Mathf.Clamp01(timer / fuseTime);

			if (timer <= 0 && !exploded)
			{
				Explode();
			}
			GrowAndShrink();
		}

		private void GrowAndShrink()
		{
			var speedMulti = Mathf.Lerp(speed.y, speed.x, timerRatio); //timeratio goes from 1 torwards 0, therefor switch y and x
			var maxScale = Mathf.Lerp(maxSize.y, maxSize.x, timerRatio); //mathf ping pong jumps between 0 and 1 and back
			var scale = Mathf.Lerp(originalSize.x, maxScale,
				Mathf.PingPong(pingPongTimer * speedMulti, 1));

			transform.localScale = originalSize * scale;

			if(timerRatio < colorFlashThreshold)
				FlashColor(speedMulti);
		}

		private IEnumerator PlayAudio()
		{
			while (IsBurning)
			{
				var audioWaitTime = Mathf.Lerp(audioWait.x, audioWait.y, timerRatio);
				audioSource.Play();
				if(!Owner.CarController.IsAi)
					Owner.CarController.RewiredPlayer.ApplyVibrationProfile(beepShakeProfile);
				yield return new WaitForSeconds(audioWaitTime+audioSource.clip.length);
			}
			beepAudio = null;
		}

		/// <summary>
		/// Set exploded true, deactivate object, play explosion, do camera shake, set health of owner 0
		/// </summary>
		private void Explode()
		{ 
			exploded = true;
			IsBurning = false;
			OnExplosion?.Invoke();
			body.SetActive(false);
			Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
			var cameraShake = FindObjectOfType<CameraShake>();
			StartCoroutine(cameraShake.Shake(shakeTime, shakeAmplitude, shakeFrequency));

			Owner.GetComponentInChildren<CaravanDamage>().OnDealDamage -= OtherCaravanHit;
			Owner.GetComponentInChildren<DecorationChanger>().SetSprite(null);
			var health = Owner.GetComponent<Health>();
			health.TakeDamage(health.MaxHealth);

			Announcer.Instance.ForceVoiceLine("HotpotatoBombExplode");

			Owner = null;
			hotPotato.SelectNextPlayer();
		}


		public void IncreaseTimer(float amount)
		{
			if (timer <= 0 || timer + amount > fuseTime)
			{
				Debug.LogWarning($"can't increase timer");
				return;
			}

			timer += amount;
		}

		/// <summary>
		/// Activates the bomb, sets IsBurning true and sets the timer
		/// </summary>
		public void StartBurning()
		{
			IsBurning = true;
			exploded = false;
			timer = fuseTime;
			body.SetActive(true);
		}

		private void FlashColor(float flashSpeed)
		{
			var targetColor = isBaseColor ? flashColor : baseColor;
			currentColor =Color.Lerp(currentColor, targetColor, flashSpeed * colorFlashSpeedMultiplier * Time.deltaTime); //lerp to target color over time

			if (currentColor == baseColor) //if color is reached lerp to other color
			{
				isBaseColor = true;
			}
			else if (currentColor == flashColor)
			{
				isBaseColor = false;
			}

			bombMaterial.color = currentColor;
		
	}

}
}


