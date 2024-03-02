using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene Base class
/// </summary>
public class SceneBase : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// View list
    /// </summary>
    [SerializeField]
    protected List<ViewBase> viewList = new List<ViewBase>();
    /// <summary>
    /// Initial view index
    /// </summary>
    [SerializeField]
    protected int initialViewIndex = 0;
    /// <summary>
    /// Initial view transition flag
    /// </summary>
    [SerializeField]
    protected bool isInitialTransition = true;

    #endregion SerializeField

    #region Protected Variables

    /// <summary>
    /// The current view
    /// </summary>
    protected ViewBase currentView = null;

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    protected virtual void Start()
    {
        Init();
    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// View move process
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public virtual async UniTask ChangeView(int index)
    {
        // Close process if current view is set
        if (currentView != null)
        {
            currentView.OnViewClosed();
            if (currentView.Transition != null) await currentView.Transition.TransitionOutWait();
        }
        // Search for a specified index view in the view list
        foreach (var view in viewList)
        {
            // To the corresponding view
            if (viewList.IndexOf(view) == index)
            {
                // Open process
                view.gameObject.SetActive(true);
                view.OnViewOpened();
                if (view.Transition != null) await view.Transition.TransitionInWait();
            }
            else view.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Scene move process
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public virtual async UniTask ChangeScene(string sceneName)
    {
        // Close process if current view is set
        if (currentView != null)
        {
            currentView.OnViewClosed();
            if (currentView.Transition != null) await currentView.Transition.TransitionOutWait();
        }
        await SceneManager.LoadSceneAsync(sceneName);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        // If the initial Index is set properly
        if (initialViewIndex >= 0)
        {
            // For all listings in the view
            foreach (var view in viewList)
            {
                // Set the scene
                view.Scene = this;
                // Processing for views at initial index
                if (viewList.IndexOf(view) == initialViewIndex)
                {
                    // When making transitions
                    if (view.Transition != null && isInitialTransition)
                    {
                        view.Transition.Canvas.alpha = 0;
                        view.gameObject.SetActive(true);
                        view.OnViewOpened();
                        view.Transition.TransitionIn();
                    }
                    // If no transitions
                    else
                    {
                        view.OnViewOpened();
                        view.gameObject.SetActive(true);
                    }
                    // Set current view
                    currentView = view;
                }
                // Processing for views other than the initial view
                else view.gameObject.SetActive(false);
            }
        }
    }

    #endregion Private Methods

    #endregion Methods
}
