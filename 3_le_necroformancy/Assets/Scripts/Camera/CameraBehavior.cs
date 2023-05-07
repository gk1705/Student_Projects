using System;
using System.Collections;
using UnityEngine;

[ DisallowMultipleComponent ]
[ RequireComponent( typeof( CameraOrbitAround ) ) ]
[ RequireComponent( typeof( CameraOrthoPerspectiveSwitcher ) ) ]
public class CameraBehavior
    : MonoBehaviour
{
    [ Tooltip( "The used game state manager game object." ) ]
    [ SerializeField ]
    private GameObject                     m_gameStateManager;

    [ Tooltip( "Specifies how long in seconds it takes until lerping from isometric to top-down and vice versa is completed." ) ]
    [ SerializeField ]
    private float                          m_lerpSpeed
        = 1.0f;

    private CameraOrbitAround              m_cameraOrbitAround;
    private CameraOrthoPerspectiveSwitcher m_cameraOrthoPerspectiveSwitcher;
    private Vector3                        m_originalOrbitRotation;

    private void Awake()
    {
        m_cameraOrbitAround                        = gameObject.GetComponent< CameraOrbitAround >();
        m_cameraOrthoPerspectiveSwitcher           = gameObject.GetComponent< CameraOrthoPerspectiveSwitcher >();
        m_originalOrbitRotation                    = m_cameraOrbitAround.LocalRotation;
    }
    
    private void Update()
    {
        var formationConfiguration = m_cameraOrbitAround.ObjectToTrack.GetComponent< FormationConfiguration >();
        if ( formationConfiguration == null ) { return; }
        var gridSize   = formationConfiguration.GetGridSize();
        var biggerSide = Mathf.Max( gridSize.X, gridSize.Z );
        var orthoSize  = biggerSide + 2.0f;
        // on left/right or rather top/bottom side leave extra space
        m_cameraOrthoPerspectiveSwitcher.SetOrthographicSize( orthoSize );
    }

    public void ChangeToPerspectiveMode()
    {
        m_cameraOrbitAround.AreControlsEnabled = true;
        m_cameraOrthoPerspectiveSwitcher.SwitchToPerspective();

        LerpLocalRotation(m_cameraOrbitAround.LocalRotation, m_originalOrbitRotation, m_lerpSpeed);
    }

    public void ChangeToOrthographicMode()
    {
        var trackedObject = m_cameraOrbitAround.ObjectToTrack;
        var trackedObjectEulerAngles = trackedObject.GetComponent<Transform>().eulerAngles;

        m_originalOrbitRotation = m_cameraOrbitAround.LocalRotation;

        m_cameraOrbitAround.AreControlsEnabled = false;
        m_cameraOrthoPerspectiveSwitcher.SwitchToOrtho();

        LerpLocalRotation(m_originalOrbitRotation, new Vector3(trackedObjectEulerAngles.y, 90.0f, m_originalOrbitRotation.z), m_lerpSpeed);
    }

    public Coroutine LerpLocalRotation( Vector3 a_src, Vector3 a_dest, float a_duration )
    {
        StopAllCoroutines();

        return StartCoroutine( LerpFromTo( a_src, a_dest, a_duration ) );
    }

    private IEnumerator LerpFromTo( Vector3 a_src, Vector3 a_dest, float a_duration )
    {
        var startTime                              = Time.time;

        while ( Time.time - startTime < a_duration )
        {
            m_cameraOrbitAround.LocalRotation      = LerpAngle( a_src, a_dest, ( Time.time - startTime ) / a_duration );
        
            yield return 1;
        }

        m_cameraOrbitAround.LocalRotation          = a_dest;
    }

    private static Vector3 LerpAngle( Vector3 a_src, Vector3 a_dest, float a_duration )
    {
        return new Vector3
        (
            Mathf.LerpAngle( a_src.x, a_dest.x, a_duration ),
            Mathf.LerpAngle( a_src.y, a_dest.y, a_duration ),
            Mathf.LerpAngle( a_src.z, a_dest.z, a_duration )
        );
    }
}
