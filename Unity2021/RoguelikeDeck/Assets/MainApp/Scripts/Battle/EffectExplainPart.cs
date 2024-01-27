using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Individual effect description processing class
/// </summary>
public class EffectExplainPart : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// CanvasGroup
    /// </summary>
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    /// <summary>
    /// Effect name display Text
    /// </summary>
    [SerializeField]
    private Text NameText = null;
    /// <summary>
    /// Composite mode display Text
    /// </summary>
    [SerializeField]
    private Text CompoModeText = null;
    /// <summary>
    /// Effect description display Text
    /// </summary>
    [SerializeField]
    private Text ExplainText = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Fade performance time
    /// </summary>
    private const float FadeTime = 0.3f;
    /// <summary>
    /// Effect description management class
    /// </summary>
    private EffectExplainDisplay effectExplainDisplay;
    /// <summary>
    /// Fade direction Tween
    /// </summary>
    private Tween fadeTween;

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
    /// Initialization (called from EffectExplainDisplay.cs)
    /// </summary>
    /// <param name="effectExplainDisplay"></param>
    public void Init(EffectExplainDisplay effectExplainDisplay)
    {
        this.effectExplainDisplay = effectExplainDisplay;
        // Hide objects at the start of a scene
        canvasGroup.alpha = 0.0f;
    }
    /// <summary>
    /// Show effect description
    /// </summary>
    /// <param name="effectData">Effect data</param>
    /// <param name="charaID">Card user's character ID</param>
    public void ShowExplain(CardEffectDefine effectData, int charaID)
    {
        // Initialization of fade direction tween
        if (fadeTween != null) fadeTween.Kill();
        // UI non-transparency
        fadeTween = canvasGroup.DOFade(1.0f, FadeTime);
        // Various text reflection
        string explainString = string.Empty;
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            // Effect name
            NameText.text = string.Format(CardEffectDefine.DicEffectNameJP[effectData.cardEffect], effectData.value);
            // Composite mode
            CompoModeText.text = CardEffectDefine.DicCompoModeNameJP[CardEffectDefine.DicEffectCompoMode[effectData.cardEffect]];
            // Effect description
            explainString = CardEffectDefine.DicEffectExplainJP[effectData.cardEffect];
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            // Effect name
            NameText.text = string.Format(CardEffectDefine.DicEffectNameEN[effectData.cardEffect], effectData.value);
            // Composite mode
            CompoModeText.text = CardEffectDefine.DicCompoModeNameEN[CardEffectDefine.DicEffectCompoMode[effectData.cardEffect]];
            // Effect description
            explainString = CardEffectDefine.DicEffectExplainEN[effectData.cardEffect];
        }
        // Reflect while applying numerical display to effect description Text
        if (effectData.cardEffect == CardEffectDefine.CardEffect.Rush)
            ExplainText.text = string.Format(explainString, effectExplainDisplay.GetBattleCountWeaponDamage(charaID));
        else if (effectData.cardEffect == CardEffectDefine.CardEffect.HealRush)
            ExplainText.text = string.Format(explainString, effectExplainDisplay.GetBattleCountHeal(charaID));
        else
            ExplainText.text = string.Format(explainString, effectData.value);
    }
    /// <summary>
    /// Display description of condition abnormality
    /// </summary>
    /// <param name="statusEffectType">Type of condition abnormality</param>
    /// <param name="value">effect value</param>
    public void ShowExplainStatusEffect(StatusEffectIcon.StatusEffectType statusEffectType, int value)
    {
        // Initialize fade direction Tween
        if (fadeTween != null) fadeTween.Kill();
        // UI non-transparency
        fadeTween = canvasGroup.DOFade(1.0f, FadeTime);
        // Various Text reflection
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            // Effect name
            NameText.text = string.Format(StatusEffectIcon.DicStatusEffectNameJP[statusEffectType], value);
            // Effect explain
            ExplainText.text = string.Format(StatusEffectIcon.DicStatusEffectExplainJP[statusEffectType], value);
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            // Effect name
            NameText.text = string.Format(StatusEffectIcon.DicStatusEffectNameEN[statusEffectType], value);
            // Effect explain
            ExplainText.text = string.Format(StatusEffectIcon.DicStatusEffectExplainEN[statusEffectType], value);
        }
        // Hide composite mode
        CompoModeText.text = string.Empty;
    }
    /// <summary>
    /// Hide effect description
    /// </summary>
    public void HideExplain()
    {
        // Initialization of fade direction tween
        if (fadeTween != null) fadeTween.Kill();
        // UI Transparency
        fadeTween = canvasGroup.DOFade(0.0f, FadeTime);
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
