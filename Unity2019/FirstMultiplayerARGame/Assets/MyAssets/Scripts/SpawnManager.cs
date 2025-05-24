using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// SpawnManager Class
/// </summary>
public class SpawnManager : MonoBehaviourPunCallbacks
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    public enum RaiseEventCodes
    {
        PlayerSpawnEventCode = 0
    }
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
    /// <summary>
    /// Battle arena gameobject
    /// </summary>
    [SerializeField]
    private GameObject battleArenaGameobject;
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
    /// <summary>
    /// OnJoinedRoom Event
    /// </summary>
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            //     if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out var playerSelectionNumber))
            //     {
            //         Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length - 1)].position;
            //         var playerName = playerPrefabs[(int)playerSelectionNumber].name;
            //         PhotonNetwork.Instantiate(playerName, spawnPosition, Quaternion.identity);
            //         Debug.Log($"Player selection number: {playerSelectionNumber}, {playerName}");
            //     }
            SpawnPlayer();
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
    /// <summary>
    /// Spawn player
    /// </summary>
    private void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, out var playerSelectionNumber))
        {
            Debug.Log($"Player selection number: {playerSelectionNumber}");
            Vector3 instantiatePosition = spawnPositions[Random.Range(0, spawnPositions.Length - 1)].position;
            GameObject playerGameObject = Instantiate(playerPrefabs[(int)playerSelectionNumber], instantiatePosition, Quaternion.identity);
            PhotonView photonView = playerGameObject.GetComponent<PhotonView>();
            if (PhotonNetwork.AllocateRoomViewID(photonView))
            {
                var data = new object[]
                {
                    playerGameObject.transform.position,
                    battleArenaGameobject.transform.position,
                    playerGameObject.transform.rotation,
                    photonView,
                    playerSelectionNumber
                };
                var raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };
                var sendOptions = new SendOptions
                {
                    Reliability = true
                };
                // Raise events
                PhotonNetwork.RaiseEvent
                (
                    (byte)RaiseEventCodes.PlayerSpawnEventCode,
                    data,
                    raiseEventOptions,
                    sendOptions
                );
            }
            else
            {
                Debug.Log("Failed to allocate a viewID");
                Destroy(playerGameObject);
            }
        }
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
