using System;
using UnityEngine;

/// <summary>
/// Defines proxy info used to extend the formation editor configuration.
/// </summary>
[ Serializable ]
public struct FormationEditorProxyInfo
{
    [ Tooltip( "The proxy type to use for the object to display in the formation editor." ) ]
    public FormationEditorProxyType ProxyType;

    [ Tooltip( "The proxy prefab to use for the object to display in the formation editor." ) ]
    public GameObject               ProxyPrefab;

    [ Tooltip( "Contains the proxy detector function which is used to replace a certain type of objects by proxies." ) ]
    public GameObject               ProxyDetector;
}
