using System;
using System.Collections.Generic;
using System.Data.SQLite;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using S = SingletonData;

/// <summary>
/// CloudSocialGameService Class
/// </summary>
public class CloudSocialGameService : BaseDao
{
    public void CreateTableIfNotExists<T>() where T : class, new()
    {
        var tableName = string.Empty;
        var sql = string.Empty;
        switch (typeof(T).Name)
        {
            case nameof(UserProfile):
                tableName = nameof(UserProfile);
                sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.CreateUserProfileTable)?.text;
                break;
            default:
                break;
        }
        var result = CheckTableExistence<TableExistence>(tableName);
        if (!result)
        {
           base.ExecuteNonQuery(sql);
        }
    }
    /// <summary>
    /// Get data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetData<T>(string id = "") where T : class, new()
    {
        var sql = string.Empty;
        var parameterList = new List<SQLiteParameter>();
        Func<SQLiteDataReader, T> readFunc = null;
        switch (typeof(T).Name)
        {
            case nameof(UserProfile):
                sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.SelectUserProfile)?.text;
                if (!string.IsNullOrEmpty(id))
                {
                    sql = sql + " WHERE UserId = @UserId;";
                    parameterList.Add(new SQLiteParameter("@UserId", id));
                }
                readFunc = reader => new UserProfile
                {
                    userId = reader[nameof(UserProfile.userId)].ToString(),
                    userName = reader[nameof(UserProfile.userName)].ToString(),
                    crystal = int.Parse(reader[nameof(UserProfile.crystal)].ToString()),
                    crystalFree = int.Parse(reader[nameof(UserProfile.crystalFree)].ToString()),
                    friendCoin = int.Parse(reader[nameof(UserProfile.friendCoin)].ToString()),
                    tutorialProgress = int.Parse(reader[nameof(UserProfile.tutorialProgress)].ToString()),
                } as T;
                break;
        }
        return base.ExecuteReader(sql, readFunc, parameterList);
    }
    /// <summary>
    /// Add data to a table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterList"></param>
    /// <returns></returns>
    public int AddData<T>(List<SQLiteParameter> parameterList) where T : class, new()
    {
        var sql = string.Empty;
        switch (typeof(T).Name)
        {
            case nameof(UserProfile):
                sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.InsertUserProfile)?.text;
                break;
            default:
                sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.InsertUserProfile)?.text;
                break;
        }
        return base.ExecuteNonQuery(sql, parameterList);
    }
    /// <summary>
    /// Update a table data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parameterList"></param>
    /// <returns></returns>
    public int UpdateData<T>(string id, Dictionary<string, SQLiteParameter> dic) where T : class, new()
    {
        var sql = string.Empty;
        var columns = string.Empty;
        var parameterList = new List<SQLiteParameter>();
        switch (typeof(T).Name)
        {
            case nameof(UserProfile):
                sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.UpdateUserProfile)?.text;
                parameterList.Add(new SQLiteParameter("@userId", id));
                foreach (var list in dic)
                {
                    columns += list.Key + ",";
                    parameterList.Add(list.Value);
                }
                sql = string.Format(sql, columns.Substring(0, columns.Length - 1));
                break;
            default:
                sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.UpdateUserProfile)?.text;
                break;
        }
        return base.ExecuteNonQuery(sql, parameterList);
    }

    private bool CheckTableExistence<T>(string tableName) where T : class, new()
    {
        var sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.CheckTableExistence)?.text;
        Func<SQLiteDataReader, T> readFunc = reader => new T();
        var parameterList = new List<SQLiteParameter> { new SQLiteParameter("@TableName", tableName) };
        var result = base.ExecuteReader(sql, readFunc, parameterList);
        return result != null;
    }
}