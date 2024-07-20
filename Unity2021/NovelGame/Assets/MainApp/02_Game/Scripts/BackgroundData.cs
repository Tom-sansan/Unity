using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Background Data Class
/// </summary>
[CreateAssetMenu(fileName = "BackgroundData", menuName = "ScriptableObjects/BackgroundData")]
public class BackgroundData : ScriptableObject
{
    #region Nested Class

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Parameter
    {
        // Name
        public string Name = string.Empty;
        // List of image parameter
        public Sprite Sprite = null;
    }

    #endregion Nested Class

    #region Variables

    #region SerializeField

    /// <summary>
    /// Memo
    /// </summary>
    [SerializeField]
    private string Memo = string.Empty;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Parameter list
    /// </summary>
    public List<Parameter> Parameters = new List<Parameter>();

    #endregion Public Variables

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Get image from image name
    /// </summary>
    /// <param name="imageName"></param>
    /// <returns></returns>
    public Sprite GetSprite(string imageName)
    {
        foreach (var parameter in Parameters)
            if (parameter.Name.Equals(imageName)) return parameter.Sprite;
        return null;
    }

    #endregion Public Methods

    #endregion Methods
}
