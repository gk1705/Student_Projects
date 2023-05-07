using System;
using UnityEngine;

/// <summary>
/// Lets the camera orbit around a specific object.
/// </summary>
[ DisallowMultipleComponent ]
public class CameraOrbitAround
    : MonoBehaviour
{
    /// <summary>
    /// The camera transform ( used for zoom ).
    /// </summary>
    private Transform m_cameraTransform;

    /// <summary>
    /// The cameras parent transform ( used for orbital movement ).
    /// </summary>
    private Transform m_parentTransform;

    /// <summary>
    /// The current distance to the camera.
    /// </summary>
    private float m_distanceToCamera
        = 10.0f;
    
    [ Tooltip( "Defines the local rotation of the camera." ) ]
    [ SerializeField ]
    private Vector3 m_localRotation
        = new Vector3( 0.0f, 45.0f, 0.0f );
    
    [ Tooltip( "Defines the minimal vertical orbital rotation ( in degrees )." ) ]
    [ SerializeField ]
    private float m_orbitalVerticalMin
        = 10.0f;
    
    [ Tooltip( "Defines the maximal vertical orbital rotation ( in degrees )." ) ]
    [ SerializeField ]
    private float m_orbitalVerticalMax
        = 80.0f;
    
    [ Tooltip( "Enables the vertical orbital rotation clamping." ) ]
    [ SerializeField ]
    private bool m_orbitalVerticalClampingIsEnabled
        = true;

    [ Tooltip( "The orbital movement speed scaling factor." ) ]
    [ SerializeField ]
    private float m_orbitalSpeed
        = 3.0f;

    [ Tooltip( "Dampens the orbital movement speed ( the higher the value the faster the transition )." ) ]
    [ SerializeField ]
    private float m_orbitalSpeedDampening
        = 25.0f;
    
    [ Tooltip( "The zoom speed scaling factor." ) ]
    [ SerializeField ]
    private float m_zoomSpeed
        = 1.0f;

    [ Tooltip( "Dampens the zoom speed ( the higher the value the faster the transition )." ) ]
    [ SerializeField ]
    private float m_zoomSpeedDampening
        = 1.0f;
    
    [ Tooltip( "Defines the minimal distance to the object to track." ) ]
    [ SerializeField ]
    private float m_minCameraDistance
        = 5.0f;
    
    [ Tooltip( "Defines the maximal distance to the object to track." ) ]
    [ SerializeField ]
    private float m_maxCameraDistance
        = 100.0f;

    [ Tooltip( "Defines the object to track." ) ]
    [ SerializeField ]
    private GameObject m_objectToTrack
        = null;
    
    [ Tooltip( "Index of the mouse button to use for orbital movement." ) ]
    [ SerializeField ]
    private int m_mouseButtonIndex
        = 2;

    [ Tooltip( "Name of the 'Mouse Scroll Wheel' axis." ) ]
    [ SerializeField ]
    private string m_axisMouseScrollWheel
        = "Mouse ScrollWheel";
    
    [ Tooltip( "Name of the 'Mouse X' axis." ) ]
    [ SerializeField ]
    private string m_axisMouseX
        = "Mouse X";
    
    [ Tooltip( "Name of the 'Mouse Y' axis." ) ]
    [ SerializeField ]
    private string m_axisMouseY
        = "Mouse Y";

    /// <summary>
    /// Gets or sets the local camera rotation.
    /// </summary>
    public Vector3 LocalRotation
    {
        get { return m_localRotation; }
        set { m_localRotation = value; }
    }

    /// <summary>
    /// Gets the object to track.
    /// </summary>
    public GameObject ObjectToTrack
    {
        get { return m_objectToTrack; }
    }

    /// <summary>
    /// Gets a value indicating whether controls are enabled (zoom, orbital movement).
    /// </summary>
    public bool AreControlsEnabled { get; set; }

    /// <summary>
    /// Pre-initializes the script.
    /// </summary>
    private void Awake()
    {
        AreControlsEnabled = true;
    }

    /// <summary>
    /// Initializes the script.
    /// </summary>
    private void Start()
    {
        m_cameraTransform = gameObject.GetComponent< Transform >();
        m_parentTransform = m_cameraTransform.parent;

        if ( m_parentTransform == null )
        // disable component and notify about constraint
        {
            enabled       = false;
            throw new InvalidOperationException( "Can not start " + GetType().Name + " script because camera needs to have a parent transform." );
        }
    }

    /// <summary>
    /// Update camera after everything else has been updated.
    /// </summary>
    private void LateUpdate()
    {
        if ( AreControlsEnabled )
        {
            UpdateOrbitalMovement();
            UpdateZoom();
        }

        FollowObjectToTrack();
        ClampDistanceToCamera();
        TransformCamera();
    }

    /// <summary>
    /// Updates the camera orbital movement.
    /// </summary>
    private void UpdateOrbitalMovement()
    {
        if ( Input.GetMouseButton( m_mouseButtonIndex ) == false ) { return; }
        // capture mouse movement
        var mouseOffsetX  = Input.GetAxis( m_axisMouseX );
        var mouseOffsetY  = Input.GetAxis( m_axisMouseY );

        // calculate new orbital position ( invert vertical movement )
        var orbitalX      = m_localRotation.x + mouseOffsetX * m_orbitalSpeed;
        var orbitalY      = m_localRotation.y - mouseOffsetY * m_orbitalSpeed;

        if ( m_orbitalVerticalClampingIsEnabled )
        // clamp orbital movement
        {
            orbitalY      = Mathf.Clamp( orbitalY, m_orbitalVerticalMin, m_orbitalVerticalMax );
        }

        // set new orbital position
        m_localRotation.x = orbitalX;
        m_localRotation.y = orbitalY;
    }

    /// <summary>
    /// Updates the camera zoom.
    /// </summary>
    private void UpdateZoom()
    {
        var scrollDistance      = Input.GetAxis( m_axisMouseScrollWheel );
        if ( Mathf.Abs( scrollDistance ) > 0.0f )
        // mouse wheel has been scrolled
        {
            // adaptively scroll faster if farther away
            scrollDistance     *= m_distanceToCamera * m_zoomSpeed;
            // update camera distance
            m_distanceToCamera -= scrollDistance;
        }
    }

    /// <summary>
    /// Follows the object to track.
    /// </summary>
    private void FollowObjectToTrack()
    {
        if ( m_objectToTrack == null ) { return; }
        var objectToTrackTransform = m_objectToTrack.GetComponent< Transform >();
        var targetPosition         = objectToTrackTransform.position;
        m_parentTransform.position = targetPosition;
    }

    /// <summary>
    /// Clamps the current camera distance.
    /// </summary>
    private void ClampDistanceToCamera()
    {
        m_distanceToCamera = Mathf.Clamp( m_distanceToCamera, m_minCameraDistance, m_maxCameraDistance );
    }

    /// <summary>
    /// Transforms the camera to the new desired position.
    /// </summary>
    private void TransformCamera()
    {
        // update rotation
        var quaternion                  = Quaternion.Euler( m_localRotation.y, m_localRotation.x, 0.0f );
        var rotationInterpolation       = m_orbitalSpeedDampening * Time.deltaTime;
        m_parentTransform.rotation      = Quaternion.Lerp( m_parentTransform.rotation, quaternion, rotationInterpolation );

        // update translation
        var translationInterpolation    = m_zoomSpeedDampening * Time.deltaTime;
        var newZ                        = Mathf.Lerp( m_cameraTransform.localPosition.z, -m_distanceToCamera, translationInterpolation );
        m_cameraTransform.localPosition = new Vector3( 0.0f, 0.0f, newZ );
    }
}
