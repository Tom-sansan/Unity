using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Save Data Class
/// </summary>
public class SaveData
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

    public int StoryNumber = 0;

    public string TestString = "Test..";

    #region Private Variables

    #endregion Private Variables

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

    /// <summary>
    /// Save data
    /// </summary>
    /// <param name="data"></param>
    public void Save(SaveData data)
    {
        var json = JsonUtility.ToJson(data);
        Debug.Log("SaveData\n" + json);
        // Write
        var folderPath = GetDirPath();
        var filePath = GetFilePath();
        Debug.Log(filePath);
        CheckDirAndFile(folderPath, filePath);
        using var sw = new StreamWriter(filePath, false, Encoding.UTF8);
        sw.Write(json);
    }
    /// <summary>
    /// Load save data
    /// </summary>
    /// <returns></returns>
    public SaveData Load()
    {
        var dirPath = GetDirPath();
        var filePath = GetFilePath();
        CheckDirAndFile(dirPath, filePath);
        // Load
        using var sr = new StreamReader(filePath, Encoding.UTF8);
        var result = sr.ReadToEnd();
        Debug.Log(result);
        return JsonUtility.FromJson<SaveData>(result);
    }
    /// <summary>
    /// Check directory and file paths.
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="filePath"></param>
    public void CheckDirAndFile(string dirPath, string filePath)
    {
        if (!Directory.Exists(dirPath))
        {
            // Create directory
            Directory.CreateDirectory(dirPath);
            Debug.Log("Creating Savedata Directory...");
        }
        CheckFile(filePath);
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// check if file is existed, and if no file then create new one.
    /// </summary>
    /// <param name="filePath"></param>
    private void CheckFile(string filePath)
    {
        if (File.Exists(filePath)) return;
        else
        {
            // Create a new file
            var saveData = new SaveData();
            var json = JsonUtility.ToJson(saveData);
            Debug.Log("Create new save data...");
            using var sw = new StreamWriter(filePath, false, Encoding.UTF8);
            sw.Write(json);
        }
    }
    /// <summary>
    /// Get directory path for save
    /// </summary>
    /// <returns></returns>
    private string GetDirPath() =>
        Application.persistentDataPath + "/savedata";
    /// <summary>
    /// Get file path for save
    /// </summary>
    /// <returns></returns>
    private string GetFilePath() =>
        GetDirPath() + "/savedata.json";

    #endregion Private Methods

    #endregion Methods
}
