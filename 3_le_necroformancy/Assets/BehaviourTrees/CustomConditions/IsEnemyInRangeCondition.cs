using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class IsEnemyInRange : ConditionTask {

	protected override bool OnCheck()
    {
        var l_range = blackboard.GetValue<float>("Range");
        var l_closestEnemy = blackboard.GetValue<GameObject>("ClosestEnemy");

        if (l_closestEnemy == null) { throw new System.InvalidOperationException("Enemy should not be null!"); }

        if (Vector3.Distance(l_closestEnemy.transform.position, agent.transform.position) < l_range)
        {
            return true;
        }

        return false;
    }
}
