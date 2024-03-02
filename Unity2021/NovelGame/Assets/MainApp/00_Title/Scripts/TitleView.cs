using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleView : ViewBase
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

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

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Call at view open
    /// </summary>
    public override void OnViewOpened()
    {
        base.OnViewOpened();
    }
    /// <summary>
    /// Call at view close
    /// </summary>
    public override void OnViewClosed()
    {
        base.OnViewClosed();
    }
    /// <summary>
    /// Screen tap callback
    /// </summary>
    public async void OnScreenButtonClicked()
    {
        await Scene.ChangeScene("01_Home");
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
