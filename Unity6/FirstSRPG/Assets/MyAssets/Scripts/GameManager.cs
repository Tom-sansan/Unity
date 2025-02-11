using System;
using System.Collections;
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
    /// GUIManager object
    /// </summary>
    private GUIManager guiManager;
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
    /// <summary>
    /// List of blocks that the selected character can attack enemy
    /// 選択中のキャラクターが攻撃可能なブロックリスト
    /// </summary>
    private List<MapBlock> attackableBlocks;
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
    /// <summary>
    /// Attack command button pressed
    /// </summary>
    public void AttackCommand()
    {
        // Hide command buttons
        guiManager.HideCommandButtons();
        // Get a list of blocks that the selected character can attack
        // 攻撃可能な場所リストを取得
        attackableBlocks = mapManager.SearchAttackableBlocks(selectingCharacter.currentPosX, selectingCharacter.CurrentPosZ);
        // Display the list of attackable places
        // 攻撃可能な場所リストを表示
        foreach (MapBlock mapBlock in attackableBlocks)
            mapBlock.SetSelectionMode(MapBlock.Highlight.Attackable);
    }
    /// <summary>
    /// Standby command button pressed
    /// </summary>
    public void StandbyCommand()
    {
        // Hide command buttons
        guiManager.HideCommandButtons();
        // Advancing progression mode.
        // 敵のターンへ進行モードを進める
        ChangePhase(Phase.EnemyStart);
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        mapManager = GetComponent<MapManager>();
        charactersManager = GetComponent<CharactersManager>();
        guiManager = GetComponent<GUIManager>();
        reachableBlocks = new List<MapBlock>();
        attackableBlocks = new List<MapBlock>();
        // Mode of progression at start
        // 開始時の進行モード
        nowPhase = Phase.CharaStart;
    }

    private void UpdateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // バトル結果表示ウィンドウが出ているときの処理
            if (guiManager.battleWindowUI.gameObject.activeInHierarchy)
            {
                // バトル結果表示ウィンドウを閉じる
                guiManager.battleWindowUI.HideWindow();
                // 進行モードを進める（デバック用）
                ChangePhase(Phase.CharaStart);
                return;
            }
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
                // At the start
                // 開始時
                StartCharacter(targetBlock);
                break;
            case Phase.CharaMoving:
                // During movement destination selection
                // 移動先選択中
                MoveCharacter(targetBlock);
                break;
            case Phase.CharaCommand:
                // During post-movement command selection
                // 移動後のコマンド選択中
                SelectingCommnand(targetBlock);
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
        targetBlock.SetSelectionMode(MapBlock.Highlight.Select);
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
            // Display the character's status on the UI
            // キャラクターのステータスをUIに表示する
            guiManager.ShowStatusWindow(selectingCharacter);
            // Get a list of blocks that the selected character can move to
            // 選択中のキャラクターが移動可能なブロックリストを取得
            reachableBlocks = mapManager.SearchReachableBlocks(targetBlock.posX, targetBlock.posZ);
            // Display the list of movable places
            // 移動可能な場所リストを表示する
            foreach (MapBlock mapBlock in reachableBlocks)
                mapBlock.SetSelectionMode(MapBlock.Highlight.Reachable);
            // 進行モードを進める: 移動先選択中
            ChangePhase(Phase.CharaMoving);
        }
        else
        {
            // No character
            // Initialize the selected character information
            ClearSelectingCharacter();
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
        // Show command buttons
        // コマンドボタンを表示する
        guiManager.ShowCommandButtons();
        // Advance mode of progression: during post-movement command selection
        // 進行モードを進める: 移動後のコマンド選択中
        ChangePhase(Phase.CharaCommand);
    }
    /// <summary>
    /// Selecting command after moving
    /// 移動後のコマンド選択中
    /// </summary>
    /// <param name="targetBlock"></param>
    private void SelectingCommnand(MapBlock targetBlock)
    {
        // If the target block is not in the list of blocks that the character can move to, the process is terminated
        // 攻撃先ブロックがキャラクターが攻撃可能なブロックリストに含まれていない場合、処理を終了
        if (!attackableBlocks.Contains(targetBlock)) return;
        // Clear the list of attachable blocks
        // 攻撃可能なブロックリストをクリア
        attackableBlocks.Clear();
        // Release the selection status of all blocks
        // 全ブロックの選択状態を解除
        mapManager.ResetAllSelectionMode();
        // Get data on the character at the selected position
        // 選択した位置にいるキャラクターのデータを取得
        var targetCharacter = charactersManager.GetCharacterDataByPos(targetBlock.posX, targetBlock.posZ);
        if (targetCharacter != null)
        {
            // There is an enemy to attack
            // 攻撃対象がいる場合
            // Attack processing
            // キャラクター攻撃処理
            CharacterAttack(selectingCharacter, targetCharacter);
            // Advance progress mode (to action result display)
            // 進行モードを進める（行動結果表示へ）
            ChangePhase(Phase.CharaResult);
            return;
        }
        else
        {
            // No enemy to attack
            // 攻撃対象がいない場合
            // Advance mode of progression: during post-movement command selection
            // 進行モードを進める（敵のターンへ）
            ChangePhase(Phase.EnemyStart);
        }
        // Show command buttons
        // コマンドボタンを表示する
        guiManager.ShowCommandButtons();
        // Advance mode of progression: during post-movement command selection
        // 進行モードを進める: 移動後のコマンド選択中
        ChangePhase(Phase.CharaCommand);
    }
    /// <summary>
    /// Initialize the selected character information
    /// 選択中のキャラクター情報を初期化
    /// </summary>
    private void ClearSelectingCharacter()
    {
        // Initialize the selected character information
        // 選択中のキャラクター情報を初期化
        selectingCharacter = null;
        // Hide the character's status UI
        // キャラクターのステータスUIを非表示にする
        guiManager.HideStatusWindow();
    }
    /// <summary>
    /// Process of a character attacking another character
    /// キャラクターが他のキャラクターに攻撃する処理
    /// </summary>
    /// <param name="attackChara"></param>
    /// <param name="defenceChara"></param>
    private void CharacterAttack(Character attackChara, Character defenceChara)
    {
        Debug.Log($"攻撃側:{attackChara.CharaName} 防御側:{defenceChara.CharaName}");

        // Calculate the damage value
        // Damage = Attack - Defence
        // ダメージ = 攻撃力 - 防御力
        int damageValue = attackChara.Attack - defenceChara.Defence;
        if (damageValue < 0) damageValue = 0;
        // Display settings for the battle result display window(To be done before HP changes)
        // バトル結果表示ウィンドウの表示設定（HPの変更前に行う）
        guiManager.battleWindowUI.ShowWindow(defenceChara, damageValue);
        // Reduce the defender's HP by the amount of damage
        // ダメージ量分防御力側のHPを減らす
        defenceChara.NowHP -= damageValue;
        // Correct HP to fall within the range of 0 to the maximum value
        // HPが0～最大値の範囲に収まるように補正
        defenceChara.NowHP = Mathf.Clamp(defenceChara.NowHP, 0, defenceChara.MaxHP);
        // If the HP of the defender is 0, delete the character
        // HP 0 になったらキャラクターを削除する
        if (defenceChara.NowHP == 0)
        {
            // Delete the character
            // キャラクターを削除
            charactersManager.DeleteCharacter(defenceChara);
        }
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
