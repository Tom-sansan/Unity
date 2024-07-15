using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SelectButtonDialog Class
/// </summary>
public class SelectButtonDialog : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Background transition
    /// </summary>
    [SerializeField]
    private UITransition bgTransition = null;
    /// <summary>
    /// Parent of button
    /// </summary>
    [SerializeField]
    private Transform buttonParent = null;
    /// <summary>
    /// Prefab of button
    /// </summary>
    [SerializeField]
    private SelectButton buttonPrefab = null;

    #endregion SerializeField

    #region Private Variables

    /// <summary>
    /// Button list
    /// </summary>
    private List<SelectButton> buttons = new List<SelectButton>();
    /// <summary>
    /// Response
    /// </summary>
    private int response = -1;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods

    /// <summary>
    /// Button creation
    /// </summary>
    /// <param name="bgOpen">Bool if background is open or not</param>
    /// <param name="selectParams">Parameters of button text</param>
    /// <returns></returns>
    public async UniTask<int> CreateButtons(bool bgOpen, string[] selectParams)
    {
        if (selectParams == null || selectParams.Length == 0) return -1;
        try
        {
            var tasks = new List<UniTask>();
            int index = 0;
            // Background setting
            if (bgOpen)
            {
                bgTransition.gameObject.SetActive(true);
                tasks.Add(bgTransition.TransitionInWait());
            }
            else bgTransition.gameObject.SetActive(false);
            // Create buttons
            foreach (var param in selectParams)
            {
                var button = Instantiate(buttonPrefab, buttonParent);
                buttons.Add(button);
                tasks.Add(button.OnCreated(param, index, OnAnyButtonClicked));
                index++;
            }
            // Update canvas to reliably reflect layout groups
            Canvas.ForceUpdateCanvases();
            await UniTask.WhenAll(tasks);
            // Wait until some button is pressed here
            await UniTask.WaitUntil(() => response != -1, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            var res = response;
            // Close
            await Close();
            return res;
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(nameof(CreateButtons) + " has been cancelled...");
            throw e;
        }
    }
    /// <summary>
    /// Close dialog
    /// </summary>
    /// <returns></returns>
    public async UniTask Close()
    {
        // Close buttons
        var tasks = new List<UniTask>();
        foreach (var button in buttons) tasks.Add(button.Close());
        // Close background
        if (bgTransition.gameObject.activeSelf) tasks.Add(bgTransition.TransitionOutWait());
        await UniTask.WhenAll(tasks);
        bgTransition.gameObject.SetActive(false);
        response = -1;
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Process when any of the buttons is pressed
    /// </summary>
    /// <param name="index"></param>
    private void OnAnyButtonClicked(int index)
    {
        Debug.Log(index + " was clicked...");
        response = index;
    }

    #endregion Private Methods

    #endregion Methods
}
