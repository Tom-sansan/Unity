using UnityEngine;

/// <summary>
/// ResourcesData Class
/// </summary>
public static class ResourcesData
{
    /// <summary>
    /// Get Resources data
    /// </summary>
    /// <typeparam name="T">Type of T</typeparam>
    /// <param name="resouecesName">Resoueces name</param>
    /// <returns>object</returns>
    public static T GetResourcesData<T>(string resouecesName) where T : Object =>
        Resources.Load<T>(resouecesName);
}
