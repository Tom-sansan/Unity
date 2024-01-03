using System.Collections;
using System.Collections.Generic;
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
    }
    /// <summary>
    /// Set the initial deck when the game is launched for the first time
    /// </summary>
    public void InitializeData()
    {
        // Reflects initial deck settings in the player's current deck data
        deckCardList = new List<int>();
        foreach (var cardData in playerInitialDeck) AddCardToDeck(cardData.serialNum);
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Add a card to the deck
    /// </summary>
    /// <param name="cardSerialNum">Serial number of card</param>
    private static void AddCardToDeck(int cardSerialNum)
    {
        deckCardList.Add(cardSerialNum);
        deckCardList.Sort();
    }
    #endregion Private Methods

    #endregion Methods
}
