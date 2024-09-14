using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using UnityEngine;
using S = SingletonData;

/// <summary>
/// BaseDao Class
/// </summary>
public abstract class BaseDao : MonoBehaviour
{
    /// <summary>
    /// Get connection
    /// </summary>
    /// <returns></returns>
    protected SQLiteConnection GetConnection() =>
        new SQLiteConnection(S.Instance.ConnectionString);
    /// <summary>
    /// Create DB if not exits
    /// </summary>
    protected void CreateDbIfNotExists(string dbName, string sql)
    {
        string dbPath = $"{Application.persistentDataPath}/{S.Instance.Data.DBDir}";
        string fullDbPath = $"{dbPath}/{dbName}";

        string DbPath = Application.persistentDataPath + dbName;
        // Check if db exists or not
        if (!File.Exists(fullDbPath))
        {
            // Check if directory db exists or not
            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
                Debug.Log(string.Format(S.Instance.Data.LogDBDirCreated, DbPath));
            }
            // Create DB file
            {
                using (File.Create(fullDbPath))
                {
                    Debug.Log(string.Format(S.Instance.Data.LogDBCreated, dbName));
                }
            }
        }
        // Set string of connectionString
        S.Instance.ConnectionString = $"Data Source={fullDbPath};Version=3;";
    }
    /// <summary>
    /// ExecuteNonQuery
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    protected int ExecuteNonQuery(string sql, List<SQLiteParameter> parameterList = null)
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                if (parameterList != null) command.Parameters.AddRange(parameterList.ToArray());
                return command.ExecuteNonQuery();
            }
        }
    }

    protected T ExecuteReader<T>(string sql, Func<SQLiteDataReader, T> readFunc, List<SQLiteParameter> parameterList = null) where T : class, new()
    {
        using (var connection = GetConnection())
        {
            connection.Open();
            using (var command = new SQLiteCommand(sql, connection))
            {
                if (parameterList != null) command.Parameters.AddRange(parameterList.ToArray());
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) return readFunc(reader);
                }
            }
        }
        return null;
    }
}
