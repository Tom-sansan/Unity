using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data (ScriptableObject) for each card (before composite) used by the player or the enemy
/// </summary>
[Serializable]
[CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardDataSO : ScriptableObject
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    [Header("通し番号(カードごとに固有の値)")]
    public int serialNum;

    [Header("カード名(日本語)")]
    public string cardNameJP;

    [Header("カード名(英語)")]
    public string cardNameEN;

    [Header("アイコン")]
    public Sprite iconSprite;

    [Header("効果リスト")]
    public List<CardEffectDefine> effectList;

    [Header("強度")]
    public int force;

    [HideInInspector]
    public int totalEffectValue;

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

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
