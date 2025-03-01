using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// TargetFinder class
/// </summary>
public static class TargetFinder
{
    #region Nested Class

    /// <summary>
    /// ActionPlan class
    /// 行動プランクラス
    /// （行動する敵キャラクター、移動先の位置、攻撃相手のキャラクターの3データを1纏めに扱う）
    /// </summary>
    public class ActionPlan
    {
        /// <summary>
        /// Characters in action
        /// 行動するキャラクター
        /// </summary>
        public Character CharaData;
        /// <summary>
        /// Destination location
        /// 移動先の位置
        /// </summary>
        public MapBlock ToMoveBlock;
        /// <summary>
        /// Character of the attacker
        /// 攻撃相手のキャラクター
        /// </summary>
        public Character ToAttackChara;
    }
    #endregion Nested Class

    #region Enum

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

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods
    /// <summary>
    /// A process that searches for all possible attack plans of action and returns one of them at random
    /// 攻撃可能な行動プランを全て検索し、その内の1つをランダムに返す処理
    /// </summary>
    /// <param name="mapManager">シーン内のMapManagerの参照</param>
    /// <param name="charactersManager">シーン内のCharacterManagerの参照</param>
    /// <param name="enemyCharas">敵キャラクターのリスト</param>
    /// <returns></returns>
    public static ActionPlan GetRandomActionPlan(MapManager mapManager, CharactersManager charactersManager, List<Character> enemyCharas)
    {
        // All action plans (added each time a possible attacker is found)
        // 全行動プラン（攻撃可能な相手が見つかる度に追加される）
        var actionPlans = new List<ActionPlan>();
        // Movement range list
        // 移動範囲リスト
        var reachableBlocks = new List<MapBlock>();
        // Attack range list
        // 攻撃範囲リスト
        var attackableBlocks = new List<MapBlock>();
        // Search all action plans
        // 全行動プラン検索
        foreach (Character enemyChara in enemyCharas)
        {
            // Search for all blocks that can be moved
            // 移動可能な全ブロックを検索
            reachableBlocks = mapManager.SearchReachableBlocks(enemyChara.currentPosX, enemyChara.CurrentPosZ);
            // Processes for each mobile location
            // それぞれの移動可能な場所ごとの処理
            foreach (MapBlock mapBlock in reachableBlocks)
            {
                // Search for all blocks that can be attacked
                // 攻撃可能な場所リストを取得
                attackableBlocks = mapManager.SearchAttackableBlocks(mapBlock.posX, mapBlock.posZ);
                foreach (MapBlock attackBlock in attackableBlocks)
                {
                    // Find an opponent character (on the player's side) who can attack
                    // 攻撃できる相手キャラクター（プレイヤー側のキャラクター）を探す
                    Character targetChara = charactersManager.GetCharacterDataByPos(attackBlock.posX, attackBlock.posZ);
                    if (targetChara != null && !targetChara.IsEnemy)
                    {
                        // 行動プラン追加
                        var newPlan = new ActionPlan
                        {
                            CharaData = enemyChara,
                            ToMoveBlock = mapBlock,
                            ToAttackChara = targetChara
                        };
                        // Add to all action plan list
                        // 全行動プランリストに追加
                        actionPlans.Add(newPlan);
                    }
                }
            }
        }
        //
        // 検索終了後、行動プランが1つでもあるならその内の1つをランダムに返す
        if (actionPlans.Count > 0)
        {
            // Return one of the action plans at random
            // 行動プランの内の1つをランダムに返す
            return actionPlans[Random.Range(0, actionPlans.Count)];
        }
        else
        {
            // If there is no action plan, return null
            // 行動プランがない場合はnullを返す
            return null;
        }
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
