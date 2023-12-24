using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Field Managefr Class
/// </summary>
public class FieldManager : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Card prefab
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab = null;
    /// <summary>
    /// Parent Transform of the card object to be generated
    /// </summary>
    [SerializeField]
    private Transform cardsParent = null;

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Main Camera
    /// </summary>
    public Camera mainCamera;
    /// <summary>
    /// RectTransform of Canvas
    /// </summary>
    public RectTransform canvasRectTransform;

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Battle manager
    /// </summary>
    private BattleManager battleManager;
    /// <summary>
    /// Card under drag operation
    /// </summary>
    private Card draggingCard;
    /// <summary>
    /// Generated player manipulation card list
    /// </summary>
    private List<Card> cardInstances;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {
        // Processing during drag operation
        if (draggingCard != null) UpdateDragging();
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Field Manager Initialization
    /// </summary>
    /// <param name="battleManager"></param>
    public void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        Debug.Log(nameof(FieldManager) + ".cs : Completed initialization");

        cardInstances = new List<Card>();
        // Create card object from card prefab under card parent
        var obj = Instantiate(cardPrefab, cardsParent);
        // Obtain card processing class and store in list
        var objCard = obj.GetComponent<Card>();
        cardInstances.Add(objCard);
        // Initialize Card class
        objCard.Init(this);
    }
    /// <summary>
    /// Start card dragging operation
    /// </summary>
    /// <param name="dragCard"></param>
    public void StartDragging(Card dragCard)
    {
        draggingCard = dragCard;
    }
    /// <summary>
    /// Terminate the card dragging operation
    /// </summary>
    public void EndDragging()
    {
        // Return the card to its original position
        draggingCard.BackToBasePos();
        // Post-processing
        draggingCard = null;
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Drag operation update process
    /// </summary>
    private void UpdateDragging()
    {
        // Get tap position
        Vector2 tapPos = Input.mousePosition;
        // Convert tap coordinates (screen coordinates → local coordinates of Canvas)
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            canvasRectTransform,    // RectTransform of Canvas
            tapPos,                 // source coordinate data
            mainCamera,             // Main Camera
            out tapPos              // Destination coordinate data
        );
        // Apply coordinates
        draggingCard.rectTransform.anchoredPosition = tapPos;
    }

    #endregion Private Methods

    #endregion Methods
}
