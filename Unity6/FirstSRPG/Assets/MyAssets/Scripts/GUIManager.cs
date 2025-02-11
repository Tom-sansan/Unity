using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GUIManager Class
/// </summary>
public class GUIManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// StatusWindow GameObject
    /// </summary>
    [SerializeField]
    private GameObject statusWindow;
    /// <summary>
    /// Name Text
    /// </summary>
    [SerializeField]
    private Text nameText;
    /// <summary>
    /// AttributeIcon Image
    /// </summary>
    [SerializeField]
    private Image attributeIcon;
    /// <summary>
    /// HPGage Image
    /// </summary>
    [SerializeField]
    private Image hpGageImage;
    /// <summary>
    /// HP Text
    /// </summary>
    [SerializeField]
    private Text hpText;
    /// <summary>
    /// Attack Text
    /// </summary>
    [SerializeField]
    private Text attackText;
    /// <summary>
    /// Defence Text
    /// </summary>
    [SerializeField]
    private Text defenceText;
    /// <summary>
    /// Water attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeWater;
    /// <summary>
    /// Fire attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeFire;
    /// <summary>
    /// Wind attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeWind;
    /// <summary>
    /// Soil attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeSoil;
    /// <summary>
    /// CommandButtons gameObject for character
    /// Parent object of all command buttons
    /// </summary>
    [SerializeField]
    private GameObject commandButtons;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// BattleWindowUI gameObject
    /// </summary>
    public BattleWindowUI battleWindowUI;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
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
    /// Show StatusWindow
    /// </summary>
    /// <param name="character"></param>
    public void ShowStatusWindow(Character character)
    {
        statusWindow.SetActive(true);
        nameText.text = character.CharaName;
        attributeIcon.sprite = character.attribute switch
        {
            Character.Attribute.Water => attributeWater,
            Character.Attribute.Fire => attributeFire,
            Character.Attribute.Wind => attributeWind,
            Character.Attribute.Soil => attributeSoil,
            _ => null,
        };

        //switch (character.attribute)
        //{
        //    case Character.Attribute.Water:
        //        attributeIcon.sprite = attributeWater;
        //        break;
        //    case Character.Attribute.Fire:
        //        attributeIcon.sprite = attributeFire;
        //        break;
        //    case Character.Attribute.Wind:
        //        attributeIcon.sprite = attributeWind;
        //        break;
        //    case Character.Attribute.Soil:
        //        attributeIcon.sprite = attributeSoil;
        //        break;
        //}

        // Show HP Gage
        // Set the ratio of the current HP to the maximum value to the fillAmount of the gauge Image
        // 最大値に対する現在HPの割合をゲージImageのfillAmountに設定する
        var ratio = (float)character.NowHP / character.MaxHP;
        hpGageImage.fillAmount = ratio;
        // Show HP Text
        hpText.text = $"{character.NowHP}/{character.MaxHP}";
        // Show Attack Text
        attackText.text = character.Attack.ToString();
        // Show Defence Text
        defenceText.text = character.Defence.ToString();
    }
    /// <summary>
    /// Hide StatusWindow
    /// </summary>
    public void HideStatusWindow() =>
        statusWindow.SetActive(false);
    /// <summary>
    /// Show CommandButtons
    /// </summary>
    public void ShowCommandButtons() =>
        commandButtons.SetActive(true);
    /// <summary>
    /// Hide CommandButtons
    /// </summary>
    public void HideCommandButtons() =>
        commandButtons.SetActive(false);
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        HideStatusWindow();
        HideCommandButtons();
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
