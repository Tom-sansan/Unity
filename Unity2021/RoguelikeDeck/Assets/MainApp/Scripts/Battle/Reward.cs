using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Processing of reward acquisition screen after battle
/// </summary>
public class Reward : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Canvas group
    /// </summary>
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    /// <summary>
    /// Reward text
    /// </summary>
    [SerializeField]
    private Text rewardText = null;
    /// <summary>
    /// Decision (Bonus Acquisition) Button
    /// </summary>
    [SerializeField]
    private Button decideButton = null;
    /// <summary>
    /// Text in the decision (bonus acquisition) button
    /// </summary>
    [SerializeField]
    private Text decideButtonText = null;
    /// <summary>
    /// Card prefab
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab = null;
    /// <summary>
    /// Parent Transform of the bonus card object
    /// </summary>
    [SerializeField]
    private Transform cardsParent = null;
    /// <summary>
    /// List of bonus-only card data (stored in Inspector in EXP, Gold, and Heal order)
    /// </summary>
    [SerializeField]
    private List<CardDataSO> bonusCardSOsList = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Screen fade in/out time
    /// </summary>
    private const float FadeTime = 1.0f;
    /// <summary>
    /// Percentage of cards dedicated to the bonus
    /// </summary>
    private const float PercentageBonusOnlyCards = 0.8f;
    /// <summary>
    /// Battle screen manager
    /// </summary>
    private BattleManager battleManager;
    /// <summary>
    /// Data of enemies fought (updated when the screen is displayed)
    /// </summary>
    private EnemyStatusSO enemySO;
    /// <summary>
    /// List of generated bonus cards
    /// </summary>
    private List<Card> cardInstances;
    /// <summary>
    /// Bonus card information during selection
    /// </summary>
    private List<Card> selectingBonus;

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
    /// <param name="battleManager"></param>
    public void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        selectingBonus = new List<Card>();
        cardInstances = new List<Card>();
        // UI initialization
        gameObject.SetActive(false);
        SetCanvasGroup(false, 0.0f);
    }

    public void OpenWindow(EnemyStatusSO enemySO)
    {
        // Get enemy data
        this.enemySO = enemySO;
        // Initialize CanvasGroup
        gameObject.SetActive(true);
        SetCanvasGroup(true, 0.0f);
        // Fade in
        var fadeTween = canvasGroup.DOFade(1.0f, FadeTime);
        // Create the required number of bonus card objects
        CreateBonusCards();
        // Reward Text Setting
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            // Japanese
            // Battle result text display
            string resultMes = $"{enemySO.enemyNameJP}を撃破した！\n<size=30>獲得ボーナスを{enemySO.bonusPoint}つ選んでください</size>";
            rewardText.text = resultMes;
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            // English
            // Battle result text display
            string resultMes = $"{enemySO.enemyNameEN} destroyed！\n<size=30>Please select {enemySO.bonusPoint} bonuses to be earned.</size>";
            rewardText.text = resultMes;
        }
        // Button UI initialization
        ShowRemainingSelections();
    }
    /// <summary>
    /// select bonus card process for tapping destination
    /// </summary>
    public void SelectCard(Card targetCard)
    {
        if (selectingBonus.Contains(targetCard))
        {
            // When tapping a card that has already been selected
            // De-emphasize the card
            targetCard.SetCardHighlight(false);
            // Deselect
            selectingBonus.Remove(targetCard);
        }
        else
        {
            // When an unselected card is tapped
            // If the maximum number of selections has already been reached, it will not be processed
            if (selectingBonus.Count >= enemySO.bonusPoint) return;
            // Card highlight
            targetCard.SetCardHighlight(true);
            // Memorize selection card information
            selectingBonus.Add(targetCard);
            // Reflecte in button UI
            ShowRemainingSelections();
        }
    }
    /// <summary>
    /// Decide button process
    /// </summary>
    public void ButtonDecide()
    {
        // Close window
        var tween = CloseWindow();
        // Direction complete
        tween.OnComplete(() =>
        {
            // Apply bonus
            foreach (var card in selectingBonus) ApplyBonus(card);
            // Delete card object
            foreach (var card in cardInstances) Destroy(card.gameObject);
            // Initialize each list
            cardInstances.Clear();
            selectingBonus.Clear();
            // Deactivate battle result screen object
            gameObject.SetActive(false);
            // Resume stage progression
            battleManager.ProgressingStage();
        });
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Hide window
    /// </summary>
    /// <returns>Tween for fade-out performance</returns>
    private Tween CloseWindow()
    {
        // Initialize CanvasGroup
        SetCanvasGroup(false, 1.0f);
        // Fade out
        return canvasGroup.DOFade(0.0f, FadeTime);
    }
    /// <summary>
    /// Display a specified number of bonus cards
    /// </summary>
    private void CreateBonusCards()
    {
        // Cards appear in succession
        int nowHandNum = 0;
        for (int i = 0; i < enemySO.bonusOptions; i++)
        {
            // Draw one card
            CreateCard(nowHandNum);
            nowHandNum++;
        }
    }
    /// <summary>
    /// Create one bonus card
    /// </summary>
    /// <param name="handID"></param>
    private void CreateCard(int handID)
    {
        // Create object
        var obj = Instantiate(cardPrefab, cardsParent);
        // get card processing class and store in list
        Card objCard = obj.GetComponent<Card>();
        cardInstances.Add(objCard);
        // Randomly determine cards that appear
        CardDataSO targetCard = null; // enemySO.bonusCardList[Random.Range(0, enemySO.bonusCardList.Count)];
        // Flag when a bonus-only card appears
        bool isBonusOnlyCard;
        if (Random.value < PercentageBonusOnlyCards)
        {
            // Dedicated Bonus Card
            isBonusOnlyCard = true;
            // Determine type of card
            int rand = Random.Range(0, bonusCardSOsList.Count);
            targetCard = bonusCardSOsList[rand];
        }
        else
        {
            // Cards that players can acquire
            targetCard = enemySO.bonusCardList[Random.Range(0, enemySO.bonusCardList.Count)];
            isBonusOnlyCard = false;
        }
        // Initialize Card
        objCard.Init(this);
        // For bonus-only cards, use a different background
        if (isBonusOnlyCard) objCard.SetInitialCardData(targetCard, Card.CharaIDBonus);
        else objCard.SetInitialCardData(targetCard, Card.CharaIDPlayer);
        // Apply various effect amounts for bonus cards
        objCard.EnhanceCardEffect(CardEffectDefine.CardEffect.BonusEXP, battleManager.GetBonusEXPValue());
        objCard.EnhanceCardEffect(CardEffectDefine.CardEffect.BonusGold, battleManager.GetBonusGoldValue());
        objCard.EnhanceCardEffect(CardEffectDefine.CardEffect.BonusHeal, battleManager.GetBonusHealValue());
    }
    /// <summary>
    /// The number of bonuses remaining to be selected is displayed in the button Text
	/// Activate button if selection is complete
    /// </summary>
    private void ShowRemainingSelections()
    {
        // Get current selection status
        // Number of Selection
        int selectingNum = enemySO.bonusPoint - selectingBonus.Count;
        string mes = string.Empty;
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            // Japanese
            // Reflects display content in settings and button press enable/disable
            if (selectingNum == 0)
            {
                // When selection is complete
                mes = "獲　得";
                // Enable button
                decideButton.interactable = true;
            }
            else
            {
                // when not selected
                mes = $"あと{selectingNum}つ選択";
                // Disable button
                decideButton.interactable = false;
            }
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            // English
            // Reflects display content in settings and button press enable/disable
            if (selectingNum == 0)
            {
                // When selection is complete
                mes = "Acquisition";
                // Enable button
                decideButton.interactable = true;
            }
            else
            {
                // when not selected
                mes = $"{selectingNum} more choices";
                // Disable button
                decideButton.interactable = false;
            }
        }
        // Reflect in Text
        decideButtonText.text = mes;
    }
    /// <summary>
    /// Apply and get bonus effects for selected cards
    /// </summary>
    /// <param name="targetCard">Cards eligible for acquisition</param>
    private void ApplyBonus(Card targetCard)
    {
        // Get experience
        int valueEXP = targetCard.GetEffectValue(CardEffectDefine.CardEffect.BonusEXP);
        if (valueEXP > 0)
        {
            Data.instance.ChangePlayerEXP(valueEXP);
            battleManager.ApplyEXPText();
        }
        // Get gold coin
        int valueGold = targetCard.GetEffectValue(CardEffectDefine.CardEffect.BonusGold);
        if (valueGold > 0)
        {
            Data.instance.ChangePlayerGold(valueGold);
            battleManager.ApplyGoldText();
        }
        // Physical recovery
        int valueHeal = targetCard.GetEffectValue(CardEffectDefine.CardEffect.BonusHeal);
        if (valueHeal > 0)
            battleManager.characterManager.ChangeStatusNowHP(Card.CharaIDPlayer, valueHeal);
        // If card does not have any of the above effects, treat it as a player-acquired card
        if (valueEXP < 0 && valueGold < 0 && valueHeal < 0)
            PlayerDeckData.ChangeStorageCardNum(targetCard.baseCardData.serialNum, 1);
    }
    /// <summary>
    /// Set CanvasGroup
    /// </summary>
    /// <param name="isTrue"></param>
    /// <param name="alphaValue"></param>
    private void SetCanvasGroup(bool isTrue, float alphaValue)
    {
        canvasGroup.alpha = alphaValue;
        canvasGroup.interactable = isTrue;
        canvasGroup.blocksRaycasts = isTrue;
    }
    #endregion Private Methods

    #endregion Methods
}
