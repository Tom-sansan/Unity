using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Card Pack Data Definition Class
/// </summary>
[CreateAssetMenu(fileName = "CardPack", menuName = "ScriptableObjects/CardPack")]
public class CardPackSO : ScriptableObject
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    [Header("名前(日本語)")]
    public string nameJP;
    [Header("名前(英語)")]
    public string nameEN;

    [Header("説明(日本語)")]
    public string explainJP;
    [Header("説明(英語)")]
    public string explainEN;

    [Header("パックアイコン画像")]
    public Sprite packIcon;

    [Header("値段(消費Gold)")]
    public int price;
    [Header("カード入手枚数")]
    public int cardNum;

    [Header("出現カードリスト(同じカードを複数入れて確率変更可)")]
    public List<CardDataSO> includedCards;

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
