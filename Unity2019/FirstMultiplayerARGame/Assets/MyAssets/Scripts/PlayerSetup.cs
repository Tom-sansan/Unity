using Photon.Pun;
using TMPro;
using UnityEngine;

/// <summary>
/// PlayerSetup Class
/// </summary>
public class PlayerSetup : MonoBehaviourPun
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// PlayerNameText
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI playerNameText;

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
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // The player is local or remote
        bool isMine = photonView.IsMine;
        transform.GetComponent<MovementController>().enabled = isMine;
        transform.GetComponent<MovementController>().Joystick.gameObject.SetActive(isMine);
        SetPlayerName();
    }
    /// <summary>
    /// Set player name
    /// </summary>
    private void SetPlayerName()
    {
        if (string.IsNullOrEmpty(playerNameText?.text)) return;
        if (photonView.IsMine)
        {
            playerNameText.text = "You";
            playerNameText.color = Color.red;
        }
        else playerNameText.text = photonView.Owner.NickName;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
