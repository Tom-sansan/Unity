using System;
using UnityEngine;

/// <summary>
/// Data Class
/// </summary>
public class Data : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// Variables for singleton management
    /// シングルトン管理用変数
    /// </summary>
    [HideInInspector]
    public static bool _instance = false;
    /// <summary>
    /// Maximum HP increase
    /// 最大HP上昇量
    /// </summary>
    public int AddHP;
    /// <summary>
    /// Attack increase
    /// 攻撃力上昇量
    /// </summary>
    public int AddAttack;
    /// <summary>
    /// Defense increase
    /// 防御力上昇量
    /// </summary>
    public int AddDefense;
    /// <summary>
    /// Data key for AddHP
    /// AddHP用のデータキー
    /// </summary>
    public const string KeyAddHP = "AddHP";
    /// <summary>
    /// Data key for AddAttack
    /// AddAttack用のデータキー
    /// </summary>
    public const string KeyAddAttack = "AddAttack";
    /// <summary>
    /// Data key for AddDefense
    /// AddDefense用のデータキー
    /// </summary>
    public const string KeyAddDefense = "AddDefense";
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        // Process for singleton ... If a data manager already exists in the scene, it erases itself
        // シングルトン用処理 … 既にデータマネージャがシーン内に存在している場合は自分を消去
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }
        _instance = true;
        DontDestroyOnLoad(gameObject);
        // Load saved data
        LoadSavedData();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Save current player enhancement data
    /// 現在のプレイヤー強化データを保存する
    /// </summary>
    public void WriteSaveData()
    {
        PlayerPrefs.SetInt(KeyAddHP, AddHP);
        PlayerPrefs.SetInt(KeyAddAttack, AddAttack);
        PlayerPrefs.SetInt(KeyAddDefense, AddDefense);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Delete saved data
    /// 保存データの削除を行う
    /// </summary>
    public void DeleteSavedData()
    {
        PlayerPrefs.DeleteAll();
        ResetData();
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Load saved data
    /// 保存データを読み込む
    /// </summary>
    private void LoadSavedData()
    {
        AddHP = PlayerPrefs.GetInt(KeyAddHP, 0);
        AddAttack = PlayerPrefs.GetInt(KeyAddAttack, 0);
        AddDefense = PlayerPrefs.GetInt(KeyAddDefense, 0);
    }
    /// <summary>
    /// Reset data
    /// データをリセットする
    /// </summary>
    private void ResetData()
    {
        AddHP = 0;
        AddAttack = 0;
        AddDefense = 0;
    }
    #endregion Private Methods

    #endregion Methods

}
