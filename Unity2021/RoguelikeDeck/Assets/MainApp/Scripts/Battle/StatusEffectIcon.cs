using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Status Abnormality Icon Handling Class on Status UI
/// </summary>
public class StatusEffectIcon : MonoBehaviour
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

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

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
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
