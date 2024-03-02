using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeView : ViewBase
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
    /// Change to title scene
    /// </summary>
    public async void OnGameButtonClicked() =>
        await Scene.ChangeScene("02_Game");

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
