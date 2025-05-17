using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// ARScaleController Class
/// </summary>
public class ARScaleController : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Scale Slider
    /// </summary>
    [SerializeField]
    private Slider scaleSlider;

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
    /// AR Session Origin
    /// </summary>
    private ARSessionOrigin aRSessionOrigin;
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
        aRSessionOrigin = GetComponent<ARSessionOrigin>();
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

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        scaleSlider.onValueChanged.AddListener(OnScaleSliderValueChanged);
    }

    private void OnScaleSliderValueChanged(float value)
    {
        if (scaleSlider != null)
        {
            aRSessionOrigin.transform.localScale = Vector3.one * value;
        }
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
