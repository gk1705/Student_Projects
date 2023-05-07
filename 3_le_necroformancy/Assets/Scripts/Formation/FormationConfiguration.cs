using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Holds general information about a formation.
/// </summary>
[ DisallowMultipleComponent ]
[ RequireComponent( typeof( FormationLeader ) ) ]
public class FormationConfiguration
    : MonoBehaviour
{
    [ Tooltip( "Defines the formation grid size in slots." ) ]
    [ SerializeField ]
    private FormationGridPosition m_gridSize
        = new FormationGridPosition( 9, 9 );
    
    /// <summary>
    /// Gets the formation grid size in slots.
    /// </summary>
    public FormationGridPosition GetGridSize()
    {
        return m_gridSize;
    }

    /// <summary>
    /// Grows the grid size ( new grid size must result in odd X and Z values ).
    /// </summary>
    /// <param name="a_x">
    /// The x value to grow ( must be non-negative ).
    /// </param>
    /// <param name="a_z">
    /// The z value to grow ( must be non-negative ).
    /// </param>
    public void GrowGridSize( int a_x, int a_z )
    {
        // ensure growth
        if ( a_x < 0 || a_z < 0 ) { return; }

        var gridSizeOld = m_gridSize;
        var gridSizeNew = new FormationGridPosition( gridSizeOld.X + a_x, gridSizeOld.Z + a_z );

        // ensure odd grid size
        if ( gridSizeNew.X % 2 == 0 || gridSizeNew.Z % 2 == 0 ) { return; }
        // set new grid size
        m_gridSize      = gridSizeNew;
    }
    
    /// <summary>
    /// Gets the total formation slot count.
    /// </summary>
    public int GetSlotCount()
    {
        return m_gridSize.X * m_gridSize.Z;
    }

    [ Tooltip( "Defines the distance in world units between two formation slots." ) ]
    [ SerializeField ]
    private Vector3 m_slotDistance
        = new Vector3( 3.0f, 0.0f, 3.0f );
    
    /// <summary>
    /// Gets the distance in world units between two formation slots.
    /// </summary>
    public Vector3 GetSlotDistance()
    {
        return m_slotDistance;
    }
    
    /// <summary>
    /// Checks if a formation slot is valid.
    /// </summary>
    /// <param name="a_position">
    /// The formation slot to check.
    /// </param>
    public bool IsValid( FormationGridPosition a_position )
    {
        if ( a_position.X < -( m_gridSize.X - 1 ) / 2 || a_position.X > ( m_gridSize.X - 1) / 2 ) { return false; }
        if ( a_position.Z < -( m_gridSize.Z - 1 ) / 2 || a_position.Z > ( m_gridSize.Z - 1) / 2 ) { return false; }

        return true;
    }

    /// <summary>
    /// Gets the world offset for a specific formation slot.
    /// </summary>
    /// <param name="a_position">
    /// A position within the formation grid.
    /// </param>
    /// <param name="a_offset">
    /// The calculated offset.
    /// </param>
    /// <returns>
    /// true if the position was within the formation grid; otherwise false
    /// </returns>
    public bool TryGetSlotOffset( FormationGridPosition a_position, out Vector3 a_offset )
    {
        if ( IsValid( a_position ) == false )
        {
            a_offset = Vector3.zero;
            return false;
        }
        
        var leaderRotationY    = GetComponent< Transform >().eulerAngles.y;
        var offsetUnrotated    = new Vector3( m_slotDistance.x * a_position.X, 0.0f, m_slotDistance.z * a_position.Z );
        var formationTransform = Quaternion.AngleAxis( leaderRotationY, Vector3.up );
        a_offset               = formationTransform * offsetUnrotated;
        return true;
    }
    
    /// <summary>
    /// Contains all minions that are part of this formation.
    /// </summary>
    private readonly List< Minion > m_minions
        = new List<Minion>();

    /// <summary>
    /// Adds a minion to the formation.
    /// </summary>
    /// <param name="a_minion">
    /// The minion to add.
    /// </param>
    public void AddMinion( Minion a_minion )
    {
        if ( a_minion == null ) { return; }
        if ( m_minions.Contains( a_minion ) ) { return; }
        
        m_minions.Add( a_minion );
    }

    /// <summary>
    /// Enumerates through all minions that are part of the formation configuration.
    /// </summary>
    public IEnumerable< Minion > EnumerateMinions()
    {
        return m_minions.Where( a_x => IsValid( a_x.GridPosition ) );
    }
    
    /// <summary>
    /// Checks if a formation slot is occupied.
    /// </summary>
    /// <param name="a_position">
    /// The formation slot to check.
    /// </param>
    public bool IsOccupied( FormationGridPosition a_position )
    {
        foreach ( var minion in EnumerateMinions() )
        // check each minion
        {
            var minionPosition = minion.GridPosition;
            if ( minionPosition.X == a_position.X && minionPosition.Z == a_position.Z )
            // check if current minion occupies position to check
            {
                return true;
            }
        }
        // position is not occupied
        return false;
    }
    
    [ Tooltip( "The used manager node game object." ) ]
    [ SerializeField ]
    private GameObject m_managerNode;
    
    /// <summary>
    /// Gets the manager node game object.
    /// </summary>
    public GameObject GetManagerNode()
    {
        return m_managerNode;
    }
}
