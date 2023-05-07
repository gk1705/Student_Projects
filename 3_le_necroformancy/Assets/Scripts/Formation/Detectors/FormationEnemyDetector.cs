using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ DisallowMultipleComponent ]
[ RequireComponent( typeof( FormationBoundsUpdater ) ) ]
public class FormationEnemyDetector : MonoBehaviour
{
    bool m_enemyDetectedOld = false;
    bool m_enemyDetectedNew = false;

    [Tooltip("The used manager node game object.")]
    [SerializeField]
    private GameObject m_managerNode;
    private List<GameObject> m_battleReadyLeaders;

    private void Start()
    {
        m_battleReadyLeaders = GlobalBlackboardExtensions.GetValue<List<GameObject>>("BattleReadyLeaders");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_battleReadyLeaders.Clear();

        if (m_enemyDetectedOld != m_enemyDetectedNew)
        {
            if (m_enemyDetectedNew)
            {
                Debug.Log("Enemy has been detected.");
                // change back from battle state
                // todo. set hostile strategy for minions (friendly and hostile)
            }
            else
            {
                Debug.Log("Enemy has been lost.");
                // change back from battle state
                // todo. set hostile strategy for minions (friendly and hostile)
            }
        }

        m_enemyDetectedOld = m_enemyDetectedNew;
        m_enemyDetectedNew = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (GlobalBlackboardExtensions.GetValue<string>("GameState") == "Playing")
        {
            var l_formationLeader = other.gameObject.GetComponent<FormationLeader>();
            if ( l_formationLeader == null ) { return; }
            m_battleReadyLeaders.Add(other.gameObject);
            if (m_battleReadyLeaders.Contains(gameObject) == false)
            {
                m_battleReadyLeaders.Add(gameObject);
            }
            m_enemyDetectedNew = true;
        }
    }
}
