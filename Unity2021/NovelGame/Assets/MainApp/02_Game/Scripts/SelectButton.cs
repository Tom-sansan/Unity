using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// SelectButton Class
/// </summary>
public class SelectButton : MonoBehaviour
{
    #region Nested Class

    /// <summary>
    /// Click Event Class
    /// </summary>
    public class ClickEvent : UnityEvent<int> { }

    #endregion Nested Class

    #region Variables

    #region SerializeField

    /// <summary>
    /// Text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI buttonText = null;
    /// <summary>
    /// Transition
    /// </summary>
    [SerializeField]
    private UITransition transition = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Click event
    /// </summary>
    public ClickEvent OnClicked = new ClickEvent();
    /// <summary>
    /// Button index
    /// </summary>
    public int buttonIndex = 0;

    #endregion Public Variables

    #endregion Variables

    #region Methods

    #region Public Methods
    /// <summary>
    /// Call at creation
    /// </summary>
    /// <param name="text"></param>
    /// <param name="index"></param>
    /// <param name="onClick"></param>
    /// <returns></returns>
    public async UniTask OnCreated(string text, int index, UnityAction<int> onClick)
    {
        transition.Canvas.alpha = 0;
        buttonText.text = text;
        buttonIndex = index;
        OnClicked.AddListener(onClick);
        await transition.TransitionInWait();
    }
    /// <summary>
    /// Close
    /// </summary>
    /// <returns></returns>
    public async UniTask Close()
    {
        try
        {
            await transition.TransitionOutWait();
            Destroy(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log("Close Exception:" + e);
        }
    }
    /// <summary>
    /// Button click call back
    /// </summary>
    public void OnButtonClicked()
    {
        OnClicked.Invoke(buttonIndex);
    }
    #endregion Public Methods

    #endregion Methods
}
