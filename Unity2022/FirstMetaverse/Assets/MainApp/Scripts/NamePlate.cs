using TMPro;
using UnityEngine;

/// <summary>
/// NamePlate Class
/// </summary>
public class NamePlate : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Name Text Componen
    /// </summary>
    [SerializeField]
    private TextMeshPro nameText;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    /// <summary>
    /// Set player name into text
    /// </summary>
    /// <param name="nickName"></param>
    public void SetNickName(string nickName) =>
        nameText.text = nickName;

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void LateUpdate()
    {
        SetNameTextRotation();
    }

    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    private void SetNameTextRotation()
    {
        // Always make the text of the player's name face the front of the camera
        // プレイヤー名のテキストを、常にカメラの正面向きにする
        nameText.transform.rotation = Camera.main.transform.rotation;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
