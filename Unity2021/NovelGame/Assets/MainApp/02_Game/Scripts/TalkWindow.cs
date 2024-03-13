using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Talk Window Class
/// </summary>
public class TalkWindow : MonoBehaviour
{
    #region Class

    /// <summary>
    /// Talk Parameters
    /// </summary>
    [Serializable]
    public class StoryData
    {
        /// <summary>
        /// Display name
        /// </summary>
        public string Name = string.Empty;
        /// <summary>
        /// Conversation content
        /// </summary>
        [Multiline(3)]
        public string Talk = string.Empty;
    }
    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Name text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI nameText = null;
    /// <summary>
    /// Talk text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI talkText = null;
    /// <summary>
    /// Next page display image
    /// </summary>
    [SerializeField]
    private Image nextArrow = null;
    /// <summary>
    /// List of conversation parameters
    /// </summary>
    [SerializeField]
    private List<StoryData> talks = new List<StoryData>();

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Next flag
    /// </summary>
    private bool goToNextPage = false;
    /// <summary>
    /// Flag to go to the next
    /// </summary>
    private bool currentPageCompleted = false;
    /// <summary>
    /// Skip flag
    /// </summary>
    private bool isSkip = false;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        StartTalk(talks);
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Start talk
    /// </summary>
    /// <param name="talkList"></param>
    /// <param name="wordInterval"></param>
    /// <returns></returns>
    public async UniTask StartTalk(List<StoryData> talkList, float wordInterval = 0.2f)
    {
        foreach (var talk in talkList)
        {
            nameText.text = talk.Name;
            talkText.text = string.Empty;
            goToNextPage = false;
            currentPageCompleted = false;
            isSkip = false;
            nextArrow.gameObject.SetActive(false);
            await UniTask.Delay((int)(0.5f * 1000f));
            foreach (var word in talk.Talk)
            {
                talkText.text += word;
                await UniTask.Delay((int)(wordInterval * 1000f));
                if (isSkip)
                {
                    talkText.text = talk.Talk;
                    break;
                }
            }
            currentPageCompleted = true;
            nextArrow.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => goToNextPage == true);
        }
    }
    /// <summary>
    /// Next button click call back
    /// </summary>
    public void OnTextButtonClicked()
    {
        if (currentPageCompleted) goToNextPage = true;
        else isSkip = true;
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
