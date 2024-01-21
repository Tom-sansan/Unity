using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

/// <summary>
/// Field Managefr Class
/// </summary>
public class FieldManager : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Dammy hand controll class
    /// </summary>
    [SerializeField]
    private DammyHandUI dammyHandUI = null;
    /// <summary>
    /// Card prefab
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab = null;
    /// <summary>
    /// Parent Transform of the card object to be generated
    /// </summary>
    [SerializeField]
    private Transform cardsParent = null;
    /// <summary>
    /// Transform of Deck object
    /// </summary>
    [SerializeField]
    private Transform deckIconTrs = null;
    /// <summary>
    /// Deck Icon Image Object
    /// </summary>
    [SerializeField]
    private GameObject deckIconObject = null;
    /// <summary>
    /// Number of decks remainingText
    /// </summary>
    [SerializeField]
    private Text deckNumText = null;
    /// <summary>
    /// Card effect activation button
    /// </summary>
    [SerializeField]
    private Button cardPlayButton = null;
    /// <summary>
    /// Job icon Image
    /// </summary>
    [SerializeField]
    private Image playerJobIconImage = null;
    /// <summary>
    /// Job name Text
    /// </summary>
    [SerializeField]
    private Text playerJobNameText = null;
    /// <summary>
    /// Job explanation Text
    /// </summary>
    [SerializeField]
    private Text playerJobExplainText = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Main Camera
    /// </summary>
    public Camera mainCamera;
    /// <summary>
    /// RectTransform of Canvas
    /// </summary>
    public RectTransform canvasRectTransform;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Draw interval time
    /// </summary>
    private const float DrawIntervalTime = 0.1f;
    /// <summary>
    /// Determination of the number of cards to be drawn
    /// </summary>
    private const int NextHandCardsNum = 5;
    /// <summary>
    /// Boundary to display text in white
    /// </summary>
    private const int SufficientLine = 10;
    /// <summary>
    /// Battle manager
    /// </summary>
    private BattleManager battleManager;
    /// <summary>
    /// Card under drag operation
    /// </summary>
    private Card draggingCard;
    /// <summary>
    /// Generated player manipulation card list
    /// </summary>
    private List<Card> cardInstances;
    /// <summary>
    /// Player's Current Deck (deck) Data
    /// </summary>
    private List<CardDataSO> playerDeckData;
    /// <summary>
    /// Backup of player's current deck data (required when adding deck cards during battle)
    /// </summary>
    private List<CardDataSO> playerDeckDataBackup;
    /// <summary>
    /// Hand align flag
    /// </summary>
    private bool reserveHandAlign;
    /// <summary>
    /// true: The hand is being replenished
    /// </summary>
    private bool isDrawing;
    /// <summary>
    /// Flag indicating that the card effect is being processed
    /// </summary>
    private bool isCardPlaying;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {
        // Processing during drag operation
        if (draggingCard != null) UpdateDragging();
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Field Manager Initialization
    /// </summary>
    /// <param name="battleManager"></param>
    public void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        cardInstances = new List<Card>();
        // UI initialization
        // Transparent display of the number of cards remaining in the deck
        deckNumText.color = Color.clear;
        SetJobUIs();
        // Debug draw process (delayed execution)
        //DOVirtual.DelayedCall(
        //    1.0f,   // 1.0 second delay
        //    () =>
        //    {
        //        OnBattleStarting();
        //        OnTurnStarting();
        //    }
        //);
    }
    /// <summary>
    /// Add the number of cards in the deck during battle.
    /// </summary>
    /// <param name="amount">Additional amounts</param>
    public void AddDeckCardsNum(int amount)
    {
        // Add one piece at a time
        for (int i = 0; i < amount; i++)
        {
            // Cards to be added are randomly determined from the deck data at the start of the battle
            CardDataSO targetCard = playerDeckDataBackup[UnityEngine.Random.Range(0, playerDeckDataBackup.Count)];
            // Add to current deck data
            playerDeckData.Add(targetCard);
        }
        // Display of the number of cards remaining in the deck
        PrintPlayerDeckNum();
    }

    #region Card drag process

    /// <summary>
    /// Start card dragging operation
    /// </summary>
    /// <param name="dragCard"></param>
    public void StartDragging(Card dragCard)
    {
        // If the hand is being replenished, the game ends
        if (isDrawing) return;
        // If the effect of the card is being executed, return
        if (isCardPlaying) return;
        // Memorize the card to be operated
        draggingCard = dragCard;
        // Putting the card object first among siblings before any other card object (frontmost display)
        draggingCard.transform.SetAsLastSibling();
    }
    /// <summary>
    /// Terminate the card dragging operation
    /// </summary>
    public void EndDragging()
    {
        // Exit if the card is not in operation
        if (draggingCard == null) return;
        // Get all information on overlapping objects
        // (All objects that need to be judged have BoxCollider2D assigned to them, so they are judged using that)
        // Get the screen coordinates of this object
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(mainCamera, draggingCard.transform.position);
        // Send Ray from the main camera to the coordinates obtained above
        Ray ray = mainCamera.ScreenPointToRay(pos);
        // Get object to be dragged
        // Drag target card zone
        CardZone targetZone = null;
        // Drug Destination Card
        Card targetCard = null;
        // Process for all objects hit by Ray
        foreach (var hit in Physics2D.RaycastAll(ray.origin, ray.direction, 10.0f))
        {
            // If the object hit does not exist, exit
            if (!hit.collider) break;
            // If the object hit is the same as the card being dragged, go to the next step
            var hitObj = hit.collider.gameObject;
            if (hitObj == draggingCard.gameObject) continue;
            // If the object is in a card area, get it and move on
            var hitArea = hitObj.GetComponent<CardZone>();
            if (hitArea != null)
            {
                targetZone = hitArea;
                continue;
            }
            // If the object is a card, get it and move on
            var hitCard = hitObj.GetComponent<Card>();
            if (hitCard != null)
            {
                targetCard = hitCard;
                continue;
            }
        }
        // Processing by overlapping objects
        if (targetCard != null &&
            (targetCard.nowZone >= CardZone.ZoneType.PlayBoard0 &&
            targetCard.nowZone <= CardZone.ZoneType.PlayBoard4))
        {
            // If card overlaps with a card on the play board, composit process
            CompositeCard(targetCard, draggingCard);
            CheckHandCardsNum();
        }
        else if (targetZone != null)
        {
            // If card does not overlap with the card but overlaps with the card area
            // 
            draggingCard.PutToZone(targetZone.zoneType, targetZone.GetComponent<RectTransform>().position);
            CheckHandCardsNum();
            // When moving from non-hand to hand, the card is placed last in the list
            if (draggingCard.nowZone == CardZone.ZoneType.Hand)
            {
                cardInstances.Remove(draggingCard);
                cardInstances.Add(draggingCard);
            }
        }
        else
        {
            // If none of the above overlaps with
            // Return card (that has been moved) to its original position
            draggingCard.BackToBasePos();
        }
        // Post-processing
        draggingCard = null;
    }
    /// <summary>
    /// Combine two cards into one card
    /// </summary>
    /// <param name="baseCard">Original card (main card)</param>
    /// <param name="consumeCard">Cards to be used as materials for composition (consumption cards)</param>
    private void CompositeCard(Card baseCard, Card consumeCard)
    {
        if (baseCard == null || consumeCard == null) return;
        // Check if there is an effect that makes the card itself unsynthesizable(カード自体の合成を不可にする効果の有無を確認)
        // ①: If the body card has a material-only effect(本体カードに素材限定効果がある場合)
        // ②: If the body card has a body-only effect(素材カードに本体限定効果がある場合)
        // ③: If the body card has an effect that disables composition(本体カードに合成無効効果がある場合)
        // ④: If the material card has an effect that disables composition(素材カードに合成無効効果がある場合)
        if (baseCard.CheckContainEffect(CardEffectDefine.CardEffect.PartsOnly) ||
            consumeCard.CheckContainEffect(CardEffectDefine.CardEffect.BaseOnly) ||
            baseCard.CheckContainEffect(CardEffectDefine.CardEffect.NoCompo) ||
            consumeCard.CheckContainEffect(CardEffectDefine.CardEffect.NoCompo))
        {
            // when compositing is not possible
            // Return the dragged card to its original position and exit without compositing
            draggingCard.BackToBasePos();
            return;
        }
        // Intensity addition and destroy condition check
        if (baseCard.SetForcePoint(baseCard.forcePoint + consumeCard.forcePoint))
        {
            // Destruction condition is met
            // Card deletion
            DestroyCardObject(baseCard);
            DestroyCardObject(consumeCard);
            return;
        }
        // Compose effects
        foreach (var effect in consumeCard.effects) baseCard.CompoCardEffect(effect);
        // Add icons
        foreach (var iconSprite in consumeCard.iconSprites) baseCard.AddCardIcon(iconSprite);
        // Delete consumeCard
        DestroyCardObject(consumeCard);
    }
    #endregion Card drag process

    #region Game progression

    /// <summary>
    /// Processing at the start of combat with the enemy
    /// </summary>
    public void OnBattleStarting()
    {
        // Get deck data
        playerDeckData = new List<CardDataSO>();
        playerDeckDataBackup = new List<CardDataSO>();
        foreach (var cardData in PlayerDeckData.deckCardList)
        {
            playerDeckData.Add(PlayerDeckData.CardDataBySerialNum[cardData]);
            playerDeckDataBackup.Add(PlayerDeckData.CardDataBySerialNum[cardData]);
        }
        // Display of the number of cards remaining in the deck
        PrintPlayerDeckNum();
    }
    /// <summary>
    /// Processing at start of turn
    /// </summary>
    public void OnTurnStarting()
    {
        // Determine the number of cards to draw
        // Number of cards in hand
        int nextHandCardsNum = Data.instance.playerHandNum;
        // If job is "Wise Man", one additional piece is added.職業「賢者」なら１枚追加
        if (Data.instance.playerJob == JobDataDefine.JobType.Sage)
            nextHandCardsNum++;
        // Draw process
        DrawCardsUntilNum(nextHandCardsNum);
        reserveHandAlign = true;
        // Enable card execution button
        cardPlayButton.interactable = true;
        // Place enemy cards
        PlacingEnemyCards();
    }
    /// <summary>
    /// Process executed at the end of the turn
    /// </summary>
    public void OnTurnEnd()
    {
        // Effect execution processing in progress flag
        isCardPlaying = false;
    }
    /// <summary>
    /// Processing when the button for activating card effects is pressed
    /// </summary>
    public void CardPlayButton()
    {
        // If card is being dragged, it will not be processed
        if (draggingCard != null) return;
        // Temporarily disable Run button
        cardPlayButton.interactable = false;
        // Effect execution processing in progress flag
        isCardPlaying = true;
        // Create an array of cards on the play board
        Card[] boardCards = new Card[PlayBoardManager.PlayBoardCardNum];
        // Retrieve cards on the play board and store them in an array
        foreach (var card in cardInstances)
        {
            // Stores the corresponding card at the specified position in the array
            if (card.nowZone >= CardZone.ZoneType.PlayBoard0 &&
                card.nowZone <= CardZone.ZoneType.PlayBoard4)
            {
                int arrayID = (int)card.nowZone - (int)CardZone.ZoneType.PlayBoard0;
                boardCards[arrayID] = card;
            }
        }
        // Execute the effect of each card
        battleManager.playBoardManager.BoardCardsPlay(boardCards);
    }

    #endregion Game progression

    #region Card Deletion

    /// <summary>
    /// Remove the specified card object from the field
    /// </summary>
    /// <param name="targetCard">Cards to be deleted</param>
    public void DestroyCardObject(Card targetCard)
    {
        // Remove from list
        cardInstances.Remove(targetCard);
        // Delete object
        Destroy(targetCard.gameObject);
    }
    /// <summary>
    /// Remove all card objects from the field (except enemy cards on the play area)
    /// </summary>
    public void DestroyAllCards()
    {
        // Delete objects
        foreach (var item in cardInstances) Destroy(item.gameObject);
        // Initialize list
        cardInstances.Clear();
    }

    #endregion Card Deletion

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// OnGUI (for repeated execution and UI control like Update)
    /// * Because it did not work properly on the Update method due to a conflict with the Layout Group specification
    /// </summary>
    private void OnGUI()
    {
        // If the hand alignment flag is set, the hand is aligned
        if (reserveHandAlign)
        {
            AlignHandCards();
            reserveHandAlign = false;
        }
    }

    /// <summary>
    /// Drag operation update process
    /// </summary>
    private void UpdateDragging()
    {
        // Get tap position
        Vector2 tapPos = Input.mousePosition;
        // Convert tap coordinates (screen coordinates → local coordinates of Canvas)
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            canvasRectTransform,    // RectTransform of Canvas
            tapPos,                 // source coordinate data
            mainCamera,             // Main Camera
            out tapPos              // Destination coordinate data
        );
        // Apply coordinates
        draggingCard.rectTransform.anchoredPosition = tapPos;
    }

    #region Player-side hand and deck processing

    /// <summary>
    /// Draw a card from the deck and put it into hand
    /// </summary>
    /// <param name="handID">Target hand-number</param>
    private void DrawCard(int handID)
    {
        // If there is no deck left, the game ends without drawing
        // Number of Decks Remaining
        int deckCardNum = playerDeckData.Count;
        if (deckCardNum <= 0) return;
        // Create card object from card prefab under card parent
        var obj = Instantiate(cardPrefab, cardsParent);
        // Get card processing class and store in list
        var objCard = obj.GetComponent<Card>();
        cardInstances.Add(objCard);
        // Randomly determines which cards are drawn from the deck
        CardDataSO targetCard = playerDeckData[UnityEngine.Random.Range(0, deckCardNum)];
        // Remove the relevant card from the deck list now
        playerDeckData.Remove(targetCard);
        // Initialize Card class
        objCard.Init(this, deckIconTrs.position);
        objCard.PutToZone(CardZone.ZoneType.Hand, dammyHandUI.GetHandPos(handID));
        objCard.SetInitialCardData(targetCard, Card.CharaIDPlayer);
        // Display of the number of cards remaining in the deck
        PrintPlayerDeckNum();
    }
    /// <summary>
    /// Draw cards until player has the specified number of cards in player hand
    /// </summary>
    /// <param name="num"></param>
    private void DrawCardsUntilNum(int num)
    {
        // Get the current number of cards in hand
        int nowHandNum = 0;
        foreach (var card in cardInstances)
            if (card.nowZone == CardZone.ZoneType.Hand) nowHandNum++;
        // Get the number of new cards to be drawn
        int drawNum = num - nowHandNum;
        if (drawNum <= 0) return;
        // When the number of decks is less than the number of draws:
        // Match the number of decks
        if (playerDeckData.Count < drawNum) drawNum = playerDeckData.Count;
        // Specify number of cards in hand UI
        dammyHandUI.SetHandNum(nowHandNum + drawNum);
        // Drawing cards in sequence (Sequence)
        var drawSequence = DOTween.Sequence();
        isDrawing = true;
        for (int i = 0; i < drawNum; i++)
        {
            // Process to draw one card (added to Sequence)
            drawSequence.AppendCallback(() =>
            {
                DrawCard(nowHandNum);
                nowHandNum++;
            });
            // Add time interval to Sequence
            drawSequence.AppendInterval(DrawIntervalTime);
        }
        drawSequence.OnComplete(() => isDrawing = false);
    }
    /// <summary>
    /// Align the cards in player hand
    /// </summary>
    private void AlignHandCards()
    {
        // hand sorting
        // Reference number in hand
        int index = 0;
        dammyHandUI.ApplyLayout();
        // Move each card to match the dummy hand
        foreach (var card in cardInstances)
        {
            if (card.nowZone == CardZone.ZoneType.Hand)
            {
                card.PutToZone(CardZone.ZoneType.Hand, dammyHandUI.GetHandPos(index));
                index++;
            }
        }
    }
    /// <summary>
    /// Align the current number of cards in hand with the hand UI processing class
    /// </summary>
    private void CheckHandCardsNum()
    {
        // Get the current number of cards in hand
        int nowHandNum = 0;
        foreach (var item in cardInstances)
            if (item.nowZone == CardZone.ZoneType.Hand) nowHandNum++;
        // Specify number of cards in dummy hand
        dammyHandUI.SetHandNum(nowHandNum);
        // Align the cards in the hand according to the number of cards in the hand
        // (Delayed execution for a moment because the dummy hand object is not moving in the same frame when the number of cards in the hand is changed)
        reserveHandAlign = true;
    }
    /// <summary>
    /// Update the display of the number of cards remaining in the player's side of the deck
    /// </summary>
    private void PrintPlayerDeckNum()
    {
        // Number of Decks Remaining
        int deckCardNum = playerDeckData.Count;
        // Text display of number of sheets
        deckNumText.text = playerDeckData.Count.ToString();
        // Display color changes according to the number of sheets remaining
        if (deckCardNum >= SufficientLine) deckNumText.color = Color.white;
        else if (deckCardNum > 0) deckNumText.color = Color.yellow;
        else deckNumText.color = Color.clear;
        // Display card icons in the deck if there is at least one card left
        if (deckCardNum > 0) deckIconObject.SetActive(true);
        else deckIconObject.SetActive(false);
    }
    #endregion Player-side hand and deck processing

    /// <summary>
    /// (At the beginning of the turn) Place all cards of the enemy side on the play board
    /// (ターン開始時)敵側のカードをプレイボードに全て設置する
    /// </summary>
    private void PlacingEnemyCards()
    {
        var enemyData = battleManager.characterManager.enemyData;
        // Get the number of the enemy card set to be used this turn(このターンに使用する敵カードセットの番号を取得)
        int enemyAttackOrderID = battleManager.nowTurns % enemyData.useCardData.Count;
        // Place enemy cards
        // List of cards used this turn
        var useCardDataInThisTurn = enemyData.useCardData[enemyAttackOrderID];
        for (int i = 0; i < PlayBoardManager.PlayBoardCardNum; i++)
        {
            // Get an placed card for each zone
            CardDataSO cardData = null;
            switch (i)
            {
                case 0:
                    cardData = useCardDataInThisTurn.placeCardData0;
                    break;
                case 1:
                    cardData = useCardDataInThisTurn.placeCardData1;
                    break;
                case 2:
                    cardData = useCardDataInThisTurn.placeCardData2;
                    break;
                case 3:
                    cardData = useCardDataInThisTurn.placeCardData3;
                    break;
                case 4:
                    cardData = useCardDataInThisTurn.placeCardData4;
                    break;
            }
            // No palced card, then next
            if (cardData == null) continue;
            // Get zone ID for placement
            var areaType = CardZone.ZoneType.PlayBoard0 + i;
            // Get the Vector2 coordinates of the placement location
            Vector2 targetPosition = battleManager.playBoardManager.GetPlayZonePos(i);
            // Create object
            var obj = Instantiate(cardPrefab, cardsParent);
            // Get card processing class and store in list
            Card objCard = obj.GetComponent<Card>();
            cardInstances.Add(objCard);
            // Initialize card
            // Card appearance coordinates are the same as the enemy image
            objCard.Init(this, battleManager.characterManager.GetEnemyPosition());
            objCard.PutToZone(areaType, targetPosition);
            // Specify Enemy ID
            objCard.SetInitialCardData(cardData, Card.CharaIDEnemy);
        }
    }
    /// <summary>
    /// Set job UIs
    /// </summary>
    private void SetJobUIs()
    {
        Sprite jobIcon = Data.instance.jobIcons[(int)Data.instance.playerJob];
        if (jobIcon == null) playerJobIconImage.color = Color.clear;
        else playerJobIconImage.sprite = jobIcon;
        // Job Text
        var jobData = JobDataDefine.DicJobData[Data.instance.playerJob];
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            playerJobNameText.text = jobData.nameJP;
            playerJobExplainText.text = jobData.explainJP;
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            playerJobNameText.text = jobData.nameEN;
            playerJobExplainText.text = jobData.explainEN;
        }
    }

    #endregion Private Methods

    #endregion Methods
}
