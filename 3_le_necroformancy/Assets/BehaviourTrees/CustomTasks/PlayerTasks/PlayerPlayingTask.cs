using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class PlayerPlayingTask : ActionTask {
    protected override void OnExecute()
    {
        var playerBehavior = agent.gameObject.GetComponent<PlayerBehavior>();
        if (playerBehavior != null)
        {
            playerBehavior.SetMovementStatus(true);
        }
        else
        {
            throw new System.InvalidOperationException("Player was not equiped with a player behaviour.");
        }
    }

    protected override void OnUpdate()
    {
        var value = GlobalBlackboardExtensions.GetValue<string>("GameState");
        if (value != "Playing")
        {
            EndAction(true);
        }
    }
}
