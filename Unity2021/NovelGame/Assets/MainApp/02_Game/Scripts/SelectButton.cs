using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectButton : MonoBehaviour
{
    #region Class

    /// <summary>
    /// Click Event Class
    /// </summary>
    public class ClickEvent : UnityEvent<int> { }

    #endregion Class

    #region Enum

    #endregion Enum

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

    #region Protected Variables

    #endregion Protected Variables

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

    #endregion Public Methods

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
        await transition.TransitionOutWait();
        Destroy(gameObject);
    }

    public void OnButtonClicked()
    {
        OnClicked.Invoke(buttonIndex);
    }

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
