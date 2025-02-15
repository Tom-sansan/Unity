using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Player Deck Data Class
/// Player-side deck and card management class
/// </summary>
public class PlayerDeckData : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Dictionary with all card data and serial numbers on the player side
    /// </summary>
    public static Dictionary<int, CardDataSO> CardDataBySerialNum;
    /// <summary>
    /// Current card data in storage (managed by serial number)
    /// </summary>
    public static Dictionary<int, int> storageCardList;
    /// <summary>
    /// Current player deck data (managed by serial number)
    /// </summary>
    public static List<int> deckCardList;
    /// <summary>
    /// List of all player-side cards
    /// </summary>
    public List<CardDataSO> allPlayerCardsList;
    /// <summary>
    /// Initial deck card list on the player side
    /// </summary>
    public List<CardDataSO> playerInitialDeck;

    #endregion Public Variables

    #region Private Variables

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
    /// Initialization
    /// </summary>
    public void Init()
    {
        // Link all card data and serial numbers on the player side
        CardDataBySerialNum = new Dictionary<int, CardDataSO>();
        foreach (var item in allPlayerCardsList)
            CardDataBySerialNum.Add(item.serialNum, item);
        // Record the total effect amount of all cards on the player's side
        foreach (var item in allPlayerCardsList)
        {
            int value = 0;
            foreach (var effect in item.effectList) value += effect.value;
            item.totalEffectValue = value;
        }
    }
    /// <summary>
    /// Set the initial deck when the game is launched for the first time
    /// </summary>
    public void InitializeData()
    {
        // Reflects initial deck settings in the player's current deck data
        deckCardList = new List<int>();
        foreach (var cardData in playerInitialDeck) AddCardToDeck(cardData.serialNum);
        // Clear stored card information
        storageCardList = new Dictionary<int, int>();
        foreach (var playerCard in allPlayerCardsList) storageCardList.Add(playerCard.serialNum, 0);
        // (for debugging) Add all cards one by one to the in storage list
        /*
        List<int> keys = storageCardList.Keys.ToList();
        for (int i = 0; i < storageCardList.Count; i++)
            // Add 1 to each of the Values corresponding to all the Keys in the Dictionary
            storageCardList[keys[i]] += 1;
        */
    }
    /// <summary>
    /// Set the possession card information when starting the game for the second time or later
    /// </summary>
    public void LoadData()
    {
        // Number of all player card types
        int playerCardListNum = allPlayerCardsList.Count;
        // Deck card info
        deckCardList = new List<int>();
        foreach (var playerCard in allPlayerCardsList)
        {
            // Get the number of cards with the corresponding number
            int num = PlayerPrefs.GetInt(Data.KeyDeckCards + playerCard.serialNum, 0);
            // Add the number of cards to the list of cards in the deck
            for (int i = 0; i < num; i++) deckCardList.Add(playerCard.serialNum);
        }
        // Card information in storage
        storageCardList = new Dictionary<int, int>();
        foreach (var playerCard in allPlayerCardsList)
        {
            // Get the number of cards with the corresponding number
            int num = PlayerPrefs.GetInt(Data.KeyStorageCards + playerCard.serialNum, 0);
            // Number of cards in storage information setting
            storageCardList.Add(playerCard.serialNum, num);
        }
    }
    /// <summary>
    /// Add a card to the deck
    /// </summary>
    /// <param name="cardSerialNum">Serial number of card</param>
    public static void AddCardToDeck(int cardSerialNum)
    {
        deckCardList.Add(cardSerialNum);
        deckCardList.Sort();
        PlayerPrefs.SetInt(Data.KeyDeckCards + cardSerialNum, deckCardList.FindAll(n => n == cardSerialNum).Count);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Remove a card from the deck
    /// </summary>
    /// <param name="cardSerialNum">Serial number of card</param>
    public static void RemoveCardFromDeck(int cardSerialNum)
    {
        deckCardList.Remove(cardSerialNum);
        PlayerPrefs.SetInt(Data.KeyDeckCards + cardSerialNum, deckCardList.FindAll(n => n == cardSerialNum).Count);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Change the quantity of cards in storage
    /// </summary>
    /// <param name="cardSerialNum">Serial number of the card</param>
    /// <param name="amount">Change amount (+ to add)</param>
    public static void ChangeStorageCardNum(int cardSerialNum, int amount)
    {
        storageCardList[cardSerialNum] += amount;
        PlayerPrefs.SetInt(Data.KeyStorageCards + cardSerialNum, storageCardList[cardSerialNum]);
        PlayerPrefs.Save();
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
