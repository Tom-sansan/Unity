using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Photon Manager
/// </summary>
public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Enum
    /// <summary>
    /// Enum for Menu UI
    /// </summary>
    enum MenuUI
    {
        LoadingPanel,
        Buttons,
        CreateRoomPanel,
        RoomPanel,
        ErrorPanel,
        RoomListPanel,
        UserNameInputPanel,
        All
    }
    #endregion Enum

    #region Variables

    #region Public Variables
    /// <summary>
    /// PhotonManager instance
    /// </summary>
    public static PhotonManager instance;
    /// <summary>
    /// Loading panel
    /// </summary>
    public GameObject loadingPanel;
    /// <summary>
    /// Parent object of buttons
    /// </summary>
    public GameObject buttons;
    /// <summary>
    /// Create Room Panel
    /// </summary>
    public GameObject createRoomPanel;
    /// <summary>
    /// Room Panel
    /// </summary>
    public GameObject roomPanel;
    /// <summary>
    /// Error Panel
    /// </summary>
    public GameObject errorPanel;
    /// <summary>
    /// Room List
    /// </summary>
    public GameObject roomListPanel;
    /// <summary>
    /// User Name Input Panel
    /// </summary>
    public GameObject UserNameInputPanel;
    /// <summary>
    /// Parent Object for Room Button
    /// </summary>
    public GameObject roomButtonContent;
    /// <summary>
    /// Parent object of Name Text
    /// </summary>
    public GameObject playerNameContent;
    /// <summary>
    /// Start Button
    /// </summary>
    public GameObject startButton;
    /// <summary>
    /// Room Button
    /// </summary>
    public Room originalRoomButton;
    /// <summary>
    /// Input text for room name
    /// </summary>
    public Text enterRoomName;
    /// <summary>
    /// Loading text
    /// </summary>
    public Text loadingText;
    /// <summary>
    /// Room Name
    /// </summary>
    public Text roomName;
    /// <summary>
    /// Player Text
    /// </summary>
    public Text PlayerNameText;
    /// <summary>
    /// User Name Placeholder Text
    /// </summary>
    public Text placeholderText;
    /// <summary>
    /// User Name Input
    /// </summary>
    public InputField userNameInput;
    /// <summary>
    /// Error Text
    /// </summary>
    public Text errorText;
    /// <summary>
    /// Transition scene name
    /// </summary>
    public string levelToPlay;
    /// <summary>
    /// Maximum players in a room
    /// </summary>
    [Range(1, 8)]
    public byte maxPlayersInRoom = 8;
    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Dictionary for Room Info(Room Name : Info)
    /// </summary>
    private Dictionary<string, RoomInfo> roomsList = new Dictionary<string, RoomInfo>();
    /// <summary>
    /// List for Room Buttons
    /// </summary>
    private List<Room> allRoomButtons = new List<Room>();
    /// <summary>
    /// List for Players Name
    /// </summary>
    private List<Text> allPlayerNames = new List<Text>();
    /// <summary>
    /// Check if user name is input
    /// </summary>
    private bool hasUserName;
    /// <summary>
    /// playerName String
    /// </summary>
    private const string strPlayerName = "playerName";

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetActiveMenuUI(MenuUI.LoadingPanel);
        ConnectToNetwork();
    }
    #endregion Unity Methods

    #region Public Methods

    #region Override Methods
    /// <summary>
    /// Mathod called when connecting to the Master server(Inheritance: Callback)
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // Lobby connection
        PhotonNetwork.JoinLobby();
        // Update text
        UpdateLoadingText("Joining the Lobby...");
        // Load the same level with Master client
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    /// <summary>
    /// Function called when connecting to the lobby(Inheritance: Callback)
    /// </summary>
    public override void OnJoinedLobby()
    {
        DisplayLobbyMenu();
        // Initialaize dictionary
        roomsList.Clear();
        // Random name
        PhotonNetwork.NickName = Random.Range(0, 1000).ToString();
        // Update UI after checking if name is already input
        CheckUserNameInput();
    }
    /// <summary>
    /// Method called when joining a room
    /// </summary>
    public override void OnJoinedRoom()
    {
        SetActiveMenuUI(MenuUI.RoomPanel);
        // Set room name
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        // Get player info in room
        GetAllPlayer();
        // Check if player is room master
        SetStartButtonActive();
    }
    /// <summary>
    /// Method called when exiting a room(Inheritance: Callback)
    /// </summary>
    public override void OnLeftRoom()
    {
        // Display lobby UI
        DisplayLobbyMenu();
        // Initialize Dictionary
        roomsList.Clear();
    }
    /// <summary>
    /// Method called when room creation fails(Inheritance: Callback)
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Change UI display
        errorText.text = "Failed to create room...\r\n" + message;
        SetActiveMenuUI(MenuUI.ErrorPanel);
    }
    /// <summary>
    /// Method called when there is an update to the room list(Inheritance: Callback)
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Initialization for UI of room buttons
        RoomUIInitialization();
        // Add to dictionary
        UpdateRoomList(roomList);
    }
    /// <summary>
    /// Method called when a player enters a room(Inheritance: Callback)
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer) =>
        GeneratePlayerText(newPlayer);
    /// <summary>
    /// Method called when a player leaves the room or becomes inactive(Inheritance: Callback)
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer) =>
        GetAllPlayer();
    /// <summary>
    /// Method called when the master is switched
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient) =>
        SetStartButtonActive();
    #endregion Override Methods

    /// <summary>
    /// Display Lobby UI
    /// </summary>
    public void DisplayLobbyMenu() =>
        SetActiveMenuUI(MenuUI.Buttons);
    /// <summary>
    /// Open create room panel
    /// </summary>
    public void OpenCreateRoomPanel() =>
        SetActiveMenuUI(MenuUI.CreateRoomPanel);
    /// <summary>
    /// Method for Create Room button
    /// </summary>
    public void CreateRoomButton()
    {
        if (string.IsNullOrEmpty(enterRoomName.text)) return;

        var options = new RoomOptions();
        options.MaxPlayers = maxPlayersInRoom;
        // Create room
        PhotonNetwork.CreateRoom(enterRoomName.text, options);
        SetActiveMenuUI(MenuUI.LoadingPanel);
        UpdateLoadingText("Creating Your Room...");
    }
    /// <summary>
    /// Leave room
    /// </summary>
    public void LeaveRoom()
    {
        // Leave room
        PhotonNetwork.LeaveRoom();
        // Update UI
        UpdateLoadingText("Leaving...");
        SetActiveMenuUI(MenuUI.LoadingPanel);
    }
    /// <summary>
    /// Open room list
    /// </summary>
    public void FindRoom() =>
        SetActiveMenuUI(MenuUI.RoomListPanel);
    /// <summary>
    /// Join room
    /// </summary>
    /// <param name="roomInfo"></param>
    public void JoinRoom(RoomInfo roomInfo)
    {
        // Join room
        PhotonNetwork.JoinRoom(roomInfo.Name);
        // UI setting
        UpdateLoadingText("Joining room...");
        SetActiveMenuUI(MenuUI.LoadingPanel);
    }
    /// <summary>
    /// Set user name
    /// </summary>
    public void SetUserName()
    {
        var userName = userNameInput.text;
        // Checks if text is entered in the input field
        if (string.IsNullOrEmpty(userName)) return;
        
        // Register user name
        PhotonNetwork.NickName = userName;
        // Save
        PlayerPrefs.SetString(strPlayerName, userName);
        // Display Lobby menu UI
        DisplayLobbyMenu();
        hasUserName = true;
    }
    /// <summary>
    /// Play game after transition
    /// </summary>
    public void PlayGame() =>
        PhotonNetwork.LoadLevel(levelToPlay);
    /// <summary>
    /// Quit game
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion Public Methods

    #region Private Mathods
    private void ConnectToNetwork()
    {
        // Update Text
        UpdateLoadingText("Connecting to the network...");
        // Connect to the network based on the settings of PhotonServerSettings
        if (!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
    }
    /// <summary>
    /// Add room info to the Dictionary
    /// </summary>
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        RoomInfo info;
        for (int i = 0; i < roomList.Count; i++)
        {
            info = roomList[i];
            // RemovedFromList: If room is full or closed, true will be returned
            if (info.RemovedFromList) roomsList.Remove(info.Name);
            else roomsList[info.Name] = info;   // room is available
        }
        // Display room buttons
        DisplayRoomList(roomsList);
    }
    /// <summary>
    /// Create and show room buttons
    /// </summary>
    /// <param name="cachedRoomList"></param>
    private void DisplayRoomList(Dictionary<string, RoomInfo> cachedRoomList)
    {
        Room newButton;
        foreach (var roomInfo in cachedRoomList)
        {
            // Create a button
            newButton = Instantiate(originalRoomButton);
            // Set room info to the created button
            newButton.RegisterRoomDetails(roomInfo.Value);
            // Set parent to the room button
            newButton.transform.SetParent(roomButtonContent.transform);
            // Add the created button to the room button list
            allRoomButtons.Add(newButton);
        }
    }
    /// <summary>
    /// Initialization for UI of room buttons
    /// </summary>
    private void RoomUIInitialization()
    {
        // Delete
        foreach (var rm in allRoomButtons) Destroy(rm.gameObject);
        // Initialization for List
        allRoomButtons.Clear();

    }
    /// <summary>
    /// Initialize Player List
    /// </summary>
    private void InitializePlayerList()
    {
        foreach (var rm in allPlayerNames) Destroy(rm.gameObject);
        allPlayerNames.Clear();
    }
    /// <summary>
    /// Display Player
    /// </summary>
    private void DisplayPlayer()
    {
        // Create each player UI who are taking part in room
        foreach (var players in PhotonNetwork.PlayerList)
        {
            // Create UI
            GeneratePlayerText(players);
        }
    }
    /// <summary>
    /// Generate player text
    /// </summary>
    /// <param name="players"></param>
    private void GeneratePlayerText(Player players)
    {
        // Create UI
        Text newPlayerText = Instantiate(PlayerNameText);
        // Set name to Text
        newPlayerText.text = players.NickName;
        // Set parent object
        newPlayerText.transform.SetParent(playerNameContent.transform);
        // Add to list
        allPlayerNames.Add(newPlayerText);
    }
    /// <summary>
    /// Get players info in room
    /// </summary>
    private void GetAllPlayer()
    {
        // Initialize name text UI
        InitializePlayerList();
        // Display player
        DisplayPlayer();
    }
    /// <summary>
    /// Update UI after checking if name is already input
    /// </summary>
    private void CheckUserNameInput()
    {
        var playerName = PlayerPrefs.GetString(strPlayerName);
        if (!hasUserName)
        {
            SetActiveMenuUI(MenuUI.UserNameInputPanel);
            if (PlayerPrefs.HasKey(strPlayerName))
            {
                placeholderText.text =
                userNameInput.text = playerName;
            }
        }
        else PhotonNetwork.NickName = playerName;
    }
    /// <summary>
    /// Set Start Button active if player is room master
    /// </summary>
    private void SetStartButtonActive() =>
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    /// <summary>
    /// Update loading text
    /// </summary>
    /// <param name="text"></param>
    private void UpdateLoadingText(string text) =>
        loadingText.text = text;
    /// <summary>
    /// Set active Menu UI
    /// </summary>
    /// <param name="menuUI"></param>
    private void SetActiveMenuUI(MenuUI menuUI)
    {
        var isLoadingPanelActive = false;
        var isButtonsActive = false;
        var isCreateRoomPanelActive = false;
        var isRoomPanelActive = false;
        var isErrorPanelActive = false;
        var isRoomListPanelActive = false;
        var isUserNameInputPanel = false;
        switch (menuUI)
        {
            case MenuUI.LoadingPanel:
                isLoadingPanelActive = true;
                break;
            case MenuUI.Buttons:
                isButtonsActive = true;
                break;
            case MenuUI.CreateRoomPanel:
                isCreateRoomPanelActive = true;
                break;
            case MenuUI.RoomPanel:
                isRoomPanelActive = true;
                break;
            case MenuUI.ErrorPanel:
                isErrorPanelActive = true;
                break;
            case MenuUI.RoomListPanel:
                isRoomListPanelActive = true;
                break;
            case MenuUI.UserNameInputPanel:
                isUserNameInputPanel = true;
                break;
            case MenuUI.All:
            default:
                break;
        }
        loadingPanel.SetActive(isLoadingPanelActive);
        buttons.SetActive(isButtonsActive);
        createRoomPanel.SetActive(isCreateRoomPanelActive);
        roomPanel.SetActive(isRoomPanelActive);
        errorPanel.SetActive(isErrorPanelActive);
        roomListPanel.SetActive(isRoomListPanelActive);
        UserNameInputPanel.SetActive(isUserNameInputPanel);
    }

#endregion Private Mathods

#endregion Methods
}
