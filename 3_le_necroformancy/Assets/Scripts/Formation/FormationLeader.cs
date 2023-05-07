using UnityEngine;

/// <summary>
/// Represents a formation leader.
/// </summary>
[ DisallowMultipleComponent ]
public class FormationLeader
    : MonoBehaviour
{
    [ Tooltip( "Defines the formation grid position of the leader." ) ]
    [ SerializeField ]
    private FormationGridPosition m_gridPosition
        = new FormationGridPosition( 0, 0 );

    /// <summary>
    /// Gets the formation grid position of the leader.
    /// </summary>
    public FormationGridPosition GetGridPosition()
    {
        return m_gridPosition;
    }

    [ Tooltip( "Indicates whether the formation leader is virtual." ) ]
    [ SerializeField ]
    private bool m_isVirtualLeader
        = false;
}
