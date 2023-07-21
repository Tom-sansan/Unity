using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Room Class
/// </summary>
public class Room : MonoBehaviour
{
    #region Variables

    #region Public Variables
    /// <summary>
    /// Text for room name
    /// </summary>
    public Text buttonText;
    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Room info
    /// </summary>
    private RoomInfo info;
    #endregion

    #endregion Variables

    #region Methods

    #region Public Methods
    /// <summary>
    /// Register room info in the button
    /// </summary>
    public void RegisterRoomDetails(RoomInfo info)
    {
        // Store room info
        this.info = info;
        // UI
        buttonText.text = this.info.Name;
    }
    /// <summary>
    /// Take part in the room that the romm button manages
    /// </summary>
    public void OpenRoom() =>
        // Call JoinRoom method
        PhotonManager.instance.JoinRoom(info);
    #endregion Public Methods

    #endregion Methods
}
