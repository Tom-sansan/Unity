using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Game Manager Class
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Internal Class

    /// <summary>
    /// Player Info Class
    /// </summary>
    [Serializable]
    public class PlayerInfo
    {
        public string name;
        public int actor;
        public int kills;
        public int deaths;

        public PlayerInfo(string name, int actor, int kills, int deaths)
        {
            this.name = name;
            this.actor = actor;
            this.kills = kills;
            this.deaths = deaths;
        }
    }

    #endregion Internal Class

    #region Enum

    /// <summary>
    /// Event Codes
    /// </summary>
    public enum EventCodes : byte
    {
        NewPlayer,
        ListPlayers,
        UpdateState
    }
    /// <summary>
    /// Game State
    /// </summary>
    public enum GameState
    {
        Playing,
        Ending
    }

    #endregion Enum

    #region Variables

    #region Public Variables

    /// <summary>
    /// List for Player Info
    /// </summary>
    public List<PlayerInfo> playerList = new List<PlayerInfo>();
    /// <summary>
    /// Game State
    /// </summary>
    public GameState state;
    /// <summary>
    /// Number of kills to clear
    /// </summary>
    public int targetNumber = 3;
    /// <summary>
    /// Time with clear panel displayed
    /// </summary>
    public float waitAfterEnding = 5f;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// UIManager
    /// </summary>
    private UIManager uiManager;
    /// <summary>
    /// PlayerInformation List
    /// </summary>
    private List<PlayerInformation> playerInfoList = new List<PlayerInformation>();

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }
    private void Start()
    {
        // Load title scene
        if (!PhotonNetwork.IsConnected) SceneManager.LoadScene(0);
        else
        {
            GetNewPlayer(PhotonNetwork.NickName);
            state = GameState.Playing;
        }
    }

    private void Update()
    {
        // Call scoreboard with tab key detection
        if (Input.GetKeyDown(KeyCode.Tab))
            // Open scoreboard while updating
            ShowScoreboard();
        else if (Input.GetKeyUp(KeyCode.Tab))
            uiManager.ChangeScoreUI();
    }
    /// <summary>
    /// Called when player leaves the room
    /// </summary>
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// On Event Callback method
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        // Check if custom event(Original Events:less than 200) or not(photon event:more than or equal to 200)
        if (photonEvent.Code < 200)
        {
            EventCodes eventCode = (EventCodes)photonEvent.Code;
            // Set sent event data 
            // PlayerInfo data = (PlayerInfo)photonEvent.CustomData;
            object[] data = (object[])photonEvent.CustomData;
            switch (eventCode)
            {
                case EventCodes.NewPlayer:
                    SetNewPlayer(data);
                    break;
                case EventCodes.ListPlayers:
                    SetListPlayers(data);
                    break;
                case EventCodes.UpdateState:
                    SetScore(data);
                    break;
            }
        }
    }
    /// <summary>
    /// Called when the component is on
    /// </summary>
    public override void OnEnable() =>
        // Register GameManager as callback method
        PhotonNetwork.AddCallbackTarget(this);
    /// <summary>
    /// Called when the component is off
    /// </summary>
    public override void OnDisable() =>
        // Remove GameManager as callback method
        PhotonNetwork.RemoveCallbackTarget(this);
    /// <summary>
    /// New users send their information to the master via the network
    /// </summary>
    /// <param name="name"></param>
    public void GetNewPlayer(string name)
    {
        object[] info = new object[4];
        info[0] = name;
        info[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        info[2] = 0;    // kills
        info[3] = 0;    // deaths
        // PlayerInfo info = new PlayerInfo(name, PhotonNetwork.LocalPlayer.ActorNumber, 0, 0);
        // Raise event for new user
        PhotonNetwork.RaiseEvent
        (
            (byte)EventCodes.NewPlayer,
            info,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },    // Send info for Master Client only
            new SendOptions { Reliability = true }
        );
    }
    /// <summary>
    /// Stores information on new players sent to the list.
    /// Master only is called.
    /// </summary>
    public void SetNewPlayer(object[] data)
    // public void SetNewPlayer(PlayerInfo info)
    {
        var playerInfo = new PlayerInfo((string)data[0], (int)data[1], (int)data[2], (int)data[3]);
        // var playerInfo = new PlayerInfo(info.name, info.actor, info.kills, info.deaths);
        playerList.Add(playerInfo);
        // Send player info to all players in room
        GetListPlayers();
    }
    /// <summary>
    /// Methods to get the number of kills and deaths and trigger events
    /// </summary>
    /// <param name="actor">The number to manage user</param>
    /// <param name="state">0:kill, 1:death</param>
    /// <param name="amount"></param>
    public void GetScore(int actor, int state, int amount)
    {
        object[] package = new object[] { actor, state, amount };
        // Kill death event occurs
        PhotonNetwork.RaiseEvent
        (
            (byte)EventCodes.UpdateState,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Send the acquired player information to all players in the room
    /// </summary>
    private void GetListPlayers()
    {
        // Set user info
        object[] info = new object[playerList.Count + 1];
        // Set game state
        info[0] = state;
        for (int i = 0; i < playerList.Count; i++)
        {
            object[] temp = new object[4];
            // Set user info
            temp[0] = playerList[i].name;
            temp[1] = playerList[i].actor;
            temp[2] = playerList[i].kills;
            temp[3] = playerList[i].deaths;

            info[i + 1] = temp;
        }
        // Information sharing event occurs
        PhotonNetwork.RaiseEvent
        (
            (byte)EventCodes.ListPlayers,
            info,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true }
        );
    }
    /// <summary>
    /// Set new player information in the list
    /// </summary>
    private void SetListPlayers(object[] data)
    {
        // Initialize player list
        playerList.Clear();
        state = (GameState)data[0];
        for (int i = 1; i < data.Length; i++)
        {
            object[] info = (object[])data[i];
            var player = new PlayerInfo
                (
                    (string)info[0],
                    (int)info[1],
                    (int)info[2],
                    (int)info[3]
                );
            playerList.Add(player);
        }
        // Check game state
        CheckGameState();
    }

    /// <summary>
    /// Reflects received data in the list
    /// </summary>
    /// <param name="data"></param>
    private void SetScore(object[] data)
    {
        Debug.Log("SetScore is called...");
        int actor = (int)data[0];
        int state = (int)data[1];    // 0:kill, 1:death
        int amount = (int)data[2];
        Debug.Log($"SetScore is called...state:{state}, amount:{amount}");
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].actor == actor)
            {
                switch (state)
                {
                    case 0:
                        playerList[i].kills += amount;
                        break;
                    case 1:
                        playerList[i].deaths += amount;
                        break;
                }
                break;
            }
        }
        // Check to see if player have achieved the game's clear conditions
        CheckTargetScore();
    }
    /// <summary>
    /// Open scoreboard while updating
    /// </summary>
    private void ShowScoreboard()
    {
        // Open score
        uiManager.ChangeScoreUI();
        // Delete currently displayed score
        foreach (var info in playerInfoList) Destroy(info.gameObject);
        playerInfoList.Clear();
        // Loop for the number of users participating
        foreach (var player in playerList)
        {
            // Create score display UI
            var newPlayerDisplay = Instantiate(uiManager.info, uiManager.info.transform.parent);
            // Reflect player information in the UI
            newPlayerDisplay.SetPlayerDetails(player.name, player.kills, player.deaths);
            // Display
            newPlayerDisplay.gameObject.SetActive(true);
            // Add to list
            playerInfoList.Add(newPlayerDisplay);
        }
    }
    /// <summary>
    /// Check to see if player have achieved the game's clear conditions
    /// </summary>
    private void CheckTargetScore()
    {
        var clear = false;
        // Whether there is anyone who has achieved the clear condition
        foreach (var player in playerList)
        {
            // Check the number of kill
            if (player.kills >= targetNumber && targetNumber > 0)
            {
                clear = true;
                break;
            }
        }
        if (clear && PhotonNetwork.IsMasterClient && state != GameState.Ending)
        {
            // Change game state
            state = GameState.Ending;
            // Share game state
            GetListPlayers();
        }
    }
    /// <summary>
    /// Check game state
    /// </summary>
    private void CheckGameState()
    {
        if (state == GameState.Ending) EndGame();
    }
    /// <summary>
    /// End game
    /// </summary>
    private void EndGame()
    {
        // Delete network object
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.DestroyAll();
        // Display game end panel
        uiManager.OpenEndPanel();
        // Display score panel while updating
        ShowScoreboard();
        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        // Process after ending game
        Invoke(nameof(ProcessingAfterCompletion), waitAfterEnding);
    }
    /// <summary>
    /// Process after ending game
    /// </summary>
    private void ProcessingAfterCompletion()
    {
        // De-synchronization of scenes
        PhotonNetwork.AutomaticallySyncScene = false;
        // Leava room
        PhotonNetwork.LeaveRoom();
    }
    /// <summary>
    /// Delete player information from the list where player information is maintained
    /// </summary>
    /// <param name="actor"></param>
    public void RemovePlayerInfo(int actor)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].actor == actor)
            {
                playerList.RemoveAt(i);
                break;
            }
        }
        // Update player info after deletion and share it
        GetListPlayers();
    }

    #endregion Private Mathods

    #endregion Methods
}
