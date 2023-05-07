using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

public class MinionSetMinionStrategyAttackTask
    : MinionSetStrategyTaskTemplate<MinionAttackStrategy>
{
    protected override string GetReferredGameState()
    {
        return "Playing";
    }
}

public class MinionSetMinionStrategyCombatTask
    : MinionSetStrategyTaskTemplate<MinionCombatStrategy>
{
    protected override string GetReferredGameState()
    {
        return "Playing";
    }
}

public class MinionSetMinionStrategyHoldFormationTask
    : MinionSetStrategyTaskTemplate<MinionStrategyHoldFormation>
{
    protected override string GetReferredGameState()
    {
        return "Playing";
    }
}

public class MinionSetMinionDeactivationStrategyTask
    : MinionSetStrategyTaskTemplate<MinionDeactivationStrategy>
{
    protected override string GetReferredGameState()
    {
        return "Formation";
    }
}

public abstract class MinionSetStrategyTaskTemplate<TStrategy>
    : ActionTask
    where TStrategy : FormationUnderlingStrategy, new()
{
    protected abstract string GetReferredGameState();

    protected override void OnExecute()
    {
        var minion = agent.gameObject.GetComponent<Minion>();
        if (minion != null)
        {
            minion.SetStrategy<TStrategy>();
        }
        else
        {
            throw new System.InvalidOperationException("Minion was not equiped with a minion script.");
        }
        EndAction(true);
    }
}
