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
    /// <summary>
    /// Change the order of deck card objects
    /// </summary>
    public void AlignDeckList()
    {
        // Align the list of deck card objects
        deckCardObjects.Sort((a, b) =>
            dicDeckCardObjectByCard[a].baseCardData.serialNum - dicDeckCardObjectByCard[b].baseCardData.serialNum);
        // Transform sibling relationships are also aligned based on the sorted list
        int length = deckCardObjects.Count;
        for (int i = 0; i < length; i++) deckCardObjects[i].transform.SetAsLastSibling();
    }
    /// <summary>
    /// Change the order of card objects in storage
    /// </summary>
    public void AlignStorageList()
    {
        // Align list of objects
        storageCardObjects.Sort((a, b) =>
            dicStorageCardObjectByCard[a].baseCardData.serialNum - dicStorageCardObjectByCard[b].baseCardData.serialNum);
        // Transform sibling relationships are also aligned based on the sorted list
        int length = storageCardObjects.Count;
        for (int i = 0; i < length; i++) storageCardObjects[i].transform.SetAsLastSibling();
    }
    /// <summary>
    /// Tap destination card selection process
    /// </summary>
    /// <param name="targetCard"></param>
    public void SelectCard(Card targetCard)
    {
        // End of highlighting of the previously selected card
        if (selectCard != null) selectCard.SetCardHighlight(false);
        // Get selection info
        selectCard = targetCard;
        // Selection Card Highlighting
        targetCard.SetCardHighlight(true);
        // Change the status of various button presses
        if (selectCard.isInDeckCard)
        {
            // When selecting a card in the deck being edited
            backToStorageButton.interactable = true;
            IntoDeckButton.interactable = false;
        }
        else
        {
            // When a card in the in-storage list is selected
            backToStorageButton.interactable = false;
            IntoDeckButton.interactable = true;
        }
    }
    /// <summary>
    /// Deselect card
    /// </summary>
    public void DeselectCard()
    {
        // End of selection card highlighting
        if (selectCard != null) selectCard.SetCardHighlight(false);
        // Clear selection info
        selectCard = null;
        // Change the status of various button presses
        backToStorageButton.interactable = false;
        IntoDeckButton.interactable = false;
    }

    #region processing when a button is pressed

    /// <summary>
    /// Remove button from deck
    /// </summary>
    public void ButtonBackToStorage()
    {
        // When the number of decks reaches the lower limit, end without adding
        if (PlayerDeckData.deckCardList.Count <= MinDeckNum) return;
        // Memory/unselection of selected cards
        if (selectCard == null) return;
        Card targetCard = selectCard;
        DeselectCard();
        // Memorize the card to the right of one
        Card nextSelectCard = null;
        int selectCardIndexInList = deckCardObjects.IndexOf(targetCard.gameObject);
        if (selectCardIndexInList + 1 < deckCardObjects.Count)
            nextSelectCard = dicDeckCardObjectByCard[deckCardObjects[selectCardIndexInList + 1]];
        // Remove cards from the deck
        PlayerDeckData.RemoveCardFromDeck(targetCard.baseCardData.serialNum);
        // Add a card to the in storage list
        PlayerDeckData.ChangeStorageCardNum(targetCard.baseCardData.serialNum, 1);
        // Additional card objects in storage area
        Card cardInstanceInStorageArea = null;
        for (int i = 0; i < storageCardObjects.Count; i++)
        {
            var targetStorageCard = dicStorageCardObjectByCard[storageCardObjects[i]];
            if (targetCard.baseCardData.serialNum == targetStorageCard.baseCardData.serialNum)
            {
                cardInstanceInStorageArea = targetStorageCard;
                break;
            }
        }
        if (cardInstanceInStorageArea != null)
            // The corresponding card object exists
            // Update card display
            cardInstanceInStorageArea.ShowCardAmountInStorage();
        else
        {
            // The corresponding card object does not exist
            // Card object created
            CreateStorageCardObject(targetCard.baseCardData);
            // Aligning the list of cards in custody
            AlignStorageList();
        }
        // Delete objects in the deck area being edited
        deckCardObjects.Remove(targetCard.gameObject);
        dicDeckCardObjectByCard.Remove(targetCard.gameObject);
        Destroy(targetCard.gameObject);
        // Number of decks updated
        RefreshDeckNumToUI();
        // Card deselection
        DeselectCard();
        // If the next card is available, it is automatically selected
        if (nextSelectCard != null) SelectCard(nextSelectCard);
    }
    /// <summary>
    /// Put button on deck
    /// </summary>
    public void ButtonIntoDeck()
    {
        // When the number of decks has reached the limit, end without adding more decks
        if (PlayerDeckData.deckCardList.Count >= MaxDeckNum) return;
        // Memory/unselection of selected cards
        if (selectCard == null) return;
        Card targetCard = selectCard;
        // Add Cards to Deck
        PlayerDeckData.AddCardToDeck(targetCard.baseCardData.serialNum);
        // Remove a card from the in storage list
        PlayerDeckData.ChangeStorageCardNum(targetCard.baseCardData.serialNum, -1);
        // Delete card objects in the storage area
        if (PlayerDeckData.storageCardList[targetCard.baseCardData.serialNum] > 0)
            targetCard.ShowCardAmountInStorage();
        else
        {
            // Card deselection
            DeselectCard();
            // Delete object
            storageCardObjects.Remove(targetCard.gameObject);
            dicStorageCardObjectByCard.Remove(targetCard.gameObject);
            Destroy(targetCard.gameObject);
        }
        // Number of decks updated
        RefreshDeckNumToUI();
        // Add objects in the deck area being edited
        CreateDeckCardObject(targetCard.baseCardData);
        // Deck card list alignment
        AlignDeckList();
    }

    #endregion processing when a button is pressed

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
    private void CreateDeckCardObject(CardDataSO cardData)
    {
        // Create object
        var obj = Instantiate(cardPrefab, deckAreaTransform);
        // Get card processing class and store in list
        Card objCard = obj.GetComponent<Card>();
        deckCardObjects.Add(obj);
        dicDeckCardObjectByCard.Add(obj, objCard);
        // Initialize Card
        objCard.Init(this, true);
        objCard.SetInitialCardData(cardData, Card.CharaIDPlayer);
        // Alignment of card objects in the deck
        int alignToIndex = 99;
        var cardObjectListLength = deckAreaTransform.childCount;
        for (int i = 0; i < cardObjectListLength; i++)
        {
            var cardObject = deckAreaTransform.GetChild(i);
            if (objCard.baseCardData.serialNum <= cardObject.GetComponent<Card>().baseCardData.serialNum)
            {
                alignToIndex = i;
                break;
            }
        }
        obj.transform.SetSiblingIndex(alignToIndex);
    }
    /// <summary>
    /// Update the display of the number of decks
    /// </summary>
    private void RefreshDeckNumToUI()
    {
        int nowDeckNum = PlayerDeckData.deckCardList.Count;
        // Update the display of the number of decks
        deckLogoText.text = string.Empty;
        deckLogoText.text += $"<size=20>({nowDeckNum})</size>";
        // Highlight when the number of decks is less than or equal to the minimum number of decks
        if (nowDeckNum < MinDeckNum)
        {
            deckLogoText.text = deckLogoText.text.Insert(0, "<color=red>");
            deckLogoText.text += "</color>";
        }
        if (Data.nowLanguage == SystemLanguage.Japanese)
            deckLogoText.text = deckLogoText.text.Insert(0, "デッキ");
        else if (Data.nowLanguage == SystemLanguage.English)
            deckLogoText.text = deckLogoText.text.Insert(0, "Deck");
    }
    #endregion Private Methods

    #endregion Methods
}
