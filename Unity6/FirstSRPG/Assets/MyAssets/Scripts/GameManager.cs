using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// GameManager Class
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum
    /// <summary>
    /// Turn progression mode
    /// ターン進行モード
    /// </summary>
    private enum Phase
    {
        // My character's turn: start
        // 自分のターン：開始時
        CharaStart,
        // My character's turn: moving
        // 自分のターン：移動先選択中
        CharaMoving,
        // My character's turn: selecting a command after moving
        // 自分のターン：移動後のコマンド選択中
        CharaCommand,
        // My character's turn: selecting an attack target
        // 自分のターン：攻撃の対象を選択中
        CharaTargeting,
        // My character's turn: displaying the action result
        // 自分のターン：行動結果表示中
        CharaResult,
        // Enemy's turn: start
        // 敵のターン：開始時
        EnemyStart,
        // Enemy's turn: displaying the action result
        // 敵のターン：行動結果表示中
        EnemyResult
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// MapManager object
    /// </summary>
    private MapManager mapManager;
    /// <summary>
    /// All character management class
    /// 全キャラクター管理クラス
    /// </summary>
    private CharactersManager charactersManager;
    /// <summary>
    /// Character currently selected
    /// 選択中のキャラクター(誰も選択していないならfalse)
    /// </summary>
    private Character selectingCharacter;
    /// <summary>
    /// Current progress mode
    /// 現在の進行モード
    /// </summary>
    private Phase nowPhase;
    /// <summary>
    /// List of blocks that the selected character can move to
    /// 選択中のキャラクターが移動可能なブロックリスト
    /// </summary>
    private List<MapBlock> reachableBlocks;
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
        UpdateInput();
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        mapManager = GetComponent<MapManager>();
        charactersManager = GetComponent<CharactersManager>();
        reachableBlocks = new List<MapBlock>();
        // Mode of progression at start
        // 開始時の進行モード
        nowPhase = Phase.CharaStart;
    }

    private void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the block at the tap destination and start the selection process
            // タップ先にあるブロックを取得して選択処理を開始する
            GetMapBlockByTapPos();
        }
    }
    /// <summary>
    /// Finds the object in the tapped location and starts the selection process
    /// タップした場所にあるオブジェクトを見つけ、選択処理などを開始する
    /// </summary>
    private void GetMapBlockByTapPos()
    {
        // Object of tap target
        GameObject targetObj = null;
        // Fly Ray from the camera in the direction of the tap
        // タップした方向にカメラからRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Get the object that exists at the position that hits the Ray (the object must have a Collider attached to it)
            // Rayに当たる位置に存在するオブジェクトを取得（対象にColliderが付いている必要がある）
            targetObj = hit.collider.gameObject;
        }
        // If the target object (map block) exists
        // 対象オブジェクト（マップブロック）が存在する場合
        if (targetObj != null)
        {
            // Process block selection
            // ブロック選択時処理
            SelectBlock(targetObj.GetComponent<MapBlock>());
        }
    }
    /// <summary>
    /// Process to make the specified block selected
    /// 指定したブロックを選択状態にする処理
    /// </summary>
    /// <param name="targetBlock">対象のブロックデータ(Target block data)</param>
    private void SelectBlock(MapBlock targetBlock)
    {
        switch (nowPhase)
        {
            case Phase.CharaStart:
                StartCharacter(targetBlock);
                break;
            case Phase.CharaMoving:
                MoveCharacter(targetBlock);
                break;
            case Phase.CharaCommand:
                break;
            case Phase.CharaTargeting:
                break;
            case Phase.CharaResult:
                break;
            case Phase.EnemyStart:
                break;
            case Phase.EnemyResult:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Start the character's turn
    /// </summary>
    /// <param name="targetBlock"></param>
    private void StartCharacter(MapBlock targetBlock)
    {
        // Set the block
        SetBlock(targetBlock);
        // Set the character
        SetCharacter(targetBlock);
    }
    /// <summary>
    /// Set the block
    /// </summary>
    /// <param name="targetBlock"></param>
    private void SetBlock(MapBlock targetBlock)
    {
        // Release the selection status of all blocks
        // 全ブロックの選択状態を解除
        mapManager.ResetAllSelectionMode();
        // Display the block in a selected state
        // ブロックを選択状態の表示にする
        targetBlock.SetSelectionMode(true);
    }
    /// <summary>
    /// Get data on the character at the selected position
    /// 選択した位置にいるキャラクターのデータを取得
    /// </summary>
    /// <param name="targetBlock"></param>
    private void SetCharacter(MapBlock targetBlock)
    {
        // Get data on the character at the selected position
        // 選択した位置にいるキャラクターのデータを取得
        var characterData = charactersManager.GetCharacterDataByPos(targetBlock.posX, targetBlock.posZ);
        if (characterData != null)
        {
            // 選択中のキャラクター情報に設定
            selectingCharacter = characterData;
            // Get a list of blocks that the selected character can move to
            // 選択中のキャラクターが移動可能なブロックリストを取得
            reachableBlocks = mapManager.SearchReachableBlocks(targetBlock.posX, targetBlock.posZ);
            // 進行モードを進める: 移動先選択中
            ChangePhase(Phase.CharaMoving);
        }
        else
        {
            Debug.Log("Character is not found.");
        }
    }
    /// <summary>
    /// Change the turn progression mode
    /// ターン進行モードを変更する
    /// </summary>
    /// <param name="newPhase">変更先モード(Next mode)</param>
    private void ChangePhase(Phase newPhase)
    {
        // モード変更を保存
        nowPhase = newPhase;
    }
    /// <summary>
    /// Move the character
    /// </summary>
    /// <param name="targetBlock"></param>
    private void MoveCharacter(MapBlock targetBlock)
    {
        // If the target block is not in the list of blocks that the character can move to, the process is terminated
        // 移動先ブロックがキャラクターが移動可能なブロックリストに含まれていない場合、処理を終了
        if (!reachableBlocks.Contains(targetBlock)) return;
        // Move the character to the destination block
        // 移動先のブロックにキャラクターを移動
        selectingCharacter.MovePosition(targetBlock.posX, targetBlock.posZ);
        // Clear the list of reachable blocks
        // 移動可能なブロックリストをクリア
        reachableBlocks.Clear();
        // Release the selection status of all blocks
        // 全ブロックの選択状態を解除
        mapManager.ResetAllSelectionMode();
        // Advance mode of progression: during post-movement command selection
        // 進行モードを進める: 移動後のコマンド選択中
        ChangePhase(Phase.CharaCommand);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
