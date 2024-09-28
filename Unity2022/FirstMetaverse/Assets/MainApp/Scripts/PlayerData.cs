using UnityEngine;

public class PlayerData
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #region Properties

    /// <summary>
    /// Nick name property
    /// </summary>
    public static string NickName
    {
        // Get Player name
        get => PlayerPrefs.GetString("NickName", "No Name");
        // Set Player name
        set => PlayerPrefs.SetString("NickName", value);

    }
    #endregion Properties

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
