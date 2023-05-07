using UnityEngine;
using NodeCanvas.Framework;

[ DisallowMultipleComponent ]
[ RequireComponent(typeof(Blackboard)) ]
public class FormationUnderling
    : MonoBehaviour
{
    /// <summary>
    /// Defines the formation grid position.
    /// </summary>
    [SerializeField]
    private FormationGridPosition m_gridPosition
        = FormationGridPosition.Invalid;
    
    /// <summary>
    /// Gets the formation grid position of the minion.
    /// </summary>
    public FormationGridPosition GridPosition
    {
        get { return m_gridPosition; }
        set { m_gridPosition = value; }
    }

    [SerializeField]
    private GameObject m_formationLeader;

    /// <summary>
    /// Gets the formation leader game object.
    /// </summary>
    public GameObject FormationLeader
    {
        get { return m_formationLeader; }
        set { m_formationLeader = value; }
    }
    
    /// <summary>
    /// The currently active proxy minion strategy.
    /// </summary>
    private FormationUnderlingStrategy m_strategy;
    private bool m_hasStrategyChanged;

    /// <summary>
    /// Sets the new active proxy minion strategy.
    /// </summary>
    /// <param name="a_value">
    /// The value to set.
    /// </param>
    public TStrategy SetStrategy<TStrategy>()
        where TStrategy : FormationUnderlingStrategy, new()
    {
        if (m_strategy is TStrategy) { return m_strategy as TStrategy; }
        if (m_strategy != null)
        {
            m_strategy.Exit();
        }

        var newStrategy = new TStrategy();
        m_strategy = newStrategy;
        m_strategy.SetFormationUnderling(this);
        m_hasStrategyChanged = true;

        return newStrategy;
    }

    public FormationUnderlingStrategy GetStrategy()
    {
        return m_strategy;
    }

    /// <summary>
    /// Check formation leader and display error.
    /// </summary>
    protected virtual void Start()
    {
        if ( FormationLeader == null )
        {
            enabled                = false;
            return;
        }

        // set leader in blackboard
        // if there's no blackboard, do nothing
        // that is the case when we spawn the prefabs for the formation editor
        var blackboard = gameObject.GetComponent<Blackboard>();
        if (blackboard != null)
        {
            var variable = blackboard.SetValue("Leader", m_formationLeader);
            if (variable == null)
            {
                throw new System.InvalidOperationException("Leader could not be set.");
            }
        }

        var formationConfiguration = FormationLeader.GetComponent< FormationConfiguration >();
        if ( formationConfiguration == null )
        // formation leader requires formation configuration
        {
            enabled                = false;
            return;
        }
        
        // hold formation grid position
        SetStrategy<FormationUnderlingStrategyHoldFormation>();
    }

    /// <summary>
    /// Updates the minion.
    /// </summary>
    protected virtual void Update()
    {
        if ( m_strategy == null ) { return; }
        if (m_hasStrategyChanged)
        {
            m_strategy.Enter();
        }

        m_strategy.Update();
        m_hasStrategyChanged = false;
    }
}
