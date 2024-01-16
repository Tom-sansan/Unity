using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI processing class for each training item
/// </summary>
public class TrainingItem : MonoBehaviour
{
    #region Enum

    public enum TrainingMode
    {
        MaxHPUP,    // Maximum strength increase
        HandNumUP,  // Increase in the number of cards in the hand
        UnlockJob,  // Job release
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    #region UI Object

    /// <summary>
    /// Item Icon
    /// </summary>
    [SerializeField]
    private Image iconImage = null;
    /// <summary>
    /// Item name Text
    /// </summary>
    [SerializeField]
    private Text nameText = null;
    /// <summary>
    /// Item explain Text
    /// </summary>
    [SerializeField]
    private Text explainText = null;
    /// <summary>
    /// Obtain button
    /// </summary>
    [SerializeField]
    private Button obtainButton = null;
    /// <summary>
    /// Price Text
    /// </summary>
    [SerializeField]
    private Text priceText = null;

    #endregion UI Object

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Amount of experience required per number of training sessions for increasing the number of cards in hand
    /// (手札枚数上昇の訓練回数ごとの必要経験値量)
    /// </summary>
    public static readonly int[] TrainPriceHandNumUP = new int[] { 100, 1000, 5000, 10000, 50000 };
    /// <summary>
    /// Type of this item
    /// </summary>
    public TrainingMode trainingMode;
    /// <summary>
    /// For job release items: Job data to which this item corresponds
    /// </summary>
    public JobDataDefine.JobType jobType;
    /// <summary>
    /// Amount of EXP required to get item
    /// </summary>
    public int price;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Amount of experience required to increase maximum strength (multiply current maximum strength by this constant)
    /// </summary>
    private const int TrainPriceMaxHPUP = 5;

    /// <summary>
    /// Training window class
    /// </summary>
    private TrainingWindow trainingWindow;

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
    /// Initialization (called from TrainingWindow.cs)
    /// </summary>
    /// <param name="trainingWindow"></param>
    /// <param name="trainingMode"></param>
    /// <param name="jobType"></param>
    public void Init(TrainingWindow trainingWindow, TrainingMode trainingMode, JobDataDefine.JobType jobType = JobDataDefine.JobType.None)
    {
        this.trainingWindow = trainingWindow;
        this.trainingMode = trainingMode;
        this.jobType = jobType;
        // Initialize UI
        RefreshUI();
    }
    /// <summary>
    /// Update UI display
    /// </summary>
    public void RefreshUI()
    {
        switch (trainingMode)
        {
            case TrainingMode.MaxHPUP:
                // Maximum strength increase
                // Display Text
                if (Data.nowLanguage == SystemLanguage.Japanese)
                {
                    nameText.text = "最大体力上昇";
                    explainText.text = $"レイヤーの最大体力が上昇します。\n(現在の最大体力:{Data.instance.playerMaxHP})";
                }
                else if (Data.nowLanguage == SystemLanguage.English)
                {
                    nameText.text = "Grow Max HP";
                    explainText.text = $"The maximum strength of the player is increased.\n(Current max HP:{Data.instance.playerMaxHP})";
                }
                // Required EXP amount
                price = Data.instance.playerMaxHP * TrainPriceMaxHPUP;
                priceText.text = price.ToString("#,0");
                CheckPrice();
                break;
            case TrainingMode.HandNumUP:
                // Increase in the number of cards in hand
                // Display Text
                if (Data.nowLanguage == SystemLanguage.Japanese)
                {
                    nameText.text = "手札枚数上昇";
                    explainText.text = $"ターン開始時の手札枚数が増加します。\n(現在{Data.instance.playerHandNum}枚)";
                }
                else if (Data.nowLanguage == SystemLanguage.English)
                {
                    nameText.text = "Grow Hand Num";
                    explainText.text = $"The number of cards in hand at the beginning of the turn is increased.\n(currently {Data.instance.playerHandNum} cards)";
                }
                // Required EXP amount
                price = TrainPriceHandNumUP[trainingWindow.GetHandNumTrainCount()];
                priceText.text = price.ToString("#,0");
                CheckPrice();
                break;
            case TrainingMode.UnlockJob:
                // Job release
                var jobData = JobDataDefine.DicJobData[JobDataDefine.GetJobTypeByInt((int)jobType)];
                // Job name Text
                if (Data.nowLanguage == SystemLanguage.Japanese)
                {
                    nameText.text = "職業解放：\n" + jobData.nameJP;
                    explainText.text = jobData.explainJP;
                }
                else if (Data.nowLanguage == SystemLanguage.English)
                {
                    nameText.text = "Job unlock：\n" + jobData.nameEN;
                    explainText.text = jobData.explainEN;
                }
                // Required EXP amount
                price = jobData.requireEXP;
                priceText.text = price.ToString("#,0");
                CheckPrice();
                break;
        }
    }
    /// <summary>
    /// Set icon image to be displayed
    /// </summary>
    /// <param name="sprite"></param>
    public void SetIcon(Sprite sprite)
    {
        if (sprite == null) iconImage.color = Color.clear;
        else
        {
            iconImage.color = Color.white;
            iconImage.sprite = sprite;
        }
    }
    /// <summary>
    /// Determine if there is enough EXP to get item
    /// (項目の取得に必要なEXPが足りているかを判定)
    /// </summary>
    public void CheckPrice()
    {
        if (Data.instance.playerEXP >= price)
            // Sufficiency
            obtainButton.interactable = true;
        else
            // Deficiency
            obtainButton.interactable = false;
    }

    /// <summary>
    /// Processing when the acquisition button is pressed
    /// </summary>
    public void ObtainButton() =>
        trainingWindow.ObtainItem(this);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
