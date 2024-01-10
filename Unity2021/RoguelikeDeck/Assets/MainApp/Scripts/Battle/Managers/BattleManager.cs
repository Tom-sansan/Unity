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
    /// Gauge display performance time
    /// </summary>
    private const float GageAnimationTime = 2.0f;
    /// <summary>
    /// Progression at which stage boss appears
    /// </summary>
    private int battleNum;

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
        // Get progression where stage boss appears
        battleNum = stageSO.appearEnemyTables.Count;
        // Managers initialization
        Init();
        // Show Stage info
        ApplyStageUIs();
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
            // Start stage clear animation
            stageClear.StartAnimation();
        }
        // Process appearance of enemy characters
        // Determines which enemies will appear
        var appearEnemyTable = stageSO.appearEnemyTables[nowProgress].appearEnemies;
        int rand = Random.Range(0, appearEnemyTable.Count);
        characterManager.SpawnEnemy(appearEnemyTable[rand]);
        // If there is only one type of enemy that appears, then the direction for the boss
        if (appearEnemyTable.Count == 1) bossIncoming.StartAnimation();
        // Start battle(delayed execution)
        DOVirtual.DelayedCall(
            1.0f,
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
                        ProgressingStage();
                    }
                }
            );
            return;
        }
        // Start next turn
        TurnStart();
    }

    #endregion Stage progression

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

    #endregion Private Methods

    /// <summary>
    /// Display stage progress gauge
    /// </summary>
    /// <param name="ratio">Progression rate (0.0f-1.0f)</param>
    private void ShowProgressGage(float ratio) =>
        progressGageImage.DOFillAmount(ratio, GageAnimationTime);

    #endregion Methods
}
