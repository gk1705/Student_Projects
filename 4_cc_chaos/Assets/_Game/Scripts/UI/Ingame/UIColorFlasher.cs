using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorFlasher : MonoBehaviour
{
	[SerializeField] private Color flashColor;
	[Tooltip("how fast the color changes")]
	[SerializeField] private float colorLerpAmount;
	[SerializeField] private Image uiElement;

	private Color baseColor;
	private Color currentColor;
	private bool isFlashing, isBaseColor,isFlashingOnHit;

	// Start is called before the first frame update
	void Start()
	{
		currentColor = baseColor = uiElement.color;
	}

    // Update is called once per frame
    void Update()
    {
		if (isFlashing)
		{
			Flash();
		}
		if (isFlashingOnHit)
		{
			Flash();
		}
	}

	public void SetFlashing(bool value)
	{
		isFlashing = value;
	}

	public void SetFlashingOnHit(bool value)
	{
		isFlashingOnHit = value;
	}

	public bool IsFlashing => isFlashing;
	public bool IsFlashingOnHit => isFlashingOnHit;

	void Flash()
	{
		var targetColor = isBaseColor ? flashColor : baseColor;
		currentColor = Color.Lerp(currentColor, targetColor, colorLerpAmount * Time.deltaTime); //lerp to target color over time

		if (currentColor == baseColor) //if color is reached lerp to other color
		{
			isBaseColor = true;
		}
		else if (currentColor == flashColor)
		{
			isBaseColor = false;
		}

		uiElement.color = currentColor;
	}
}
