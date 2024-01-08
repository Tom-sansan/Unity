using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Change the Sprite of the Image in the same object at the start of the scene according to the setting language
/// (for Images that do not change during the scene)
/// </summary>
public class SimpleTranslationImage : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    [Header("表示画像(日本語)")]
    [SerializeField] private Sprite spriteJP = null;
    [Header("表示画像(英語)")]
    [SerializeField] private Sprite spriteEN = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Translation();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Set language
    /// </summary>
    public void Translation()
    {
        var imageUI = GetComponent<UnityEngine.UI.Image>();
        // Change by setting language
        if (Data.nowLanguage == SystemLanguage.Japanese) imageUI.sprite = spriteJP;
        else if (Data.nowLanguage == SystemLanguage.English) imageUI.sprite = spriteEN;
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
