using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CaravanCrashChaos;
using UnityEngine;

/// <summary>
/// Random assortment of utility methods
/// </summary>
public static class Utility 
{
	/// <summary>
	/// copies a hingejoint into a temp value holder.
	/// Copying the component via reflection like you find in the unity forums did not work for me
	/// </summary>
	/// <param name="source"></param>
	/// <param name="destination"></param>
	public static void CopyHingeJoint(HingeJoint source, HingeJointValueHolder destination)
	{
		if (source == null || destination == null)
		{
			Debug.Log($"hingejoint src or dst null");
			return;
		}
		destination.connectedBody = source.connectedBody;
		destination.anchor = source.anchor;
		destination.axis = source.axis;
		destination.autoConfigureConnectedAnchor = source.autoConfigureConnectedAnchor;
		destination.connectedAnchor = source.connectedAnchor;
		destination.useSpring = source.useSpring;
		if (destination.useSpring)
			destination.spring = new JointSpring { spring = source.spring.spring, damper = source.spring.damper, targetPosition = source.spring.targetPosition };
		destination.useMotor = source.useMotor;
		if (destination.useMotor)
			destination.motor = new JointMotor { targetVelocity = source.motor.targetVelocity, force = source.motor.force, freeSpin = source.motor.freeSpin };
		destination.useLimits = source.useLimits;
		if (destination.useLimits)
		{
			destination.limits = new JointLimits
			{
				min = source.limits.min,
				max = source.limits.max,
				bounciness = source.limits.bounciness,
				bounceMinVelocity = source.limits.bounceMinVelocity,
				contactDistance = source.limits.contactDistance
			};
		}
		destination.breakForce = source.breakForce;
		destination.breakTorque = source.breakTorque;
		destination.enableCollision = source.enableCollision;
		destination.enablePreprocessing = source.enablePreprocessing;
		destination.massScale = source.massScale;
		destination.connectedMassScale = source.connectedMassScale;
	}

	/// <summary>
	/// copies a hingejointvalueholder into a hingejoint.
	/// Copying the component via reflection like you find in the unity forums did not work for me
	/// </summary>
	/// <param name="source"></param>
	/// <param name="destination"></param>
	public static void CopyHingeJoint(HingeJointValueHolder source, HingeJoint destination)
	{
		destination.connectedBody = source.connectedBody;
		destination.anchor = source.anchor;
		destination.axis = source.axis;
		destination.autoConfigureConnectedAnchor = source.autoConfigureConnectedAnchor;
		destination.connectedAnchor = source.connectedAnchor;
		destination.useSpring = source.useSpring;
		destination.spring = new JointSpring { spring = source.spring.spring, damper = source.spring.damper, targetPosition = source.spring.targetPosition };
		destination.useMotor = source.useMotor;
		destination.motor = new JointMotor { targetVelocity = source.motor.targetVelocity, force = source.motor.force, freeSpin = source.motor.freeSpin };
		destination.useLimits = source.useLimits;
		destination.limits = new JointLimits
		{
			min = source.limits.min,
			max = source.limits.max,
			bounciness = source.limits.bounciness,
			bounceMinVelocity = source.limits.bounceMinVelocity,
			contactDistance = source.limits.contactDistance
		};
		destination.breakForce = source.breakForce;
		destination.breakTorque = source.breakTorque;
		destination.enableCollision = source.enableCollision;
		destination.enablePreprocessing = source.enablePreprocessing;
		destination.massScale = source.massScale;
		destination.connectedMassScale = source.connectedMassScale;
	}

	public static float Map(this float x, float x1, float x2, float y1, float y2)
	{
		var m = (y2 - y1) / (x2 - x1);
		var c = y1 - m * x1; // point of interest: c is also equal to y2 - m * x2, though float math might lead to slightly different results.

		return m * x + c;
	}

	/// <summary>
	/// converts a vector3 to a vector2
	/// </summary>
	/// <param name="v3"></param>
	/// <returns>a vector2 with the x and z of the vector3</returns>
	public static Vector2 ToVector2(this Vector3 v3)
	{
		return new Vector2(v3.x, v3.z);
	}

	public static void ApplyVibrationProfile(this Rewired.Player player, ControllerShakeProfile profile, float strength)
	{
		if(profile.LeftMotor) player.SetVibration(0, strength, profile.Length, profile.StopAllOtherMotors);
		if(profile.RightMotor) player.SetVibration(1, strength, profile.Length, profile.StopAllOtherMotors);
	}
	public static void ApplyVibrationProfile(this Rewired.Player player, ControllerShakeProfile profile, float strength, float length)
	{
		if (profile.LeftMotor) player.SetVibration(0, strength, length, profile.StopAllOtherMotors);
		if (profile.RightMotor) player.SetVibration(1, strength, length, profile.StopAllOtherMotors);
	}
	public static void ApplyVibrationProfile(this Rewired.Player player, ControllerShakeProfile profile)
	{
		if (profile.LeftMotor) player.SetVibration(0, profile.LeftStrength, profile.Length, profile.StopAllOtherMotors);
		if (profile.RightMotor) player.SetVibration(1, profile.LeftStrength, profile.Length, profile.StopAllOtherMotors);
	}

	public static float PingPong(float value, float min, float max)
	{
		return Mathf.PingPong(value, max - min) + min;
	}
}

public class HingeJointValueHolder
{
	public Rigidbody connectedBody;
	public Vector3 anchor;
	public Vector3 axis;
	public bool autoConfigureConnectedAnchor;
	public Vector3 connectedAnchor;
	public bool useSpring;
	public JointSpring spring;
	public bool useMotor;
	public JointMotor motor;
	public bool useLimits;
	public JointLimits limits;
	public float breakForce;
	public float breakTorque;
	public bool enableCollision;
	public bool enablePreprocessing;
	public float massScale;
	public float connectedMassScale;
}
