using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// EnhanceManager Class
/// </summary>
public class EnhanceManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum
    /// <summary>
    /// Enum for Sound Effect
    /// </summary>
    public enum SE
    {
        HP,
        Attack,
        Defence
    }
    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Status enhance button
    /// </summary>
    [SerializeField]
    private List<Button> enhanceButtons;
    /// <summary>
    /// Button for Go Game
    /// </summary>
    [SerializeField]
    private Button goFirstSRPGButton;
    /// <summary>
    /// Audio clip list
    /// </summary>
    [SerializeField]
    private List<AudioClip> SEList;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Data class
    /// </summary>
    private Data data;
    /// <summary>
    /// SE source
    /// </summary>
    private AudioSource seSource;
    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Add maximum HP
    /// 最大HPを増やす
    /// </summary>
    public void EnhanceAddHP()
    {
        data.AddHP += 2;
        EnhanceComplete((int)SE.HP);
    }
    /// <summary>
    /// Add maximum Attack
    /// 攻撃力を増やす
    /// </summary>
    public void EnhanceAddAttack()
    {
        data.AddAttack++;
        EnhanceComplete((int)SE.Attack);
    }
    /// <summary>
    /// Add maximum Defence
    /// 防御力を増やす
    /// </summary>
    public void EnhanceAddDefence()
    {
        data.AddDefense++;
        EnhanceComplete((int)SE.Defence);
    }
    /// <summary>
    /// Go to FirstSRPG Scene
    /// </summary>
    public void GoFirstSRPGScene() =>
        SceneManager.LoadScene("FirstSRPGScene");
    /// <summary>
    /// 
    /// </summary>
    public void DeleteSavedData() =>
        data.DeleteSavedData();
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        data = GameObject.Find("DataManager").GetComponent<Data>();
        // Do not allow the “Play Again” button to be pressed until one of them is enhanced
        // いずれかを強化するまでは「もう一度プレイ」ボタンを押せないようにする
        // goFirstSRPGButton.interactable = false;
        seSource = gameObject.AddComponent<AudioSource>();
    }
    /// <summary>
    /// Common processing upon completion of player reinforcement
    /// プレイヤー強化完了時の共通処理
    /// </summary>
    private void EnhanceComplete(int clipIndex)
    {
        // Play designated clip
        // 指定されたclip を再生する
        PlayClip(SEList[clipIndex]);
        // Disable the reinforcement button from being pressed
        // 強化ボタンを押下不可にする
        foreach (Button button in enhanceButtons) button.interactable = false;
        // Enable “Play Again” button
        // 「もう一度プレイ」ボタンを押下可能にする
        // goFirstSRPGButton.interactable = true;
        // Save changes
        // 変更データを保存
        data.WriteSaveData();
    }
    /// <summary>
    /// Play clip
    /// AudioClipを再生する
    /// </summary>
    /// <param name="clip"></param>
    private void PlayClip(AudioClip clip)
    {
        seSource.clip = clip;
        seSource.Play();
    }
    #endregion Private Methods

    #endregion Methods
}
