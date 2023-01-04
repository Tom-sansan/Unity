using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// App Game Controller Class
/// </summary>
public class AppGameController : MonoBehaviour
{
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
    /// Enemy ppearance Points
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
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
