using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Data Manager Class
/// One instance of the same object always exists during game startup
/// </summary>
public class Data : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Singleton maintenance
    /// </summary>
    public static Data instance;
    /// <summary>
    /// The current language setting
    /// </summary>
    public static SystemLanguage nowLanguage;
    /// <summary>
    /// Deck management class
    /// </summary>
    public PlayerDeckData playerDeckData;
    /// <summary>
    /// List of all stages available for selection on the title screen
    /// </summary>
    public List<StageSO> stageSOs;
    /// <summary>
    /// List of icons for each occupation (stored in a list in the order of job definition Enum)
    /// </summary>
    public List<Sprite> jobIcons;
    /// <summary>
    /// Selected job
    /// </summary>
    public JobDataDefine.JobType playerJob;
    /// <summary>
    /// Job release status list
    /// </summary>
    public List<bool> jobUnlocks;
    /// <summary>
    /// Stage ID in progress
    /// </summary>
    [HideInInspector]
    public int nowStageID;
    /// <summary>
    /// Player's gold coin
    /// </summary>
    public int playerGold;
    /// <summary>
    /// Earned Experience
    /// </summary>
    public int playerEXP;
    /// <summary>
    /// Player's maximum HP
    /// </summary>
    public int playerMaxHP = 20;
    /// <summary>
    /// Number of cards in the player's hand for each turn (not including the increase due to occupation)
    /// </summary>
    public int playerHandNum = 5;

    #endregion Public Variables

    #region Private Variables

    #region Key definitions for PlayerPrefs

    /// <summary>
    /// Initialization Flag
    /// </summary>
    private const string KeyInit = "Init";
    /// <summary>
    /// Player's job during selection
    /// プレイヤー選択中職業
    /// </summary>
    private const string KeyPlayerJob = "PlayerJob";
    /// <summary>
    /// List of released jobs (add numbers after constants)
    /// 解放済み職業リスト(定数の後に数字を追加)
    /// </summary>
    private const string KeyUnlockJob_ = "UnlockJob_";
    /// <summary>
    /// Amount of gold coins in one's possession
    /// 所持金貨量
    /// </summary>
    private const string KeyPlayerGold = "PlayerGold";
    /// <summary>
    /// Amount of experience in possession
    /// 所持経験値量
    /// </summary>
    private const string KeyPlayerEXP = "PlayerEXP";
    /// <summary>
    /// Player's maximum HP
    /// プレイヤーの最大HP
    /// </summary>
    private const string KeyPlayerMaxHP = "PlayerMaxHP";
    /// <summary>
    /// Number of cards in player's hand for each turn
    /// プレイヤーの各ターンの手札枚数
    /// </summary>
    private const string KeyPlayerHandNum = "PlayerHandNum";
    /// <summary>
    /// Number of cards in storage (add each serial number after the constant)
    /// 保管中カード枚数(定数の後に各通し番号を追加)
    /// </summary>
    public const string KeyStorageCards = "StorageCards_";
    /// <summary>
    /// Number of cards in deck (add each serial number after the constant)
    /// デッキ内カード枚数(定数の後に各通し番号を追加)
    /// </summary>
    public const string KeyDeckCards = "DeckCards_";

    #endregion Key definitions for PlayerPrefs

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        // Game startup process
        InitialProcess();
    }
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    #region Various player data change processes

    /// <summary>
    /// Change the amount of gold coins player has
    /// </summary>
    /// <param name="value">Amount of change (+ for increase)</param>
    public void ChangePlayerGold(int value)
    { 
        playerGold += value;
        PlayerPrefs.SetInt(KeyPlayerGold, playerGold);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Change the amount of player experience
    /// </summary>
    /// <param name="value">Amount of change (+ for increase)</param>
    public void ChangePlayerEXP(int value)
    {
        playerEXP += value;
        PlayerPrefs.SetInt(KeyPlayerEXP, playerEXP);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Change the maximum HP of player
    /// </summary>
    /// <param name="value">Amount of change (+ for increase)</param>
    public void ChangePlayerMaxHP(int value)
    {
        playerMaxHP += value;
        PlayerPrefs.SetInt(KeyPlayerMaxHP, playerMaxHP);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Change number of cards in player's hand each turn
    /// </summary>
    /// <param name="value">Amount of change (+ for increase)</param>
    public void ChangePlayerHandNum(int value)
    {
        playerHandNum += value;
        PlayerPrefs.SetInt(KeyPlayerHandNum, playerHandNum);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Change player's job
    /// </summary>
    /// <param name="jobID">Number in the Enum of job to be changed</param>
    public void SetPlayerJob(int jobID)
    {
        playerJob = JobDataDefine.GetJobTypeByInt(jobID);
        PlayerPrefs.SetInt(KeyPlayerJob, jobID);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Release job
    /// </summary>
    /// <param name="jobTypeID">Job type ID</param>
    public void UnlockJob(int jobTypeID)
    {
        jobUnlocks[jobTypeID] = true;
        PlayerPrefs.SetInt(KeyUnlockJob_ + jobTypeID, 1);
        PlayerPrefs.Save();
    }

    #endregion Various player data change processes

    #endregion Public Methods

    /// <summary>
    /// Initialization and loading of all game data
    /// </summary>
    public void InitialiseAllGameData()
    {
        if (PlayerPrefs.GetInt(KeyInit, 0) == 0)
        {
            // Initialization
            // Initialization completion flag
            PlayerPrefs.SetInt(KeyInit, 1);
            PlayerPrefs.Save();
            // Player's job during selection
            playerJob = JobDataDefine.JobType.None;
            // Job release status
            jobUnlocks = new List<bool>();
            for (int i = 0; i < (int)JobDataDefine.JobType._Max; i++)
                jobUnlocks.Add(false);
            // Initial job release
            UnlockJob((int)JobDataDefine.JobType.None);
            // Gold coins in player's possession
            playerGold = 0;
            // Earned Experience
            playerEXP = 0;
            // Player's maximum HP
            playerMaxHP = 20;
            // Number of cards in the player's hand for each turn
            playerHandNum = TrainingWindow.InitPlayerHandNum;
            // Initialization of player's card data
            playerDeckData.InitializeData();
        }
        else
        {
            // Data loading
            // Player's job during selection
            playerJob = JobDataDefine.GetJobTypeByInt(PlayerPrefs.GetInt(KeyPlayerJob, (int)JobDataDefine.JobType.None));
            // Job release status
            jobUnlocks = new List<bool>();
            for (int i = 0; i < (int)JobDataDefine.JobType._Max; i++)
            {
                // Get release status
                int unlockValue = PlayerPrefs.GetInt(KeyUnlockJob_ + i, 0);
                // Reflect release status
                if (unlockValue == 0) jobUnlocks.Add(false);
                else jobUnlocks.Add(true);
            }
            // Initial job release
            UnlockJob((int)JobDataDefine.JobType.None);
            // Gold coins in player's possession
            playerGold = PlayerPrefs.GetInt(KeyPlayerGold, 0);
            // Earned Experience
            playerEXP = PlayerPrefs.GetInt(KeyPlayerEXP, 0);
            // Player's maximum HP
            playerMaxHP = PlayerPrefs.GetInt(KeyPlayerMaxHP);
            // Number of cards in the player's hand for each turn
            playerHandNum = PlayerPrefs.GetInt(KeyPlayerHandNum, TrainingWindow.InitPlayerHandNum);
            // Initialization of player's card data
            playerDeckData.InitializeData();
            // Loading of player's card data
            playerDeckData.LoadData();
        }
    }

    #region Private Methods

    /// <summary>
    /// Process executed only once at game start (instance creation)
    /// </summary>
    private void InitialProcess()
    {
        // Initialization of random number seed value
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        // Get the language setting of the execution environment
        nowLanguage = GetLanguageData(); // SystemLanguage.English;
        // Player deck data initialization
        playerDeckData.Init();
        // Various data initialization (initial startup or data loading)
        InitialiseAllGameData();

        /*
        // Initialization of player's card data (called at another time after the save function is implemented)
        playerDeckData.InitializeData();
        // Initialize job data
        // Player's current job
        playerJob = JobDataDefine.JobType.None;
        // Job release status
        jobUnlocks = new List<bool>();
        int length = (int)JobDataDefine.JobType._Max;
        for (int i = 0; i < length; i++) jobUnlocks.Add(false);
        // Initial job release
        UnlockJob((int)JobDataDefine.JobType.None);
        */
    }
    /// <summary>
    /// Retrieve and return the language settings of the execution environment
    /// </summary>
    /// <returns>Language data</returns>
    private SystemLanguage GetLanguageData()
    {
        // Get the language setting of the execution environment
        var language = Application.systemLanguage;
        // If the language is not Japanese, language setting is in English
        if (language != SystemLanguage.Japanese)
            language = SystemLanguage.English;
        return language;
    }
    #endregion Private Methods

    #endregion Methods
}
