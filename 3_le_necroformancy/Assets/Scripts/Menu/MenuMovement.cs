using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MenuMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_leader;

    [SerializeField]
    private GameObject m_company;

    [SerializeField]
    private Vector3 m_destination;

    [SerializeField]
    private Vector3 m_moveBackBy;

    private NavMeshAgent m_navMeshAgent;


    // Use this for initialization
    void Start ()
    {
        m_navMeshAgent = m_leader.GetComponent<NavMeshAgent>();
        m_navMeshAgent.SetDestination(m_destination);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (Vector3.Distance(m_destination, m_leader.transform.position) < 1.0f)
	    {
	        ResetCompany();
	    }
    }

    void ResetCompany()
    {
        Debug.Log("Reset Company");
        m_leader.transform.position = new Vector3(m_moveBackBy.x, 0, m_moveBackBy.z);
        m_company.transform.Translate(m_moveBackBy.x ,0,m_moveBackBy.z, Space.World);
    }
}
