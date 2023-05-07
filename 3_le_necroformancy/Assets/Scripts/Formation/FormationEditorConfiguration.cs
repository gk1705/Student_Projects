using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines additional configuraiton for the formation editor.
/// </summary>
[ DisallowMultipleComponent ]
public class FormationEditorConfiguration
    : MonoBehaviour
{
    [ Tooltip( "A collection of proxy infos used to display objects in formation editor appropriate." ) ]
    [ SerializeField ]
    private FormationEditorProxyInfo[] m_proxyInfos
        = new FormationEditorProxyInfo[ 0 ];

    /// <summary>
    /// Gets a collection of proxy infos used to display objects in formation editor appropriate.
    /// </summary>
    public FormationEditorProxyInfo[] GetProxyInfos()
    {
        return m_proxyInfos;
    }

    private void Start()
    {
        var alreadyDefinedProxyTypes = new Dictionary< FormationEditorProxyType, FormationEditorProxyType >();

        foreach ( var proxyInfo in m_proxyInfos )
        {
            if ( proxyInfo.ProxyDetector == null )
            {
                enabled = false;
                throw new InvalidOperationException( "Can not start " + GetType().Name + " script because proxy info for type " + proxyInfo.ProxyType + " has no proxy detector." );
            }

            if ( alreadyDefinedProxyTypes.ContainsKey( proxyInfo.ProxyType ) )
            {
                enabled = false;
                throw new InvalidOperationException( "Can not start " + GetType().Name + " script because proxy info for type " + proxyInfo.ProxyType + " was declared multiple times." );
            }

            alreadyDefinedProxyTypes.Add( proxyInfo.ProxyType, proxyInfo.ProxyType );
        }
    }
}
