using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss Enemy Appearance UI Class
/// </summary>
public class BossIncoming : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    // UI object
    /// <summary>
    /// Canvas Group
    /// </summary>
    [SerializeField] private CanvasGroup canvasGroup = null;
    /// <summary>
    /// RectTransform of logo image
    /// </summary>
    [SerializeField] private RectTransform logoRectTransform = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Logo image initial Scale value
    /// </summary>
    private const float LogoStartScale = 1.0f;
    /// <summary>
    /// Logo image end Scale value
    /// </summary>
    private const float LogoEndScale = 3.0f;

    #endregion Private Variables

    #endregion Variables

    #region Methods

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
    /// Start processing
    /// </summary>
    public void StartAnimation()
    {
        // Initial UI Setup
        gameObject.SetActive(true);
        // Set CanvasGroup
        canvasGroup.alpha = 0.0f;
        // Set logo
        logoRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        // Logo image initial coordinates
        Vector2 LogoStartPos = new Vector2(0.0f, 400.0f);
        // Logo image end coordinate
        Vector2 LogoEndPos = Vector2.zero;
        // UI display animation
        var sequence = DOTween.Sequence();
        // Display panel
        sequence.Append(canvasGroup.DOFade(1.0f, 0.5f));
        // Move logo
        sequence.Append(logoRectTransform.DOAnchorPos(LogoEndPos, 1.0f)
                .SetEase(Ease.OutQuint));
        // Wait time
        sequence.AppendInterval(0.6f);
        // Zoom logo
        sequence.Append(logoRectTransform.DOScale(LogoEndScale, 1.0f));
        // Hide panel
        sequence.Join(canvasGroup.DOFade(0.0f, 1.0f));
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
