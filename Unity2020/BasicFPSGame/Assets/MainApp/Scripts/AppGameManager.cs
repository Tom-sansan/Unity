using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// App Game Controller Class
/// </summary>
public class AppGameManager : MonoBehaviour
{
    #region Enum
    public enum EnemyType
    {
        Crocodile,
        Bat
    }
    #endregion

    #region Variables
    #region SerializeField
    /// <summary>
    /// Enemy prefab on ground
    /// </summary>
    [SerializeField]
    private GameObject enemyPrefab = null;
    /// <summary>
    /// Enemy prefab in the sky
    /// </summary>
    [SerializeField]
    private GameObject skyEnemyPrefab = null;
    /// <summary>
    /// Enemy appearance Points
    /// </summary>
    [SerializeField]
    private List<Transform> enemySpawnPoints = new List<Transform>();
    /// <summary>
    /// The number of enemies on field
    /// </summary>
    [SerializeField]
    private int enemyCount = 20;
    #endregion
    /// <summary>
    /// Enemies currently on the field
    /// </summary>
    private List<EnemyBase> currentFieldEnemies = new List<EnemyBase>();
    /// <summary>
    /// Flag to regenerate reduced enemies
    /// </summary>
    private bool isEnemyRespawn = false;
    #endregion

    private void Start()
    {
        InitSpawn();
    }
    private void Update()
    {
        UpdateEnemyCount();
    }
    /// <summary>
    /// Initialization
    /// </summary>
    public void InitSpawn()
    {
        StartCoroutine(InitEnemySpawnCor());
    }
    /// <summary>
    /// Create enemy
    /// </summary>
    /// <param name="enemyType"></param>
    public void CreateEnemy(EnemyType enemyType)
    {
        var num = Random.Range(0, enemySpawnPoints.Count);
        var pos = enemySpawnPoints[num].position;
        switch (enemyType)
        {
            case EnemyType.Crocodile:
                {
                    var go = Instantiate(enemyPrefab, pos, Quaternion.identity);
                    var enemy = go.GetComponent<EnemyCrocodile>();
                    enemy.Init(this, 3, 1f);
                    enemy.SetMoveTargetList(enemySpawnPoints);
                    currentFieldEnemies.Add(enemy);
                }
                break;
            case EnemyType.Bat:
                {
                    pos.y += 5f;
                    var go = Instantiate(skyEnemyPrefab, pos, Quaternion.identity);
                    var enemy = go.GetComponent<EnemyBat>();
                    enemy.Init(this, 1, 1f);
                    currentFieldEnemies.Add(enemy);
                }
                break;
            default:
                Debug.LogWarning("Please set EnemyType collectly");
                break;
        }
    }
    /// <summary>
    /// Enemy dead call
    /// </summary>
    /// <param name="enemy"></param>
    public void OnDeadEnemy(EnemyBase enemy)
    {
        if (currentFieldEnemies.Contains(enemy)) currentFieldEnemies.Remove(enemy);
    }
    /// <summary>
    /// Coroutine for Initialization
    /// * Create one enemy at a time at regular intervals, ending when the maximum number of enemies is reached
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitEnemySpawnCor()
    {
        while (currentFieldEnemies.Count < enemyCount)
        {
            yield return new WaitForSeconds(1f);
            var num = Random.Range(0, 2);
            CreateEnemy((EnemyType)num);
            if (currentFieldEnemies.Count >= enemyCount)
            {
                isEnemyRespawn = true;
                break;
            }
        }
    }
    /// <summary>
    /// Add enemy
    /// </summary>
    private void UpdateEnemyCount()
    {
        if (!isEnemyRespawn) return;
        if (currentFieldEnemies.Count < enemyCount)
        {
            isEnemyRespawn = false;
            // Create enemy
            var def = enemyCount - currentFieldEnemies.Count;
            StartCoroutine(EnemySpawnCor(def));
        }
    }
    /// <summary>
    /// Coroutine for adding anemy
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    private IEnumerator EnemySpawnCor(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            yield return new WaitForSeconds(1f);
            var num = Random.Range(0, 2);
            CreateEnemy((EnemyType)num);
            if (i >= count) isEnemyRespawn = true;
        }
    }
}
