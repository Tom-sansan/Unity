using UnityEngine;

/// <summary>
/// SingletonData class
/// </summary>
public class SingletonData
{
    #region Nested Class
    /// <summary>
    /// JsonData class
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// DB directory
        /// </summary>
        public string DBDir;
        /// <summary>
        /// Service DB
        /// </summary>
        public string ServiceDb;
        /// <summary>
        /// Check if the specified table exists or not
        /// </summary>
        public string CheckTableExistence;
        /// <summary>
        /// SQL for 'CreateUserProfileTable'
        /// </summary>
        public string CreateUserProfileTable;
        /// <summary>
        /// SQL for 'SelectUserProfile'
        /// </summary>
        public string SelectUserProfile;
        /// <summary>
        /// SQL for 'InsertUserProfile'
        /// </summary>
        public string InsertUserProfile;
        /// <summary>
        /// SQL for 'UpdateUserProfile'
        /// </summary>
        public string UpdateUserProfile;
        /// <summary>
        /// URL for HttpGetUserProfile
        /// </summary>
        public string HttpGetUserProfile;
        /// <summary>
        /// Log for DB dir created
        /// </summary>
        public string LogDBDirCreated;
        /// <summary>
        /// Log for DB created
        /// </summary>
        public string LogDBCreated;
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
    /// <summary>
    /// JsonData instance
    /// </summary>
    public JsonData Data { get; private set; }

    public string ConnectionString { get; set; }

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Field data of SingletonData
    /// </summary>
    private static SingletonData instance;

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
    /// Load data from json file
    /// </summary>
    /// <param name="json"></param>
    public void LoadData(string jsonFileName) =>
        Data = JsonUtility.FromJson<JsonData>(jsonFileName);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
