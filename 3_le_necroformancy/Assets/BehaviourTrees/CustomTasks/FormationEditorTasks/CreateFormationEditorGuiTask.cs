
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEngine;

public class CreateFormationEditorGuiTask
    : ActionTask
{
    private readonly SortedDictionary< FormationGridPosition, GameObject > m_gridSlotInfos
        = new SortedDictionary< FormationGridPosition, GameObject >( new FormationGridPositionComparer() );
    private readonly List< GameObject >                                    m_proxies
        = new List< GameObject >();
    private          GameObject                                            m_draggingObject;

    protected override void OnExecute()
    {
        var playerObject                 = GlobalBlackboardExtensions.GetValue< GameObject >( "PlayerObject" );
        if ( playerObject                 == null ) { return; }
        var playerTransform              = playerObject.GetComponent< Transform >();

        var formationConfiguration       = playerObject.GetComponent< FormationConfiguration >();
        if ( formationConfiguration       == null ) { return; }

        var managerNode                  = GlobalBlackboardExtensions.GetValue< GameObject >( "ManagerNode" );
        if ( managerNode                  == null ) { return; }

        var formationEditorConfiguration = managerNode.GetComponent< FormationEditorConfiguration >();
        if ( formationEditorConfiguration == null ) { return; }

        // add minion infos
        foreach ( var minion in formationConfiguration.EnumerateMinions() )
        {
            m_gridSlotInfos.Add( minion.GridPosition, minion.gameObject );
        }

        // fill up empty slots
        for ( int x = -formationConfiguration.GetGridSize().X / 2; x <= formationConfiguration.GetGridSize().X / 2; ++x )
        {
            for ( int z = -formationConfiguration.GetGridSize().Z / 2; z <= formationConfiguration.GetGridSize().Z / 2; ++z )
            {
                var position                                 = new FormationGridPosition( x, z );
                if ( m_gridSlotInfos.ContainsKey( position ) ) { continue; }

                m_gridSlotInfos.Add( position, null );
            }
        }

        // create proxies for grid objects
        foreach ( var gridSlotInfo in m_gridSlotInfos )
        {
            var position                                     = gridSlotInfo.Key;
            var gameObject                                   = gridSlotInfo.Value;

            foreach ( var proxyInfo in formationEditorConfiguration.GetProxyInfos() )
            {
                if ( proxyInfo.ProxyDetector == null ) { continue; }
                
                var proxyDetector = proxyInfo.ProxyDetector.GetComponent< FormationEditorProxyDetector >();
                if ( proxyDetector           == null ) { continue; }

                GameObject proxy;
                if ( proxyDetector.IsReplacedBy( gameObject, proxyInfo.ProxyPrefab, out proxy ) )
                {
                    var proxyTransform                       = proxy.GetComponent< Transform >();
                    proxyTransform.position                  = playerTransform.position;

                    var formationUnderling                   = proxy.GetComponent< FormationUnderling >();
                    if ( formationUnderling != null )
                    {
                        formationUnderling.FormationLeader   = playerObject;
                        formationUnderling.GridPosition      = position;
                        var formationUnderlingStrategy = formationUnderling.SetStrategy<FormationUnderlingStrategyHoldFormation>();
                        formationUnderlingStrategy.SetFadeDelay(0.75f);
                    }

                    var dragDropTarget                       = proxy.GetComponent< FormationEditorDragDropTarget >();
                    if ( dragDropTarget     != null )
                    {
                        dragDropTarget.FormationGridPosition = position;
                        dragDropTarget.Origin                = gameObject;
                    }

                    m_proxies.Add( proxy );
                    break;
                }
            }
        }
    }

    protected override void OnStop()
    {
        foreach ( var proxy in m_proxies )
        {
            var dragDropTarget = proxy.GetComponent< FormationEditorDragDropTarget >();
            if ( dragDropTarget != null )
                // write back new position from formation editor to real underlings
            {
                var origin = dragDropTarget.Origin;
                if ( origin != null )
                {
                    origin.GetComponent< FormationUnderling >().GridPosition = dragDropTarget.FormationGridPosition;
                }
            }

            Object.Destroy( proxy );
        }

        m_gridSlotInfos.Clear();
        m_proxies.Clear();
        m_draggingObject = null;
    }

    protected override void OnUpdate()
    {
        var value = GlobalBlackboardExtensions.GetValue<string>("GameState");
        if (value != "Formation")
        {
            EndAction(true);
            return;
        }

        if (m_draggingObject == null)
        {
            if (Input.GetMouseButtonDown(MouseButtonIndex.Left) == true)
            // remember game object if player clicked on a proxy
            {
                RaycastHit hit;
                if (PhysicsMisc.RaycastFromMousePointer(out hit, LayerName.UserInterface) == true)
                {
                    var hitObject = hit.collider.gameObject;
                    var dragDropTarget = hitObject.GetComponent<FormationEditorDragDropTarget>();
                    // ignore object if it is no drag drop target or an empty slot
                    if (dragDropTarget == null
                      || dragDropTarget.GetEntityType() == FormationEditorProxyType.FreeSlot) { return; }
                    // ignore in future ray casts until object is dropped again
                    hitObject.layer = LayerMask.NameToLayer(LayerName.IgnoreRaycast);
                    m_draggingObject = hitObject;
                    // change formation strategy to follow mouse cursor
                    var underlingSource = m_draggingObject.GetComponent<FormationUnderling>();
                    underlingSource.SetStrategy<FormationUnderlingStrategyFollowMouse>();
                }
            }
        }
        else // m_draggingObject != null
        {
            if (Input.GetMouseButtonUp(MouseButtonIndex.Left) == true)
            // check if player releases dragged object
            {
                FormationEditorDragDropTarget dragDropTarget;
                RaycastHit hit;
                if (PhysicsMisc.RaycastFromMousePointer(out hit, LayerName.UserInterface) == true
                  && (dragDropTarget = hit.collider.gameObject.GetComponent<FormationEditorDragDropTarget>()) != null)
                {
                    var dragDropSource = m_draggingObject.GetComponent<FormationEditorDragDropTarget>();
                    var underlingSource = m_draggingObject.GetComponent<FormationUnderling>();
                    var underlingTarget = dragDropTarget.GetComponent<FormationUnderling>();
                    var sourcePosition = underlingSource.GridPosition;
                    var targetPosition = underlingTarget.GridPosition;

                    // swap positions of entities
                    dragDropSource.FormationGridPosition = targetPosition;
                    underlingSource.GridPosition = targetPosition;
                    dragDropTarget.FormationGridPosition = sourcePosition;
                    underlingTarget.GridPosition = sourcePosition;
                    // change formation strategy to hold formation position
                    underlingSource.SetStrategy<FormationUnderlingStrategyHoldFormation>();
                    underlingTarget.SetStrategy<FormationUnderlingStrategyHoldFormation>();
                }
                else // PhysicsMisc.RaycastFromMousePointer( out hit, LayerName.UserInterface ) == false
                // reset to original position
                {
                    // change formation strategy to hold formation position
                    var underlingSource = m_draggingObject.GetComponent<FormationUnderling>();
                    underlingSource.SetStrategy<FormationUnderlingStrategyHoldFormation>();
                }

                // reset layer to default
                m_draggingObject.layer = LayerMask.NameToLayer(LayerName.UserInterface);
                // then clear dragging object
                m_draggingObject = null;
            }
        }
    }
}
