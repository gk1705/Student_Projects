using UnityEngine;

public class FormationEditorDragDropTarget
    : MonoBehaviour
{
    private FormationGridPosition m_formationGridPosition;

    public FormationGridPosition FormationGridPosition
    {
        get { return m_formationGridPosition; }
        set { m_formationGridPosition = value; }
    }

    private GameObject m_origin;

    public GameObject Origin
    {
        get { return m_origin; }
        set { m_origin = value; }
    }

    [ Tooltip( "The type of proxy the drag drop target represents." ) ]
    [ SerializeField ]
    private FormationEditorProxyType m_entityType;

    /// <summary>
    /// Gets the type of proxy the drag drop target represents.
    /// </summary>
    public FormationEditorProxyType GetEntityType()
    {
        return m_entityType;
    }
}
