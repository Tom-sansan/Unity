using UnityEngine;

/// <summary>
/// View Base class
/// </summary>
public class ViewBase : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

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

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
