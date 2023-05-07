using UnityEngine;

public class FormationEditorProxyDetectorFreeSlot
    : FormationEditorProxyDetector
{
    public override bool IsReplacedBy( GameObject a_nonProxy, GameObject a_proxyPrefab, out GameObject a_proxy )
    {
        a_proxy    = null;

        if ( a_proxyPrefab == null ) { return false; }
        if ( a_nonProxy    != null ) { return false; }

        a_proxy    = Instantiate( a_proxyPrefab );

        return true;
    }
}