using System;
using UnityEngine;

/// <summary>
/// Story Data Class
/// </summary>
[Serializable]
public class StoryData
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    /// <summary>
    /// Character
    /// </summary>
    public string Name = string.Empty;
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

    #region Private Variables

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

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
