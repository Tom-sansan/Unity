using UnityEngine;

/// <summary>
/// HomeScene Class
/// </summary>
public class HomeScene : SceneBase
{
    /// <summary>
    /// Json file name
    /// </summary>
    [SerializeField]
    private string jsonFileName;

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        LoadDataFromJson();
    }
    protected override void Start()
    {
        base.Start();
    }

    #endregion Unity Methods

    #region Private Methods

    /// <summary>
    /// Load data from json file
    /// </summary>
    private void LoadDataFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFileName);
        SingletonData.Instance.LoadData(jsonFile.text);
        Debug.Log(jsonFile.text);
    }
    #endregion Private Methods

    #endregion Methods
}
