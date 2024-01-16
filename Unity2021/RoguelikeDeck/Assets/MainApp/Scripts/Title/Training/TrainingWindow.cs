using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Training window class
/// </summary>
public class TrainingWindow : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Scrollbar in ScrollView
    /// </summary>
    [SerializeField]
    private Scrollbar scrollbar = null;
    /// <summary>
    /// Amount of EXP in possessionText
    /// </summary>
    [SerializeField]
    private Text statusValueText = null;
    /// <summary>
    /// Training Item: Icon Image for Maximum Strength Increase
    /// </summary>
    [SerializeField]
    private Sprite itemIconMaxHPUP = null;
    /// <summary>
    /// Training item: Icon image for increasing the number of cards in hand
    /// </summary>
    [SerializeField]
    private Sprite itemIconHandNumUP = null;
    /// <summary>
    /// Training Item UI Prefabrication
    /// </summary>
    [SerializeField]
    private GameObject trainingItemPrefab = null;
    /// <summary>
    /// Parent Transform of the training item UI
    /// </summary>
    [SerializeField]
    private Transform trainingItemsParent = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Number of cards in player's initial hand
    /// </summary>
    public const int InitPlayerHandNum = 5;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Window display animation time
    /// </summary>
    private const float WindowAnimTime = 0.3f;
    /// <summary>
    /// Title manager class
    /// </summary>
    private TitleManager titleManager;
    /// <summary>
    /// RectTransform of window
    /// </summary>
    private RectTransform windowRectTransform;
    /// <summary>
    /// List of generated training items
    /// </summary>
    private List<TrainingItem> trainingItemInstances;
    /// <summary>
    /// Window Show/Hide Tween
    /// </summary>
    private Tween windowTween;
    /// <summary>
    /// Scrollbar initialization flag
    /// </summary>
    private bool reserveResetScrollBar;

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
    /// Initialization (called from TitleManager.cs)
    /// </summary>
    /// <param name="titleManager"></param>
    public void Init(TitleManager titleManager)
    {
        // TODO: For Debug
        Data.instance.ChangePlayerEXP(100000);

        this.titleManager = titleManager;
        windowRectTransform = GetComponent<RectTransform>();
        trainingItemInstances = new List<TrainingItem>();
        // Create training item
        // Max. increase in physical strength
        // Create item
        {
            var obj = Instantiate(trainingItemPrefab, trainingItemsParent);
            var trainingItem = obj.GetComponent<TrainingItem>();
            trainingItemInstances.Add(trainingItem);
            // Set item
            trainingItem.Init(this, TrainingItem.TrainingMode.MaxHPUP);
            trainingItem.SetIcon(itemIconMaxHPUP);
        }
        // Increase in the number of cards in hand
        if (GetHandNumTrainCount() < TrainingItem.TrainPriceHandNumUP.Length)
        {
            // (Create if player has not completed this training)
            // Creaate item
            var obj = Instantiate(trainingItemPrefab, trainingItemsParent);
            var trainingItem = obj.GetComponent<TrainingItem>();
            trainingItemInstances.Add(trainingItem);
            // Set item
            trainingItem.Init(this, TrainingItem.TrainingMode.HandNumUP);
            trainingItem.SetIcon(itemIconHandNumUP);
        }
        // Job release
        int length = (int)JobDataDefine.JobType._Max;
        for (int i = 0; i < length; i++)
        {
            // Create for the number of unopened jobs
            if (Data.instance.jobUnlocks[i]) continue;
            // Creaate item
            var obj = Instantiate(trainingItemPrefab, trainingItemsParent);
            var trainingItem = obj.GetComponent<TrainingItem>();
            trainingItemInstances.Add(trainingItem);
            // Set item
            trainingItem.Init(this, TrainingItem.TrainingMode.UnlockJob, JobDataDefine.GetJobTypeByInt(i));
            trainingItem.SetIcon(Data.instance.jobIcons[(int)JobDataDefine.GetJobTypeByInt(i)]);
        }
        // Display of the amount of EXP in possession
        statusValueText.text = Data.instance.playerEXP.ToString("#,0");
        // Hide window
        windowRectTransform.transform.localScale = Vector2.zero;
        windowRectTransform.gameObject.SetActive(true);
    }
    /// <summary>
    /// Display window
    /// </summary>
    public void OpenWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window display Tween
        windowTween = windowRectTransform
            .DOScale(1.0f, WindowAnimTime)
            .SetEase(Ease.OutBack);
        // Enable background panel of window
        titleManager.SetWindowBackPanelActive(true);
        // Reset screen scrolling to initial values (to be executed in the next OnGUI)
        reserveResetScrollBar = true;
    }
    /// <summary>
    /// Hide window
    /// </summary>
    public void CloseWindow()
    {
        if (windowTween != null) windowTween.Kill();
        // Window hide Tween
        windowTween = windowRectTransform
            .DOScale(0.0f, WindowAnimTime)
            .SetEase(Ease.InBack);
        // Disable background panel of window
        titleManager.SetWindowBackPanelActive(false);
    }
    /// <summary>
    /// Get (purchase) corresponding item
    /// </summary>
    /// <param name="targetItem"></param>
    public void ObtainItem(TrainingItem targetItem)
    {
        // EXP amount reduced
        Data.instance.ChangePlayerEXP(-targetItem.price);
        OnChangePlayerEXP();
        // Player acquires item to which the item corresponds(項目が対応する物をプレイヤーが獲得する)
        switch (targetItem.trainingMode)
        {
            case TrainingItem.TrainingMode.MaxHPUP:
                // Maximum physical strength increase
                Data.instance.ChangePlayerMaxHP(1);
                targetItem.RefreshUI();
                break;
            case TrainingItem.TrainingMode.HandNumUP:
                // Increase in the number of cards in hand
                Data.instance.ChangePlayerHandNum(1);
                if (GetHandNumTrainCount() < TrainingItem.TrainPriceHandNumUP.Length)
                    // Still trainable
                    targetItem.RefreshUI();
                else
                {
                    // No longer trainable
                    // Delete object
                    Destroy(targetItem.gameObject);
                    // Delete from list
                    trainingItemInstances.Remove(targetItem);
                }
                break;
            case TrainingItem.TrainingMode.UnlockJob:
                // Job release
                Data.instance.UnlockJob((int)targetItem.jobType);
                // Delete object
                Destroy(targetItem.gameObject);
                // Delete from list
                trainingItemInstances.Remove(targetItem);
                break;
        }
    }
    /// <summary>
    /// Called when a player's EXP amount is changed
    /// </summary>
    public void OnChangePlayerEXP()
    {
        // Display of the amount of EXP in possession
        statusValueText.text = Data.instance.playerEXP.ToString("#,0");
        // Setting of whether or not to press the "Get" button for each item
        foreach (var item in trainingItemInstances) item.CheckPrice();
    }
    /// <summary>
    /// Return the number of times trained to increase the number of cards in hand
    /// </summary>
    /// <returns></returns>
    public int GetHandNumTrainCount() =>
        Data.instance.playerHandNum - InitPlayerHandNum;

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// OnGUI (run frame by frame)
    /// </summary>
    private void OnGUI()
    {
        // Restore screen scrolling to initial values
        if (reserveResetScrollBar)
        {
            scrollbar.value = 1.0f;
            reserveResetScrollBar = false;
        }
    }

    #endregion Private Methods

    #endregion Methods
}
