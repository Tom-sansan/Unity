using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Status Abnormality Icon Handling Class on Status UI
/// </summary>
public class StatusEffectIcon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Enum

    /// <summary>
    /// Definition of the type of condition abnormality
    /// </summary>
    public enum StatusEffectType
    {
        Poison,
        Flame,
        _MAX
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Effect description management class
    /// </summary>
    [SerializeField]
    private EffectExplainDisplay effectExplainDisplay;
    /// <summary>
    /// Effect amount displayText
    /// </summary>
    [SerializeField]
    private Text valueText = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// The state abnormality to which this object corresponds
    /// </summary>
    public StatusEffectType statusEffectType;
    /// <summary>
    /// State abnormality name(JP)
    /// </summary>
    public static readonly Dictionary<StatusEffectType, string> DicStatusEffectNameJP = new Dictionary<StatusEffectType, string>()
    {
        {StatusEffectType.Poison, "毒 {0}" },
        {StatusEffectType.Flame, "炎上 {0}" },
    };
    /// <summary>
    /// State abnormality explain(EN)
    /// </summary>
    public static readonly Dictionary<StatusEffectType, string> DicStatusEffectNameEN = new Dictionary<StatusEffectType, string>()
    {
        {StatusEffectType.Poison, "Poison {0}" },
        {StatusEffectType.Flame, "Flame {0}" },
    };
    /// <summary>
    /// Explanation of status abnormality (JP)
    /// </summary>
    public static readonly Dictionary<StatusEffectType, string> DicStatusEffectExplainJP = new Dictionary<StatusEffectType, string>()
    {
        {StatusEffectType.Poison, "ターン終了時に体力が{0}減少します。\n（ターンごとに効果が1減少）" },
        {StatusEffectType.Flame, "最大体力減少時に体力が{0}減少します。\n（ターンごとに効果が1減少）" },
    };
    /// <summary>
    /// Explanation of status abnormality (EN)
    /// </summary>
    public static readonly Dictionary<StatusEffectType, string> DicStatusEffectExplainEN = new Dictionary<StatusEffectType, string>()
    {
        {StatusEffectType.Poison, "HP is reduced by {0} at the end of the turn\n(Effect decreases by 1 per turn)" },
        {StatusEffectType.Flame, "HP is reduced by {0} when maximum HP is reduced\n(Effect decreases by 1 per turn)" },
    };

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    /// <summary>
    /// Amount of effect at final display
    /// </summary>
    private int lastValue;

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Set icon display
    /// </summary>
    /// <param name="value">Amount of effect</param>
    public void SetValue(int value)
    {
        if (value > 0)
        {
            // Display icon
            gameObject.SetActive(true);
            // Effect amountText reflection
            valueText.text = value.ToString();
        }
        // Hide icons
        else gameObject.SetActive(false);
        // Keep effective value
        lastValue = value;
    }
    /// <summary>
    /// Execute at the start of tap
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerDown(PointerEventData eventData) =>
        effectExplainDisplay.ShowStatusEffectExplain(statusEffectType, lastValue);
    /// <summary>
    /// Execute at the end of tap
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerUp(PointerEventData eventData) =>
        effectExplainDisplay.HideExplains();

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
