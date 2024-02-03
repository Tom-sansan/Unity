using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Card Class
/// </summary>
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// CardUI display setting class
    /// </summary>
    [SerializeField]
    private CardUI cardUI = null;
    /// <summary>
    /// CanvasGroup of object
    /// </summary>
    [SerializeField] private CanvasGroup canvasGroup = null;

    #endregion SerializeField

    #region Public Variables

    #region Card Data

    /// <summary>
    /// Card data on the basic ScriptableObject side
    /// </summary>
    [HideInInspector]
    public CardDataSO baseCardData;
    /// <summary>
    /// Card icon list
    /// </summary>
    [HideInInspector]
    public List<Sprite> iconSprites;
    /// <summary>
    /// Card effect list
    /// </summary>
    [HideInInspector]
    public List<CardEffectDefine> effects;
    /// <summary>
    /// Intensity
    /// </summary>
    [HideInInspector]
    public int forcePoint;
    /// <summary>
    /// Character ID used for the card 
    /// </summary>
    [HideInInspector]
    public int controllerCharaID;

    #endregion Card Data

    #region Character ID

    /// <summary>
    /// Number of characters in battle
    /// </summary>
    public const int CharaNum = 2;
    /// <summary>
    /// Character ID: Main character (player character)
    /// </summary>
    public const int CharaIDPlayer = 0;
    /// <summary>
    /// Character ID: Enemy
    /// </summary>
    public const int CharaIDEnemy = 1;
    /// <summary>
    /// Character ID: None
    /// </summary>
    public const int CharaIDNone = -1;
    /// <summary>
    /// Character ID: For Bonus
    /// </summary>
    public const int CharaIDBonus = 2;

    #endregion Character ID

    #region Other Variables

    /// <summary>
    /// The card zone where this card is placed
    /// </summary>
    [HideInInspector]
    public CardZone.ZoneType nowZone;
    /// <summary>
    /// RectTransform
    /// </summary>
    public RectTransform rectTransform;
    /// <summary>
    /// Card flags in deck (for deck edit screen)
    /// </summary>
    public bool isInDeckCard;

    #endregion

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Maximum number of card icons
    /// </summary>
    private const int MaxIcons = 6;
    /// <summary>
    /// Maximum number of card effects
    /// </summary>
    private const int MaxEffects = 6;
    /// <summary>
    /// Card movement animation time
    /// </summary>
    private const float MoveTime = 0.4f;
    /// <summary>
    /// Alpha value of CanvasGroup to be changed
    /// </summary>
    private const float TargetAlpha = 0.5f;
    /// <summary>
    /// Performancen time at 0.3
    /// </summary>
    private const float AnimTime03 = 0.3f;
    /// <summary>
    /// Destination X relative coordinate
    /// </summary>
    private const float TargetPositionXRelative = -300.0f;
    /// <summary>
    /// Performancen time at 1.0
    /// </summary>
    private const float AnimTime1 = 1.0f;
    /// <summary>
    /// Upper limit of card strength (if this limit is exceeded, the card is destroyed)
    /// </summary>
    private const int MaxForcePoint = 9;
    /// <summary>
    /// Field manger
    /// </summary>
    private FieldManager fieldManager;
    /// <summary>
    /// For deck edit screen
    /// </summary>
    private DeckEditWindow deckEditWindow;
    /// <summary>
    /// For battle reward screen
    /// </summary>
    private Reward rewardPanel;
    /// <summary>
    /// Move Tween
    /// </summary>
    private Tween moveTween;
    /// <summary>
    /// Basic coordinates (coordinates to return to after the end of the drag)
    /// </summary>
    private Vector2 basePos;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    void Start()
    {
        // Debug.Log("Scene has started!");
    }

    void Update()
    {

    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="fieldManager"></param>
    /// <param name="initPos">Initial position</param>
    public void Init(FieldManager fieldManager, Vector2 initPos)
    {
        this.fieldManager = fieldManager;
        // Initialization of subordinate components
        cardUI.Init(this);
        rectTransform.position = initPos;
        basePos = initPos;
        nowZone = CardZone.ZoneType.Hand;
        iconSprites = new List<Sprite>();
        effects = new List<CardEffectDefine>();
    }
    /// <summary>
    /// Initialization (for calling from deck edit class)
    /// </summary>
    /// <param name="deckEditWindow">DeckEditWindow reference</param>
    /// <param name="isInDeckCard">Card flags in the deck</param>
    public void Init(DeckEditWindow deckEditWindow, bool isInDeckCard)
    {
        this.deckEditWindow = deckEditWindow;
        this.isInDeckCard = isInDeckCard;
        Init(null, Vector2.zero);
    }
    /// <summary>
    /// Initialization (for calling from the battle reward screen)
    /// </summary>
    /// <param name="rewardPanel"></param>
    public void Init(Reward rewardPanel)
    {
        this.rewardPanel = rewardPanel;
        Init(null, Vector2.zero);
    }
    /// <summary>
    /// Get and set various parameters from card definition data
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="cardControllerCharaID"></param>
    public void SetInitialCardData(CardDataSO cardData, int cardControllerCharaID)
    {
        baseCardData = cardData;
        // Card name
        cardUI.SetCardNameText(cardData.cardNameJP, cardData.cardNameEN);
        // Card Icon
        AddCardIcon(cardData.iconSprite);
        // Card effect list
        foreach (var item in cardData.effectList) AddCardEffect(item);
        // intensity
        SetForcePoint(cardData.force);
        // Cardholder data
        controllerCharaID = cardControllerCharaID;
        // Applied to card background UI
        cardUI.SetCardBackSprite(cardControllerCharaID);
    }
    /// <summary>
    /// Run a tween that makes the card a little thinner
    /// </summary>
    /// <returns>Running Tween</returns>
    public Tween HideFadeTween() =>
        canvasGroup.DOFade(TargetAlpha, AnimTime03);
    /// <summary>
    /// Execute a tween to move a card off-screen
    /// </summary>
    public void HideMoveTween() =>
        rectTransform
            .DOAnchorPosX(TargetPositionXRelative, AnimTime1)
            .SetRelative();

    #region Tap event

    /// <summary>
    /// Executed at the start of a tap.
    /// IPointerDownHandler is necessary.
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Drag start process
        if (fieldManager != null) fieldManager.StartDragging(this);
        else if (deckEditWindow != null) deckEditWindow.SelectCard(this);
        else if (rewardPanel != null) rewardPanel.SelectCard(this);
    }
    /// <summary>
    /// Executed at the end of a tap
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Drag end process
        if (fieldManager != null) fieldManager.EndDragging();
    }

    #endregion Tap event

    #region Object movement and display direction

    /// <summary>
    /// Move card to the basic coordinates
    /// </summary>
    public void BackToBasePos()
    {
        // Stop any moving animations already in progress
        if (moveTween != null) moveTween.Kill();
        // Animation to move to a specified point (DOTween)
        moveTween = rectTransform
            .DOMove(basePos, MoveTime)  // Moving Tween
            .SetEase(Ease.OutQuad);     // Specify how to change
    }
    /// <summary>
    /// Place cards in designated zones
    /// </summary>
    /// <param name="zoneType">Target card zone</param>
    /// <param name="targetPos">Target coordinates</param>
    public void PutToZone(CardZone.ZoneType zoneType, Vector2 targetPos)
    {
        // Get coordinates and move
        basePos = targetPos;
        BackToBasePos();
        // Save card zone type
        nowZone = zoneType;
    }

    #endregion Object movement and display direction

    #region Parameter addition/update

    /// <summary>
    /// Add card icon
    /// </summary>
    /// <param name="newIcon">New icon to be added</param>
    public void AddCardIcon(Sprite newIcon)
    {
        // If the number of card icons is capped, it ends
        if (iconSprites.Count >= MaxIcons) return;
        // Add icon list
        iconSprites.Add(newIcon);
        // Show UI
        cardUI.AddCardIconImage(newIcon);

    }
    /// <summary>
    /// Add card effects through composition
    /// </summary>
    /// <param name="newEffect">Types of effects and numerical data</param>
    public void CompoCardEffect(CardEffectDefine newEffect)
    {
        // Get the possibility of composing the effect
        var compoMode = CardEffectDefine.DicEffectCompoMode[newEffect.cardEffect];
        // Get whether this card is on the enemy side
        bool isEnemyCard = false;
        if (controllerCharaID == CharaIDEnemy) isEnemyCard = true;
        // If composition is not possible, exit
        switch (compoMode)
        {
            case CardEffectDefine.EffectCompoMode.Impossible:
                // Impossible to compose
                break;
            case CardEffectDefine.EffectCompoMode.OnlyOwn:
                // Composition is possible only between player and card
                if (isEnemyCard) return;
                break;
            case CardEffectDefine.EffectCompoMode.OnlyOwnNew:
                // Can be combined only with player and card (only for new card)
                if (isEnemyCard) return;
                break;
        }
        // Find out if the same type of effect already exists as the effect being added
        // Number of current effect types
        int length = effects.Count;
        // Number in sequence of the same effect data
        int index;
        for (index = 0; index < length; index++)
            // If present: Save the number and exit the loop
            if (effects[index].cardEffect == newEffect.cardEffect) break;
        // Composite and additive processing of effects
        if (index < length)
        {
            // Same type of effect: Composition process
            // If the effect is limited to newly added effects, do not compose
            if (compoMode == CardEffectDefine.EffectCompoMode.OnlyNew ||
                compoMode == CardEffectDefine.EffectCompoMode.OnlyOwnNew)
                return;
            // Effect value increase/decrease
            EnhanceCardEffect(index, newEffect.value);
        }
        // No effect of the same type: new addition process
        else AddCardEffect(newEffect);
    }
    /// <summary>
    /// Add card effect
    /// </summary>
    public void AddCardEffect(CardEffectDefine newEffect)
    {
        // If the number of card effects is capped, it ends
        if (effects.Count >= MaxEffects) return;
        // Create new effect data
        var effectData = new CardEffectDefine();
        effectData.cardEffect = newEffect.cardEffect;
        effectData.value = newEffect.value;
        // Add effect data to effect list
        effects.Add(effectData);
        // Show UI
        cardUI.AddCardEffectText(effectData);
    }
    /// <summary>
    /// Increase or decrease the effect value of a card
    /// </summary>
    /// <param name="index">Number in sequence of target effect data</param>
    /// <param name="value">Amount of change</param>
    public void EnhanceCardEffect(int index, int value)
    {
        // Change effect value
        var effectData = effects[index];
        effectData.value += value;
        // Show UI
        cardUI.ApplyCardEffectText(effects[index]);
    }
    /// <summary>
    /// Increases or decreases the effect value of a card (specifies the type of effect directly)
	/// If the specified effect does not exist, exit without doing anything
    /// </summary>
    /// <param name="effectType">Target effect data</param>
    /// <param name="value">Variation</param>
    public void EnhanceCardEffect(CardEffectDefine.CardEffect effectType, int value)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].cardEffect == effectType)
            {
                // Target effect exists
                // Effect value is changed
                effects[i].value += value;
                // Display UI
                cardUI.ApplyCardEffectText(effects[i]);
                return;
            }
        }
    }
    /// <summary>
    /// Set card intensity
    /// Returns true when a card's destruction condition (strength 10 or greater) is met
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool SetForcePoint(int value)
    {
        // Set parameter
        forcePoint = value;
        // Show UI
        cardUI.SetForcePointText(forcePoint);
        // Confirmation of destruction conditions
        return value > MaxForcePoint;
    }

    #endregion Parameter addition/update

    #region Get/Set

    /// <summary>
    /// Find the existence of the relevant effect type in the effect list
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public bool CheckContainEffect(CardEffectDefine.CardEffect effectType)
    {
        // returns true if applicable effect
        foreach (var effect in effects)
            if (effect.cardEffect == effectType) return true;
        // Returns false if not present
        return false;
    }
    /// <summary>
    /// Return effect amount if effect type exists in effect list
	/// (-1 if effect does not exist)
    /// </summary>
    /// <param name="effectType"></param>
    /// <returns></returns>
    public int GetEffectValue(CardEffectDefine.CardEffect effectType)
    {
        // Return effect size if applicable
        foreach (var effect in effects)
            if (effect.cardEffect == effectType) return effect.value;
        // If not, return -1
        return -1;
    }
    /// <summary>
    /// Displays the number of cards in the player's storage (for deck editing)
    /// </summary>
    public void ShowCardAmountInStorage()
    {
        int amount = PlayerDeckData.storageCardList[baseCardData.serialNum];
        cardUI.SetAmountText(amount);
    }
    /// <summary>
    /// Change the highlighting status of a card
    /// </summary>
    /// <param name="mode"></param>
    public void SetCardHighlight(bool mode) =>
        cardUI.SetHighlightImage(mode);
    /// <summary>
    /// Reset display of icons and effects
    /// </summary>
    public void ClearIconsAndEffects()
    {
        iconSprites.Clear();
        effects.Clear();
        cardUI.ClearIconsAndEffects();
    }
    #endregion Get/Set

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
