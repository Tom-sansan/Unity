using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Concurrent;
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

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Character data
    /// </summary>
    [SerializeField]
    private CharacterData characterData;
    /// <summary>
    /// Left character image
    /// </summary>
    [SerializeField]
    private Image leftCharacterImage = null;
    /// <summary>
    /// Center character image
    /// </summary>
    [SerializeField]
    private Image centerCharacterImage = null;
    /// <summary>
    /// Right character image
    /// </summary>
    [SerializeField]
    private Image rightCharacterImage = null;
    /// <summary>
    /// Left character transition
    /// </summary>
    [SerializeField]
    private UITransition leftCharacterImageTransition = null;
    /// <summary>
    /// Center character transition
    /// </summary>
    [SerializeField]
    private UITransition centerCharacterImageTransition = null;
    /// <summary>
    /// Right character transition
    /// </summary>
    [SerializeField]
    private UITransition rightCharacterImageTransition = null;
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
    /// Talk transition
    /// </summary>
    [SerializeField]
    private UITransition talkWindowTransition = null;
    /// <summary>
    /// Word interval time
    /// </summary>
    [SerializeField]
    private float wordInterval = 0.1f;
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
    /// The current left character info
    /// </summary>
    private string currentLeft = string.Empty;
    /// The current center character info
    /// </summary>
    private string currentCenter = string.Empty;
    /// The current right character info
    /// </summary>
    private string currentRight = string.Empty;
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
    async void Start()
    {
        await Open();
        await StartTalk(talks);
        await Close();
        Debug.Log("Test finished!!");
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Open window
    /// </summary>
    /// <param name="initName"></param>
    /// <param name="initText"></param>
    /// <returns></returns>
    public async UniTask Open(string initName = "", string initText = "")
    {
        SetCharacter(null).Forget();
        nameText.text = initName;
        talkText.text = initText;
        nextArrow.gameObject.SetActive(false);
        talkWindowTransition.gameObject.SetActive(true);
        await talkWindowTransition.TransitionInWait();
    }
    /// <summary>
    /// Close window
    /// </summary>
    /// <returns></returns>
    public async UniTask Close()
    {
        await talkWindowTransition.TransitionOutWait();
        talkWindowTransition.gameObject.SetActive(false);
    }
    /// <summary>
    /// Start talk
    /// </summary>
    /// <param name="talkList"></param>
    /// <param name="wordInterval"></param>
    /// <returns></returns>
    public async UniTask StartTalk(List<StoryData> talkList)
    {
        currentLeft =
        currentCenter =
        currentRight = string.Empty;

        foreach (var talk in talkList)
        {
            nameText.text = characterData.GetCharacterName(talk.Name);
            talkText.text = string.Empty;
            goToNextPage = false;
            currentPageCompleted = false;
            isSkip = false;
            nextArrow.gameObject.SetActive(false);
            await SetCharacter(talk);

            // A slight pause when the page opens
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

    /// <summary>
    /// Set character image
    /// </summary>
    /// <param name="storyData"></param>
    /// <returns></returns>
    private async UniTask SetCharacter(StoryData storyData)
    {
        // Delete all if null
        if (storyData == null)
        {
            leftCharacterImage.gameObject.SetActive(false);
            centerCharacterImage.gameObject.SetActive(false);
            rightCharacterImage.gameObject.SetActive(false);
            return;
        }

        var tasks = new List<UniTask>();
        bool hideLeft = false;
        bool hideCenter = false;
        bool hideRight = false;

        // Set left character
        SetCharacterData(leftCharacterImageTransition, leftCharacterImage, storyData.Left, ref currentLeft, ref hideLeft);
        // Set center character
        SetCharacterData(centerCharacterImageTransition, centerCharacterImage, storyData.Center, ref currentCenter, ref hideCenter);
        // Set left character
        SetCharacterData(rightCharacterImageTransition, rightCharacterImage, storyData.Right, ref currentRight, ref hideRight);

        //if (string.IsNullOrEmpty(storyData.Left))
        //{
        //    tasks.Add(leftCharacterImageTransition.TransitionOutWait());
        //    hideLeft = true;
        //}
        //else if (currentLeft != storyData.Left)
        //{
        //    var img = characterData.GetCharacterSprite(storyData.Left);
        //    leftCharacterImage.sprite = img;
        //    leftCharacterImage.gameObject.SetActive(true);
        //    tasks.Add(leftCharacterImageTransition.TransitionInWait());
        //    currentLeft = storyData.Left;
        //}
        //else Debug.Log("No change as it is the same.");

        //if (string.IsNullOrEmpty(storyData.Center))
        //{
        //    tasks.Add(centerCharacterImageTransition.TransitionOutWait());
        //    hideCenter = true;
        //}
        //else if (currentCenter != storyData.Center)
        //{
        //    var img = characterData.GetCharacterSprite(storyData.Center);
        //    centerCharacterImage.sprite = img;
        //    centerCharacterImage.gameObject.SetActive(true);
        //    tasks.Add(centerCharacterImageTransition.TransitionInWait());
        //    currentCenter = storyData.Center;
        //}
        //else Debug.Log("No change as it is the same.");

        //if (string.IsNullOrEmpty(storyData.Right))
        //{
        //    tasks.Add(rightCharacterImageTransition.TransitionOutWait());
        //    hideRight = true;
        //}
        //else if (currentRight != storyData.Right)
        //{
        //    var img = characterData.GetCharacterSprite(storyData.Right);
        //    rightCharacterImage.sprite = img;
        //    rightCharacterImage.gameObject.SetActive(true);
        //    tasks.Add(rightCharacterImageTransition.TransitionInWait());
        //    currentRight = storyData.Right;
        //}
        //else Debug.Log("No change as it is the same.");

        // Wait
        await UniTask.WhenAll(tasks);
        // Hide character to hide
        if (hideLeft) leftCharacterImage.gameObject.SetActive(false);
        if (hideCenter) centerCharacterImage.gameObject.SetActive(false);
        if (hideRight) rightCharacterImage.gameObject.SetActive(false);

        // Set character data
        void SetCharacterData (UITransition uITransition, Image charaImg, string direction, ref string currentDirection, ref bool hideDirection)
        {
            if (string.IsNullOrEmpty(direction))
            {
                tasks.Add(uITransition.TransitionOutWait());
                hideDirection = true;
            }
            else if (currentDirection != direction)
            {
                var img = characterData.GetCharacterSprite(direction);
                charaImg.sprite = img;
                charaImg.gameObject.SetActive(true);
                tasks.Add(uITransition.TransitionInWait());
                currentDirection = direction;
            }
            else Debug.Log("No change as it is the same.");
        }
    }

    #endregion Private Methods

    #endregion Methods
}
