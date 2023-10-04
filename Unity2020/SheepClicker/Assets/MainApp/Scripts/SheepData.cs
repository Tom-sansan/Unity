using UnityEngine;

/// <summary>
/// SheepData Class
/// </summary>
[CreateAssetMenu]
public class SheepData : ScriptableObject
{
    /// <summary>
    /// Sheep color
    /// </summary>
    public Color color;
    /// <summary>
    /// Initial price
    /// </summary>
    public int basePrice;
    /// <summary>
    /// Price increase
    /// </summary>
    public int extendPrice;
    /// <summary>
    /// Maximum number of purchases
    /// </summary>
    public int maxCount;
    /// <summary>
    /// Wool amount
    /// </summary>
    public int woolAmount;
}
