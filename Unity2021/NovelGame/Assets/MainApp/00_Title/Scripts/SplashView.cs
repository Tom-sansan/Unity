using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashView : ViewBase
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
    public override async void OnViewOpened()
    {
        base.OnViewOpened();
        await ChangeViewWaitForSeconds();
    }
    /// <summary>
    /// Call at view close
    /// </summary>
    public override void OnViewClosed()
    {
        base.OnViewClosed();
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    /// <summary>
    /// Wait 2 seconds and move to Title View
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private async UniTask ChangeViewWaitForSeconds(float waitTime = 2f)
    {
        try
        {
            await UniTask.Delay((int)(waitTime * 1000f), false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            Scene.ChangeView(1).Forget();
        }
        catch (OperationCanceledException e)
        {
            Debug.Log("ChangeViewWaitForSeconds has cancelled..." + e);
        }
    }

    #endregion Methods
}
