using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

    #endregion Public Variables

    #region Private Variables

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
    public void ChangePlayerGold(int value) =>
        playerGold += value;
    /// <summary>
    /// Change the amount of player experience
    /// </summary>
    /// <param name="value">Amount of change (+ for increase)</param>
    public void ChangePlayerEXP(int value) =>
        playerEXP += value;

    #endregion Various player data change processes

    #endregion Public Methods

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
        // Initialization of player's card data (called at another time after the save function is implemented)
        playerDeckData.InitializeData();
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
