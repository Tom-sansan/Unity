using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Management class for player and enemy characters
/// </summary>
public class CharacterManager : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    // Enemy image object
    /// <summary>
    /// Parent Transform of the generated enemy object
    /// </summary>
    [SerializeField]
    private Transform enemyPictureParent = null;
    /// <summary>
    /// Enemy Character Image Prefab
    /// </summary>
    [SerializeField]
    private GameObject enemyPicturePrefab = null;
    // Status UI processing class
    /// <summary>
    /// Player status
    /// </summary>
    [SerializeField]
    private StatusUI playerStatusUI = null;
    /// <summary>
    /// Enemy status
    /// </summary>
    [SerializeField]
    private StatusUI enemyStatusUI = null;
    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Vibration time
    /// </summary>
    public const float ShakeAnimTime = 0.4f;
    /// <summary>
    /// Battle Screen Manager class
    /// </summary>
    [HideInInspector]
    public BattleManager battleManager;
    /// <summary>
    /// Enemy definition data (not changed here during the battle)
    /// </summary>
    [HideInInspector] public EnemyStatusSO enemyData;
    // Status data by character ID
    // Current HP data
    public int[] nowHP;
    // Maximum HP data
    public int[] maxHP;
    /// <summary>
    /// Each abnormality point [character ID, abnormality ID]
    /// </summary>
    public int[,] statusEffectsPoints;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Vibration intensity
    /// </summary>
    private const float ShakeAnimPower = 18.0f;
    /// <summary>
    /// Enemy object handling class during appearance
    /// </summary>
    private EnemyPicture enemyPicture;

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
    /// Initialization (called from BattleManager.cs)
    /// </summary>
    /// <param name="gameManager"></param>
    public void Init(BattleManager gameManager)
    {
        battleManager = gameManager;

        nowHP = new int[Card.CharaNum];
        maxHP = new int[Card.CharaNum];
        ResetHPPlayer();
        // Initialization of state abnormality point 
        statusEffectsPoints = new int[Card.CharaNum, (int)StatusEffectIcon.StatusEffectType._MAX];
        // UI initialization
        playerStatusUI.SetHPView(nowHP[Card.CharaIDPlayer], maxHP[Card.CharaIDPlayer]);
        // Hide enemy status (no animation)
        enemyStatusUI.HideCanvasGroup(false);
    }
    /// <summary>
    /// Initialize player's HP
    /// </summary>
    public void ResetHPPlayer()
    {
        // HP initialization
        // Player's maximum HP
        maxHP[Card.CharaIDPlayer] = Data.instance.playerMaxHP; // 30;
        nowHP[Card.CharaIDPlayer] = maxHP[Card.CharaIDPlayer];
        playerStatusUI.SetHPView(nowHP[Card.CharaIDPlayer], maxHP[Card.CharaIDPlayer]);
        // Initialization of various state abnormalities
        statusEffectsPoints = new int[Card.CharaNum, (int)StatusEffectIcon.StatusEffectType._MAX];
        RefleshStatusEffectUI();
    }
    /// <summary>
    /// Change the current HP of the designated character
    /// </summary>
    /// <param name="charaID">Character ID</param>
    /// <param name="value">Amount of change (+ for recovery)(- for damage)</param>
    public void ChangeStatusNowHP(int charaID, int value)
    {
        // If player already has 0 HP now, return
        if (nowHP[charaID] <= 0) return;
        // Change the current HP
        nowHP[charaID] += value;
        // Process to avoid exceeding the maximum HP
        if (nowHP[charaID] > maxHP[charaID]) nowHP[charaID] = maxHP[charaID];
        // Abnormality: Apply the effect of fire (damage when maximum HP is reduced)
        if (value < 0)
        {
            // When maximum HP is reduced
            int flameDamage = statusEffectsPoints[charaID, (int)StatusEffectIcon.StatusEffectType.Flame];
            nowHP[charaID] -= flameDamage;
        }
        // UI Reflection
        if (charaID == Card.CharaIDPlayer) playerStatusUI.SetHPView(nowHP[charaID], maxHP[charaID]);
        else
        {
            Debug.Log($"enemyStatusUI.SetHPView({nowHP[charaID]}, {maxHP[charaID]});");
            enemyStatusUI.SetHPView(nowHP[charaID], maxHP[charaID]);
            // (Enemy only) Damage direction
            DirectEnemy(value);
        }
    }
    /// <summary>
    /// Change the maximum HP of the designated character
    /// </summary>
    /// <param name="charaID">Character ID</param>
    /// <param name="value">Amount of change (+ for recovery)(- for damage)</param>
    public void ChangeStatusMaxHP(int charaID, int value)
    {
        // If player already has 0 HP now, return
        if (maxHP[charaID] <= 0) return;
        // Change the maximum HP
        maxHP[charaID] += value;
        // Reflects current upper and lower HP limits
        nowHP[charaID] = Mathf.Clamp(nowHP[charaID], 0, maxHP[charaID]);
        // UI Reflection
        if (charaID == Card.CharaIDPlayer) playerStatusUI.SetHPView(nowHP[charaID], maxHP[charaID]);
        else
        {
            enemyStatusUI.SetHPView(nowHP[charaID], maxHP[charaID]);
            // (Enemy only) Damage direction
            DirectEnemy(value);
        }
    }
    /// <summary>
    /// Restore the player's maximum HP
    /// </summary>
    public void RecoverMaxHPPlayer()
    {
        maxHP[Card.CharaIDPlayer] = Data.instance.playerMaxHP; // 30;
        if (nowHP[Card.CharaIDPlayer] > maxHP[Card.CharaIDPlayer])
            nowHP[Card.CharaIDPlayer] = maxHP[Card.CharaIDPlayer];
        playerStatusUI.SetHPView(nowHP[Card.CharaIDPlayer], maxHP[Card.CharaIDPlayer]);
    }
    /// <summary>
    /// Returns whether player has less than or equal to 0 HP
    /// </summary>
    /// <returns>Player Defeat Flag</returns>
    public bool IsPlayerDefeated()
    {
        if (nowHP[Card.CharaIDPlayer] <= 0) return true;
        else return false;
    }

    #region Process for Enemy

    /// <summary>
    /// Appearance and display of enemies
    /// </summary>
    /// <param name="spawnEnemyData">Appearance Enemy Data</param>
    public void SpawnEnemy(EnemyStatusSO spawnEnemyData)
    {
        // Get enemy data
        enemyData = spawnEnemyData;
        // Initialize enemy status
        nowHP[Card.CharaIDEnemy] = enemyData.maxHP;
        maxHP[Card.CharaIDEnemy] = enemyData.maxHP;
        // Creation enemy image object
        var obj = Instantiate(enemyPicturePrefab, enemyPictureParent);
        // Get enemy image processing class
        enemyPicture = obj.GetComponent<EnemyPicture>();
        // Initialize enemy image processing class
        enemyPicture.Init(this, enemyData.charaSprite);
        // Show enemy status UI
        enemyStatusUI.ShowCanvasGroup();
        if (Data.nowLanguage == SystemLanguage.Japanese)
            enemyStatusUI.SetCharacterName(enemyData.enemyNameJP);
        else if (Data.nowLanguage == SystemLanguage.English)
            enemyStatusUI.SetCharacterName(enemyData.enemyNameEN);
        enemyStatusUI.SetHPView(nowHP[Card.CharaIDEnemy], maxHP[Card.CharaIDEnemy]);
    }
    /// <summary>
    /// Returns whether enemy has 0 or less HP.
    /// </summary>
    /// <returns>Enemy Destroyed Flag</returns>
    public bool IsEnemyDefeated()
    {
        if (nowHP[Card.CharaIDEnemy] <= 0) return true;
        else return false;
    }
    /// <summary>
    /// Delete enemy
    /// </summary>
    public void DeleteEnemy()
    {
        // Delete object
        Destroy(enemyPicture.gameObject);
    }
    /// <summary>
    /// Return coordinates of the enemy image
    /// </summary>
    /// <returns></returns>
    public Vector2 GetEnemyPosition() =>
        enemyPicture.transform.position;

    #endregion Process for Enemy

    #region Related to state abnormalities

    /// <summary>
    /// Change the amount of effect of the specified character's state abnormalities
    /// </summary>
    /// <param name="charaID"></param>
    /// <param name="effectType"></param>
    /// <param name="value"></param>
    public void ChangeStatusEffect(int charaID, StatusEffectIcon.StatusEffectType effectType, int value)
    {
        // Change effect amount
        statusEffectsPoints[charaID, (int)effectType] += value;
        if (statusEffectsPoints[charaID, (int)effectType] < 0)
            statusEffectsPoints[charaID, (int)effectType] = 0;
        // Reflect UI
        RefleshStatusEffectUI();
    }
    /// <summary>
    /// Update the display of all state abnormalities UIs
    /// </summary>
    public void RefleshStatusEffectUI()
    {
        // Display player-side status abnormality
        playerStatusUI.SetStatusEffectUI
        (
            StatusEffectIcon.StatusEffectType.Poison,
            statusEffectsPoints[Card.CharaIDPlayer, (int)StatusEffectIcon.StatusEffectType.Poison]
        );
        playerStatusUI.SetStatusEffectUI
        (
            StatusEffectIcon.StatusEffectType.Flame,
            statusEffectsPoints[Card.CharaIDPlayer, (int)StatusEffectIcon.StatusEffectType.Flame]
        );
        // Display enemy-side status abnormality
        enemyStatusUI.SetStatusEffectUI
        (
            StatusEffectIcon.StatusEffectType.Poison,
            statusEffectsPoints[Card.CharaIDEnemy, (int)StatusEffectIcon.StatusEffectType.Poison]
        );
        enemyStatusUI.SetStatusEffectUI
        (
            StatusEffectIcon.StatusEffectType.Flame,
            statusEffectsPoints[Card.CharaIDEnemy, (int)StatusEffectIcon.StatusEffectType.Flame]
        );
    }

    #endregion Related to state abnormalities

    /// <summary>
    /// Process executed at the end of the turn
    /// </summary>
    public void OnTurnEnd()
    {
        // State abnormality process for each character
        for (int i = 0; i < Card.CharaNum; i++)
        {
            // Activate poison effect
            int poisonDamange = statusEffectsPoints[i, (int)StatusEffectIcon.StatusEffectType.Poison];
            if (poisonDamange > 0) ChangeStatusNowHP(i, -poisonDamange);
            // Decrease in remaining effect size
            ChangeStatusEffect(i, StatusEffectIcon.StatusEffectType.Poison, -1);
            ChangeStatusEffect(i, StatusEffectIcon.StatusEffectType.Flame, -1);
        }

    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Direct enemy
    /// </summary>
    private void DirectEnemy(int value)
    {
        // Defeat animation
        if (IsEnemyDefeated()) enemyPicture.DefeatAnimation();
        // Damage animation
        else if (value < 0) enemyPicture.DamageAnimation();
    }

    #endregion Private Methods

    #endregion Methods
}
