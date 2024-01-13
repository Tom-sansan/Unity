using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Title Scene Management Class
/// </summary>
public class TitleManager : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Stage select window class
    /// </summary>
    [SerializeField]
    private StageSelectWindow stageSelectWindow = null;
    /// <summary>
    /// Deck Edit Window Class
    /// </summary>
    [SerializeField]
    private DeckEditWindow deckEditWindow = null;
    /// <summary>
    /// Title Logo RectTransform
    /// </summary>
    [SerializeField]
    private RectTransform titleLogoRectTransform = null;
    /// <summary>
    /// Various button and button background RectTransform lists
    /// </summary>
    [SerializeField]
    private List<RectTransform> titleButtonUIs = null;
    /// <summary>
    /// Window Background Panel Object
    /// </summary>
    [SerializeField]
    private GameObject windowBackObject = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Amount of X-directional movement of various button-related UI
    /// </summary>
    private const float ButtonsUIMoveLengthY = 400.0f;
    /// <summary>
    /// Title logo performance time
    /// </summary>
    private const float TitleLogoTweenTime = 3.0f;
    /// <summary>
    /// Button-related UI performance time
    /// </summary>
    private const float ButtonsUITweenTime = 1.0f;
    /// <summary>
    /// Time interval to execute direction on various button-related UIs
    /// </summary>
    private const float ButtonsUITweenDistance = 0.15f;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Enable/disable window background panel object
    /// </summary>
    /// <param name="mode"></param>
    public void SetWindowBackPanelActive(bool mode) =>
        windowBackObject.SetActive(mode);

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        // Initialization of managed components
        stageSelectWindow.Init(this);
        deckEditWindow.Init(this);
        // Play animation at game startup
        InitAnimation();
        // Disable window background object
        SetWindowBackPanelActive(false);
    }
    /// <summary>
    /// Animation at game startup
    /// </summary>
    private void InitAnimation()
    {
        // Title logo Image
        Image titleLogoImage = titleLogoRectTransform.GetComponent<Image>();
        // Startup Animation Sequence
        Sequence initSequence = DOTween.Sequence();
        // Initial preparation before animation playback
        // Make the title logo large and transparent
        titleLogoRectTransform.transform.localScale = new Vector2(1.8f, 1.8f);
        titleLogoImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // Move various button-related UI to the bottom of the screen
        int buttonUIsLength = titleButtonUIs.Count;
        for (int i = 0; i < buttonUIsLength; i++)
            titleButtonUIs[i].anchoredPosition += new Vector2(0.0f, -ButtonsUIMoveLengthY);

        // Set animation
        // Change size of title logo
        initSequence.Append(titleLogoRectTransform.DOScale(1.0f, TitleLogoTweenTime));
        // Non-transparency of title logo
        initSequence.Join(titleLogoImage.DOFade(1.0f, TitleLogoTweenTime));
        // Move various button-related UI from the bottom of the screen
        for (int i = 0; i < buttonUIsLength; i++)
        {
            initSequence.Join(
                titleButtonUIs[i].DOAnchorPosY(ButtonsUIMoveLengthY, ButtonsUITweenTime)
                .SetRelative()  // Relative coordinates
                .SetDelay(ButtonsUITweenDistance)   // A little delay(少しずつ遅延していく)
            );
        }
    }

    #endregion Private Methods

    #endregion Methods
}
