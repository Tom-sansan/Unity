using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shopping Window Class
/// </summary>
public class ShoppingWindow : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Pack opening performance processing class
    /// </summary>
    [SerializeField]
    private OpenPackEffect openPackEffect;
    /// <summary>
    /// Scrollbar in ScrollView
    /// </summary>
    [SerializeField]
    private Scrollbar scrollbar = null;
    /// <summary>
    /// Text of Amount of possession gold
    /// </summary>
    [SerializeField]
    private Text goldText = null;
    /// <summary>
    /// List of card packs of products
    /// </summary>
    [SerializeField]
    private List<CardPackSO> cardPackSOs = null;
    /// <summary>
    /// Product Item UI Prefab
    /// </summary>
    [SerializeField]
    private GameObject shoppingItemPrefab = null;
    /// <summary>
    /// Parent Transform of the product item UI
    /// </summary>
    [SerializeField]
    private Transform shoppingItemsParent = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Window display animation time
    /// </summary>
    private const float WindowAnimTime = 0.3f;
    /// <summary>
    /// Title management class
    /// </summary>
    private TitleManager titleManager;
    /// <summary>
    /// RectTransform of window
    /// </summary>
    private RectTransform windowRectTransform = null;
    /// <summary>
    /// List of generated product items
    /// </summary>
    private List<ShoppingItem> shoppingItemInstances;
    /// <summary>
    /// Window show/hide Tween
    /// </summary>
    private Tween windowTween;
    /// <summary>
    /// Scrollbar initialization flag
    /// </summary>
    private bool reserveResetScrollBar;

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
    /// Initialization (called from TitleManager.cs)
    /// </summary>
    /// <param name="titleManager"></param>
    public void Init(TitleManager titleManager)
    {
        this.titleManager = titleManager;
        windowRectTransform = GetComponent<RectTransform>();
        shoppingItemInstances = new List<ShoppingItem>();
#if UNITY_EDITOR
        // TODO: For UNITY EDITOR Only
        // Add gold coins
        Data.instance.ChangePlayerGold(100000);
#endif
        // Create product item
        for (int i = 0; i < cardPackSOs.Count; i++)
        {
            // Create item
            var obj = Instantiate(shoppingItemPrefab, shoppingItemsParent);
            var shoppingItem = obj.GetComponent<ShoppingItem>();
            // Set item
            shoppingItemInstances.Add(shoppingItem);
            // Set item
            shoppingItem.Init(this, cardPackSOs[i]);
        }
        // Display of the amount of Gold in possession
        goldText.text = Data.instance.playerGold.ToString("#,0");
        // Hide window
        windowRectTransform.transform.localScale = Vector2.zero;
        windowRectTransform.gameObject.SetActive(true);
        openPackEffect.Init(this);
    }
    /// <summary>
    /// Display window
    /// </summary>
    public void OpenWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window hidden Tween
        windowTween = windowRectTransform
            .DOScale(1.0f, WindowAnimTime)
            .SetEase(Ease.OutBack);
        // Disable window background panels
        titleManager.SetWindowBackPanelActive(true);
        // Reset screen scrolling to initial values (to be executed in the next OnGUI)
        reserveResetScrollBar = true;
    }
    /// <summary>
    /// Hide window
    /// </summary>
    public void CloseWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window hidden Tween
        windowTween = windowRectTransform
            .DOScale(0.0f, WindowAnimTime)
            .SetEase(Ease.InBack);
        // Disable window background panels
        titleManager.SetWindowBackPanelActive(false);
    }
    /// <summary>
    /// Buy a relevant product
    /// </summary>
    /// <param name="targetItem"></param>
    public void BuyItem(ShoppingItem targetItem)
    {
        // Randomly get a specified number of cards
        // Pack data
        var targetPack = targetItem.cardpackData;
        // List of all cards obtained
        var obtainedCards = new List<CardDataSO>();
        for (int i = 0; i < targetPack.cardNum; i++)
        {
            // Determine which cards to get
            var cardData = targetPack.includedCards[Random.Range(0, targetPack.includedCards.Count)];
            obtainedCards.Add(cardData);
            PlayerDeckData.ChangeStorageCardNum(cardData.serialNum, 1);
        }
        // Card acquisition direction playback
        openPackEffect.StartEffect(obtainedCards);
        // Decrease in the amount of Gold
        Data.instance.ChangePlayerGold(-targetItem.price);
        OnChangePlayerGold();
    }
    /// <summary>
    /// Called when player's Gold amount is changed.
    /// </summary>
    public void OnChangePlayerGold()
    {
        // Display of the amount of GOLD in possession
        goldText.text = Data.instance.playerGold.ToString("#,0");
        // Setting of whether or not to press the "Get" button for each item
        foreach (var item in shoppingItemInstances) item.CheckPrice();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// OnGUI
    /// </summary>
    private void OnGUI()
    {
        // Restore screen scrolling to initial values
        if (reserveResetScrollBar)
        {
            scrollbar.value = 1.0f;
            reserveResetScrollBar = false;
        }
    }

    #endregion Private Methods

    #endregion Methods
}
