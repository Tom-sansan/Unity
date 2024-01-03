using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy status definition class (ScriptableObject)
/// </summary>
[CreateAssetMenu(fileName = "EnemyStatusSO", menuName = "ScriptableObjects/EnemyStatusSO", order = 2)]

public class EnemyStatusSO : ScriptableObject
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables
    [Header("名前(日本語)")]
    public string enemyNameJP;

    [Header("名前(英語)")]
    public string enemyNameEN;

    [Header("敵画像")]
    public Sprite charaSprite;

    [Header("最大HP(初期HP)")]
    public int maxHP;

    [Header("各ターンに使用するカードと設置先のリスト")]
    public List<EnemyUseCardData> useCardDatas;

    [Header("撃破ボーナス：選択肢の個数")]
    public int bonusOptions;

    [Header("撃破ボーナス：獲得できる個数")]
    public int bonusPoint;

    [Header("撃破ボーナス：選択肢に出現するプレイヤーカード")]
    public List<CardDataSO> bonusCardList;

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
    /// Data class of cards to be used and installed per turn
    /// (When editing with Inspector, the length of the array cannot be fixed, so variables are declared for the necessary length.)
    /// </summary>
    [Serializable]
    public class EnemyUseCardData
    {
        /// <summary>
        /// 0th card data
        /// </summary>
        public CardDataSO placeCardData0;
        /// <summary>
        /// 1st card data
        /// </summary>
        public CardDataSO placeCardData1;
        /// <summary>
        /// 2nd card data
        /// </summary>
        public CardDataSO placeCardData2;
        /// <summary>
        /// 3rd card data
        /// </summary>
        public CardDataSO placeCardData3;
        /// <summary>
        /// 4th card data
        /// </summary>
        public CardDataSO placeCardData4;
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
