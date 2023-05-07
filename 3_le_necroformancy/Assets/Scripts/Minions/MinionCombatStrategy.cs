using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class MinionCombatStrategy : FormationUnderlingStrategy
{
    float m_timePassed;
    float m_attackTime;
    Animator m_anim;
    Blackboard m_blackboard;

	public override void Update ()
    {
        m_timePassed += Time.deltaTime;

        if (m_timePassed >= m_attackTime)
        {
            // we assume enemy has already been found
            GetFormationUnderling().SetStrategy<MinionAttackStrategy>();
        }
	}

    public override void Enter()
    {
        Debug.Log("Schlag mi in oasch.");

        m_blackboard = GetFormationUnderling().GetComponent<Blackboard>();
        if (m_blackboard == null)
        {
            throw new System.InvalidOperationException("Blackboard can't be null.");
        }
        m_anim = GetFormationUnderling().GetComponent<Animator>();
        if (m_anim == null)
        {
            throw new System.InvalidOperationException("Animator can't be null.");
        }

        // define time that we stay
        m_timePassed = 0;
        m_attackTime = m_blackboard.GetValue<float>("AttackSpeed");
        // define attack that is used -> and trigger the attack
        int value = Random.Range(1, 10 + 1);
        m_anim.SetInteger("Random", value);
        m_anim.SetTrigger("Attack");
    }
}
