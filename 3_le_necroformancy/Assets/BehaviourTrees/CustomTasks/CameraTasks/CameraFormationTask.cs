using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class CameraFormationTask : ActionTask {
    protected override void OnExecute()
    {
        var l_cameraComponent = agent.gameObject.GetComponent<CameraBehavior>();
        if (l_cameraComponent != null)
        {
            Debug.Log("Changed Camera to Orthographic Mode");
            l_cameraComponent.ChangeToOrthographicMode();
        }
        else
        {
            throw new System.InvalidOperationException("Camera was not equiped with a camera behaviour.");
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
