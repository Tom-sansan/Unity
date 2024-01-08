using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Translation Text Class
/// Change the contents of Text in the same object at the start of a scene by setting language
/// (for Text that does not change during a scene)
/// </summary>
public class SimpleTranslationText : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    [Header("表示文章(日本語)")]
    [TextArea]
    [SerializeField]
    private string textJP = "";
    [Header("表示文章(英語)")]
    [TextArea]
    [SerializeField]
    private string textEN = "";

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

    #endregion Public Methods

    /// <summary>
    /// Set language
    /// </summary>
    public void Translation()
    {
        var textUI = GetComponent<Text>();
        // Change by setting language
        if (Data.nowLanguage == SystemLanguage.Japanese) textUI.text = textJP;
        else if (Data.nowLanguage == SystemLanguage.English) textUI.text = textEN;
    }

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
