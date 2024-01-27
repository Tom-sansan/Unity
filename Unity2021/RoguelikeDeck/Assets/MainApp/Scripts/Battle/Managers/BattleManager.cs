using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Battle Manager Class
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Enemy data (for debugging)
    /// </summary>
    [SerializeField]
    private EnemyStatusSO enemyStatusSO = null;
    /// <summary>
    /// Stage Name Text
    /// </summary>
    [SerializeField]
    private Text stageNameText = null;
    /// <summary>
    /// Stage Icon Image
    /// </summary>
    [SerializeField]
    private Image stageIconImage = null;
    /// <summary>
    /// Stage Background Image
    /// </summary>
    [SerializeField]
    private Image stageBackGroundImage = null;
    /// <summary>
    /// Stage progress gauge image
    /// </summary>
    [SerializeField] private Image progressGageImage = null;
    /// <summary>
    /// Amount of experienceText
    /// </summary>
    [SerializeField]
    private Text playerEXPText = null;
    /// <summary>
    /// Gold in possessionText
    /// </summary>
    [SerializeField]
    private Text playerGoldText = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Field manager class
    /// </summary>
    public FieldManager fieldManager;
    /// <summary>
    /// Character data management class
    /// </summary>
    public CharacterManager characterManager;
    /// <summary>
    /// Card effect activation management class
    /// </summary>
    public PlayBoardManager playBoardManager;
    /// <summary>
    /// Battle Reward Screen Class
    /// </summary>
    public Reward rewardPanel;
    /// <summary>
    /// Boss appearance processing class
    /// </summary>
    public BossIncoming bossIncoming;
    /// <summary>
    /// Stage clear class
    /// </summary>
    public StageClear stageClear;
    /// <summary>
    /// Game over class
    /// </summary>
    public GameOver gameOver;
    /// <summary>
    /// Stage data under attack
    /// </summary>
    public StageSO stageSO;
    /// <summary>
    /// Current number of turns(0-)
    /// </summary>
    [HideInInspector]
    public int nowTurns;
    /// <summary>
    /// Current stage progression (0-)
    /// </summary>
    [HideInInspector]
    public int nowProgress;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Amount of stage progress added when calculating bonus amount
    /// (ボーナス量計算時のステージ進行度加算量)
    /// </summary>
    private const int BonusValueBase = 4;
    /// <summary>
    /// // Bonus amount random range: min
    /// </summary>
    private const float BonusRandomMultiMin = 0.7f;
    /// <summary>
    /// Random range of bonus amount: max.
    /// </summary>
    private const float BonusRandomMultiMax = 1.3f;
    /// <summary>
    /// Gauge display performance time
    /// </summary>
    private const float GageAnimationTime = 2.0f;
    /// <summary>
    /// Production time
    /// </summary>
    private const float AnimationTime = 1.0f;
    /// <summary>
    /// Progression at which stage boss appears
    /// </summary>
    private int battleNum;
    /// <summary>
    /// Variable for displaying experience amountText
    /// </summary>
    private int playerEXPDisp;
    /// <summary>
    /// Variable for displaying Text of gold coins in possession
    /// </summary>
    private int playerGoldDisp;
    /// <summary>
    /// Infinite stage mode
    /// </summary>
    private bool isInfinity;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // Get stage info
        stageSO = Data.instance.stageSOs[Data.instance.nowStageID];
        // progression initialization
        nowProgress = -1;
        // Set infinity stage mode
        isInfinity = stageSO.infinityMode;
        SetInfinityStageMode(isInfinity);
        // Get progression where stage boss appears
        battleNum = stageSO.appearEnemyTables.Count;
        // Managers initialization
        Init();
        // Show Stage info
        ApplyStageUIs();
        // Initialize experience and gold coin UI
        ApplyEXPText();
        ApplyGoldText();
        // Start battle
        DOVirtual.DelayedCall(
            1.0f,
            () =>
            {
                // Spawn enemy
                //characterManager.SpawnEnemy(enemyStatusSO);
                ProgressingStage();
            }
            // false
        );

        // Card effect name display test
        //foreach (var cardEffect in testCardData.effectList)
        //{
        //    // Get effect name string
        //    string nameText = CardEffectDefine.DicEffectNameJP[cardEffect.cardEffect];
        //    nameText = string.Format(nameText, cardEffect.value);
        //    Debug.Log(nameText);
        //}
    }
    void Update()
    {
        // For debug
        // if (Input.GetKeyDown(KeyCode.Space))
        //    characterManager.ChangeStatusNowHP(Card.CharaIDPlayer, -5);
#if UNITY_EDITOR
        ProcessDebug();
#endif
    }
    #endregion Unity Methods

    #region Public Methods

    #region Stage progression

    /// <summary>
    /// Advance the stage progression to start battle or exit the stage
    /// </summary>
    public void ProgressingStage()
    {
        // advance the level of progress
        nowProgress++;
        // Progress is acquired and displayed in 0.0f~1.0f
        float progressRatio = (float)(nowProgress % battleNum + 1) / battleNum;
        progressRatio = Mathf.Clamp(progressRatio, 0.0f, 1.0f);
        ShowProgressGage(progressRatio);
        // Next Enemy Appearance
        DOVirtual.DelayedCall(
            0.5f,
            () =>
            {
                BattleStart();
            }
        );
    }
    /// <summary>
    /// Start a battle with a new enemy
    /// </summary>
    public void BattleStart()
    {
        // Turns initialization
        nowTurns = 0;
        // Stage Clear Confirmation
        if (nowProgress >= stageSO.appearEnemyTables.Count)
        {
            // Victory in battle against all enemies
            if (!isInfinity)
            {
                // Normal stage
                // Start stage clear animation
                stageClear.StartAnimation();
            }
        }
        // Process appearance of enemy characters
        if (!isInfinity)
        {
            // Normal stage
            // Determines which enemies appear
            var appearEnemyTable = stageSO.appearEnemyTables[nowProgress].appearEnemies;
            int rand = Random.Range(0, appearEnemyTable.Count);
            characterManager.SpawnEnemy(appearEnemyTable[rand]);
            // If there is only one type of enemy that appears, then the direction for the boss
            if (appearEnemyTable.Count == 1) bossIncoming.StartAnimation();
        }
        else
        {
            // Infinity stage
            // Determines which enemies appear
            var appearEnemyTable = stageSO.infinityEnemyData;
            // HP Increase
            int hpIncrease = nowProgress * stageSO.enemyHPIncrease;
            // During boss battles, determined by the boss enemy group
            if (nowProgress % battleNum == battleNum - 1)
            {
                // During a boss battle
                appearEnemyTable = stageSO.infinityBossData;
                bossIncoming.StartAnimation();
            }
            // Enemy appearance
            int randEnemyID = Random.Range(0, appearEnemyTable.Count);
            characterManager.SpawnEnemy(appearEnemyTable[randEnemyID], hpIncrease);
        }
        // Start battle(delayed execution)
        DOVirtual.DelayedCall(
            // TODO 1.0f??
            0.5f,
            () =>
            {
                // Processing at the start of combat on the FieldManager side
                fieldManager.OnBattleStarting();
                // Start turn
                TurnStart();
            },
            false
        );
    }
    /// <summary>
    /// Start turn
    /// </summary>
    public void TurnStart()
    {
        // Processing at the start of FieldManager's turn
        fieldManager.OnTurnStarting();
        // Count the number of turn
        nowTurns++;
    }
    /// <summary>
    /// End turn
    /// </summary>
    public void TurnEnd()
    {
        // FieldManager side end processing call
        fieldManager.OnTurnEnd();
        // Call for processing at end of CharacterManager side
        characterManager.OnTurnEnd();
        // Confirm the end of battle
        bool isPlayerWin = characterManager.IsEnemyDefeated();
        bool isPlayerLose = characterManager.IsPlayerDefeated();
        // End-of-battle processing
        if (isPlayerWin || isPlayerLose)
        {
            // Delete cards on the field
            fieldManager.DestroyAllCards();
            // End of sustained effects of cards activated during combat(戦闘中に発動したカードの持続効果終了)
            playBoardManager.ClearAllSustainedEffects();
            // Processing by winning character (delayed execution)
            DOVirtual.DelayedCall(
                0.5f,
                () =>
                {
                    // Player lose
                    if (isPlayerLose)
                    {
                        // Start game over animation
                        gameOver.StartAnimation();
                    }
                    // Player win
                    else if (isPlayerWin)
                    {
                        // Increase in progression
                        // ProgressingStage();
                        // Display the battle reward screen
                        rewardPanel.OpenWindow(characterManager.enemyData);
                    }
                }
            );
            return;
        }
        // Start next turn
        TurnStart();
    }

    #endregion Stage progression

    #region Stage Battle Rewards

    /// <summary>
    /// Return amount of gold obtained from enemy destruction bonus
    /// </summary>
    /// <returns></returns>
    public int GetBonusGoldValue() =>
        GetBonusValue(stageSO.bonusGold * (BonusValueBase + nowProgress));

    /// <summary>
    /// Return amount of experience gained from enemy destruction bonus
    /// </summary>
    /// <returns></returns>
    public int GetBonusEXPValue() =>
        // Base acquisition
        GetBonusValue(stageSO.bonusEXP * (BonusValueBase + nowProgress));
    /// <summary>
    /// Return amount of HP recovery for enemy destruction bonus
    /// </summary>
    /// <returns></returns>
    public int GetBonusHealValue() =>
        GetBonusValue(stageSO.bonusHeal);
    /// <summary>
    /// Update display of experience amount Text
    /// </summary>
    public void ApplyEXPText()
    {
        // Gradually changing number production
        DOTween.To(() =>
            playerEXPDisp,
            (n) => playerEXPDisp = n,
            Data.instance.playerEXP,
            AnimationTime
        ).OnUpdate(() =>
        {
            playerEXPText.text = playerEXPDisp.ToString("#,0") + " EXP";
        });
    }
    /// <summary>
    /// Update display of gold in possessionText
    /// </summary>
    public void ApplyGoldText()
    {
        // Gradually changing number production
        DOTween.To(() =>
            playerGoldDisp,
            (n) => playerGoldDisp = n,
            Data.instance.playerGold,
            AnimationTime
        ).OnUpdate(() =>
        {
            playerGoldText.text = playerGoldDisp.ToString("#,0") + " G";
        });
    }

    #endregion  Stage Battle Rewards

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Managers initialization
    /// </summary>
    private void Init()
    {
        fieldManager.Init(this);
        characterManager.Init(this);
        playBoardManager.Init(this);
        bossIncoming.Init();
        stageClear.Init();
        gameOver.Init();
        rewardPanel.Init(this);
    }

    #region Stage UI

    /// <summary>
    /// Update the display of various stage information UI based on the data of the stage under attack
    /// </summary>
    private void ApplyStageUIs()
    {
        // Stage name
        if (Data.nowLanguage == SystemLanguage.Japanese) stageNameText.text = stageSO.nameJP;
        else if (Data.nowLanguage == SystemLanguage.English) stageNameText.text = stageSO.nameEN;
        // Stage icon
        stageIconImage.sprite = stageSO.stageIcon;
        // Stage background
        stageBackGroundImage.sprite = stageSO.stagePicture;
        // Stage progress gauge initialization
        progressGageImage.fillAmount = 0.0f;
    }

    #endregion Stage UI

    /// <summary>
    /// Display stage progress gauge
    /// </summary>
    /// <param name="ratio">Progression rate (0.0f-1.0f)</param>
    private void ShowProgressGage(float ratio)
    {
        progressGageImage.DOFillAmount(ratio, GageAnimationTime);
        // (For infinite stages) Display the number of floors at the end of the stage name
        if (isInfinity)
        {
            int floorNum = nowProgress + 1;
            if (Data.nowLanguage == SystemLanguage.Japanese)
                stageNameText.text = stageSO.nameJP + " " + floorNum + "階";
            else if (Data.nowLanguage == SystemLanguage.English)
                stageNameText.text = stageSO.nameEN + " " + floorNum + "F";
        }
    }
    /// <summary>
    /// Get bonus value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private int GetBonusValue(int value) =>
        // Random width application
        (int)(value * Random.Range(BonusRandomMultiMin, BonusRandomMultiMax));
    /// <summary>
    /// Set infinity stage mode
    /// </summary>
    /// <param name="isInfinity"></param>
    private void SetInfinityStageMode(bool isInfinity) =>
        battleNum = isInfinity ?
            stageSO.bossDistance + 1 :          // Infinity state
            stageSO.appearEnemyTables.Count;    // Normal state

    #region For Debug

    /// <summary>
    /// For debug
    /// </summary>
    private void ProcessDebug()
    {
        if (Input.GetKeyDown(KeyCode.T)) Time.timeScale = 8.0f;
        else if (Input.GetKeyUp(KeyCode.T)) Time.timeScale = 1.0f;
        else if (Input.GetKeyUp(KeyCode.Y)) DebugDefeatProcess();
    }
    /// <summary>
    /// Enemy destruction for DEBUG
    /// </summary>
    private void DebugDefeatProcess()
    {
        characterManager.ChangeStatusMaxHP(Card.CharaIDPlayer, 99);
        characterManager.ChangeStatusNowHP(Card.CharaIDPlayer, 99);
        characterManager.ChangeStatusNowHP(Card.CharaIDEnemy, -9999);
        fieldManager.CardPlayButton();
    }

    #endregion For Debug

    #endregion Private Methods

    #endregion Methods
}
