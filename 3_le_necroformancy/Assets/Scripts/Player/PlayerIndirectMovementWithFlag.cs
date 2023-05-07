using System;
using UnityEngine;

/// <summary>
/// Spawns a flag on the desired player position.
/// </summary>
[ DisallowMultipleComponent ]
[ RequireComponent( typeof( PlayerIndirectMovement ) ) ]
public class PlayerIndirectMovementWithFlag
    : MonoBehaviour
{
    /// <summary>
    /// Used to move the player indirectly.
    /// </summary>
    private PlayerIndirectMovement m_playerIndirectMovement;

    [ Tooltip( "The prefab game object for the flag to spawn." ) ]
    [ SerializeField ]
    private GameObject m_prefabFlag;

    /// <summary>
    /// The currently used flag game object.
    /// </summary>
    private GameObject m_flag;
    
    /// <summary>
    /// Get required game components in awake.
    /// </summary>
    private void Awake()
    {
        m_playerIndirectMovement = gameObject.GetComponent< PlayerIndirectMovement >();
        // check if prefab is set
        if ( m_prefabFlag == null )
        {
            enabled              = false;
            throw new InvalidOperationException( "Can not awake " + GetType().Name + " script because flag prefab is not set." );
        }
    }

    /// <summary>
    /// Updates the flag position and disables the flag if necessary.
    /// </summary>
    private void Update()
    {
        CreateFlag();
        UpdateFlag();
    }

    /// <summary>
    /// Creates the flag if required.
    /// </summary>
    private void CreateFlag()
    {
        if ( m_flag != null ) { return; }
        m_flag = Instantiate( m_prefabFlag );
    }

    /// <summary>
    /// Updates the flag.
    /// </summary>
    private void UpdateFlag()
    {
        // set active if player is following desired position; otherwise disable
        m_flag.SetActive( m_playerIndirectMovement.GetIsFollowingDesiredPosition() );
        // update world position to desired player position
        var flagTransform      = m_flag.GetComponent< Transform >();
        var desiredPosition    = m_playerIndirectMovement.GetDesiredPosition();
        flagTransform.position = desiredPosition;
    }
}
