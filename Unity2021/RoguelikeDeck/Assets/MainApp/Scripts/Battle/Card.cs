using DG.Tweening;
using System.Collections.Generic;
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
    /// Field manger
    /// </summary>
    private FieldManager fieldManager;
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
        fieldManager.StartDragging(this);
        Debug.Log("Card is tapped.");
    }
    /// <summary>
    /// Executed at the end of a tap
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Drag end process
        fieldManager.EndDragging();
        Debug.Log("Tap to card has ended.");
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
    /// Set card intensity
    /// </summary>
    /// <param name="value"></param>
    public void SetForcePoint(int value)
    {
        // Set parameter
        forcePoint = value;
        // Show UI
        cardUI.SetForcePointText(forcePoint);
    }

    #endregion Parameter addition/update

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
