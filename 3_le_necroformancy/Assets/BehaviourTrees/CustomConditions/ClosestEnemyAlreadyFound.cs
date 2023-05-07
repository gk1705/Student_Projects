using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;
public class ClosestEnemyAlreadyFound : ConditionTask {

    protected override bool OnCheck()
    {
        // enemy has already been found
        var l_closestEnemy = blackboard.GetValue<GameObject>("ClosestEnemy");
        if (l_closestEnemy == null)
            return false;

        return true;
    }
}
