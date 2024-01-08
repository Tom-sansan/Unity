using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Title scene/stage select window class
/// </summary>
public class StageSelectWindow : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    /// <summary>
    /// Title management class
    /// </summary>
    private TitleManager titleManager;
    /// <summary>
    /// Window RectTransform
    /// </summary>
    private RectTransform windowRectTransform = null;
    /// <summary>
    /// Window Show/Hide Tween;.
    /// </summary>
    private Tween windowTween;
    /// <summary>
    /// Window display animation time
    /// </summary>
    private const float WindowAnimTime = 0.3f;

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Initialization (called from TitleManager.cs)
    /// </summary>
    /// <param name="titleManager"></param>
    public void Init(TitleManager titleManager)
    {
        this.titleManager = titleManager;
        windowRectTransform = GetComponent<RectTransform>();
        // Hide Window
        windowRectTransform.transform.localScale = Vector2.zero;
        windowRectTransform.gameObject.SetActive(true);
    }
    /// <summary>
    /// Show window
    /// </summary>
    public void OpenWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window display tween
        windowTween = windowRectTransform.DOScale(1.0f, WindowAnimTime).SetEase(Ease.OutBack);
        // Activate window background panel
        titleManager.SetWindowBackPanelActive(true);
    }
    /// <summary>
    /// Hide window
    /// </summary>
    public void CloseWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window hide tween
        windowTween = windowRectTransform.DOScale(0.0f, WindowAnimTime).SetEase(Ease.InBack);
        // Disable window background panel
        titleManager.SetWindowBackPanelActive(false);
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
