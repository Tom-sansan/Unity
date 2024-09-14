using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using S = SingletonData;

/// <summary>
/// CommunicationManager Class
/// </summary>
public class CommunicationManager : BaseDao
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

    private UserProfileController _userProfileController;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        _userProfileController = new UserProfileController();
    }

    void Start()
    {
        StartCoroutine(ConnectServer(string.Format(S.Instance.Data.HttpGetUserProfile, "1")));
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    public IEnumerator ConnectServer(string url)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        yield return unityWebRequest.SendWebRequest();
        // Error handling
        if (!string.IsNullOrEmpty(unityWebRequest.error)) Debug.LogError(unityWebRequest.error);
        // Get response
        string text = unityWebRequest.downloadHandler.text;
        Debug.Log(text);
        // Convert json to UserProfile property
        var userProfile = JsonUtility.FromJson<UserProfile>(text);
        Debug.Log(userProfile);
        // Save UserProfile to SQLite
        if (!string.IsNullOrEmpty(userProfile.userId))
            // _userProfileController.AddUserProfileData(userProfile);

        if (!string.IsNullOrEmpty(userProfile.userId))
            _userProfileController.UpdateUserProfileData(userProfile);

    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
