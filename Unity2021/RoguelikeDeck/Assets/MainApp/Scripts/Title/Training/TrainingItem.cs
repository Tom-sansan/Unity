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
        HandNumUP   // increase in the number of cards in the hand
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
    public static readonly int[] TrainPriceHandUP = new int[] { 100, 1000, 5000, 10000, 50000 };
    /// <summary>
    /// Type of this item
    /// </summary>
    public TrainingMode trainingMode;
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

    public void Init(TrainingWindow trainingWindow, TrainingMode trainingMode)
    {
        this.trainingWindow = trainingWindow;
        this.trainingMode = trainingMode;
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
                break;
            case TrainingMode.HandNumUP:
                break;
            default:
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
