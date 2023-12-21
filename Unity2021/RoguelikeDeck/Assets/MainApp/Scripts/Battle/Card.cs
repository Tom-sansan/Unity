using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Card Class
/// </summary>
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Variables

    #region Public Variables



    #endregion Public Variables

    #region Private Variables



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
    /// Executed at the start of a tap
    /// IPointerDownHandler is necessary
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Card is tapped.");
    }
    /// <summary>
    /// Executed at the end of a tap
    /// </summary>
    /// <param name="eventData">Tap info</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Tap to card has ended.");
    }

    #endregion Public Methods

    #region Private Methods



    #endregion Private Methods

    #endregion Methods
}
