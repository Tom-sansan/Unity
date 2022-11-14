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
    #endregion

    void Start()
    {
        player.GameOverEvent.AddListener(OnGameOver);
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
}
