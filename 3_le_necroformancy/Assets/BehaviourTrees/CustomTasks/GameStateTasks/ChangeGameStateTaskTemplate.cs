
using NodeCanvas.Framework;
using UnityEngine;

public class ChangeFormationGameStateTask
    : ChangeGameStateTaskTemplate
{
    protected override string GetTriggeringAxisName()
    {
        return AxisName.ExitFormationEditor;
    }
    
    protected override string GetReferredGameState()
    {
        return "Formation";
    }

    protected override string GetNextGameState()
    {
        return "Playing";
    }
}

public class ChangePlayingGameStateTask
    : ChangeGameStateTaskTemplate
{
    protected override string GetTriggeringAxisName()
    {
        return AxisName.EnterFormationEditor;
    }

    protected override string GetReferredGameState()
    {
        return "Playing";
    }

    protected override string GetNextGameState()
    {
        return "Formation";
    }
}

public abstract class ChangeGameStateTaskTemplate
    : ActionTask
{
    protected abstract string GetTriggeringAxisName();
    protected abstract string GetReferredGameState();
    protected abstract string GetNextGameState();

    protected override void OnExecute()
    {
        if (GlobalBlackboardExtensions.GetValue<string>("GameState") != GetReferredGameState())
        {
            EndAction(false);
        }
    }

    protected override void OnUpdate()
    {
        if (Input.GetButtonDown(GetTriggeringAxisName()) == false ) { return; }
        GlobalBlackboardExtensions.SetValue("GameState", GetNextGameState());
        EndAction(true);
    }
}
