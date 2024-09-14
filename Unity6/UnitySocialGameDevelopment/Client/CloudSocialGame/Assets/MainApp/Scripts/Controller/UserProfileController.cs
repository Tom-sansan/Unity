using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UnityEngine;
using S = SingletonData;

public class UserProfileController : BaseDao
{
    private CloudSocialGameService _cloudSocialGameService;

    private void Awake()
    {
        try
        {
            base.CreateDbIfNotExists(S.Instance.Data.ServiceDb, S.Instance.Data.CreateUserProfileTable);
            _cloudSocialGameService = new CloudSocialGameService();
            _cloudSocialGameService.CreateTableIfNotExists<UserProfile>();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // _cloudSocialGameService = new CloudSocialGameService();
        // AddUserProfileData();
        GetUserProfileData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Get UserProfile data
    /// </summary>
    public void GetUserProfileData()
    {
        try
        {
            string userId = "1";
            var userProfile = _cloudSocialGameService.GetData<UserProfile>(userId);

            if (userProfile != null)
            {
                Debug.Log($"User ID: {userProfile.userId}");
                Debug.Log($"User Name: {userProfile.userName}");
                Debug.Log($"Crystal: {userProfile.crystal}");
                Debug.Log($"Crystal Free: {userProfile.crystalFree}");
                Debug.Log($"Friend Coin: {userProfile.friendCoin}");
                Debug.Log($"Tutorial Progress: {userProfile.tutorialProgress}");
            }
            else
            {
                Debug.Log("User profile not found.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    /// <summary>
    /// Add UserProfile data
    /// </summary>
    /// <param name="userProfile"></param>
    public void AddUserProfileData(UserProfile userProfile)
    {
        try
        {
            var parameterList = new List<SQLiteParameter>()
            {
                new SQLiteParameter($"@{nameof(UserProfile.userId)}", userProfile.userId),
                new SQLiteParameter($"@{nameof(UserProfile.userName)}", userProfile.userName),
                new SQLiteParameter($"@{nameof(UserProfile.crystal)}", userProfile.crystal),
                new SQLiteParameter($"@{nameof(UserProfile.crystalFree)}", userProfile.crystalFree),
                new SQLiteParameter($"@{nameof(UserProfile.friendCoin)}", userProfile.friendCoin),
                new SQLiteParameter($"@{nameof(UserProfile.tutorialProgress)}", userProfile.tutorialProgress)
            };
            if (_cloudSocialGameService == null) _cloudSocialGameService = new CloudSocialGameService();
            var result = _cloudSocialGameService.AddData<UserProfile>(parameterList);
            if (result > 0) Debug.Log("AddUserProfileData succeeded!");
            else Debug.Log("AddUserProfileData failed...");
        }
        catch (Exception e)
        {
            Debug.LogError (e);
        }
    }
    /// <summary>
    /// Update UserProfile data
    /// </summary>
    /// <param name="userProfile"></param>
    public void UpdateUserProfileData(UserProfile userProfile)
    {
        try
        {
            var dicParameterList = new Dictionary<string, SQLiteParameter>();
            if (!string.IsNullOrEmpty(userProfile.userName))
                CreateDicParameterList(dicParameterList, nameof(UserProfile.userName), userProfile.userName);
            if (userProfile.crystal != 0)
                CreateDicParameterList(dicParameterList, nameof(UserProfile.crystal), userProfile.crystal);
            if (_cloudSocialGameService == null) _cloudSocialGameService = new CloudSocialGameService();
            var result = _cloudSocialGameService.UpdateData<UserProfile>(userProfile.userId, dicParameterList);
            if (result > 0) Debug.Log("UpdateUserProfileData succeeded!");
            else Debug.Log("UpdateUserProfileData failed...");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        void CreateDicParameterList(Dictionary<string, SQLiteParameter> dic, string columnName, object data) =>
            dic.Add($"{columnName} = @{columnName}", new SQLiteParameter($"@{columnName}", data));
    }
}
