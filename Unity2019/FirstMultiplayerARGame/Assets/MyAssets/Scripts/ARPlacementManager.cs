using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// ARPlacementManager Class
/// </summary>
public class ARPlacementManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// AR Camera
    /// </summary>
    [SerializeField]
    private Camera aRCamera;
    /// <summary>
    /// Battle Area GameObject
    /// </summary>
    [SerializeField]
    private GameObject battleAreaGameObject;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// ARRaycastManager Component
    /// </summary>
    private ARRaycastManager aRRaycastManager;
    /// <summary>
    /// Ray
    /// </summary>
    private Ray ray;
    /// <summary>
    /// Hit Pose
    /// </summary>
    private Pose hitPose;
    /// <summary>
    /// Vector3 of Center of Screen
    /// </summary>
    private Vector3 centerOfScreen;
    /// <summary>
    /// Place Position
    /// </summary>
    private Vector3 palcePosition;
    /// <summary>
    /// Raycast Hits
    /// </summary>
    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }
    void Start()
    {
        Init();
    }
    void Update()
    {
        SetBattleAreaPosition();
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }
    /// <summary>
    /// Set battle area position
    /// </summary>
    private void SetBattleAreaPosition()
    {
        centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
        ray = aRCamera.ScreenPointToRay(centerOfScreen);
        if (aRRaycastManager.Raycast(ray, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            hitPose = raycastHits[0].pose;
            palcePosition = hitPose.position;
            battleAreaGameObject.transform.position = palcePosition;
        }
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
