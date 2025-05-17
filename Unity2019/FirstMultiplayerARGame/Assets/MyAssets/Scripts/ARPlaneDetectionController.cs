using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneDetectionController : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Place Button
    /// </summary>
    [SerializeField]
    private GameObject placeButton;
    /// <summary>
    /// Adjust Button
    /// </summary>
    [SerializeField]
    private GameObject adjustButton;
    /// <summary>
    /// Search for Game Button
    /// </summary>
    [SerializeField]
    private GameObject searchForGameButton;
    /// <summary>
    /// Inform UI Panel GameObject
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI informUIPanelText;
    /// <summary>
    /// Scale Slider
    /// </summary>
    [SerializeField]
    private GameObject scaleSlider;

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

    private ARPlaneManager aRPlaneManager;
    private ARPlacementManager aRPlacementManager;

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
        aRPlaneManager = GetComponent<ARPlaneManager>();
        aRPlacementManager = GetComponent<ARPlacementManager>();
    }
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Enable AR Plane Detection
    /// </summary>
    /// <param name="enable"></param>
    public void EnableARPlaneDetection(bool enable)
    {
        aRPlaneManager.enabled = enable;
        aRPlacementManager.enabled = enable;
        SetAllPlanesActive(enable);
        placeButton.SetActive(enable);
        adjustButton.SetActive(!enable);
        searchForGameButton.SetActive(!enable);
        scaleSlider.SetActive(enable);
        SetInformUIPanelText(enable ?
            "Move phone to detect planes and place the Battle Arena!"
            : "Great! You placed the ARENA..Now, search for games to BATTLE!");
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        searchForGameButton.SetActive(false);
        scaleSlider.SetActive(true);
        SetInformUIPanelText("Move phone to detect planes and place the Battle Arena!");
    }
    /// <summary>
    /// Set all planes active
    /// </summary>
    private void SetAllPlanesActive(bool isActive)
    {
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(isActive);
        }
    }
    /// <summary>
    /// Set inform UI panel text
    /// </summary>
    /// <param name="text"></param>
    private void SetInformUIPanelText(string text)
    {
        informUIPanelText.text = text;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
