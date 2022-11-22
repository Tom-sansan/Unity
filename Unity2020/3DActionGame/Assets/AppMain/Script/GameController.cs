using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Game over object
    /// </summary>
    [SerializeField]
    private GameObject gameOver = null;
    /// <summary>
    /// Player
    /// </summary>
    [SerializeField]
    private PlayerController player = null;
    /// <summary>
    /// Enemy list
    /// </summary>
    [SerializeField]
    private List<EnemyBase> enemies = new List<EnemyBase>();
    /// <summary>
    /// Target list for enemy movement
    /// </summary>
    [SerializeField]
    private List<Transform> enemyTargets = new List<Transform>();
    /// <summary>
    /// Enemy prefab list
    /// </summary>
    [SerializeField]
    private List<GameObject> enemyPrefabList = new List<GameObject>();
    /// <summary>
    /// List of Enemy Appearance Points
    /// </summary>
    [SerializeField]
    private List<Transform> enemyGateList = new List<Transform>();
    /// <summary>
    /// The number of enemy
    /// </summary>
    [SerializeField]
    private int numberOfEnemy = 2;
    /// <summary>
    /// Enemy list on field
    /// </summary>
    private List<EnemyBase> fieldEnemys = new List<EnemyBase>();
    /// <summary>
    /// Enemy automatic generating flag
    /// </summary>
    private bool isEnemySpawn = false;
    /// <summary>
    /// The number of beaten enemy
    /// </summary>
    private int currentBossCount = 0;
    #endregion

    void Start()
    {
        AddEventListeners();
        gameOver.SetActive(false);
        // CreateEnemy(numberOfEnemy);
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnRetryButtonClicked()
    {
        // Player retry
        player.Retry();
        // Enemy retry
        // foreach (var enemy in enemies) enemy.OnRetry();
        foreach (var enemy in fieldEnemys) Destroy(enemy.gameObject);
        fieldEnemys.Clear();
        // Show player
        player.gameObject.SetActive(true);
        // Hide game over object
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        // Start to create enemy
        isEnemySpawn = true;
        StartCoroutine(EnemyCreateLoop());
        currentBossCount = 0;
    }
    private IEnumerator EnemyCreateLoop()
    {
        while (isEnemySpawn)
        {
            yield return new WaitForSeconds(2f);
            if (fieldEnemys.Count < 10) CreateEnemy();
            // Stop creating enemy if enemies more than or equal to 10 are beaten
            if (currentBossCount > 10)
            {
                isEnemySpawn = false;
                break;
            }
        }
    }
    /// <summary>
    /// Creating Boss
    /// </summary>
    private void CreateBoss()
    {
        Debug.Log("Boss appears!!");
    }
    /// <summary>
    /// Called by player when game over
    /// </summary>
    private void OnGameOver()
    {
        // Show game over
        gameOver.SetActive(true);
        // Hide player
        player.gameObject.SetActive(false);
        // Disable enemy attack flags
        // foreach (var enemy in enemies) enemy.IsBattle = false;
        foreach (var enemy in fieldEnemys) enemy.IsBattle = false;
    }
    /// <summary>
    /// Add event listeners
    /// </summary>
    private void AddEventListeners()
    {
        player.GameOverEvent.AddListener(OnGameOver);
        // foreach (var enemy in enemies) enemy.ArrivalEvent.AddListener(EnemyMove);
    }
    /// <summary>
    /// Get target randomly from list
    /// </summary>
    /// <returns></returns>
    private Transform GetEnemyMoveTarget()
    {
        if (enemyTargets == null || enemyTargets.Count == 0) return null;
        else if (enemyTargets.Count == 1) return enemyTargets[0];

        var num = Random.Range(0, enemyTargets.Count);
        return enemyTargets[num];
    }
    /// <summary>
    /// Set the next destination to enemy
    /// </summary>
    /// <param name="enemy"></param>
    private void EnemyMove(EnemyBase enemy)
    {
        var target = GetEnemyMoveTarget();
        if (target != null) enemy.SetNextTarget(target);
    }
    /// <summary>
    /// Create enemy
    /// </summary>
    /// <param name="count"></param>
    private void CreateEnemy(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            var num = Random.Range(0, enemyPrefabList.Count);
            var prefab = enemyPrefabList[num];

            var posNum = Random.Range(0, enemyGateList.Count);
            var pos = enemyGateList[posNum];

            var obj = Instantiate(prefab, pos.position, Quaternion.identity);
            var enemy = obj.GetComponent<EnemyBase>();

            enemy.ArrivalEvent.AddListener(EnemyMove);
            enemy.DestroyEvent.AddListener(EnemyDestroy);

            fieldEnemys.Add(enemy);
        }
    }
    /// <summary>
    /// Event at the time of enemy destroy
    /// </summary>
    /// <param name="enemy"></param>
    private void EnemyDestroy(EnemyBase enemy)
    {
        if (fieldEnemys.Contains(enemy)) fieldEnemys.Remove(enemy);
        Destroy(enemy.gameObject);
        currentBossCount++;
        if (currentBossCount > 10) CreateBoss();
    }
}
