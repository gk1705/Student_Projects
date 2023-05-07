
public abstract class FormationUnderlingStrategy 
{
    /// <summary>
    /// The formation underling the strategy belongs to.
    /// </summary>
    private FormationUnderling m_formationUnderling;

    /// <summary>
    /// Gets the formation underling the strategy belongs to.
    /// </summary>
    public FormationUnderling GetFormationUnderling()
    {
        return m_formationUnderling;
    }

    public void SetFormationUnderling(FormationUnderling a_value)
    {
        m_formationUnderling = a_value;
    }
    
    /// <summary>
    /// Enter the new strategy before first update.
    /// </summary>
    public virtual void Enter()
    {
    }
    
    /// <summary>
    /// Updates the strategy.
    /// </summary>
    public virtual void Update()
    {
    }

    /// <summary>
    /// Enter the old strategy after the last update.
    /// </summary>
    public virtual void Exit()
    {
    }
}
