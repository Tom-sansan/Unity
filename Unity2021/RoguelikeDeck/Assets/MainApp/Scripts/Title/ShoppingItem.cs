using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Each product item UI processing class
/// </summary>
public class ShoppingItem : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Item Icons
    /// </summary>
    [SerializeField]
    private Image iconImage = null;
    /// <summary>
    /// Item name Text
    /// </summary>
    [SerializeField]
    private Text nameText = null;
    /// <summary>
    /// Item explanation Text
    /// </summary>
    [SerializeField]
    private Text explainText = null;
    /// <summary>
    /// Obtain button
    /// </summary>
    [SerializeField]
    private Button obtainButton = null;
    /// <summary>
    /// Price Text
    /// </summary>
    [SerializeField]
    private Text priceText = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Pack data for this product
    /// </summary>
    public CardPackSO cardpackData;
    /// <summary>
    /// Amount of GOLD required to get item
    /// </summary>
    public int price;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Shopping window class
    /// </summary>
    private ShoppingWindow shoppingWindow;

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
    /// Initialization (called from ShoppingWindow.cs)
    /// </summary>
    /// <param name="shoppingWindow"></param>
    /// <param name="cardpackData"></param>
    public void Init(ShoppingWindow shoppingWindow, CardPackSO cardpackData)
    {
        this.shoppingWindow = shoppingWindow;
        this.cardpackData = cardpackData;
        // UI initialization
        // Pack name Text
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            nameText.text = cardpackData.nameJP + "(" + cardpackData.cardNum + "枚)";
            explainText.text = cardpackData.explainJP;
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            nameText.text = cardpackData.nameEN + "(" + cardpackData.cardNum + " cards)";
            explainText.text = cardpackData.explainEN;
        }
        // Pack icon Image
        iconImage.sprite = cardpackData.packIcon;
        // The amount of necessary gold
        price = cardpackData.price;
        priceText.text = price.ToString("#,0");
        CheckPrice();
    }
    /// <summary>
    /// Determine if there is enough EXP to acquire an item
    /// </summary>
    public void CheckPrice()
    {
        if (Data.instance.playerGold >= price)
            // Sufficiency
            obtainButton.interactable = true;
        else
            // Shortage
            obtainButton.interactable = false;
    }
    /// <summary>
    /// Processing when purchase button is pressed
    /// </summary>
    public void ObtainButton() =>
        shoppingWindow.BuyItem(this);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
