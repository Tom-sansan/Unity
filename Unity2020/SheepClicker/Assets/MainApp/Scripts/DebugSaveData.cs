using System.Numerics;

/// <summary>
/// DebugSaveData Class
/// </summary>
public class DebugSaveData : ISaveData
{
    public BigInteger LoadMoney()
    {
        // for debug
        return 100000000000;
    }

    public int LoadSheepCount(int id)
    {
        // for debug
        return 5;
    }

    public void SaveMoney(BigInteger money)
    {

    }

    public void SaveSheep(int id, int count)
    {

    }
}
