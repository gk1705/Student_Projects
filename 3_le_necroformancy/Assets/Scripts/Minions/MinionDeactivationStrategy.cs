using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;

public class MinionDeactivationStrategy : FormationUnderlingStrategy
{
    public override void Enter()
    {
        foreach (var behaviour in GetFormationUnderling().GetComponentsInChildren<Behaviour>())
        {
            if (behaviour == GetFormationUnderling()) { continue; }
            if (behaviour is BehaviourTreeOwner) { continue; }

            if (behaviour != null)
                behaviour.enabled = false;
        }
        foreach (var r in GetFormationUnderling().GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
    }

    public override void Exit()
    {
        foreach (var behaviour in GetFormationUnderling().GetComponentsInChildren<Behaviour>())
        {
            if (behaviour == GetFormationUnderling()) { continue; }
            if (behaviour is BehaviourTreeOwner) { continue; }

            if (behaviour != null)
                behaviour.enabled = true;
        }
        foreach (var r in GetFormationUnderling().GetComponentsInChildren<Renderer>())
        {
            r.enabled = true;
        }
    }
}
