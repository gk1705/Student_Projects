
using UnityEngine;
using UnityEngine.AI;
using NodeCanvas.Framework;

public class MinionStrategyHoldFormation
    : FormationUnderlingStrategy
{
    private Transform m_transform;
    private NavMeshAgent m_navMeshAgent;
    private float m_rotationSyncTime;

    public override void Enter()
    {
        base.Enter();

        m_transform = GetFormationUnderling().GetComponent<Transform>();
        m_navMeshAgent = GetFormationUnderling().GetComponent<NavMeshAgent>();

        var l_blackboard = GetFormationUnderling().GetComponent<Blackboard>();
        if (l_blackboard == null)
        {
            throw new System.InvalidOperationException("Blackboard Component wasn't found.");
        }

        l_blackboard.SetValue("ClosestEnemy", null);
    }

    /// <summary>
    /// Holds the formation grid position.
    /// </summary>
    public override void Update()
    {
        base.Update();

        var formationLeader        = GetFormationUnderling().FormationLeader;
        if ( formationLeader == null ) { return; }
        var formationConfiguration = formationLeader.GetComponent< FormationConfiguration >();
        if ( formationConfiguration == null ) { return; }
        // calculate formation offset
        Vector3 offset;
        if ( formationConfiguration.TryGetSlotOffset(GetFormationUnderling().GridPosition, out offset ) == false ) { return; }
        // calculate formation world position
        var formationPosition      = formationLeader.GetComponent< Transform >().position + offset;
        // seek formation position
        m_navMeshAgent.SetDestination( formationPosition );

        var leaderTransform = formationLeader.GetComponent<Transform>();

        var leaderEulerAngleY = leaderTransform.eulerAngles.y;
        var entityEulerAngleY = m_transform.eulerAngles.y;

        var rotationOffset = Mathf.Abs(leaderEulerAngleY - entityEulerAngleY);
        if (rotationOffset < 0.1f)
        {
            m_rotationSyncTime = 0.0f;
            return;
        }

        var distanceToTarget = (formationPosition - m_transform.position).magnitude;
        if (distanceToTarget > 1.0f)
        {
            m_rotationSyncTime = 0.0f;
            return;
        }

        var syncDuration = 1.0f;
        // calculate lerp scale
        var lerpScale = m_rotationSyncTime / syncDuration;
        // calculate new entity look rotation
        var lookRotationNew = MathUtil.LerpQuaternion(m_transform.rotation, leaderTransform.rotation, lerpScale);

        // Update rotation sync time if not in formation state
        m_rotationSyncTime += Time.deltaTime;
        // follow leader look rotation if not moving
        m_transform.rotation = lookRotationNew;
    }
}
