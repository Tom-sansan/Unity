using System.Numerics;

/// <summary>
/// SaveData Interface
/// </summary>
public interface ISaveData
{
    /// <summary>
    /// Save money
    /// </summary>
    /// <param name="money">Money</param>
    void SaveMoney(BigInteger money);
    /// <summary>
    /// Save sheep
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="count">Count</param>
    void SaveSheep(int id, int count);
    /// <summary>
    /// Load money
    /// </summary>
    /// <returns></returns>
    BigInteger LoadMoney();
    /// <summary>
    /// Load the number of sheep
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    int LoadSheepCount(int id);
}
