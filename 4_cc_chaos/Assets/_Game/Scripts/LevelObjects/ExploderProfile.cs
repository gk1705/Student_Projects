using System.Collections;
using System.Collections.Generic;
using Exploder;
using UnityEngine;

[CreateAssetMenu(fileName = "New Exploder Profile", menuName = "ExploderProfile")]
public class ExploderProfile : ScriptableObject
{
	[Header("Main Settings")]
	public float Force;
	public float Radius;
	public int TargetFragments;
	public bool UseCustomForceVector;
	public Vector3 ForceVector;
	[Range(0.5f, 15f)]
	public float FrameBudgetMs = 2f;

	[Header("Fragment Settings")]
	public float Mass;
	public float AngularVelocity;
	public float Drag = 0;
	public float AngularDrag = 0.05f;
	public RigidbodyInterpolation Interpolation = RigidbodyInterpolation.Interpolate;
	public bool UseGravity;
	public bool DisableColliders;
	public bool RandomAngularVelocityVector;
	public bool ExplodeFragments;

	/// <summary>
	/// Sets all the variables specified in the profile on the exploder object
	/// </summary>
	/// <param name="exploder"></param>
	/// <param name="profile"></param>
	/// <param name="customForceVector">Use this if UseCustomForceVector on the profile is set to false and you want to use your own forcevector, set to Vector3.zero if you don't want to use it</param>
	public void SetExploderVariables(ExploderObject exploder, ExploderProfile profile, Vector3 customForceVector)
	{
		exploder.Force = profile.Force;
		exploder.TargetFragments = profile.TargetFragments;
		exploder.Radius = profile.Radius;
		exploder.FragmentOptions.Mass = profile.Mass;
		exploder.FragmentOptions.UseGravity = profile.UseGravity;
		exploder.FragmentOptions.AngularVelocity = profile.AngularVelocity;
		exploder.FragmentOptions.DisableColliders = profile.DisableColliders;
		exploder.FragmentOptions.RandomAngularVelocityVector = profile.RandomAngularVelocityVector;
		exploder.FragmentOptions.ExplodeFragments = profile.ExplodeFragments;
		exploder.FragmentOptions.RigidbodyInterpolation = profile.Interpolation;
		exploder.FragmentOptions.Drag = profile.Drag;
		exploder.FragmentOptions.AngularDrag = profile.AngularDrag;
		exploder.FrameBudget = profile.FrameBudgetMs;

		if (profile.UseCustomForceVector)
		{
			exploder.UseForceVector = true;
			exploder.ForceVector = profile.ForceVector;
		}
		else if(customForceVector != Vector3.zero)
		{
			exploder.UseForceVector = true;
			exploder.ForceVector = customForceVector;
		}
	}

}
