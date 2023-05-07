//Caravan Crash Chaos MMP3
//Michael Schwaiger, Gabriel Koidl, Daniel Wiendl, Michael Etschbacher
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaravanCrashChaos
{
	public class Gasstation : MonoBehaviour
	{

		[SerializeField] private float coolDown = 8f;
		[SerializeField] private Color normalColor = Color.green;
		[SerializeField] private Color coolDownColor = Color.red;
		[SerializeField] private Image slider;
		private float timer = 0;
		private Material material;

		private void Start()
		{
			timer = coolDown;
			slider.color = normalColor;
			slider.fillAmount = 0;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag("Car") || timer < coolDown) return;

			var caravan = other.transform.parent.GetComponentInChildren<CaravanController>();
			if (caravan.IsDetached) //reattach caravan if detached
			{
				caravan.ReattachCaravan();
				timer = 0;
			}
				
		}

		private void OnTriggerStay(Collider other)
		{
			if (!other.CompareTag("Car") || timer < coolDown) return;

			var caravan = other.transform.parent.GetComponentInChildren<CaravanController>();
			if (caravan.IsDetached) //reattach caravan if detached
			{
				caravan.ReattachCaravan();
				timer = 0;
			}
		}

		void FixedUpdate()
		{
			//Debug.Log($"timer {timer}");
			timer += Time.fixedDeltaTime;
			SliderTimer();
			AdjustColor();
		}

		void SliderTimer()
		{
			var timerRatio = timer / coolDown;
			Mathf.Clamp01(timerRatio);
			slider.fillAmount = 1 - timerRatio;
		}

		private void AdjustColor()
		{
			if (timer < coolDown && slider.color != coolDownColor)
			{
				slider.color = coolDownColor;
			}
				
			else if (timer >= coolDown && slider.color != normalColor)
			{
				slider.color = normalColor;
			}
				
		}
	}
}


