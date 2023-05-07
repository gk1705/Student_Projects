using System;
using UnityEngine;
using UnityEngine.AI;

[ DisallowMultipleComponent ]
[ RequireComponent( typeof( NavMeshAgent ) ) ]
[ RequireComponent( typeof( FormationConfiguration ) ) ]
[ RequireComponent( typeof( PlayerIndirectMovement ) ) ]
public class PlayerBehavior
    : MonoBehaviour
{
    private NavMeshAgent                             m_navMeshAgent;
    private FormationConfiguration                   m_formationConfiguration;
    private PlayerIndirectMovement                   m_playerIndirectMovement;

    private void Awake()
    {
        m_navMeshAgent                               = gameObject.GetComponent< NavMeshAgent >();
        m_formationConfiguration                     = gameObject.GetComponent< FormationConfiguration >();
        m_playerIndirectMovement                     = gameObject.GetComponent< PlayerIndirectMovement >();
    }
    
    /// <summary>
    /// Sets the entity movement to either true or false.
    /// </summary>
    /// <param name="a_status"></param>
    public void SetMovementStatus(bool a_status)
    {
        m_navMeshAgent.enabled                       = a_status;
        m_playerIndirectMovement.enabled             = a_status;
    }
}
