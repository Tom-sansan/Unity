using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager Class
/// </summary>
public class GameManager : MonoBehaviour
{
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
    /// <summary>
    /// Damage ratio normal
    /// </summary>
    [SerializeField]
    private float damageRatioNormal = 1.0f;
    /// <summary>
    /// 攻撃の相性が良い(攻撃側が有利)
    /// Damage ratio high
    /// </summary>
    [SerializeField]
    private float damageRatioHigh = 1.2f;
    /// <summary>
    /// 攻撃の相性が悪い(攻撃側が不利)
    /// Damage ratio low
    /// </summary>
    [SerializeField]
    private float damageRatioLow = 0.8f;
    /// <summary>
    /// AudioManager
    /// </summary>
    [SerializeField]
    private AudioManager audioManager;
    #endregion SerializeField

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
    /// Skill on selection (NONE fixed during normal attack)
    /// 選択中の特技(通常攻撃時はNONE固定)
    /// </summary>
    private SkillDefine.Skill selectingSkill;
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
    /// <summary>
    /// Block where the selected character will attack
    /// 選択キャラクターの攻撃先のブロック
    /// </summary>
    private MapBlock attackBlock;
    /// <summary>
    /// Flag for game end(true after it's settled)
    /// </summary>
    private bool isGameSet;
    /// <summary>
    /// Position of the selected character before moving (X direction)
    /// 選択キャラクターの移動前の位置(X方向)
    /// </summary>
    private int characterStartPosX;
    /// <summary>
    /// Position of the selected character before moving (Z direction)
    /// 選択キャラクターの移動前の位置(Z方向)
    /// </summary>
    private int characterStartPosZ;
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
        if (isGameSet) return;
        UpdateInput();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Attack command button pressed
    /// </summary>
    public void AttackCommand()
    {
        // Turn off skill selection
        // 特技の選択をオフにする
        selectingSkill = SkillDefine.Skill._None;
        // Retrieve and display the attack range
        // 攻撃範囲を取得して表示する
        GetAttackableBlocks();
    }
    /// <summary>
    /// 特技コマンドボタン処理
    /// Process SkillCommand button
    /// </summary>
    public void SkillCommand()
    {
        // Selective state of skill possessed by the character
        // キャラクターの持つ特技を選択状態にする
        selectingSkill = selectingCharacter.Skill;
        // Retrieve and display the attack range
        // 攻撃範囲を取得して表示する
        GetAttackableBlocks();
    }
    /// <summary>
    /// Standby command button pressed
    /// </summary>
    public void StandbyCommand()
    {
        // Hide command buttons
        guiManager.ShowHideCommandButtons(false);
        // Advancing progression mode.
        // 敵のターンへ進行モードを進める
        ChangePhase(Phase.EnemyStart);
    }
    /// <summary>
    /// Release the waiting state for move input for the currently selected character
    /// 選択中のキャラクターの移動入力待ち状態を解除する
    /// </summary>
    public void CancelMoving()
    {
        // Release the selection state of all blocks
        // 全ブロックの選択状態を解除
        mapManager.ResetAllSelectionMode();
        // Initialize the movable location list
        // 移動可能な場所リストを初期化する
        reachableBlocks.Clear();
        // Initialize the currently selected character information
        // 選択中のキャラクター情報を初期化する
        ClearSelectingCharacter();
        // Hide button to stop movement
        // 移動を止めるボタン非表示
        guiManager.ShowHideMoveCancelButton(false);
        // Undo phase (do not show logo)
        // フェーズを元に戻す（ロゴを表示しない）
        ChangePhase(Phase.CharaStart, true);
    }
    /// <summary>
    /// Checks to see if the game's termination conditions are met, and if so, terminates the game
    /// ゲームの終了条件を満たすか確認し、満たすならゲームを終了する
    /// </summary>
    public void CheckGameSet()
    {
        // Player victory flag (Off if there is a living enemy)
        // プレイヤー勝利フラグ(生きている敵がいるならOffになる)
        bool isWin = true;
        // Player defeat flag (Off if there are any allies alive)
        // プレイヤー敗北フラグ(生きている味方がいるならOffになる)
        bool isLose = true;
        // Check for the presence of living enemies and allies, respectively
        // それぞれ生きている敵・味方が存在するかをチェック
        foreach (var charaData in charactersManager.characters)
        {
            if (charaData.IsEnemy) isWin = false;   // 敵が居るので勝利フラグOff
            else isLose = false;                    // 味方が居るので敗北フラグOff
        }
        // If the game is still flagged for victory or defeat, the game ends
        // 勝利または敗北のフラグが立ったままならゲームを終了する
        if (isWin || isLose)
        {
            // Set the game end flag
            // ゲーム終了フラグを立てる
            isGameSet = true;
            // Show Logo UI
            DOVirtual.DelayedCall(1.5f, () =>
            {
                if (isWin) guiManager.ShowLogoGameClear();  // Game clear
                else guiManager.ShowLogoGameOver();         // Game over
                // Start fade in
                guiManager.StartFadeIn();
            });
            // Reload EnhanceScene
            DOVirtual.DelayedCall(7.0f, () =>
            {
                SceneManager.LoadScene("EnhanceScene");
            });
        }
    }
    /// <summary>
    /// Action content decision button processing
    /// 行動内容決定ボタン処理
    /// </summary>
    public void DecideButton()
    {
        // Hide action decision and cancel buttons
        // 行動決定・キャンセルボタンを非表示にする
        guiManager.ShowHideDecideButtons(false);
        // Unhighlight the block to be attacked
        // 攻撃先のブロックの強調表示を解除する
        attackBlock.SetSelectionMode(MapBlock.Highlight.Off);
        // Get data on the character at the position of the attack target
        // 攻撃対象の位置に居るキャラクターのデータを取得
        var targetCharacter = charactersManager.GetCharacterDataByPos(attackBlock.posX, attackBlock.posZ);
        if (targetCharacter != null)
        {
            // The character to be attacked exists
            // Character Attack Processing
            // 攻撃対象のキャラクターが存在する
            // キャラクター攻撃処理
            CharacterAttack(selectingCharacter, targetCharacter);
            // Advance progress mode (to action result display)
            // 進行モードを進める(行動結果表示へ)
            ChangePhase(Phase.CharaResult);
            return;
        }
        else
        {
            // The character to be attacked does not exist
            // Advance the progress mode (to the enemy's turn)
            // 攻撃対象が存在しない
            // 進行モードを進める(敵のターンへ)
            ChangePhase(Phase.EnemyStart);
        }
    }
    /// <summary>
    /// Action content reset button processing
    /// 行動内容リセットボタン処理
    /// </summary>
    public void CancelButton()
    {
        // Hide action decision and cancel buttons
        // 行動決定・キャンセルボタンを非表示にする
        guiManager.ShowHideDecideButtons(false);
        // Unhighlight the block to be attacked
        // 攻撃先のブロックの強調表示を解除する
        attackBlock.SetSelectionMode(MapBlock.Highlight.Off);
        // Return the character to its pre-movement position
        // キャラクターを移動前の位置に戻す
        selectingCharacter.MovePosition(characterStartPosX, characterStartPosZ);
        // Deselecting a character
        // キャラクターの選択を解除する
        ClearSelectingCharacter();
        // Return to progress mode (to the beginning of the turn)
        // 進行モードを戻す(ターンの最初へ)
        ChangePhase(Phase.CharaStart, true);
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
    /// <summary>
    /// Update Input Processing
    /// </summary>
    private void UpdateInput()
    {
        // タップ検出処理
        // IsPointerOverGameObject:UIへのタップを検出する
        // オブジェクトとUIへの同時タップ判定を防ぐ
        if (Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject())
        {
            // UIでない部分でタップが行われた場合の処理
            // バトル結果表示ウィンドウが出ているときの処理
            if (guiManager.battleWindowUI.gameObject.activeInHierarchy)
            {
                // バトル結果表示ウィンドウを閉じる
                guiManager.battleWindowUI.HideWindow();
                // 進行モードを進める（デバック用）
                // ChangePhase(Phase.CharaStart);
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
            case Phase.CharaResult:
            case Phase.EnemyStart:
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
            // Keep current position of selected character
            // 選択キャラクターの現在位置を記憶
            characterStartPosX = selectingCharacter.currentPosX;
            characterStartPosZ = selectingCharacter.CurrentPosZ;
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
            // Show MoveCancelButton
            // 移動キャンセルボタン表示
            guiManager.ShowHideMoveCancelButton(true);
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
    /// <param name="noLogos">ロゴ非表示フラグ(省略可能・省略するとfalse)</param>
    private void ChangePhase(Phase newPhase, bool noLogos = false)
    {
        if (isGameSet) return;
        // モード変更を保存
        nowPhase = newPhase;
        // Processing of the timing of the switch to a specific mode
        // 特定のモードに切り替わったタイミングの処理
        switch (nowPhase)
        {
            case Phase.CharaStart:
                // My character's turn: start
                // 自分のターン：開始時
                if (!noLogos) guiManager.ShowLogoTurnImage(guiManager.playerTurnImage);
                break;
            case Phase.CharaMoving:
                // My character's turn: moving
                // 自分のターン：移動先選択中
                break;
            case Phase.CharaCommand:
                // My character's turn: selecting a command after moving
                // 自分のターン：移動後のコマンド選択中
                break;
            case Phase.CharaTargeting:
                // My character's turn: selecting an attack target
                // 自分のターン：攻撃の対象を選択中
                break;
            case Phase.CharaResult:
                // My character's turn: displaying the action result
                // 自分のターン：行動結果表示中
                break;
            case Phase.EnemyStart:
                // Enemy's turn: start
                // 敵のターン：開始時
                StartEnemyTurn(noLogos);
                break;
            case Phase.EnemyResult:
                // Enemy's turn: displaying the action result
                // 敵のターン：行動結果表示中
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Move the character
    /// </summary>
    /// <param name="targetBlock"></param>
    private void MoveCharacter(MapBlock targetBlock)
    {
        // If an enemy character is selected, cancel the move and return
        // 敵キャラクターを選択中なら移動をキャンセルして終了
        if (selectingCharacter.IsEnemy)
        {
            CancelMoving();
            return;
        }
        // If the target block is not in the list of blocks that the character can move to, the process is terminated
        // 移動先ブロックがキャラクターが移動可能なブロックリストに含まれていない場合、処理を終了
        if (!reachableBlocks.Contains(targetBlock)) return;
        // Move the character to the destination block
        // 移動先のブロックにキャラクターを移動
        // TODO:移動先のブロックにキャラクターの足ともを選択しない
        selectingCharacter.MovePosition(targetBlock.posX, targetBlock.posZ);
        // Clear the list of reachable blocks
        // 移動可能なブロックリストをクリア
        reachableBlocks.Clear();
        // Release the selection status of all blocks
        // 全ブロックの選択状態を解除
        mapManager.ResetAllSelectionMode();
        // Hide MoveCancelButton
        guiManager.ShowHideMoveCancelButton(false);
        // 移動キャンセルボタン非表示
        // Execute the process after a specified number of seconds
        // 指定秒数経過後に処理を実行する
        DOVirtual.DelayedCall(1.0f, () =>
        {
            // Show command buttons
            // コマンドボタンを表示する
            guiManager.ShowHideCommandButtons(true, selectingCharacter);
            // Advance mode of progression: during post-movement command selection
            // 進行モードを進める: 移動後のコマンド選択中
            ChangePhase(Phase.CharaCommand);
        });
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
        // Keep block information of attacker
        // 攻撃先のブロック情報を記憶
        attackBlock = targetBlock;
        // Show  Decision and Cancel buttons
        // 行動決定・キャンセルボタンを表示する
        guiManager.ShowHideDecideButtons(true);
        // Clear the list of attachable blocks
        // 攻撃可能なブロックリストをクリア
        attackableBlocks.Clear();
        // Release the selection status of all blocks
        // 全ブロックの選択状態を解除
        mapManager.ResetAllSelectionMode();
        // 攻撃先のブロックを強調表示する
        // Highlight the block to be attacked
        attackBlock.SetSelectionMode(MapBlock.Highlight.Attackable);
        // Advancing mode of progress: the target of the attack is being selected
        // 進行モードを進める：攻撃の対象を選択中
        ChangePhase(Phase.CharaTargeting);

        #region DecideButton/CancelButton 導入により以下は削除
        /*
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
        */
        #endregion
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
        int attackPoint = attackChara.Attack;
        int defencePoint = defenceChara.Defence;
        // Handling of the zero defense debuff
        // 防御力0化デバフがかかっていた時の処理
        if (defenceChara.IsDefenceBreak) defencePoint = 0;
        // Calculate the damage value
        // Damage = Attack - Defence
        // ダメージ = 攻撃力 - 防御力
        int damageValue = attackPoint - defencePoint;
        // Calculate the damage multiplier based on compatibility
        // 相性によるダメージ倍率を計算
        // Get the damage multiplier
        // ダメージ倍率を取得
        float ratio = GetDamageRatioByAttribute(attackChara.attribute, defenceChara.attribute);
        damageValue = (int)(damageValue * ratio);
        // If the amount of damage is less than zero, set to zero
        // ダメージ量が0未満なら0にする
        if (damageValue < 0) damageValue = 0;
        // Damage value correction and effect processing by selected specialties
        // 選択した特技によるダメージ値補正および効果処理
        switch (selectingSkill)
        {
            case SkillDefine.Skill.Critical:
                // 会心の一撃
                // Damage doubled
                // ダメージ2倍
                damageValue *= 2;
                // Disables the use of skill
                // 特技使用不可状態にする
                attackChara.IsSkillLock = true;
                break;
            case SkillDefine.Skill.DefBreak:
                // Destroy sheild
                // シールド破壊
                damageValue = 0;
                // Defensive 0-defense debuff set
                // 防御力0化デバフをセット
                defenceChara.IsDefenceBreak = true;
                break;
            case SkillDefine.Skill.Heal:
                // Heal
                // The amount of recovery is half of the attack power.By making it a negative number, it recovers when calculating damage
                // (回復量は攻撃力の半分。負数にする事でダメージ計算時に回復する)
                damageValue = (int)(attackPoint * -0.5f);
                break;
            case SkillDefine.Skill.FieBall:
                // Damage reduction by half
                // ダメージ半減
                damageValue /= 2;
                break;
            default:
                // 特技無しor通常攻撃時
                // Noskill or normal attack
                break;
        }
        // Show the character's attack animation(No animation for heal) & play attack sound
        // キャラクター攻撃アニメーション(回復はアニメ無し)
        if (selectingSkill != SkillDefine.Skill.Heal
            && selectingSkill != SkillDefine.Skill.FieBall)
        {
            attackChara.AnimateAttack(defenceChara);
            // Play the SE at about the time the attack hits in the animation
            // アニメーション内で攻撃が当たったくらいのタイミングでSEを再生
            audioManager.PlayRandomAttackSE();
        }
        // Play heal sound
        else if (selectingSkill == SkillDefine.Skill.Heal) audioManager.PlayHealSE();
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
            // Delete the character
            // キャラクターを削除
            charactersManager.DeleteCharacter(defenceChara);
        // Remove skill selection status
        // 特技の選択状態を解除する
        selectingSkill = SkillDefine.Skill._None;
        // turn switching process
        // ターン切替処理
        DOVirtual.DelayedCall(2.0f, () =>
        {
            // Hide the battle result display window
            // バトル結果表示ウィンドウを非表示
            guiManager.battleWindowUI.HideWindow();
            // Change the turn
            // ターンの切替
            if (nowPhase == Phase.CharaResult) ChangePhase(Phase.EnemyStart); // 敵のターンへ
            else ChangePhase(Phase.CharaStart); // 自分のターンへ
        });
    }
    /// <summary>
    /// End the turn by having one of the enemy characters act (called out at the start of the enemy's turn)
    /// 敵キャラクターのうちいずれか一体を行動させてターンを終了（敵のターン開始時に呼出）
    /// </summary>
    private void CommandEnemy()
    {
        // Turn off skill selection
        selectingSkill = SkillDefine.Skill._None;
        // 特技の選択をオフにする
        // Get a list of enemy characters that are still alive
        // 生存中の敵キャラクターのリスト作成
        var enemyCharas = new List<Character>();
        foreach (Character character in charactersManager.characters)
        {
            // Add characters with the enemy flag set to the list
            // 全生存キャラクターから敵フラグの立っているキャラクターをリストに追加
            if (character.IsEnemy) enemyCharas.Add(character);
        }
        // Randomly get one of the possible attack character-position combinations
        // 攻撃可能なキャラクター・位置の組み合わせの内1つをランダムに取得
        var actionPlan = TargetFinder.GetRandomActionPlan(mapManager, charactersManager, enemyCharas);
        // If the data for the combination exists, the attack is launched
        // 組み合わせのデータが存在すれば攻撃開始
        if (actionPlan != null)
        {
            // Enemy characters start to move
            // 敵キャラクター移動開始
            actionPlan.CharaData.MovePosition(actionPlan.ToMoveBlock.posX, actionPlan.ToMoveBlock.posZ);
            // Enemy character attack start
            // 敵キャラクター攻撃開始
            DOVirtual.DelayedCall(1.0f, () =>
            {
                CharacterAttack(actionPlan.CharaData, actionPlan.ToAttackChara);
            });
            // Advance progress mode (to action result display).
            // 進行モードを進める（行動結果表示へ）
            ChangePhase(Phase.EnemyResult);
            return;
        }
        // Dealing with one attackable enemy character until it is found
        // 攻撃可能な敵キャラクター1体を見つけるまで処理
        foreach (Character enemyData in enemyCharas)
        {
            // Get a list of mobile locations
            // 移動可能な場所リストを取得
            reachableBlocks = mapManager.SearchReachableBlocks(enemyData.currentPosX, enemyData.CurrentPosZ);
            // Processes for each mobile location
            // それぞれの移動可能な場所ごとの処理
            foreach (MapBlock mapBlock in reachableBlocks)
            {
                // Get a list of attackable locations
                // 攻撃可能な場所リストを取得
                attackableBlocks = mapManager.SearchAttackableBlocks(mapBlock.posX, mapBlock.posZ);
                // Processes for each attackable location
                // それぞれの攻撃可能な場所ごとの処理
                foreach (MapBlock attackBlock in attackableBlocks)
                {
                    // Find an opponent character (on the player's side) who can attack
                    // 攻撃できる相手キャラクター（プレイヤー側のキャラクター）を探す
                    var targetCharacter = charactersManager.GetCharacterDataByPos(attackBlock.posX, attackBlock.posZ);
                    if (targetCharacter != null && !targetCharacter.IsEnemy)
                    {
                        // Opponent character is present
                        // 相手キャラクターがいる
                        // Enemy character move processing
                        // 敵キャラクター移動処理
                        enemyData.MovePosition(mapBlock.posX, mapBlock.posZ);
                        // Enemy character attack processing
                        // 敵キャラクター攻撃処理
                        DOVirtual.DelayedCall(1.0f, () =>
                        {
                            CharacterAttack(enemyData, targetCharacter);
                        });
                        // Clear list of movement and attack locations
                        // 移動・攻撃場所リストをクリア
                        reachableBlocks.Clear();
                        attackableBlocks.Clear();
                        // Advancing progression mode(Go to action result display)
                        // 進行モードを進める（行動結果表示へ）
                        ChangePhase(Phase.EnemyResult);
                        return;
                    }
                }
            }
        }
        // If no enemy character can attack, move one character at random
        // 攻撃可能な相手が見つからなかったら移動させる1体をランダムに選ぶ
        int randomIndex = UnityEngine.Random.Range(0, enemyCharas.Count);
        Character targetEnemy = enemyCharas[randomIndex];
        // Randomly select one location from the list of target moveable locations
        // 対象の移動可能場所リストの中から1つの場所をランダムに選ぶ
        reachableBlocks = mapManager.SearchReachableBlocks(targetEnemy.currentPosX, targetEnemy.CurrentPosZ);
        if (reachableBlocks.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, reachableBlocks.Count);
            MapBlock targetBlock = reachableBlocks[randomIndex];
            // Move the enemy character to the destination block
            // 移動先のブロックに敵キャラクターを移動
            targetEnemy.MovePosition(targetBlock.posX, targetBlock.posZ);
        }
        // If there is no player character that can attack, the turn ends
        // (攻撃可能な相手が見つからなかった場合何もせずターン終了)
        // Clear list of movement and attack locations
        // 移動・攻撃場所リストをクリア
        reachableBlocks.Clear();
        attackableBlocks.Clear();
        // Advancing progression mode(Go to action result display)
        // 進行モードを進める（行動結果表示へ）
        DOVirtual.DelayedCall(1.0f, () =>
        {
            ChangePhase(Phase.CharaStart);
        });
    }
    /// <summary>
    /// Start the enemy's turn
    /// </summary>
    private void StartEnemyTurn(bool nologs)
    {
        if (!nologs) guiManager.ShowLogoTurnImage(guiManager.enemyTurnImage);
        DOVirtual.DelayedCall(1.0f, () =>
        {
            // End the turn by having one of the enemy characters act
            // 敵キャラクターのうちいずれか一体を行動させてターンを終了
            CommandEnemy();
        });
    }
    /// <summary>
    /// Return damage multipliers based on the compatibility of the attributes of the attacker and defender
    /// 攻撃側・防御側の属性の相性によるダメージ倍率を返す
    /// </summary>
    /// <param name="attackAttribute">Attributes of attacker(攻撃側の属性)</param>
    /// <param name="defenseAttribute">Attributes of defender(防御側の属性)</param>
    /// <returns></returns>
    private float GetDamageRatioByAttribute(Character.Attribute attackAttribute, Character.Attribute defenseAttribute)
    {
        // Compatibility determination process. Checks each attribute in order of good compatibility to bad compatibility and returns the normal multiplier if neither is applicable
        // 相性決定処理。属性ごとに良相性→悪相性の順でチェックし、どちらにも当てはまらないなら通常倍率を返す
        switch (attackAttribute)
        {
            case Character.Attribute.Fire:
                // 火属性：風属性に強く水属性に弱い
                return GetDamageRatio(Character.Attribute.Wind, Character.Attribute.Water);
            case Character.Attribute.Water:
                // 水属性：火属性に強く土属性に弱い
                return GetDamageRatio(Character.Attribute.Fire, Character.Attribute.Soil);
            case Character.Attribute.Wind:
                // 風属性：土属性に強く火属性に弱い
                return GetDamageRatio(Character.Attribute.Soil, Character.Attribute.Fire);
            case Character.Attribute.Soil:
                // 土属性：水属性に強く風属性に弱い
                return GetDamageRatio(Character.Attribute.Water, Character.Attribute.Wind);
            default:
                return damageRatioNormal;
        }

        // Get the damage ratio based on the attributes of the attacker and defender
        // 攻撃側と防御側の属性に基づいてダメージ倍率を取得
        float GetDamageRatio(Character.Attribute attributeHigh, Character.Attribute attributeLow)
        {
            if (defenseAttribute == attributeHigh) return damageRatioHigh;
            else if (defenseAttribute == attributeLow) return damageRatioLow;
            else return damageRatioNormal;
        }
    }
    /// <summary>
    /// Display the target block after selecting an attack or special command
    /// 攻撃・特技コマンド選択後に対象ブロックを表示する
    /// </summary>
    private void GetAttackableBlocks()
    {
        // Hide command buttons
        guiManager.ShowHideCommandButtons(false);
        // Get a list of blocks that the selected character can attack(Skill: For fireballs, map-wide)
        // 攻撃可能な場所リストを取得(特技：ファイアボールの場合はマップ全域に対応)
        attackableBlocks = selectingSkill == SkillDefine.Skill.FieBall ?
                            mapManager.MapBlocksToList()
                            : mapManager.SearchAttackableBlocks(selectingCharacter.currentPosX, selectingCharacter.CurrentPosZ);
        // Display the list of attackable places
        // 攻撃可能な場所リストを表示
        foreach (MapBlock mapBlock in attackableBlocks)
            mapBlock.SetSelectionMode(MapBlock.Highlight.Attackable);
    }
    #endregion Private Methods

    #endregion Methods
}
