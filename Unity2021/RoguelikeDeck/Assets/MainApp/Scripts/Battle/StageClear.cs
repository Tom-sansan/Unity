using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stage clear UI class
/// </summary>
public class StageClear : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Canvas Group
    /// </summary>
    [SerializeField] private CanvasGroup canvasGroup = null;
    /// <summary>
    /// RectTransform of the logo image
    /// </summary>
    [SerializeField] private RectTransform logoRectTransform = null;
    /// <summary>
    /// Logo image
    /// </summary>
    [SerializeField] private Image logoImage = null;
    /// <summary>
    /// RectTransform of the Back to Title button
    /// </summary>
    [SerializeField] private RectTransform titleButtonRectTransform = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Logo image initial Scale value
    /// </summary>
    private const float LogoStartScale = 3.0f;
    /// <summary>
    /// Logo image end Scale value
    /// </summary>
    private const float LogoEndScale = 1.0f;
    /// <summary>
    /// Button movement (Y direction)
    /// </summary>
    private const float ButtonMovePosY = 200.0f;

    #endregion Private Variables

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
    /// Initialization
    /// </summary>
    public void Init()
    {
        // Hide UI
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// Processing called when a stage is cleared
    /// </summary>
    public void StartAnimation()
    {
        // Initial UI Setup
        gameObject.SetActive(true);
        // Set CanvasGroup
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        // Set logo
        logoRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        logoImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // Set button
        var buttonPos = titleButtonRectTransform.anchoredPosition;
        buttonPos.y -= ButtonMovePosY;
        titleButtonRectTransform.anchoredPosition = buttonPos;
        // UI display animation
        var sequence = DOTween.Sequence();
        // Display panel
        sequence.Append(canvasGroup.DOFade(1.0f, 1.0f));
        // Display logo
        sequence.Append(logoRectTransform.DOScale(LogoEndScale, 0.6f)
                .SetEase(Ease.OutCubic));
        sequence.Join(logoImage.DOFade(1.0f, 0.6f));
        // Shake logo
        sequence.Append(logoRectTransform.DOShakeAnchorPos(0.5f, 60, 30));
        // Wait time
        sequence.AppendInterval(1.0f);
        // Zoom logo
        sequence.Append(logoRectTransform.DOScale(LogoEndScale, 1.0f));
        // Hide panel
        sequence.Join(titleButtonRectTransform.DOAnchorPosY(ButtonMovePosY, 0.5f)
                .SetRelative());
    }
    /// <summary>
    /// Go to title screen
    /// </summary>
    public void GoTitleScene()
    {

    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}