using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SpinningTopsGameManager Class
/// </summary>
public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Max players
    /// </summary>
    [SerializeField]
    private int maxPlayers = 2;
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
    /// UI Callback method of JoinRandomRoom
    /// </summary>
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    /// <summary>
    /// PHOTON callback methods
    /// Called when user joins a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        // base.OnJoinedRoom();
        Debug.Log($"{PhotonNetwork.NickName} joined to {PhotonNetwork.CurrentRoom.Name}");
    }
    /// <summary>
    /// PHOTON callback methods
    /// Called when user can't join a random room
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // base.OnJoinRandomFailed(returnCode, message);
        Debug.Log(message);
        CreateAndJoinRoom();
    }
    /// <summary>
    /// PHOTON callback methods
    /// Called when a user enters a room
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"{newPlayer.NickName} entered to {PhotonNetwork.CurrentRoom.Name}/rPlayer count {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }

    private void CreateAndJoinRoom()
    {
        // Create a random room name
        string randomRoomName = $"Room{Random.Range(0, 1000)}";
        // Create room options
        var roomOptions = new RoomOptions()
        {
            MaxPlayers = maxPlayers,
        };
        // Create the room
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
