using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Defines the basic stats of a minion.
/// </summary>
[ DisallowMultipleComponent ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
public class Minion
    : FormationUnderling
{
    [ Tooltip( "Indicates whether the minion should seek the nearest free formation slot position behind the leader." ) ]
    [ SerializeField ]
    private bool m_shouldSeekNearestFreePositionBehindLeader;

    /// <summary>
    /// Check formation leader and display error.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        if ( FormationLeader == null )
        {
            enabled                = false;
            return;
        }

        var formationConfiguration = FormationLeader.GetComponent< FormationConfiguration >();
        if ( formationConfiguration == null )
        // formation leader requires formation configuration
        {
            enabled                = false;
            return;
        }

        // add myself to the formation
        formationConfiguration.AddMinion( this );
        // hold formation grid position
        SetStrategy<MinionStrategyHoldFormation>();
    }

    protected override void Update()
    {
        base.Update();

        // check if seek nearest position is required
        if ( m_shouldSeekNearestFreePositionBehindLeader == false ) { return; }

        FormationGridPosition gridPosition;
        if ( GetValidFormationGridPosition( out gridPosition ) == false )
        // calculate next nearest free position behind formation leader
        {
            return;
        }

        GridPosition                                = gridPosition;
        m_shouldSeekNearestFreePositionBehindLeader = false;
    }

    private bool GetValidFormationGridPosition( out FormationGridPosition a_position )
    {
        // get formation configuration
        var formationConfiguration      = FormationLeader.GetComponent< FormationConfiguration >();
        // get formation leader position
        var formationLeader             = FormationLeader.GetComponent< FormationLeader >();
        var formationLeaderPostion      = formationLeader.GetGridPosition();
        // calculate next nearest free position behind formation leader
        var layer                       = 0;
        var startX                      = 0;
        var startZ                      = -1;
        while ( layer < formationConfiguration.GetGridSize().Z / 2 )
        // check as long as layer is inside grid
        {
            for ( var i = 0; i <= layer; ++i )
            // first go all through back line
            {
                for ( var direction = -1; direction <= 1; direction +=2 )
                // create a toggle which first is -1 and then +1
                {
                    var x               = startX + i * direction;
                    var z               = startZ - layer;
                    if ( IsOccupied( formationConfiguration, formationLeaderPostion, x, z, out a_position ) == false )
                    // searching position succeeded
                    {
                        return true;
                    }
                }
            }
            for ( var i = layer - 1; i >= 0; --i )
            // go forward to startZ again
            {
                for ( var direction = -1; direction <= 1; direction +=2 )
                // create a toggle which first is -1 and then +1
                {
                    var x               = startX + layer * direction;
                    var z               = startZ - i;
                    if ( IsOccupied( formationConfiguration, formationLeaderPostion, x, z, out a_position ) == false )
                    // searching position succeeded
                    {
                        return true;
                    }
                }
            }
            // check next layer
            ++layer;
        }

        // searching position failed
        a_position = FormationGridPosition.Invalid;
        return false;
    }

    private bool IsOccupied( FormationConfiguration a_formationConfiguration, FormationGridPosition a_formationLeaderPostion, int a_x, int a_z, out FormationGridPosition a_position )
    {
        // calculate position to check
        var positionToCheck = new FormationGridPosition( a_formationLeaderPostion.X + a_x, a_formationLeaderPostion.Z + a_z );
        if ( a_formationConfiguration.IsOccupied( positionToCheck ) )
        // check if position is occupied
        {
            a_position = FormationGridPosition.Invalid;
            return true;
        }
        // searching position succeeded
        a_position      = positionToCheck;
        return false;
    }
}
