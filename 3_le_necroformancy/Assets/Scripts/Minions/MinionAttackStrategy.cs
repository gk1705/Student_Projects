using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NodeCanvas.Framework;

public class MinionAttackStrategy : FormationUnderlingStrategy
{
    public override void Update()
    {
        GameObject nearestEnemy = null;
        try
        {
            var l_formationLeader = GetFormationUnderling().FormationLeader;
            if (l_formationLeader == null) { return; }

            var l_isPlayer = l_formationLeader.gameObject.CompareTag("Player");

            var l_possibleAttackees = new SortedDictionary<float, Minion>();

            var l_listOfLeader = GlobalBlackboardExtensions.GetValue<List<GameObject>>("BattleReadyLeaders");
            foreach (var l_leader in l_listOfLeader)
            {
                if (l_leader.gameObject.CompareTag("Player") == l_isPlayer) { continue; } // is an ally
                var l_formationConfiguration = l_leader.GetComponent<FormationConfiguration>();
                if (l_formationConfiguration == null) { continue; }
                foreach (var l_minion in l_formationConfiguration.EnumerateMinions())
                {
                    var l_minionPosition = l_minion.GetComponent<Transform>().position;
                    var l_ownPosition = GetFormationUnderling().GetComponent<Transform>().position;
                    var l_distance = (l_ownPosition - l_minionPosition).sqrMagnitude;
                    l_possibleAttackees.Add(l_distance, l_minion);
                }
            }
            if (l_possibleAttackees.Count == 0) { return; }
            nearestEnemy = l_possibleAttackees.First().Value.gameObject;
        }
        finally
        {
            GetFormationUnderling().GetComponent<Blackboard>().SetValue("ClosestEnemy", nearestEnemy);
        }
    }
}