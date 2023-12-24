using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Card Class
/// </summary>
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Variables

    #region Public Variables

    /// <summary>
    /// RectTransform
    /// </summary>
    public RectTransform rectTransform;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Field manger
    /// </summary>
    private FieldManager fieldManager;
    /// <summary>
    /// Move Tween
    /// </summary>
    private Tween moveTween;
    /// <summary>
    /// Basic coordinates (coordinates to return to after the end of the drag)
    /// </summary>
    private Vector2 basePos;
    /// <summary>
    /// Card movement animation time
    /// </summary>
    private const float MoveTime = 0.4f;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    void Start()
    {
        Debug.Log("Scene has started!");
    }

    void Update()
    {

    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="fieldManager"></param>
    public void Init(FieldManager fieldManager)
    {
        this.fieldManager = fieldManager;
        basePos = Vector2.zero;
    }
    /// <summary>
    /// Executed at the start of a tap
    /// IPointerDownHandler is necessary
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // Drag start process
        fieldManager.StartDragging(this);
        Debug.Log("Card is tapped.");
    }
    /// <summary>
    /// Executed at the end of a tap
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // Drag end process
        fieldManager.EndDragging();
        Debug.Log("Tap to card has ended.");
    }
    /// <summary>
    /// Move the card to the basic coordinates
    /// </summary>
    public void BackToBasePos()
    {
        // Stop any moving animations already in progress
        if (moveTween != null) moveTween.Kill();
        // 
        moveTween = rectTransform
            .DOMove(basePos, MoveTime)  // Moving Tween
            .SetEase(Ease.OutQuad);     // Specify how to change
    }

    #endregion Public Methods

    #region Private Methods



    #endregion Private Methods

    #endregion Methods
}
