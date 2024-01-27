using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Management class for multiple card effect description objects
/// </summary>
public class EffectExplainDisplay : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Effect description class list
    /// </summary>
    [SerializeField]
    private List<EffectExplainPart> partsList = null;
    /// <summary>
    /// Card effect execution class
    /// </summary>
    [SerializeField]
    private PlayBoardManager playBoardManager = null;

    #endregion SerializeField

    #region Public Variables

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
    /// Initialization (executed from each caller class)
    /// </summary>
    public void Init()
    {
        // Initialization of each effect description class
        foreach (var explainPart in partsList) explainPart.Init(this);
        // If the object is inactive, make it active
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Show effect description
    /// </summary>
    /// <param name="effectData"></param>
    /// <param name="charaID"></param>
    public void ShowExplains(List<CardEffectDefine> effectData, int charaID)
    {
        int listLength = partsList.Count;
        int effectNum = effectData.Count;
        for (int i = 0; i < listLength; i++)
        {
            // Display if effect data exists
            if (i < effectNum) partsList[i].ShowExplain(effectData[i], charaID);
            else partsList[i].HideExplain();
        }
    }
    /// <summary>
    /// Hide effect description
    /// </summary>
    public void HideExplains()
    {
        // Hide
        int listLength = partsList.Count;
        for (int i = 0; i < listLength; i++) partsList[i].HideExplain();
    }
    /// <summary>
    /// Return the number of weapon attacks if in combat. (If not in combat, return 0.)
    /// </summary>
    /// <param name="charaID">Target character ID</param>
    /// <returns></returns>
    public int GetBattleCountWeaponDamage(int charaID)
    {
        if (playBoardManager == null) return 0;
        return playBoardManager.weaponCount[charaID];
    }
    /// <summary>
    /// Return the number of recovery times if in combat. (If not in combat, return 0.)
    /// </summary>
    /// <param name="charaID">Target character ID</param>
    /// <returns></returns>
    public int GetBattleCountHeal(int charaID)
    {
        if (playBoardManager == null) return 0;
        return playBoardManager.healCount[charaID];
    }
    /// <summary>
    /// Display description of condition abnormality
    /// </summary>
    /// <param name="statusEffectType">Type of condition abnormality</param>
    /// <param name="value">Effect value</param>
    public void ShowStatusEffectExplain(StatusEffectIcon.StatusEffectType statusEffectType, int value)
    {
        partsList[0].ShowExplainStatusEffect(statusEffectType, value);
        // Hide unused description fields
        int listLength = partsList.Count;
        for (int i = 1; i < listLength; i++) partsList[i].HideExplain();
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
