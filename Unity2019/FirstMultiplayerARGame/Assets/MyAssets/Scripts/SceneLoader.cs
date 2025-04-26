using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneLoader Class
/// </summary>
public class SceneLoader : Singleton<SceneLoader>
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Scene name to be loaded
    /// </summary>
    private string sceneNameToBeLoaded;

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Load scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        sceneNameToBeLoaded = sceneName;
        StartCoroutine(InitializeSceneLoading());
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }
    /// <summary>
    /// Initialize scene loading
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitializeSceneLoading()
    {
        yield return SceneManager.LoadSceneAsync("Scene_Loading");
        StartCoroutine(LoadActualScene());
    }

    private IEnumerator LoadActualScene()
    {
        var asyncSceneLoading = SceneManager.LoadSceneAsync(sceneNameToBeLoaded);
        // This value stops the scene from displaying when it is still loading...
        asyncSceneLoading.allowSceneActivation = false;
        while (!asyncSceneLoading.isDone)
        {
            Debug.Log(asyncSceneLoading.progress);
            if (asyncSceneLoading.progress >= 0.9f)
            {
                // Finally, show the scene
                asyncSceneLoading.allowSceneActivation = true;
                break;
            }
            yield return null;
        }
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
