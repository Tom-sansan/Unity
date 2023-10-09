using System.Numerics;
using UnityEngine;

/// <summary>
/// SaveLoadManager Class
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    #region Valiables

    #region SerializeField
    /// <summary>
    /// Object to be preserved For possession
    /// </summary>
    [SerializeField]
    private Wallet wallet;
    /// <summary>
    /// Object to be preserved For the number of sheep
    /// </summary>
    [SerializeField]
    private Shop shop;
    #endregion SerializeField

    #region Private Valiables
    /// <summary>
    /// Save load interface
    /// </summary>
    private ISaveData saveData;
    /// <summary>
    /// MONEY string
    /// </summary>
    private const string strMONEY = "MONEY";
    /// <summary>
    /// SHEEP string
    /// </summary>
    private const string strSHEEP = "SHEEP";
    #endregion Private Valiables

    #endregion Valiables

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        saveData = new PlayerPrefsSaveData();
        // saveData = new DebugSaveData();
    }
    private void Start()
    {
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Save
    /// </summary>
    public void Save()
    {
        Debug.Log("Save");
        // Save money
        // PlayerPrefs.SetString(strMONEY, wallet.money.ToString());
        saveData.SaveMoney(wallet.money);
        // Save all of shee[
        for (int index = 0; index < shop.sheepButtonList.Count; index++)
        {
            var sheepButton = shop.sheepButtonList[index];
            // PlayerPrefs.SetInt($"{strSHEEP}{index}", sheepButton.currentCount);
            saveData.SaveSheep(index, sheepButton.currentCount);
        }
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Load
    /// </summary>
    private void Load()
    {
        Debug.Log("Load");
        // Load money
        // wallet.money = BigInteger.Parse(PlayerPrefs.GetString(strMONEY, "0"));
        wallet.money = saveData.LoadMoney();
        // Load all of sheep
        for (int index = 0; index < shop.sheepButtonList.Count; index++)
        {
            var sheepButton = shop.sheepButtonList[index];
            // var sheepCount = PlayerPrefs.GetInt($"{strSHEEP}{index}", 0);
            var sheepCount = saveData.LoadSheepCount(index);
            sheepButton.currentCount = sheepCount;
            for (int i = 0; i < sheepCount; i++) sheepButton.sheepGenerator.CreateSheep(sheepButton.sheepData);
        }
    }
    #endregion Private Methods

    #endregion Methods
}
