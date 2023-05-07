using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Switches camera projection between orthographic and perspective.
/// </summary>
[ DisallowMultipleComponent ]
[ RequireComponent( typeof( Camera ) ) ]
public class CameraOrthoPerspectiveSwitcher
    : MonoBehaviour
{
    private enum SwitcherState
    {
        None,
        Perspective,
        Ortho,
    }

    private Matrix4x4 m_ortho;
    private Matrix4x4 m_perspective;

    private float m_orthographicSize
        = 10.0f;

    /// <summary>
    /// Sets the orthographic size of the camera.
    /// </summary>
    /// <param name="a_value">
    /// The value to set.
    /// </param>
    public void SetOrthographicSize( float a_value )
    {
        m_orthographicSize = a_value;
    }

    [ Tooltip( "The camera field of view." ) ]
    [ SerializeField ]
    private float m_fov
        = 60.0f;

    [ Tooltip( "The camera near plane." ) ]
    [ SerializeField ]
    private float m_near
        = 0.3f;

    [ Tooltip( "The camera far plane." ) ]
    [ SerializeField ]
    private float m_far
        = 1000.0f;
    
    [ Tooltip( "Specifies how long in seconds it takes until lerping from perspective to isometric and vice versa is completed." ) ]
    [ SerializeField ]
    private float m_lerpSpeed
        = 1.0f;

    private SwitcherState m_nextState    = SwitcherState.Perspective;
 
    private void LateUpdate()
    {
        var aspect = ( Screen.width + 0.0f ) / ( Screen.height + 0.0f );
 
        m_perspective = Matrix4x4.Perspective( m_fov, aspect, m_near, m_far );
        m_ortho       = Matrix4x4.Ortho( -m_orthographicSize * aspect, m_orthographicSize * aspect, -m_orthographicSize, m_orthographicSize, m_near, m_far );

        // check if state has changed, update matrix accordingly
        switch ( m_nextState )
        {
            case SwitcherState.None:
                break;
            case SwitcherState.Perspective:
                BlendToMatrix( m_perspective, m_lerpSpeed );
                break;
            case SwitcherState.Ortho:
                BlendToMatrix( m_ortho, m_lerpSpeed );
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // after starting co-routine wait until next state change is requested
        m_nextState    = SwitcherState.None;
    }
 
    private static Matrix4x4 MatrixLerp( Matrix4x4 a_from, Matrix4x4 a_to, float a_time )
    {
        var ret = new Matrix4x4();
        for ( int i = 0; i < 16; i++ )
        {
            ret[ i ] = Mathf.Lerp( a_from[ i ], a_to[ i ], a_time );
        }

        return ret;
    }
 
    private IEnumerator LerpFromTo( Matrix4x4 a_src, Matrix4x4 a_dest, float a_duration )
    {
        var startTime = Time.time;
        while ( Time.time - startTime < a_duration )
        {
            gameObject.GetComponent< Camera >().projectionMatrix = MatrixLerp( a_src, a_dest, ( Time.time - startTime ) / a_duration );
        
            yield return 1;
        }

        gameObject.GetComponent< Camera >().projectionMatrix = a_dest;
    }
 
    public Coroutine BlendToMatrix( Matrix4x4 a_targetMatrix, float a_duration )
    {
        StopAllCoroutines();

        return StartCoroutine( LerpFromTo( gameObject.GetComponent< Camera >().projectionMatrix, a_targetMatrix, a_duration ) );
    }

    /// <summary>
    /// Tells the camera to switch to perspective view.
    /// </summary>
    public void SwitchToPerspective()
    {
        m_nextState = SwitcherState.Perspective;
    }
    
    /// <summary>
    /// Tells the camera to switch to orthographic view.
    /// </summary>
    public void SwitchToOrtho()
    {
        m_nextState = SwitcherState.Ortho;
    }
}
