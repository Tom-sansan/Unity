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
    [Header("ステージアイコン画像")]
    public Sprite stageIcon;
    [Header("ステージ背景画像")]
    public Sprite stagePicture;

    [Space(10)]
    [Header("各進行度別の敵の出現テーブル")]
    public List<AppearEnemyTable> appearEnemyTables;
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
