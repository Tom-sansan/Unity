using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Variables
    #region SerializeField
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
    /// Game clear object
    /// </summary>
    [SerializeField]
    private GameObject gameClear = null;
    /// <summary>
    /// Boss prefab
    /// </summary>
    [SerializeField]
    private GameObject bossPrefab = null;
    /// <summary>
    /// Time display text on game clear screen
    /// </summary>
    [SerializeField]
    private Text gameClearTimeText = null;
    /// <summary>
    /// Text to display time on screen during normal operation
    /// </summary>
    [SerializeField]
    private Text timerText = null;
    #endregion
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
    /// <summary>
    /// Boss appearance flag
    /// </summary>
    private bool isBossAppeared = false;
    /// <summary>
    /// Current time
    /// </summary>
    private float currentTime = 0;
    /// <summary>
    /// Time measurement flag
    /// </summary>
    private bool isTimer = false;
    #endregion

    void Start()
    {
        AddEventListeners();
        gameOver.SetActive(false);
        // CreateEnemy(numberOfEnemy);
        Init();
    }

    void Update()
    {
        if (isTimer)
        {
            currentTime += Time.deltaTime;
            if (currentTime > 999f) timerText.text = "999.9";
            else timerText.text = currentTime.ToString("000.0");
        }
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
        // Hide game over
        gameOver.SetActive(false);
        // Hide game clear
        gameClear.SetActive(false);

        Init();
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
        isBossAppeared= false;
        currentTime = 0;
        isTimer = true;
        timerText.text = "0:00";
    }
    private IEnumerator EnemyCreateLoop()
    {
        while (isEnemySpawn)
        {
            yield return new WaitForSeconds(2f);
            if (fieldEnemys.Count <= 10) CreateEnemy();
            // Stop creating enemy if enemies more than 10 are beaten
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
        if (isBossAppeared) return;
        var posNum = Random.Range(0, enemyGateList.Count);
        var pos = enemyGateList[posNum];

        var obj = Instantiate(bossPrefab, pos.position, Quaternion.identity);
        var enemy = obj.GetComponent<EnemyBase>();

        enemy.ArrivalEvent.AddListener(EnemyMove);
        enemy.DestroyEvent.AddListener(EnemyDestroy);
        Debug.Log("Boss appears!!");
    }
    /// <summary>
    /// Called by player when game over
    /// </summary>
    private void OnGameOver()
    {
        isTimer = false;
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

        if (!enemy.IsBoss)
        {
            currentBossCount++;
            if (currentBossCount > 3) CreateBoss();
        }
        else
        {
            isTimer = false;
            if (currentTime > 999f) gameClearTimeText.text = "Time : 999.9";
            else gameClearTimeText.text = "Time : " + currentTime.ToString("000.0");
            // Show game clear
            gameClear.SetActive(true);

            isEnemySpawn = false;
            // Remove enemy on field and reset list
            foreach (var fieldEnemy in fieldEnemys) Destroy(fieldEnemy.gameObject);
            fieldEnemys.Clear();
            Debug.Log("Game Clear!!");
        }
    }
}
