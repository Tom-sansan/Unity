using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MapManager Class
/// </summary>
public class MapManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// マップブロックの親オブジェクトのTransform
    /// </summary>
    [SerializeField]
    private Transform blockParent;
    /// <summary>
    /// 草ブロック
    /// </summary>
    [SerializeField]
    private GameObject blockPrefabGrass;
    /// <summary>
    /// 水場ブロック
    /// </summary>
    [SerializeField]
    private GameObject blockPrefabWater;
    /// <summary>
    /// Map data
    /// </summary>
    [SerializeField]
    private MapBlock[,] mapBlocks;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// マップの横幅
    /// </summary>
    private const int MAP_WIDTH = 9;
    /// <summary>
    /// マップの縦幅
    /// </summary>
    private const int MAP_HEIGHT = 9;
    /// <summary>
    /// 草ブロックが生成される確率
    /// </summary>
    private const int GENERATE_RATIO_GRASS = 90;
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
    /// Release all block selection states
    /// 全てのブロック選択状態を解除する
    /// </summary>
    public void ResetAllSelectionMode()
    {
        for (int i = 0; i < MAP_WIDTH; i++)
            for (int j = 0; j < MAP_HEIGHT; j++)
                mapBlocks[i, j].SetSelectionMode(MapBlock.Highlight.Off);
    }
    /// <summary>
    /// Returns a list of map blocks where the character can be reached from the passed position
    /// 渡された位置からキャラクターが到達できる場所のマップブロックをリストにして返す
    /// </summary>
    /// <param name="posX">基点x位置</param>
    /// <param name="posZ">基点z位置</param>
    /// <returns></returns>
    public List<MapBlock> SearchReachableBlocks(int posX, int posZ)
    {
        // List of blocks that satisfy the condition
        // 条件を満たすブロックのリスト
        var results = new List<MapBlock>();
        // Search for the number ( index ) in the array of the base block
        // 基点となるブロックの配列内番号( index )を検索
        // Initialize the base block position
        // 配列内番号
        (int baseX, int baseZ) = SearchIndexesInMapBlocks(posX, posZ);
        // int baseX = -1, baseZ = -1;
        //for (int i = 0; i < MAP_WIDTH; i++)
        //{
        //    for (int j = 0; j < MAP_HEIGHT; j++)
        //    {
        //        if (mapBlocks[i, j].posX == posX && mapBlocks[i, j].posZ == posZ)
        //        {
        //            // Find map blocks matching the specified coordinates.
        //            // Get the array number and end the loop
        //            // 指定された座標に一致するマップブロックを発見
        //            // 配列内番号を取得してループを終了
        //            baseX = i;
        //            baseZ = j;
        //            break;
        //        }
        //    }
        //    // If the base block is found, end the loop
        //    // 既に基点ブロックが見つかっていればループを終了
        //    if (baseX != -1) break;
        //}

        // Get and add to the list the block data up to the dead end in each direction in turn
        // それぞれの方向に向かって行き止まりまでのブロックデータを順番に取得・リストに追加する
        // X+ direction
        // X+方向
        for (int i = baseX + 1; i < MAP_WIDTH; i++)
            if (AddReachableList(results, mapBlocks[i, baseZ])) break;
        // X- direction
        // X-方向 (i >= 0 は、マップ(mapBlocks)の範囲外にアクセスしないための条件)
        for (int i = baseX - 1; i >= 0; i--)
            if (AddReachableList(results, mapBlocks[i, baseZ])) break;
        // Z+ direction
        // Z+方向
        for (int j = baseZ + 1; j < MAP_HEIGHT; j++)
            if (AddReachableList(results, mapBlocks[baseX, j])) break;
        // Z- direction
        // Z-方向
        for (int j = baseZ - 1; j >= 0; j--)
            if (AddReachableList(results, mapBlocks[baseX, j])) break;
        // Foot blocks
        // 足元のブロック
        results.Add(mapBlocks[baseX, baseZ]);
        return results;
    }
    /// <summary>
    /// Returns a list of map blocks where the character can attack from the passed position
    /// 渡された位置からキャラクターが攻撃できる場所のマップブロックをリストにして返す
    /// </summary>
    /// <param name="posX">基点x位置</param>
    /// <param name="posZ">基点z位置</param>
    /// <returns>条件を満たすマップブロックのリスト</returns>
    public List<MapBlock> SearchAttackableBlocks(int posX, int posZ)
    {
        // List of blocks that satisfy the condition
        // 条件を満たすブロックのリスト
        var results = new List<MapBlock>();
        // Search for the number ( index ) in the array of the base block
        // 基点となるブロックの配列内番号( index )を検索
        (int baseX, int baseZ) = SearchIndexesInMapBlocks(posX, posZ);
        // Initialize the base block position
        // 配列内番号
        //int baseX = -1, baseZ = -1;
        //for (int i = 0; i < MAP_WIDTH; i++)
        //{
        //    for (int j = 0; j < MAP_HEIGHT; j++)
        //    {
        //        if (mapBlocks[i, j].posX == posX && mapBlocks[i, j].posZ == posZ)
        //        {
        //            // Find map blocks matching the specified coordinates.
        //            // Get the array number and end the loop
        //            // 指定された座標に一致するマップブロックを発見
        //            // 配列内番号を取得してループを終了
        //            baseX = i;
        //            baseZ = j;
        //            break;
        //        }
        //    }
        //    // If the base block is found, end the loop
        //    // 既に基点ブロックが見つかっていればループを終了
        //    if (baseX != -1) break;
        //}

        // Set each block at a position one square ahead in each of the four directions
        // 4方向に1マス進んだ位置のブロックをそれぞれセット
        // X+ direction
        // X+方向
        AddAttackableList(results, baseX + 1, baseZ);
        // X- direction
        // X-方向
        AddAttackableList(results, baseX - 1, baseZ);
        // Z+ direction
        // Z+方向
        AddAttackableList(results, baseX, baseZ + 1);
        // Z- direction
        // Z-方向
        AddAttackableList(results, baseX, baseZ - 1);
        // Set blocks of four diagonal squares
        // 斜め4マスのブロックをセット
        // X+ Z+ direction
        // X+ Z+ 方向
        AddAttackableList(results, baseX + 1, baseZ + 1);
        // X- Z+ direction
        // X- Z+ 方向
        AddAttackableList(results, baseX - 1, baseZ + 1);
        // X+ Z- direction
        // X+ Z- 方向
        AddAttackableList(results, baseX + 1, baseZ - 1);
        // X- Z- direction
        // X- Z- 方向
        AddAttackableList(results, baseX -1, baseZ - 1);
        return results;
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // Initialize mapBlocks
        mapBlocks = new MapBlock[MAP_WIDTH, MAP_HEIGHT];
        // ブロック生成位置の基点となる座標を設定
        var defaultPos = new Vector3(0.0f, 0.0f, 0.0f);
        defaultPos.x = -MAP_WIDTH / 2;
        defaultPos.z = -MAP_HEIGHT / 2;
        // ブロック生成処理
        for (int i = 0; i < MAP_WIDTH; i++)
        {
            for (int j = 0; j < MAP_HEIGHT; j++)
            {
                // ブロックの場所を決定
                // 基点の座標を元に変数posを宣言
                var pos = defaultPos;
                // 1個目のfor分の繰り返し回数分x座標をずらす
                pos.x += i;
                // 2個目のfor分の繰り返し回数分z座標をずらす
                pos.z += j;
                // ブロックの種類を決定
                // 0~99の中から1つランダムな数字を取得
                int rand = Random.Range(0, 100);
                // 草ブロック生成フラグ
                var isGrass = false;
                // 乱数値が草ブロック確率値より小さければ草ブロックを生成する
                if (rand < GENERATE_RATIO_GRASS) isGrass = true;
                GameObject obj;
                if (isGrass) obj = Instantiate(blockPrefabGrass, blockParent);
                else obj = Instantiate(blockPrefabWater, blockParent);
                // オブジェクトの座標を適用
                obj.transform.position = pos;

                // Store block data in array mapBlocks
                // 配列mapBlocksにブロックデータを格納
                var mapBlock = obj.GetComponent<MapBlock>();
                mapBlocks[i, j] = mapBlock;

                // block data setting
                // ブロックデータ設定
                mapBlock.posX = (int)pos.x;
                mapBlock.posZ = (int)pos.z;
            }
        }
    }
    /// <summary>
    /// Add the specified block to the reachable block list
    /// 指定したブロックを到達可能ブロックリストに追加する
    /// </summary>
    /// <param name="reachableList">到達可能ブロックリスト(reachable block list)</param>
    /// <param name="targetBlock">対象ブロック(target block)</param>
    /// <returns>行き止まりフラグ(行き止まりならtrueが返る)</returns>
    
    private (int baseX, int baseZ) SearchIndexesInMapBlocks(int posX, int posZ)
    {
        // Initialize the base block position
        // 配列内番号
        int baseX = -1, baseZ = -1;
        for (int i = 0; i < MAP_WIDTH; i++)
        {
            for (int j = 0; j < MAP_HEIGHT; j++)
            {
                if (mapBlocks[i, j].posX == posX && mapBlocks[i, j].posZ == posZ)
                {
                    // Find map blocks matching the specified coordinates.
                    // Get the array number and end the loop
                    // 指定された座標に一致するマップブロックを発見
                    // 配列内番号を取得してループを終了
                    baseX = i;
                    baseZ = j;
                    break;
                }
            }
            // If the base block is found, end the loop
            // 既に基点ブロックが見つかっていればループを終了
            if (baseX != -1) break;
        }
        return (baseX, baseZ);
    }
    private bool AddReachableList(List<MapBlock> reachableList, MapBlock targetBlock)
    {
        // If the target block is impassable, it ends as a dead end.
        // 対象のブロックが通行不可ならそこを行き止まりとして終了
        if (!targetBlock.IsPassable) return true;
        // If there are already other characters at the target position, make it unreachable and exit (do not make it a dead end)
        // 対象の位置に他のキャラが既にいるなら到達不可にして終了（行き止まりにはしない）
        var characterData = GetComponent<CharactersManager>().GetCharacterDataByPos(targetBlock.posX, targetBlock.posZ);
        if (characterData != null) return false;
        // Add to reachable block list
        // 到達可能ブロックリストに追加する
        reachableList.Add(targetBlock);
        return false;
    }
    /// <summary>
    /// (キャラクター攻撃可能ブロック検索処理用)
    /// マップデータの指定された配列内番号に対するブロックを攻撃可能ブロックリストに追加する
    /// </summary>
    /// <param name="attackableList">攻撃可能ブロックリスト</param>
    /// <param name="indexX">X方向の配列内番号</param>
    /// <param name="indexZ">Y方向の配列内番号</param>
    private void AddAttackableList(List<MapBlock> attackableList, int indexX, int indexZ)
    {
        // 指定された番号が配列の外に出ていたら追加せず終了
        if (indexX < 0 || indexX >= MAP_WIDTH || indexZ < 0 || indexZ >= MAP_HEIGHT) return;
        // 到達可能ブロックリストに追加する
        attackableList.Add(mapBlocks[indexX, indexZ]);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
