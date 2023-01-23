using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static AppPlayerController;

/// <summary>
/// App Game Controller Class
/// </summary>
public class AppGameController : MonoBehaviour
{
    #region Game State
    /// <summary>
    /// Game state
    /// </summary>
    public enum GameState
    {
        Stop,
        Ready,
        Play,
        End
    }
    /// <summary>
    /// Parameters for game progress
    /// </summary>
    public class GameParam
    {
        /// <summary>
        /// Game state
        /// </summary>
        public GameState State = GameState.Stop;
        /// <summary>
        /// Preparation time (for countdown)
        /// </summary>
        public float ReadyTime = 0;
        /// <summary>
        /// Game time
        /// </summary>
        public float GameTime = 0;
        /// <summary>
        /// Number of enemy destroyed
        /// </summary>
        public int EnemyDestroyCount = 0;
    }
    #endregion

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
    /// Player
    /// </summary>
    [SerializeField]
    private AppPlayerController player = null;
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
    /// <summary>
    /// Countdown time
    /// </summary>
    [SerializeField]
    private float readyTime = 5f;
    /// <summary>
    /// Number of clear breakdowns
    /// </summary>
    [SerializeField]
    private int clearCount = 10;
    #endregion

    #region Public
    /// <summary>
    /// The current game paramter
    /// </summary>
    public GameParam CurrentGameParam = new GameParam();
    /// <summary>
    /// Clear event
    /// </summary>
    public UnityEvent ClearEvent = new UnityEvent();
    /// <summary>
    /// Event when an enemy is defeated
    /// </summary>
    public UnityEvent EnemyDeadEvent = new UnityEvent();
    #endregion

    #region Private
    /// <summary>
    /// Enemies currently on the field
    /// </summary>
    private List<EnemyBase> currentFieldEnemies = new List<EnemyBase>();
    /// <summary>
    /// Flag to regenerate reduced enemies
    /// </summary>
    private bool isEnemyRespawn = false;
    #endregion
    #endregion

    #region Method
    private void Start()
    {
        // InitSpawn();
        if (player != null) player.Init(this);
        GameReady();
    }
    private void Update()
    {
        UpdateCount();
        UpdateEnemyCount();
    }
    #region Public
    /// <summary>
    /// Reset parameters
    /// </summary>
    public void ResetGameParam()
    {
        CurrentGameParam.ReadyTime = 0;
        CurrentGameParam.GameTime = 0;
        CurrentGameParam.State = GameState.Stop;
        CurrentGameParam.EnemyDestroyCount = 0;
    }
    /// <summary>
    /// Retry
    /// </summary>
    public void Retry()
    {
        ResetGameParam();
        isEnemyRespawn = false;
        Invoke("GameReady", 2f);
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
        CurrentGameParam.EnemyDestroyCount++;
        if (CurrentGameParam.EnemyDestroyCount >= clearCount) GameClear();
        EnemyDeadEvent?.Invoke();
    }
    /// <summary>
    /// Game over
    /// </summary>
    public void GameOver() =>
        GameEnd("Game Over");
    #endregion

    #region Private
    #region Game State
    /// <summary>
    /// Transition to Ready state
    /// </summary>
    private void GameReady()
    {
        CurrentGameParam.State = GameState.Ready;
        CurrentGameParam.ReadyTime = readyTime;
    }
    /// <summary>
    /// Transition to Play state
    /// </summary>
    private void GameStart()
    {
        CurrentGameParam.State = GameState.Play;
        InitSpawn();
    }
    /// <summary>
    /// Transition to End state
    /// </summary>
    private void GameClear() =>
        GameEnd("Clear");
    #endregion
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
            if (CurrentGameParam.State != GameState.Play) break;
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
        if (CurrentGameParam.State != GameState.Play) return;
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
    /// <summary>
    /// Game time measurement with Update
    /// </summary>
    private void UpdateCount()
    {
        if (CurrentGameParam.State == GameState.Ready && CurrentGameParam.ReadyTime > 0)
        {
            CurrentGameParam.ReadyTime -= Time.deltaTime;
            if (CurrentGameParam.ReadyTime <= 0)
            {
                Debug.Log("Game Start!");
                GameStart();
            }
        }
        else if (CurrentGameParam.State == GameState.Play && CurrentGameParam.ReadyTime > -1)
        {
            CurrentGameParam.ReadyTime -= Time.deltaTime;
            CurrentGameParam.GameTime += Time.deltaTime;
            Debug.Log("Count Down: " + CurrentGameParam.ReadyTime);
        }
        else if (CurrentGameParam.State == GameState.Play)
        {
            CurrentGameParam.ReadyTime = -99999f;
            CurrentGameParam.GameTime += Time.deltaTime;
        }
    }
    /// <summary>
    /// Make game end
    /// </summary>
    /// <param name="gameState"></param>
    private void GameEnd(string gameState)
    {
        CurrentGameParam.State = GameState.End;
        if (currentFieldEnemies.Count > 0) foreach (var enemy in currentFieldEnemies) Destroy(enemy.gameObject);
        currentFieldEnemies.Clear();
        ClearEvent?.Invoke();
        Debug.Log($"{gameState}\n{CurrentGameParam.GameTime}");
    }
    #endregion
    #endregion
}
