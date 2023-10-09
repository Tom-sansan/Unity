using System.Numerics;
using UnityEngine;

/// <summary>
/// PlayerPrefsSaveData Class
/// </summary>
public class PlayerPrefsSaveData : ISaveData
{
    #region Valiables

    #region Private Valiables
    /// <summary>
    /// String MONEY
    /// </summary>
    private const string strMONEY = "MONEY";
    /// <summary>
    /// String SHEEP
    /// </summary>
    private const string strSHEEP = "SHEEP";
    #endregion Private Valiables

    #endregion Valiables

    #region Methods
    /// <summary>
    /// Load money
    /// </summary>
    /// <returns></returns>
    public BigInteger LoadMoney() =>
        BigInteger.Parse(PlayerPrefs.GetString(strMONEY, "0"));
    /// <summary>
    /// Load Sheep Count
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    public int LoadSheepCount(int id) =>
        PlayerPrefs.GetInt($"{strSHEEP}{id}", 0);
    /// <summary>
    /// Save money
    /// </summary>
    /// <param name="money">Money</param>
    public void SaveMoney(BigInteger money) =>
        PlayerPrefs.SetString(strMONEY, money.ToString());
    /// <summary>
    /// Save Sheep
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="count">Count</param>
    public void SaveSheep(int id, int count) =>
        PlayerPrefs.SetInt($"{strSHEEP}{id}", count);
    #endregion Methods
}
