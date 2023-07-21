using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player Information Class
/// </summary>
public class PlayerInformation : MonoBehaviour
{
    #region Variables

    #region Public Variables
    /// <summary>
    /// Player Name Text
    /// </summary>
    public Text playerNameText;
    /// <summary>
    /// Kill Text
    /// </summary>
    public Text killsText;
    /// <summary>
    /// Death Text
    /// </summary>
    public Text deathText;
    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Display name, the number of kill and death in the UI list
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="kill"></param>
    /// <param name="death"></param>
    public void SetPlayerDetails(string playerName, int kill, int death)
    {
        playerNameText.text = playerName;
        killsText.text = kill.ToString();
        deathText.text = death.ToString();

    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
