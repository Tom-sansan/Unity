using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stage Data Definition Class
/// </summary>
[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObjects/StageSO")]
public class StageSO : ScriptableObject
{
    [Header("ステージ名(日本語)")]
    public string nameJP;
    [Header("ステージ名(英語)")]
    public string nameEN;

    [Space(10)]
    [Header("難易度表示(日本語)")]
    public string difficultyJP;
    [Header("難易度表示(英語)")]
    public string difficultyEN;

    [Space(10)]
    [Header("ステージアイコン画像")]
    public Sprite stageIcon;
    [Header("ステージ背景画像")]
    public Sprite stagePicture;
    [Header("ステージBGM")]
    public AudioClip stageBGMClip;

    [Space(10)]
    [Header("各進行度別の敵の出現テーブル")]
    public List<AppearEnemyTable> appearEnemyTables;

    [Space(10)]
    [Header("戦闘報酬：経験値獲得量係数")]
    public int bonusEXP;
    [Header("戦闘報酬：金貨獲得量係数")]
    public int bonusGold;
    [Header("戦闘報酬：体力回復量(固定)")]
    public int bonusHeal;

    [Space(10)]
    [Header("無限ステージモード")]
    public bool infinityMode;
    [Header("無限ステージ用：出現ザコ敵リスト")]
    public List<EnemyStatusSO> infinityEnemyData;
    [Header("無限ステージ用：出現ボス敵リスト")]
    public List<EnemyStatusSO> infinityBossData;
    [Header("無限ステージ用：ボス敵出現間隔")]
    public int bossDistance;
    [Header("無限ステージ用：敵HP増加量")]
    public int enemyHPIncrease;
}

/// <summary>
/// Enemy appearance table classes for each progression
/// </summary>
[Serializable]
public class AppearEnemyTable
{
    /// <summary>
    /// Enemy appearance table (only one enemy is designated and treated as a boss enemy)
    /// </summary>
    public List<EnemyStatusSO> appearEnemies;
}
