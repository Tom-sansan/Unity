using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TalkWindow Class
/// </summary>
public class TalkWindow : MonoBehaviour
{
    #region Variables

    #region SerializeField

    #region Private

    /// <summary>
    /// Character data
    /// </summary>
    [SerializeField]
    private CharacterData characterData;
    /// <summary>
    /// Background data
    /// </summary>
    [SerializeField]
    private BackgroundData bgData = null;
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
    /// Next page display image
    /// </summary>
    [SerializeField]
    private Image nextArrow = null;
    /// <summary>
    /// Background image
    /// </summary>
    [SerializeField]
    private Image bgImage = null;
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
    /// Talk transition
    /// </summary>
    [SerializeField]
    private UITransition talkWindowTransition = null;
    /// <summary>
    /// Background transition
    /// </summary>
    [SerializeField]
    private UITransition bgTransition = null;
    /// <summary>
    /// Selection
    /// </summary>
    [SerializeField]
    private SelectButtonDialog selectButtonDialog = null;
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
    /// Word interval time
    /// </summary>
    [SerializeField]
    private float wordInterval = 0.1f;

    #endregion Private

    #endregion SerializeField

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
    /// Additional part for color change
    /// </summary>
    string tagStrings = string.Empty;
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
    /// <summary>
    /// Additional part for color change
    /// </summary>
    private bool isInTag = false;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        // Deactivate character iamges and talk window
        DeactivateCharacterAndWindow();
    }
    //async void Start()
    //{
    //    await StartTest();
    //}

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
        try
        {
            await talkWindowTransition.TransitionOutWait();
            talkWindowTransition.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// Start talk
    /// </summary>
    /// <param name="talkList"></param>
    /// <param name="wordInterval"></param>
    /// <returns></returns>
    public async UniTask<List<int>> StartTalk(List<StoryData> talkList, float wordInterval = 0.1f)
    {
        try
        {
            currentLeft = string.Empty;
            currentCenter = string.Empty;
            currentRight = string.Empty;
            List<int> responseList = new List<int>();
            foreach (var talk in talkList)
            {
                // Set backgound image
                if (!string.IsNullOrEmpty(talk.Place)) await SetBg(talk.Place, false);
                // In case of selection button(30)
                if (talk.CharacterNumber.Equals(30))
                {
                    goToNextPage = false;
                    currentPageCompleted = false;
                    isSkip = false;
                    nextArrow.gameObject.SetActive(false);
                    SetCharacter(talk).Forget();
                    string[] arr = talk.Talk.Split(',');
                    var res = await selectButtonDialog.CreateButtons(true, arr);
                    Debug.Log("Response = " + res);
                    responseList.Add(res);
                    goToNextPage = true;
                }
                // In case of 31, Game clear
                else if (talk.CharacterNumber.Equals(99))
                {
                    // Clear data
                    new HomeView().ResetData();
                    // Go to Title Page
                    GameObject.Find("GameView").GetComponent<GameView>().OnBackToHomeButtonClicked();
                }
                // In case of Character process
                else
                {
                    nameText.text = characterData.GetCharacterName(talk.CharacterNumber);
                    talkText.text = string.Empty;
                    goToNextPage = false;
                    currentPageCompleted = false;
                    isSkip = false;
                    nextArrow.gameObject.SetActive(false);
                    await SetCharacter(talk);
                    // A slight pause when the page opens
                    await UniTask.Delay((int)(0.5f * 1000f), cancellationToken : this.gameObject.GetCancellationTokenOnDestroy());
                    foreach (var word in talk.Talk)
                    {
                        #region Additional part for color change

                        bool isCloseTag = false;
                        // Determine tags
                        if (word.ToString().Equals("<"))
                        {
                            Debug.Log("< です。");
                            isInTag = true;
                        }
                        else if (word.ToString().Equals(">"))
                        {
                            Debug.Log("> です。");
                            isInTag = false;
                            isCloseTag = true;
                        }
                        // Change the additional characters for each tag situation
                        if (!isInTag && !isCloseTag && !string.IsNullOrEmpty(tagStrings))
                        {
                            // Ex; <color=#0800FF>今
                            var _word = tagStrings + word;
                            talkText.text += _word;
                            tagStrings = string.Empty;
                        }
                        else if (isInTag || isCloseTag)
                        {
                            // Creating inside tag like <color=#0800F
                            tagStrings += word;
                            Debug.Log("Tab内です。");
                            continue;
                        }
                        else talkText.text += word;

                        #endregion Additional part for color change

                        // talkText.text += word;
                        await UniTask.Delay((int)(wordInterval * 1000f), cancellationToken : this.gameObject.GetCancellationTokenOnDestroy());
                        if (isSkip)
                        {
                            talkText.text = talk.Talk;
                            break;
                        }
                    }
                }
                currentPageCompleted = true;
                nextArrow.gameObject.SetActive(true);
                await UniTask.WaitUntil(() => goToNextPage == true, cancellationToken : this.gameObject.GetCancellationTokenOnDestroy());
            }
            return responseList;
        }
        catch (OperationCanceledException e)
        {
            Debug.Log("StartTalk cancel.");
            throw e;
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
    /// <summary>
    /// Set background
    /// </summary>
    /// <param name="bgName">Backgound name</param>
    /// <param name="isImmediate"></param>
    /// <returns></returns>
    public async UniTask SetBg(string bgName, bool isImmediate)
    {
        var sp = bgData.GetSprite(bgName);
        bgTransition.gameObject.SetActive(true);
        var currentBg = bgImage.sprite.name;
        if (currentBg.Equals(sp.name))
        {
            Debug.Log("Skip Background setting becaseu of the same background..");
            return;
        }
        if (!isImmediate)
        {
            try
            {
                await bgTransition.TransitionOutWait();
                bgImage.sprite = sp;
                await bgTransition.TransitionInWait();
            }
            catch (OperationCanceledException e)
            {
                Debug.Log("SetBg Cancel");
                throw e;
            }
        }
        else bgImage.sprite = sp;
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
        // Delete all of character images if null
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
        try
        {
            // Set left character image
            SetCharacterImage(leftCharacterImageTransition, leftCharacterImage, storyData.Left, ref currentLeft, ref hideLeft);
            // Set center character image
            SetCharacterImage(centerCharacterImageTransition, centerCharacterImage, storyData.Center, ref currentCenter, ref hideCenter);
            // Set left character image
            SetCharacterImage(rightCharacterImageTransition, rightCharacterImage, storyData.Right, ref currentRight, ref hideRight);

            #region Character settings (Deleted)
/*
            if (string.IsNullOrEmpty(storyData.Left))
            {
                tasks.Add(leftCharacterImageTransition.TransitionOutWait());
                hideLeft = true;
            }
            else if (currentLeft != storyData.Left)
            {
                var img = characterData.GetCharacterSprite(storyData.Left);
                leftCharacterImage.sprite = img;
                leftCharacterImage.gameObject.SetActive(true);
                tasks.Add(leftCharacterImageTransition.TransitionInWait());
                currentLeft = storyData.Left;
            }
            else Debug.Log("No change as it is the same.");

            if (string.IsNullOrEmpty(storyData.Center))
            {
                tasks.Add(centerCharacterImageTransition.TransitionOutWait());
                hideCenter = true;
            }
            else if (currentCenter != storyData.Center)
            {
                var img = characterData.GetCharacterSprite(storyData.Center);
                centerCharacterImage.sprite = img;
                centerCharacterImage.gameObject.SetActive(true);
                tasks.Add(centerCharacterImageTransition.TransitionInWait());
                currentCenter = storyData.Center;
            }
            else Debug.Log("No change as it is the same.");

            if (string.IsNullOrEmpty(storyData.Right))
            {
                tasks.Add(rightCharacterImageTransition.TransitionOutWait());
                hideRight = true;
            }
            else if (currentRight != storyData.Right)
            {
                var img = characterData.GetCharacterSprite(storyData.Right);
                rightCharacterImage.sprite = img;
                rightCharacterImage.gameObject.SetActive(true);
                tasks.Add(rightCharacterImageTransition.TransitionInWait());
                currentRight = storyData.Right;
            }
            else Debug.Log("No change as it is the same.");
*/
            #endregion Character settings (Deleted)

            // Wait
            await UniTask.WhenAll(tasks);
            // Hide character to hide
            if (hideLeft) leftCharacterImage.gameObject.SetActive(false);
            if (hideCenter) centerCharacterImage.gameObject.SetActive(false);
            if (hideRight) rightCharacterImage.gameObject.SetActive(false);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(nameof(SetCharacter) + " has been cancelled...");
            throw e;
        }

        // Set character image
        void SetCharacterImage (UITransition uITransition, Image charaImg, string direction, ref string currentDirection, ref bool hideDirection)
        {
            if (string.IsNullOrEmpty(direction))
            {
                tasks.Add(uITransition.TransitionOutWait());
                hideDirection = true;
                currentDirection = string.Empty;
            }
            else if (currentDirection != direction)
            {
                var img = characterData.GetCharacterSprite(direction);
                charaImg.sprite = img;
                charaImg.gameObject.SetActive(true);
                // Set native size of Image
                charaImg.SetNativeSize();
                tasks.Add(uITransition.TransitionInWait());
                currentDirection = direction;
            }
            else Debug.Log("No change as it is the same.");
        }
    }
    /// <summary>
    /// Deactivate characters and talk window
    /// </summary>
    private void DeactivateCharacterAndWindow()
    {
        SetCharacter(null).Forget();
        talkWindowTransition.gameObject.SetActive(false);
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

    /// <summary>
    /// List of conversation parameters
    /// </summary>
    [SerializeField]
    public List<StoryData> Talks = new List<StoryData>();

    /// <summary>
    /// Start test method
    /// </summary>
    private async UniTask StartTest()
    {
        await Open();
        await StartTalk(Talks);
        await Close();
        Debug.Log("Test finished!!");
    }

#endif

    #endregion For Debug

}
