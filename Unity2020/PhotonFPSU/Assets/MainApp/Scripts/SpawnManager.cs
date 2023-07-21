using Photon.Pun;
using UnityEngine;

/// <summary>
/// SpawnManager Class
/// </summary>
public class SpawnManager : MonoBehaviour
{
    #region Variables

    #region Public Variables

    /// <summary>
    /// Spawn Points List
    /// </summary>
    public Transform[] spawnPoints;
    /// <summary>
    /// Player Object
    /// </summary>
    public GameObject playerPrefab;
    /// <summary>
    /// Interval time to spawn
    /// </summary>
    public float respawnInterval = 5f;

    #endregion

    #region Private Variables
    /// <summary>
    /// Player Object for private
    /// </summary>
    private GameObject player;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void Start()
    {
        // Hide all spawn objects
        HideAllSpawnPoints();
        SpawnPlayer();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// randomly select one of the spawn points
    /// </summary>
    public Transform GetSpawnPoint() =>
        spawnPoints[Random.Range(0, spawnPoints.Length)];

    /// <summary>
    /// Delete and re-spawn
    /// </summary>
    public void Die()
    {
        // Invoke SpawnPlayer()
        if (player != null) Invoke(nameof(SpawnPlayer), 5f);
        PhotonNetwork.Destroy(player);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Hide all spawn objects
    /// </summary>
    private void HideAllSpawnPoints()
    {
        foreach (Transform position in spawnPoints) position.gameObject.SetActive(false);
    }
    /// <summary>
    /// Spawn player as network object
    /// </summary>
    private void SpawnPlayer()
    {
        if (!PhotonNetwork.IsConnected) return;
        // Set random spawn positon
        Transform spawnPoint = GetSpawnPoint();
        // Create network object
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
    /// <summary>
    /// Check if Photon network is connected or not
    /// </summary>
    /// <returns></returns>
    private bool IsPhotonNetworkConnected() =>
        PhotonNetwork.IsConnected;
    #endregion

    #endregion

}
