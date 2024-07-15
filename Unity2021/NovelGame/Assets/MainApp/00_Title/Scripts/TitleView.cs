/// <summary>
/// Title View Class
/// </summary>
public class TitleView : ViewBase
{
    #region Methods

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

    #endregion Methods
}
