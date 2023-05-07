
using UnityEngine;

public class FormationUnderlingStrategyHoldFormation
    : FormationUnderlingStrategy
{
    [ Tooltip( "The duration in seconds per unity unit how long it takes to fade back to the origin of the objects grid position." ) ]
    [ SerializeField ]
    private float m_fadeToSlotDurationPerUnit
        = 0.5f;
    
    [ Tooltip( "The duration in seconds before the delay starts." ) ]
    [ SerializeField ]
    private float m_fadeDelay
        = 0.0f;

    public void SetFadeDelay( float a_value )
    {
        m_fadeDelay = Mathf.Max( a_value, 0.0f );
    }

    private Transform m_transform;
    private float     m_startTime;

    public override void Enter()
    {
        base.Enter();

        m_transform = GetFormationUnderling().GetComponent<Transform>();
        m_startTime = Time.time;
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
        if ( formationConfiguration.TryGetSlotOffset( GetFormationUnderling().GridPosition, out offset ) == false ) { return; }
        // calculate formation world position
        var formationPosition      = formationLeader.GetComponent< Transform >().position + offset;
        // seek formation position
        m_transform.position       = LerpBySpeed( m_transform.position, formationPosition );
    }

    private Vector3 LerpBySpeed( Vector3 a_src, Vector3 a_dest )
    {
        var distance               = Vector3.Distance( a_src, a_dest );
        var duration               = m_fadeToSlotDurationPerUnit * distance;

        return Lerp( a_src, a_dest, ( Time.time - m_startTime - m_fadeDelay ) /  duration );
    }

    private static Vector3 Lerp( Vector3 a_src, Vector3 a_dest, float a_duration )
    {
        return new Vector3
        (
            Mathf.Lerp( a_src.x, a_dest.x, a_duration ),
            Mathf.Lerp( a_src.y, a_dest.y, a_duration ),
            Mathf.Lerp( a_src.z, a_dest.z, a_duration )
        );
    }
}
