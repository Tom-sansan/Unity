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
    #endregion

    void Start()
    {
        AddEventListeners();
        gameOver.SetActive(false);
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
        foreach (var enemy in enemies) enemy.OnRetry();
        // Show player
        player.gameObject.SetActive(true);
        // Hide game over object
        gameObject.SetActive(false);
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
        foreach (var enemy in enemies) enemy.IsBattle = false;
    }
    /// <summary>
    /// Add event listeners
    /// </summary>
    private void AddEventListeners()
    {
        player.GameOverEvent.AddListener(OnGameOver);
        foreach (var enemy in enemies) enemy.ArrivalEvent.AddListener(EnemyMove);
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
}
