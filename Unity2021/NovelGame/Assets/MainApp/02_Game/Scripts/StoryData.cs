using System;
using UnityEngine;

/// <summary>
/// StoryData Class
/// </summary>
[Serializable]
public class StoryData
{
    #region Variables

    #region Public Variables

    /// <summary>
    /// Character Number
    /// </summary>
    public int CharacterNumber = 0;
    /// <summary>
    /// Conversation content
    /// </summary>
    [Multiline(3)]
    public string Talk = string.Empty;
    /// <summary>
    /// Place, background
    /// </summary>
    public string Place = string.Empty;
    /// <summary>
    /// Left character
    /// </summary>
    public string Left = string.Empty;
    /// <summary>
    /// Center character
    /// </summary>
    public string Center = string.Empty;
    /// <summary>
    /// Right character
    /// </summary>
    public string Right = string.Empty;

    #endregion Public Variables

    #endregion Variables
}
