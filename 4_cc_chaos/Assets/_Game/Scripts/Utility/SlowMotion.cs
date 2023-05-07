using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SlowMotion : MonoBehaviour
{
	[SerializeField] private float slowMoDuration = 2f;
	[SerializeField] private float slowMoSpeed = 0.5f;
	[SerializeField] private bool beginOnStart = false;
	[SerializeField] private bool indefinite = false;
	private float originalTimeScale, originalFixedTime;

	private void Awake()
	{
		//record the original time scales
		originalTimeScale = Time.timeScale;
		originalFixedTime = Time.fixedDeltaTime;
		Debug.Log($"timescale at start: {originalTimeScale} {originalFixedTime}");
		if (beginOnStart && !indefinite)
			StartSlowMo(slowMoDuration, slowMoSpeed);
		else if (beginOnStart && indefinite)
			SetTimeScale(slowMoSpeed);
	}

	/// <summary>
	/// Starts a slowmotion for the duration set on the object in the scene
	/// </summary>
	public void StartSlowMo()
	{
		StopAllCoroutines();
		StartCoroutine(EnterSlowMo(slowMoDuration, slowMoSpeed));
	}

	/// <summary>
	/// Starts a slowmotion for the specified duration
	/// </summary>
	/// <param name="duration"></param>
	public void StartSlowMo(float duration)
	{
		StopAllCoroutines();
		StartCoroutine(EnterSlowMo(duration, slowMoSpeed));
	}

	/// <summary>
	/// Starts a slowmotion for the specified duration
	/// </summary>
	/// <param name="duration"></param>
	/// /// <param name="speed">Timescale, everything under 1 is slower, everything above faster</param>
	public void StartSlowMo(float duration, float speed)
	{
		StopAllCoroutines();
		StartCoroutine(EnterSlowMo(duration, speed));
	}

	/// <summary>
	/// Starts the slowmo, waits and ends it
	/// </summary>
	/// <param name="duration"></param>
	/// <param name="speed"></param>
	/// <returns></returns>
	IEnumerator EnterSlowMo(float duration, float speed)
	{
		SetTimeScale(speed);
		yield return new WaitForSecondsRealtime(duration);
		EndSlowMo();
	}

	private void SetTimeScale(float speed)
	{
		Time.timeScale = speed;
		Time.fixedDeltaTime = speed * originalFixedTime;
	}

	/// <summary>
	/// Instantly ends the slowmo
	/// </summary>
	public void EndSlowMo()
	{
		StopAllCoroutines();
		Time.timeScale = originalTimeScale;
		Time.fixedDeltaTime = originalFixedTime;
	}

	private void OnDestroy()
	{
		EndSlowMo();
	}
}
