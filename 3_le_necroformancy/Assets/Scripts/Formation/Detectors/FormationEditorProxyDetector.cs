using UnityEngine;

public abstract class FormationEditorProxyDetector
    : MonoBehaviour
{
    /// <summary>
    /// Checks whether a game object is replaced by a proxy object.
    /// </summary>
    /// <param name="a_nonProxy">
    /// The non-proxy object to check.
    /// </param>
    /// <param name="a_proxyPrefab">
    /// The proxy to use in case of replacing.
    /// </param>
    /// <param name="a_proxy">
    /// The proxy replacing the non-proxy (null if no replacement happens).
    /// </param>
    /// <returns>
    /// true if the non-proxy gets replaced; otherwise false
    /// </returns>
    public abstract bool IsReplacedBy( GameObject a_nonProxy, GameObject a_proxyPrefab, out GameObject a_proxy );
}
