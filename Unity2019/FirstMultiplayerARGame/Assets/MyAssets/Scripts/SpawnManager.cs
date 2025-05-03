using Photon.Pun;
using UnityEngine;

/// <summary>
/// SpawnManager Class
/// </summary>
public class SpawnManager : MonoBehaviourPunCallbacks
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Player prefab array
    /// </summary>
    [SerializeField]
    private GameObject[] playerPrefabs;
    /// <summary>
    /// Spawn positions
    /// </summary>
    [SerializeField]
    private Transform[] spawnPositions;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out var playerSelectionNumber))
            {
                Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length - 1)].position;
                var playerName = playerPrefabs[(int)playerSelectionNumber].name;
                PhotonNetwork.Instantiate(playerName, spawnPosition, Quaternion.identity);
                Debug.Log($"Player selection number: {playerSelectionNumber}, {playerName}");
            }
        }

    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }



    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
