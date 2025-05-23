using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Character Class
/// </summary>
public class Character : MonoBehaviour
{

    #region Nested Class

    #endregion Nested Class

    #region Enum
    /// <summary>
    /// Character attribute
    /// キャラクター属性
    /// </summary>
    public enum Attribute
    {
        // Fire
        // 火
        Fire,
        // Water
        // 水
        Water,
        // Wind
        // 風
        Wind,
        // Soil
        // 土
        Soil
    }
    /// <summary>
    /// Character's movement type
    /// </summary>
    public enum MoveType
    {
        // 縦・横
        Rook,
        // 斜め
        Bishop,
        // 縦・横・斜め
        Queen
    }
    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Character's initial X Position
    /// </summary>
    [Header("Initial X Position(-4～4)")]
    [SerializeField]
    private int initPosX;
    /// <summary>
    /// Character's initial Z Position
    /// </summary>
    [Header("Initial Z Position(-4～4)")]
    [SerializeField]
    private int initPosZ;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// Enemy flag (ON to treat as enemy character).
    /// 敵フラグ(ONで敵キャラとして扱う)
    /// </summary>
    [Header("Enemy flag")]
    public bool IsEnemy;
    /// <summary>
    /// Character name
    /// キャラクター名
    /// </summary>
    [Header("Character name")]
    public string CharaName;
    /// <summary>
    /// Maximum HP (initial HP)
    /// 最大HP(初期HP)
    /// </summary>
    [Header("Maximum HP (initial HP)")]
    public int MaxHP;
    /// <summary>
    /// Character data changing during the game
    /// <summary>
    /// Attack power
    /// 攻撃力
    /// </summary>
    [Header("Attack power")]
    public int Attack;
    /// <summary>
    /// Defence power
    /// 防御力
    /// </summary>
    [Header("Defence power")]
    public int Defence;
    /// <summary>
    /// Attribute
    /// 属性
    /// </summary>
    [Header("Attribute")]
    public Attribute attribute;
    /// <summary>
    /// Character's movement type
    /// </summary>
    [Header("Movement Type")]
    public MoveType moveType;
    /// <summary>
    /// Skill
    /// </summary>
    [Header("Skill")]
    public SkillDefine.Skill Skill;
    /// ゲーム中に変化するキャラクターデータ
    /// Current x-coordinate
    /// 現在のx座標
    /// </summary>
    [HideInInspector]
    public int currentPosX;
    /// <summary>
    /// Character data changing during the game
    /// ゲーム中に変化するキャラクターデータ
    /// Current x-coordinate
    /// 現在のz座標
    /// </summary>
    [HideInInspector]
    public int CurrentPosZ;
    /// <summary>
    /// Current HP
    /// 現在のHP
    /// </summary>
    [HideInInspector]
    public int NowHP;
    /// <summary>
    /// Skill lock flag
    /// 特技使用不可状態
    /// </summary>
    public bool IsSkillLock;
    /// <summary>
    /// Zero defense debuff
    /// 防御力0化デバフ
    /// </summary>
    public bool IsDefenceBreak;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Main Camera
    /// </summary>
    private Camera mainCamera;
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
    void Update()
    {
        UpdateBillboard();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Move the character to the specified coordinates
    /// 対象の座標へとキャラクターを移動させる
    /// </summary>
    /// <param name="targetPosX">x座標</param>
    /// <param name="targetPosZ">z座標</param>
    public void MovePosition(int targetPosX, int targetPosZ)
    {
        // Set the position of the character to the target position
        // キャラクターの位置を目標位置に設定
        // Initialised in Vector3
        // (0.0f, 0.0f, 0.0f)でVector3で初期化
        var movePos = Vector3.zero;
        // Relative distance in the x direction
        // x方向の相対距離
        movePos.x = targetPosX - currentPosX;
        // movePos.y = 1.0f;
        // Relative distance in the z direction
        // z方向の相対距離
        movePos.z = targetPosZ - CurrentPosZ;
        // Character movement process
        // キャラクターの移動処理
        //transform.position += movePos;
        // Gradually change the position
        // 徐々に位置が変化するするアニメーション
        transform.DOMove(movePos, 0.5f)
            .SetEase(Ease.OutCubic)   // 変化の度合いを設定(Linear:等速, OutCubic:徐々に減速)
            .SetRelative();         // パラメーターを相対指定にする
        // Set the character's current position
        // キャラクターの現在位置を設定
        currentPosX = targetPosX;
        CurrentPosZ = targetPosZ;
    }
    /// <summary>
    /// Charact close attack animation
    /// キャラクターの近接攻撃アニメーション
    /// </summary>
    /// <param name="targetChara">相手キャラクター</param>
    public void AnimateAttack(Character targetChara)
    {
        // Jump to the position of the opponent character and return to the original position with the same movement
        // 相手キャラクターの位置へジャンプで近づき、同じ動きで元の位置に戻る
        transform.DOJump
        (
            targetChara.transform.position, // 指定位置までジャンプしながら移動する
            1.0f,                           // ジャンプの高さ
            1,                              // ジャンプ回数
            0.5f                            // アニメーション時間（秒）
        )
        .SetEase(Ease.Linear)               // 変化の度合いを設定(Linear:等速)
        .SetLoops(2, LoopType.Yoyo);        // ループ回数とループの種類を設定
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // Main Cameraを取得
        mainCamera = Camera.main;

        // 初期位置に対応する座標へオブジェクトを移動させる
        var pos = new Vector3();
        // x座標：1ブロックのサイズが1(1.0f)なのでそのまま代入
        pos.x = initPosX;
        pos.y = 1.0f;
        pos.z = initPosZ;
        transform.position = pos;

        // オブジェクトを左右反転（ビルボード処理にて一度反転してしまうため）
        var scale = transform.localScale;
        // X方向の大きさを正負入れ替える
        scale.x *= -1.0f;
        transform.localScale = scale;

        // Set the initial position of the character
        // キャラクターの初期位置を設定
        currentPosX = initPosX;
        CurrentPosZ = initPosZ;
        NowHP = MaxHP;
    }

    #endregion Private Methods
    /// <summary>
    /// Update Billboard processing
    /// </summary>
    private void UpdateBillboard()
    {
        // Point the sprite object in the direction of the main camera
        // スプライトオブジェクトをメインカメラの方向に向ける
        // Get current camera coordinates
        // 現在のカメラ座標を取得
        Vector3 cameraPos = mainCamera.transform.position;
        // Set the camera coordinates so that the character stands perpendicular to the ground
        // キャラが地面と垂直に立つようにカメラの座標を設定
        cameraPos.y = transform.position.y;
        transform.LookAt(cameraPos);
    }
    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
