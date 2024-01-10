using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Deck Edit Window Class
/// </summary>
public class DeckEditWindow : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Card prefab
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab = null;
    /// <summary>
    /// Deck name + number of deck sheets string in upper right corner
    /// </summary>
    [SerializeField]
    private Text deckLogoText = null;
    /// <summary>
    /// Button to remove from deck
    /// </summary>
    [SerializeField]
    private Button backToStorageButton = null;
    /// <summary>
    /// Put in a deck
    /// </summary>
    [SerializeField]
    private Button IntoDeckButton = null;
    /// <summary>
    /// Card in storage list object Transform
    /// </summary>
    [SerializeField]
    private Transform storageCardsAreaTransform = null;
    /// <summary>
    /// Deck Object Transform
    /// </summary>
    [SerializeField]
    private Transform deckAreaTransform = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Minimum number of decks
    /// </summary>
    public const int MinDeckNum = 1;
    /// <summary>
    /// Maximum number of decks
    /// </summary>
    public const int MaxDeckNum = 60;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Title management class
    /// </summary>
    private TitleManager titleManager;
    /// <summary>
    /// List of generated cards in storage
    /// </summary>
    private List<GameObject> storageCardObjects;
    /// <summary>
    /// Storage card object by Card
    /// </summary>
    private Dictionary<GameObject, Card> dicStorageCardObjectByCard;
    /// <summary>
    /// List of cards in generated decks
    /// </summary>
    private List<GameObject> deckCardObjects;
    /// <summary>
    /// Deck card object by Card
    /// </summary>
    private Dictionary<GameObject, Card> dicDeckCardObjectByCard;
    /// <summary>
    /// Selecting card information
    /// </summary>
    private Card selectCard;

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
        deckCardObjects = new List<GameObject>();
        dicDeckCardObjectByCard = new Dictionary<GameObject, Card>();
        storageCardObjects = new List<GameObject>();
        dicStorageCardObjectByCard = new Dictionary<GameObject, Card>();
        // UI initialization
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Show window
    /// </summary>
    public void OpenWindow()
    {
        // Delete card objects that have already been generated
        if (deckCardObjects != null)
        {
            foreach (var cardObject in deckCardObjects) Destroy(cardObject.gameObject);
            deckCardObjects.Clear();
        }
        if (storageCardObjects != null)
        {
            foreach (var cardObject in storageCardObjects) Destroy(cardObject);
            storageCardObjects.Clear();
        }
        // UI initialization
        gameObject.SetActive(true);
        DeselectCard();
        RefreshDeckNumToUI();
        // Reflecting cards in custody
        foreach (var storageCard in PlayerDeckData.storageCardList)
            if (storageCard.Value > 0)
                CreateStorageCardObject(PlayerDeckData.CardDataBySerialNum[storageCard.Key]);
        // Reflect deck
        foreach (var deckCard in PlayerDeckData.deckCardList)
            CreateDeckCardObject(PlayerDeckData.CardDataBySerialNum[deckCard]);
        // Card-list sort
        AlignDeckList();
        AlignStorageList();
    }
    /// <summary>
    /// Close window
    /// </summary>
    public void CloseWindow()
    {
        // Termination process
        DeselectCard();
        gameObject.SetActive(false);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Create a card object in storage
    /// </summary>
    /// <param name="cardData"></param>
    private void CreateStorageCardObject(CardDataSO cardData)
    {
        // Create object
        var obj = Instantiate(cardPrefab, storageCardsAreaTransform);
        // Get card processing class and store in list
        Card objCard = obj.GetComponent<Card>();
        storageCardObjects.Add(obj);
        dicStorageCardObjectByCard.Add(obj, objCard);
        // Card initialization
        objCard.Init(this, false);
        objCard.SetInitialCardData(cardData, Card.CharaIDPlayer);
        objCard.ShowCardAmountInStorage();
    }
    /// <summary>
    /// Create a card object in the deck
    /// </summary>
    /// <param name="cardData"></param>
    private void CreateDeckCardObj(CardDataSO cardData)
    {

    }
    #endregion Private Methods

    #endregion Methods
}
