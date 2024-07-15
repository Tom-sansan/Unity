using UnityEngine;

/// <summary>
/// View Base class
/// </summary>
public class ViewBase : MonoBehaviour
{
    #region Variables

    #region Public Variables

    #region Properties

    /// <summary>
    /// View transition
    /// </summary>
    public UITransition Transition => transition ??= GetComponent<UITransition>();

    #endregion Properties

    /// <summary>
    /// Scene base class
    /// </summary>
    public SceneBase Scene = null;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// View transition
    /// </summary>
    private UITransition transition = null;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Call at view open
    /// </summary>
    public virtual void OnViewOpened()
    {

    }
    /// <summary>
    /// Call at view close
    /// </summary>
    public virtual void OnViewClosed()
    {

    }

    #endregion Public Methods

    #endregion Methods
}
