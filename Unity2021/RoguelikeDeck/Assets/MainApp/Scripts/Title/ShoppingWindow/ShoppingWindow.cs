using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        // TODO
#if UNITY_EDITOR
        // For debug: Add gold coins
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
        }
        // Display of the amount of Gold in possession
        goldText.text = Data.instance.playerGold.ToString("#,0");
        // Hide window
        windowRectTransform.transform.localScale = Vector2.zero;
        windowRectTransform.gameObject.SetActive(true);
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// OnGUI
    /// </summary>
    private void OnGUI()
    {
        // 
        if (reserveResetScrollBar)
        {
            scrollbar.value = 1.0f;
            reserveResetScrollBar = false;
        }
    }

    #endregion Private Methods

    #endregion Methods
}
