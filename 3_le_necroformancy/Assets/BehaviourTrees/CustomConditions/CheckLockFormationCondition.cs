using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class CheckLockFormationCondition : ConditionTask {
    protected override bool OnCheck()
    {
        return Input.GetButton("LockFormation");
    }
}
