using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// LobbyManager Class
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    [Header("Login UI")]
    [SerializeField]
    private InputField playerNameInputField;
    /// <summary>
    /// UILoginGameObject
    /// </summary>
    [SerializeField]
    private GameObject uI_LoginGameObject;
    /// <summary>
    /// UILobbyGameObject
    /// </summary>
    [Header("Lobby UI")]
    [SerializeField]
    private GameObject uI_LobbyGameObject;
    /// <summary>
    /// UI3DGameObject
    /// </summary>
    [SerializeField]
    private GameObject uI_3DGameObject;
    /// <summary>
    /// UIConnectionStatusGameObject
    /// </summary>
    [Header("Connection Status UI")]
    [SerializeField]
    private GameObject uI_ConnectionStatusGameObject;
    /// <summary>
    /// ConnectionStatusText
    /// </summary>
    [SerializeField]
    private Text connectionStatusText;
    /// <summary>
    /// Flag for showing if ConnectionStatusText is shown
    /// </summary>
    [SerializeField]
    private bool isShowConnectionStatus = false;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

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
        UpdateConnectionStatus();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// OnEnterGameButtonClicked
    /// </summary>
    public void OnEnterGameButtonClicked()
    {
        var playerName = playerNameInputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            uI_ConnectionStatusGameObject.SetActive(true);
            uI_LoginGameObject.SetActive(false);
            isShowConnectionStatus = true;
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or empty.");
        }
    }
    /// <summary>
    /// OnConnected
    /// </summary>
    public override void OnConnected()
    {
        Debug.Log("Connected to Photon Server.");
    }
    /// <summary>
    /// OnConnectedToMaster
    /// </summary>
    public override void OnConnectedToMaster()
    {
        uI_LobbyGameObject.SetActive(true);
        uI_3DGameObject.SetActive(true);
        uI_ConnectionStatusGameObject.SetActive(false);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon Server.");
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        uI_LobbyGameObject.SetActive(false);
        uI_3DGameObject.SetActive(false);
        uI_ConnectionStatusGameObject.SetActive(false);
        uI_LoginGameObject.SetActive(true);
    }
    /// <summary>
    /// Update connectionStatus Text
    /// </summary>
    private void UpdateConnectionStatus()
    {
        if (isShowConnectionStatus)
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
