using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class PlayerFormationTask : ActionTask {
    protected override void OnExecute()
    {
        var playerBehavior = agent.gameObject.GetComponent<PlayerBehavior>();
        if (playerBehavior != null)
        {
            playerBehavior.SetMovementStatus(false);
        }
        else
        {
            throw new System.InvalidOperationException("Player was not equiped with a player behaviour.");
        }
    }

    protected override void OnUpdate()
    {
        var value = GlobalBlackboardExtensions.GetValue<string>("GameState");
        if (value != "Formation")
        {
            EndAction(true);
        }
    }
}
