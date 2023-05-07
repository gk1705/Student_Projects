using UnityEngine;

public class FormationUnderlingStrategyFollowMouse
    : FormationUnderlingStrategy
{
    private Transform m_transform;
    
    public override void Enter()
    {
        base.Enter();

        m_transform                = GetFormationUnderling().GetComponent< Transform >();
    }

    public override void Update()
    {
        base.Update();
        
        RaycastHit hit;
        if ( PhysicsMisc.RaycastFromMousePointer( out hit ) == true )
        {
            var hitPosition     = hit.point;
            var newPosition     = new Vector3( hitPosition.x, m_transform.position.y, hitPosition.z );

            m_transform.position = newPosition;
        }
    }
}
