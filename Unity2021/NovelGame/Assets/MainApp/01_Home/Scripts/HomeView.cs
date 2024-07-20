using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// HomeView Class
/// </summary>
public class HomeView : ViewBase
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Button transition
    /// </summary>
    [SerializeField]
    private UITransition buttonTransition = null;

    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Call at view open
    /// </summary>
    public override async void OnViewOpened()
    {
        base.OnViewOpened();
        await Open();
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
    /// <summary>
    /// Reset saved data
    /// </summary>
    public void OnSaveDataResetButtonClicked()
    {
        ResetData();
    }

    public void ResetData()
    {
        var saveData = new SaveData();
        var data = saveData.Load();
        data.StoryNumber = 0;
        data.TestString = string.Empty;
        saveData.Save(data);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Open window
    /// </summary>
    /// <returns></returns>
    private async UniTask Open()
    {
        buttonTransition.Canvas.alpha = 0;
        buttonTransition.gameObject.SetActive(true);
        await buttonTransition.TransitionInWait();
    }
    /// <summary>
    /// Close window
    /// </summary>
    /// <returns></returns>
    private async UniTask Close()
    {
        await buttonTransition.TransitionOutWait();
        buttonTransition.gameObject.SetActive(false);
    }

    #endregion Private Methods

    #endregion Methods
}
