using UnityEngine;

/// <summary>
/// StartupConfig Class
/// </summary>
[CreateAssetMenu(fileName = "StartupConfig", menuName = "Scriptable Objects/StartupConfig")]
public class StartupConfig : ScriptableObject
{
    /// <summary>
    /// Json file name
    /// </summary>
    public string jsonFileName;
}
