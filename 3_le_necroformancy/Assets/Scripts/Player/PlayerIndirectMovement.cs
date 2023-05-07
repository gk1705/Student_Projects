using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Lets the player move indirectly using the mouse.
/// </summary>
[ DisallowMultipleComponent ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
public class PlayerIndirectMovement
    : MonoBehaviour
{
    /// <summary>
    /// The player transform.
    /// </summary>
    private Transform m_transform;

    /// <summary>
    /// The agent that moves the player through the scene.
    /// </summary>
    private NavMeshAgent m_navMeshAgent;
    
    [ Tooltip( "Index of the mouse button to use for indirect movement." ) ]
    [ SerializeField ]
    private int m_mouseButtonIndex
        = 0;

    [ Tooltip( "The threshold in world units which the desired position must be away from the object to start moving." ) ]
    [ SerializeField ]
    private float m_followThreshold
        = 0.5f;

    /// <summary>
    /// The desired position to move to.
    /// </summary>
    private Vector3 m_desiredPosition;

    /// <summary>
    /// Gets the desired position to move to.
    /// </summary>
    public Vector3 GetDesiredPosition()
    {
        return m_desiredPosition - new Vector3( 0.0f, m_navMeshAgent.height / 2.0f, 0.0f );
    }

    /// <summary>
    /// Determines if the player is following its desired position.
    /// </summary>
    public bool GetIsFollowingDesiredPosition()
    {
        return enabled && Vector3.Distance( m_transform.position, m_desiredPosition ) > m_followThreshold;
    }

    /// <summary>
    /// Get required game components in awake.
    /// </summary>
    private void Awake()
    {
        m_transform       = gameObject.GetComponent< Transform >();
        m_navMeshAgent    = gameObject.GetComponent< NavMeshAgent >();
        m_desiredPosition = new Vector3( 0.0f, m_navMeshAgent.height / 2.0f, 0.0f );
    }

    /// <summary>
    /// Indirectly moves the object of desire.
    /// </summary>
    private void Update()
    {
        UpdateDesiredPosition();
        FollowDesiredPosition();
    }

    /// <summary>
    /// Updates the desired position if necessary.
    /// </summary>
    private void UpdateDesiredPosition()
    {
        // check if mouse button is clicked recently
        if ( Input.GetMouseButtonDown( m_mouseButtonIndex ) == false ) { return; }
        // detect scene position from mouse click
        RaycastHit hit;
        if ( PhysicsMisc.RaycastFromMousePointer( out hit, LayerName.Navigation) == false ) { return; }
        // check if click hit layer "Navigation"
        var hitLayer        = hit.collider.gameObject.layer;
        var navigationLayer = LayerMask.NameToLayer( LayerName.Navigation );
        if ( hitLayer != navigationLayer ) { return; }
        // update the desired position, add half of player height so nav mesh agent can calculate offset correctly
        m_desiredPosition   = hit.point + new Vector3( 0.0f, m_navMeshAgent.height / 2.0f, 0.0f );
    }
    
    /// <summary>
    /// Follows the desired position if necessary.
    /// </summary>
    private void FollowDesiredPosition()
    {
        // check if player needs to move
        if ( GetIsFollowingDesiredPosition() == false ) { return; }
        // seek desired position
        m_navMeshAgent.SetDestination( m_desiredPosition );
    }
}
