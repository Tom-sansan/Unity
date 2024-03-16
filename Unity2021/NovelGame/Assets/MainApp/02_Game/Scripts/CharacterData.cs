using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Data Class
/// </summary>
[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    #region Class

    /// <summary>
    /// Character's parameter
    /// </summary>
    [Serializable]
    public class Parameter
    {
        /// <summary>
        /// Name
        /// </summary>
        public string DisplayName = string.Empty;
        /// <summary>
        /// Character type
        /// </summary>
        public Type Character = Type.None;
        /// <summary>
        /// List of image parameter
        /// </summary>
        public List<ImageParam> ImageParams = new List<ImageParam>();
        /// <summary>
        /// Retrieve an image from an expression type
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public Sprite GetEmotionSprite(EmotionType emotion)
        {
            foreach (var img in ImageParams)
                if (img.Emotion == emotion) return img.Sprite;
            return null;
        }
    }
    /// <summary>
    /// Image and facial expression association parameters
    /// </summary>
    [Serializable]
    public class ImageParam
    {
        /// <summary>
        /// Expression Type
        /// </summary>
        public EmotionType Emotion = EmotionType.None;
        /// <summary>
        /// Image
        /// </summary>
        public Sprite Sprite = null;
    }

    #endregion Class

    #region Enum

    /// <summary>
    /// Character definition
    /// </summary>
    public enum Type
    {
        None = 0,
        Kitakuchan = 1,
        Orekun = 2,
        Sensei = 3
    }
    /// <summary>
    /// Facial Expression Definition for Images
    /// </summary>
    public enum EmotionType
    {
        None,
        Smile,
        Cry,
        Angry
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Memo
    /// </summary>
    [SerializeField]
    private string Memo = string.Empty;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    /// <summary>
    /// Parameter list
    /// </summary>
    public List<Parameter> Parameters = new List<Parameter>();

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

    /// <summary>
    /// Get display name from character number
    /// </summary>
    /// <param name="characterNumber"></param>
    /// <returns></returns>
    public string GetCharacterName(string characterNumber)
    {
        if (characterNumber == "1" || characterNumber == "2" || characterNumber == "3")
        {
            var param = GetParameterFromNumber(characterNumber);
            return param.DisplayName;
        }
        return string.Empty;
    }
    /// <summary>
    /// Get character image from string data
    /// </summary>
    /// <param name="dataString"></param>
    /// <returns></returns>
    public Sprite GetCharacterSprite(string dataString)
    {
        // Get first letter
        var num = dataString.Substring(0, 1);
        var emo = dataString.Substring(1);
        if (num != "0" && num != "1" && num != "2" && num != "3")
        {
            Debug.Log("Incorrect data input: " + dataString);
            return null;
        }
        var param = GetParameterFromNumber(num);
        var emotion = GetEmotionType(emo);
        var sprite = param.GetEmotionSprite(emotion);
        return sprite;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Get Parameter from character number
    /// </summary>
    /// <param name="characterNumber"></param>
    /// <returns></returns>
    private Parameter GetParameterFromNumber(string characterNumber)
    {
        foreach (var param in Parameters)
            if (((int)param.Character).ToString() == characterNumber) return param;
        return null;
    }
    /// <summary>
    /// Get EmotationType from the string of the expression part
    /// </summary>
    /// <param name="emotionString"></param>
    /// <returns></returns>
    private EmotionType GetEmotionType(string emotionString)
    {
        switch (emotionString)
        {
            case nameof(EmotionType.Smile):
                return EmotionType.Smile;
            case nameof(EmotionType.Cry):
                return EmotionType.Cry;
            case nameof(EmotionType.Angry):
                return EmotionType.Angry;
            default:
                return EmotionType.None;
        }
    }

    #endregion Private Methods

    #endregion Methods
}
