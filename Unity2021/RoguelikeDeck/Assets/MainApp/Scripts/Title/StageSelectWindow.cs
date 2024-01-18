using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Title scene/stage select window class
/// </summary>
public class StageSelectWindow : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Stage name text
    /// </summary>
    [SerializeField]
    private Text stageNameText = null;
    /// <summary>
    /// Stage Difficulty Text
    /// </summary>
    [SerializeField]
    private Text stageDifficultyText = null;
    /// <summary>
    /// Number of battles Text
    /// </summary>
    [SerializeField]
    private Text battleNumText = null;
    /// <summary>
    /// Stage icon image
    /// </summary>
    [SerializeField]
    private Image stageIconImage = null;
    /// <summary>
    /// Selected Stage Number Dot Image List
    /// </summary>
    [SerializeField]
    private List<Image> stageOrderImages = null;
    /// <summary>
    /// Individual job UI processing class list (set references in the order of left, selected, right)
    /// </summary>
    [SerializeField]
    private List<EachJobUI> eachJobUIs = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// ID in the list of job UI one left
    /// </summary>
    private const int JobUI_ID_Prev = 0;
    /// <summary>
    /// ID in the list of selected job UI
    /// </summary>
    private const int JobUI_ID_Current = 1;
    /// <summary>
    /// ID in the list of job UI one right
    /// </summary>
    private const int JobUI_ID_Next = 2;
    /// <summary>
    /// List of Released job IDs
    /// </summary>
    private List<int> unlockJobsIDList;
    /// <summary>
    /// Total number of stages
    /// </summary>
    private int stageListNum;
    /// <summary>
    /// Selecting Stage ID
    /// </summary>
    private int selectStageID;

    #endregion Private Variables

    /// <summary>
    /// Title management class
    /// </summary>
    private TitleManager titleManager;
    /// <summary>
    /// Window RectTransform
    /// </summary>
    private RectTransform windowRectTransform = null;
    /// <summary>
    /// Window Show/Hide Tween;.
    /// </summary>
    private Tween windowTween;
    /// <summary>
    /// Window display animation time
    /// </summary>
    private const float WindowAnimTime = 0.3f;

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
    /// Initialization (called from TitleManager.cs)
    /// </summary>
    /// <param name="titleManager"></param>
    public void Init(TitleManager titleManager)
    {
        this.titleManager = titleManager;
        windowRectTransform = GetComponent<RectTransform>();
        stageListNum = Data.instance.stageSOs.Count;
        // Reflect initial stage selection
        selectStageID = Data.instance.nowStageID;
        ShowStageData();
        // Hide Window
        windowRectTransform.transform.localScale = Vector2.zero;
        windowRectTransform.gameObject.SetActive(true);
    }
    /// <summary>
    /// Show window
    /// </summary>
    public void OpenWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window display tween
        windowTween = windowRectTransform.DOScale(1.0f, WindowAnimTime).SetEase(Ease.OutBack);
        // Activate window background panel
        titleManager.SetWindowBackPanelActive(true);
        // Show stage info
        ShowStageData();
        // Initialize job selection UI
        RefreshUnlockJobsList();
    }
    /// <summary>
    /// Hide window
    /// </summary>
    public void CloseWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window hide tween
        windowTween = windowRectTransform.DOScale(0.0f, WindowAnimTime).SetEase(Ease.InBack);
        // Disable window background panel
        titleManager.SetWindowBackPanelActive(false);
    }
    /// <summary>
    /// Button to switch to the stage one left (minus direction)
    /// </summary>
    public void LeftScrollButton()
    {
        // Selection Stage Switching
        selectStageID--;
        if (selectStageID < 0) selectStageID = stageListNum - 1;
        // Show selected stage information
        ShowStageData();
    }
    /// <summary>
    /// Button to switch to one stage to the right (positive direction)
    /// </summary>
    public void RightScrollButton()
    {
        // Selection Stage Switching
        selectStageID++;
        if (selectStageID >= stageListNum) selectStageID = 0;
        // Show selected stage information
        ShowStageData();
    }
    /// <summary>
    /// Stage start button
    /// </summary>
    /// <param name="isWithTutorial">true: Start with tutorial</param>
    public void StageStartButton(bool isWithTutorial)
    {
        // Keep selected stage number
        Data.instance.nowStageID = selectStageID;
        // Scene switching
        SceneManager.LoadScene("BattleScene");
    }
    /// <summary>
    /// Retrieve list of jobs being released and reflect it in the UI
    /// </summary>
    public void RefreshUnlockJobsList()
    {
        // Get a list of released job IDs
        unlockJobsIDList = new List<int>();
        for (int i = 0; i < (int)JobDataDefine.JobType._Max; i++)
            if (Data.instance.jobUnlocks[i]) unlockJobsIDList.Add(i);
        // Display UI
        ShowJobs();
    }
    /// <summary>
    /// Reflect each of the indications of each job.
    /// </summary>
    public void ShowJobs()
    {
        // Currently selected job data
        eachJobUIs[JobUI_ID_Current].SetJobUI((int)Data.instance.playerJob);
        // Display one previous release job
        // Search for IDs in the corresponding list
        int prevID = unlockJobsIDList.IndexOf((int)Data.instance.playerJob);
        prevID--;
        if (prevID < 0) prevID = unlockJobsIDList.Count - 1;
        // Display job UI
        eachJobUIs[JobUI_ID_Prev].SetJobUI(unlockJobsIDList[prevID]);
        // Display one next release job
        // Search for IDs in the corresponding list
        int nextID = unlockJobsIDList.IndexOf((int)Data.instance.playerJob);
        nextID++;
        if (nextID >= unlockJobsIDList.Count) nextID = 0;
        // Display job UI
        eachJobUIs[JobUI_ID_Next].SetJobUI(unlockJobsIDList[nextID]);
    }
    /// <summary>
    /// Switch to one previous job
    /// </summary>
    public void JobChangeButtonPrev()
    {
        // Search for IDs in the list of applicable job
        int prevID = unlockJobsIDList.IndexOf((int)Data.instance.playerJob);
        prevID--;
        if (prevID < 0) prevID = unlockJobsIDList.Count - 1;
        // Job switchover reflection
        Data.instance.SetPlayerJob(unlockJobsIDList[prevID]);
        ShowJobs();
    }
    /// <summary>
    /// Switch to one next job
    /// </summary>
    public void JobChangeButtonNext()
    {
        // Search for IDs in the list of applicable job
        int nextID = unlockJobsIDList.IndexOf((int)Data.instance.playerJob);
        nextID++;
        if (nextID >= unlockJobsIDList.Count) nextID = 0;
        // Job switchover reflection
        Data.instance.SetPlayerJob(unlockJobsIDList[nextID]);
        ShowJobs();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Show information on the currently selected stage
    /// </summary>
    private void ShowStageData()
    {
        var stageData = Data.instance.stageSOs[selectStageID];
        // UI settings by language
        if (Data.nowLanguage == SystemLanguage.Japanese)
        {
            // Stage name Text
            stageNameText.text = stageData.nameJP;
            // Stage difficulty Text
            stageDifficultyText.text = stageData.difficultyJP;
            // The number of enemy Text
            battleNumText.text = "敵の数" + stageData.appearEnemyTables.Count;
        }
        else if (Data.nowLanguage == SystemLanguage.English)
        {
            // Stage name Text
            stageNameText.text = stageData.nameEN;
            // Stage difficulty Text
            stageDifficultyText.text = stageData.difficultyEN;
            // The number of enemy Text
            battleNumText.text = stageData.appearEnemyTables.Count + " enemies";
        }
        // Stage icon Image
        stageIconImage.sprite = stageData.stageIcon;
        // Stage number Images
        for (int i = 0; i < stageListNum; i++)
        {
            if (i == selectStageID) stageOrderImages[i].transform.localScale = new Vector2(1.0f, 1.0f);
            else stageOrderImages[i].transform.localScale = new Vector2(0.4f, 0.4f);
        }
    }

    #endregion Private Methods

    #endregion Methods
}
