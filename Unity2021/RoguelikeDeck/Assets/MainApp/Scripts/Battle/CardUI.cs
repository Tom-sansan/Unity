using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #region UI Prefab

    /// <summary>
    /// Card icon prefab
    /// </summary>
    [SerializeField]
    private GameObject cardIconPrefab = null;
    /// <summary>
    /// Card effect text prefab
    /// </summary>
    [SerializeField]
    private GameObject cardEffectTextPrefab = null;

    #endregion UI Prefab

    #region UI reference in object

    /// <summary>
    /// Card background image
    /// </summary>
    [SerializeField]
    private Image cardBackImage = null;
    /// <summary>
    /// Card name text
    /// </summary>
    [SerializeField]
    private Text cardNameText = null;
    /// <summary>
    /// Parent transform of the card icon list
    /// </summary>
    [SerializeField]
    private Transform cardIconParent = null;
    /// <summary>
    /// Parent Transform of card effect text list
    /// </summary>
    [SerializeField]
    private Transform cardEffectTextParent = null;
    /// <summary>
    /// Card intensity text
    /// </summary>
    [SerializeField]
    private Text cardForceText = null;
    /// <summary>
    /// Background image for card intensity text
    /// </summary>
    [SerializeField]
    private Image cardForceBackImage = null;
    /// <summary>
    /// Card quantity text
    /// </summary>
    [SerializeField]
    private Text quantityText = null;
    /// <summary>
    /// Background image for card quantity text
    /// </summary>
    [SerializeField]
    private Image quantityBackImage = null;
    /// <summary>
    /// Highlighting image object
    /// </summary>
    [SerializeField]
    private GameObject highlightImageObject = null;

    #endregion UI reference in object

    #region Various sprite materials

    /// <summary>
    /// Player-side card background sprite
    /// </summary>
    [SerializeField]
    private Sprite cardBackSpritePlayer = null;
    /// <summary>
    /// Enemy card background sprites
    /// </summary>
    [SerializeField]
    private Sprite cardBackSpriteEnemy = null;
    /// <summary>
    /// Bonus Card Background Sprite
    /// </summary>
    [SerializeField]
    private Sprite cardBackSpriteBonus = null;

    #endregion Various sprite materials

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Card process class
    /// </summary>
    private Card card;
    /// <summary>
    /// Created Effects Text List
    /// </summary>
    private Dictionary<CardEffectDefine, Text> cardEffectTextDic;
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
    /// Initialization (called from Card.cs)
    /// </summary>
    /// <param name="card"></param>
    public void Init(Card card)
    {
        this.card = card;
        cardEffectTextDic = new Dictionary<CardEffectDefine, Text>();
        // UI initialization
        quantityText.text = string.Empty;
        quantityBackImage.color = Color.clear;
    }
    /// <summary>
    /// Set card background images for player side and enemy side
    /// </summary>
    /// <param name="cardControllerCharaID"></param>
    public void SetCardBackSprite(int cardControllerCharaID)
    {
        if (cardControllerCharaID == Card.CharaIDPlayer)
            cardBackImage.sprite = cardBackSpritePlayer;
        else if (cardControllerCharaID == Card.CharaIDEnemy)
            cardBackImage.sprite = cardBackSpriteEnemy;
        else if (cardControllerCharaID == Card.CharaIDBonus)
            cardBackImage.sprite = cardBackSpriteBonus;
    }
    /// <summary>
    /// Show card name
    /// </summary>
    /// <param name="nameJP">Card name(JP)</param>
    /// <param name="nameEN">Card name(EN)</param>
    public void SetCardNameText(string nameJP, string nameEN)
    {   
        if (Data.nowLanguage == SystemLanguage.Japanese) cardNameText.text = nameJP;
        else if (Data.nowLanguage == SystemLanguage.English) cardNameText.text = nameEN;
    }
    /// <summary>
    /// Add card icon UI
    /// </summary>
    /// <param name="sprite"></param>
    public void AddCardIconImage(Sprite sprite)
    {
        // Create object
        var obj = Instantiate(cardIconPrefab, cardIconParent);
        // Set sprite
        obj.GetComponent<Image>().sprite = sprite;
    }
    /// <summary>
    /// Add card effect text
    /// </summary>
    /// <param name="effectData"></param>
    public void AddCardEffectText(CardEffectDefine effectData)
    {
        // Create object
        var obj = Instantiate(cardEffectTextPrefab, cardEffectTextParent);
        // Relate TextUI and card effect
        cardEffectTextDic.Add(effectData, obj.GetComponent<Text>());
        // Update text content
        ApplyCardEffectText(effectData);
    }
    /// <summary>
    /// Show card intensity text
    /// </summary>
    /// <param name="value"></param>
    public void SetForcePointText(int value)
    {
        if (value > 0)
        {
            // Show
            cardForceText.text = value.ToString();
            cardForceBackImage.color = Color.white;
        }
        else
        {
            // Hide
            cardForceText.text = string.Empty;
            cardForceBackImage.color = Color.clear;
        }
    }
    /// <summary>
    /// Show card amount text
    /// </summary>
    /// <param name="value"></param>
    public void SetAmountText(int value)
    {
        quantityText.text = "x" + value;
        quantityBackImage.color = Color.white;
    }
    /// <summary>
    /// Display / hide card highlighting images
    /// </summary>
    public void SetHighlightImage(bool mode) =>
        highlightImageObject.SetActive(mode);
    /// <summary>
    /// Reset display of icons and effects
    /// </summary>
    public void ClearIconsAndEffects()
    {
        // Initialize icon
        int length = cardIconParent.childCount;
        for (int i = 0; i < length; i++)
            Destroy(cardIconParent.GetChild(i).gameObject);
        // Initialize effect
        length = cardEffectTextParent.childCount;
        for (int i = 0; i < length; i++)
            Destroy(cardEffectTextParent.GetChild(i).gameObject);
    }
    /// <summary>
    /// Update displayed content of card effect text
    /// </summary>
    /// <param name="effectData"></param>
    public void ApplyCardEffectText(CardEffectDefine effectData)
    {
        // Get target TextUI
        var targetText = cardEffectTextDic[effectData];
        // Get effect value
        int effectValue = effectData.value;
        string effectValueMes = string.Empty;
        // String effect size
        effectValueMes = effectValue.ToString();
        // Show UI
        if (Data.nowLanguage == SystemLanguage.Japanese)
            targetText.text = string.Format(CardEffectDefine.DicEffectNameJP[effectData.cardEffect], effectValueMes);
        else if (Data.nowLanguage == SystemLanguage.English)
            targetText.text = string.Format(CardEffectDefine.DicEffectNameEN[effectData.cardEffect], effectValueMes);
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
