using UnityEngine;

public static class PhysicsMisc
{
    /// <summary>
    /// Casts a ray from the screen mouse pointer position into the scene.
    /// </summary>
    /// <param name="a_hit">
    /// The hit with the scene.
    /// </param>
    /// <param name="a_layerName">
    /// Name of the layer to use.
    /// </param>
    /// <returns>
    /// true if the cast ray hit something; otherwise false
    /// </returns>
    public static bool RaycastFromMousePointer( out RaycastHit a_hit, string a_layerName = "" )
    {
        // get ray from screen mouse pointer
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );

        var layerMask = int.MaxValue;
        if ( string.IsNullOrEmpty( a_layerName ) == false )
        {
            layerMask = LayerMask.GetMask( a_layerName );
        }

        // cast ray into scene
        return Physics.Raycast(ray, out a_hit, Mathf.Infinity, layerMask);
    }
}
