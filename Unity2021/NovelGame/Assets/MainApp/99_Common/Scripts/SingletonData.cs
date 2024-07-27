using UnityEngine;

/// <summary>
/// SingletonData Class
/// </summary>
public class SingletonData
{
    #region Nested Class

    /// <summary>
    /// 
    /// </summary>
    public class JsonData
    {
        public string SaveDataJson;
        public string Link;
        public string SheetId;
        public string S01_Home;
        public string S02_Game;
        public string TalkData001;
        public string TalkData002_0;
        public string TalkData002_1;
        public string TalkData002_2;
    }

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static SingletonData Instance => instance ??= new SingletonData();

    public JsonData Data { get; private set; }


    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Field data of SingletonData
    /// </summary>
    private static SingletonData instance;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Load data from json file
    /// </summary>
    /// <param name="json"></param>
    public void LoadData(string json) =>
        Data = JsonUtility.FromJson<JsonData>(json);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
