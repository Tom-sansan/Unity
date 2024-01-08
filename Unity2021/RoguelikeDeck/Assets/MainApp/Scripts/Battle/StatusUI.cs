using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character status UI display class
/// </summary>
public class StatusUI : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// HP gage image
    /// </summary>
    [SerializeField]
    private Image hpGageImage = null;
    /// <summary>
    /// HP display Text
    /// </summary>
    [SerializeField]
    private Text hpText = null;
    /// <summary>
    /// CanvasGroup
    /// </summary>
    [Space(10)]
    [Header("敵キャラクター用")]
    [SerializeField]
    private CanvasGroup enemyCanvasGroup = null;
    /// <summary>
    /// Character name text
    /// </summary>
    [SerializeField]
    private Text enemyCharaNameText = null;
    /// <summary>
    /// State Abnormality Icon List
    /// </summary>
    [SerializeField]
    private List<StatusEffectIcon> statusEffectIcons = null;
    
    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Fade time
    /// </summary>
    private const float FadeTime = 0.8f;
    /// <summary>
    /// Tween for fade 
    /// </summary>
    private Tween fadeTween;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Show HP
    /// </summary>
    /// <param name="nowHP">The current HP</param>
    /// <param name="maxHP">The maximum HP</param>
    public void SetHPView(int nowHP, int maxHP)
    {
        // Set minimum HP display value
        if (nowHP < 0) nowHP = 0;
        if (maxHP < 0) maxHP = 0;
        // Show gage
        // Ratio of current HP to maximum HP
        float ratio = 0.0f;
        if (maxHP > 0) ratio = (float)nowHP / maxHP;
        hpGageImage.fillAmount = ratio;
        // Show text
        hpText.text = nowHP + " / " + maxHP;
    }
    /// <summary>
    /// Displays the value of the condition abnormality
    /// </summary>
    /// <param name="effectType"></param>
    /// <param name="value"></param>
    public void SetStatusEffectUI(StatusEffectIcon.StatusEffectType effectType, int value)
    {
        // Search for target icon processing class
        var max = (int)StatusEffectIcon.StatusEffectType._MAX;
        for (int i = 0; i < max; i++)
        {
            if (statusEffectIcons[i].statusEffectType == effectType)
                // Change of display contents
                statusEffectIcons[i].SetValue(value);
        }
    }
    #region Enemy status display-only

    /// <summary>
    /// Show character name
    /// </summary>
    /// <param name="charaName"></param>
    public void SetCharacterName(string charaName)
    {
        if (enemyCharaNameText != null) enemyCharaNameText.text = charaName;
    }
    /// <summary>
    /// Show all of UI
    /// </summary>
    public void ShowCanvasGroup()
    {
        if (fadeTween != null) fadeTween.Kill();
        // Display animation of all UI
        fadeTween = enemyCanvasGroup.DOFade(1.0f, FadeTime);
    }
    /// <summary>
    /// Hide all of UIs
    /// </summary>
    /// <param name="isAnimation"></param>
    public void HideCanvasGroup(bool isAnimation)
    {
        if (fadeTween != null) fadeTween.Kill();
        // All UI hide animation
        if (isAnimation) fadeTween = enemyCanvasGroup.DOFade(0.0f, FadeTime);
        else enemyCanvasGroup.alpha = 0.0f;
    }
    #endregion Enemy status display-only

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
