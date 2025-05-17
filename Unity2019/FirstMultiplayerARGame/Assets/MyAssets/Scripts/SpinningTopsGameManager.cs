using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
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
    /// <summary>
    /// UIinformPanelGameObject
    /// </summary>
    [Header("UI")]
    [SerializeField]
    private GameObject UIInformPanelGameObject;
    /// <summary>
    /// UIinformText
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI UIInformText;
    /// <summary>
    /// SearchForGamesButtonGameObject
    /// </summary>
    [SerializeField]
    private GameObject searchForGamesButtonGameObject;
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
        UIInformText.text = "Searchning for available rooms...";
        PhotonNetwork.JoinRandomRoom();
        searchForGamesButtonGameObject.SetActive(false);
    }
    /// <summary>
    /// PHOTON callback methods
    /// Called when user joins a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        // base.OnJoinedRoom();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            UIInformText.text = $"Joined to {PhotonNetwork.CurrentRoom.Name}. Waiting for other players...";
        else
        {
            UIInformText.text = $"Joined to {PhotonNetwork.CurrentRoom.Name}";
            StartCoroutine(DeactivateAfterSeconds(UIInformPanelGameObject, 2.0f));
        }
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
        UIInformText.text = message;
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
        string txt = $"{newPlayer.NickName} joined to {PhotonNetwork.CurrentRoom.Name}\nPlayer count {PhotonNetwork.CurrentRoom.PlayerCount}";
        UIInformText.text = txt;
        Debug.Log(txt);
        StartCoroutine(DeactivateAfterSeconds(UIInformPanelGameObject, 2.0f));
    }
    /// <summary>
    /// PHOTON callback methods
    /// Called when a user leaves a room
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    /// <summary>
    /// OnQuitMatchButtonClicked
    /// </summary>
    public void OnQuitMatchButtonClicked()
    {
        // Leave the room
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
        // Load lobby scene
        else SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    /// <summary>
    /// OnBackButtonClicked
    /// </summary>
    public void OnBackButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        UIInformPanelGameObject.SetActive(true);
    }
    /// <summary>
    /// Create and join room
    /// </summary>
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
    /// <summary>
    /// Deactivate after seconds
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="seconds"></param>
    /// <returns></returns>
    private IEnumerator DeactivateAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
