using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FormationLeader))]
[RequireComponent(typeof(BoxCollider))]
public class FormationBoundsUpdater
    : MonoBehaviour
{
    private FormationLeader m_formationLeader;
    private BoxCollider m_boxCollider;

    private void Awake()
    {
        m_formationLeader = gameObject.GetComponent<FormationLeader>();
        m_boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        UpdateFormationBoundingBox();
    }

    /// <summary>
    /// Updates the bounding box of the formation.
    /// </summary>
    private void UpdateFormationBoundingBox()
    {
        // calculate current formation width and height
        var left   = int.MaxValue;
        var right  = int.MinValue;
        var bottom = int.MaxValue;
        var top    = int.MinValue;

        var formationConfiguration = m_formationLeader.GetComponent< FormationConfiguration >();
        var slotDistance           = formationConfiguration.GetSlotDistance();

        foreach ( var entity in formationConfiguration.EnumerateMinions() )
        {
            // update slot position boundaries
            var slotPosition = entity.GridPosition;
            left   = Mathf.Min( left, slotPosition.X );
            right  = Mathf.Max( right, slotPosition.X );
            bottom = Mathf.Min( bottom, slotPosition.Z );
            top    = Mathf.Max( top, slotPosition.Z );
        }

        var width   = ( right - left + 6 ) * slotDistance.x;
        var height  = ( top - bottom + 6 ) * slotDistance.z;
        // calculate center by dividing by 2
        var centerX = ( right + left ) * slotDistance.x / 2.0f;
        var centerZ = ( top + bottom ) * slotDistance.z / 2.0f;

        m_boxCollider.center = new Vector3( centerX, 0.0f, centerZ );
        m_boxCollider.size   = new Vector3( width, Mathf.Max( width, height ), height );
    }
}
