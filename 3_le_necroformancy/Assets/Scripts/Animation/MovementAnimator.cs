using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAnimator : MonoBehaviour {

    private Animator mAnim;
    private NavMeshAgent mAgent;

	// Use this for initialization
	void Start () {
        mAnim = GetComponent<Animator>();
        mAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        float lVelocity = mAgent.velocity.magnitude;
        mAnim.SetFloat("Velocity", lVelocity);
	}
}
