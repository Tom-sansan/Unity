using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// PlayerSpawner Class
/// </summary>
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Player prefab
    /// </summary>
    [SerializeField]
    private GameObject playerPrefab;
    /// <summary>
    /// Spawned position of player
    /// </summary>
    [SerializeField]
    private Transform spawnPoint;



    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Method called when a player joins the game
    /// </summary>
    /// <param name="player"></param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void PlayerJoined(PlayerRef player)
    {
        // If the participating player is a local player
        if (player == Runner.LocalPlayer)
        {
            // Generate PlayerPrefab at the specified position with default rotation(Quaternion.identity:No rotation)
            Runner.Spawn(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
        throw new System.NotImplementedException();
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
