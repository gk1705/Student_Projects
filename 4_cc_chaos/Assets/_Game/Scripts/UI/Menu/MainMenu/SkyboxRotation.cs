using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
	[SerializeField] private float speedMultiplier = 2f;

    void Update()
    {
		RenderSettings.skybox.SetFloat("_Rotation", Time.time * speedMultiplier);
	}
}
