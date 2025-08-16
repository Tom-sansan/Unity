using DG.Tweening;
using UnityEngine;

/// <summary>
/// GimmicElectricalSwitch Class
/// Stage gimmic:Electrical Switch
/// Unlock the electric door when the player or the shot touches it
/// </summary>
public class GimmicElectricalSwitch : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Unlock target door
    /// </summary>
    [SerializeField]
    private GameObject targetDoor;
    /// <summary>
    /// Electrical icon that is on status
    /// </summary>
    [SerializeField]
    private Sprite iconSpriteOn;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const/Readonly Variables

    #endregion Public Const/Readonly Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Const/Readonly Variables

    #endregion Private Const/Readonly Variables

    #region Private Properties

    #endregion Private Properties
    /// <summary>
    /// SpriteRenderer of electrical icon
    /// </summary>
    private SpriteRenderer iconSpriteRenderer;
    /// <summary>
    /// Activated flag
    /// </summary>
    private bool isActivated;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        InitStart();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        // End if this has been already activated
        if (isActivated) return;
        // 
        string tag = collision.gameObject.tag;
        if (tag.Equals("Actor") || tag.Equals("Actor_Shot"))
        {
            // Activate switch
            isActivated = true;
            iconSpriteRenderer.sprite = iconSpriteOn;
            // Deletion animation for unlock target door
            targetDoor.transform
                .DOScale(0.0f, 1.0f)
                .OnComplete(() => Destroy(targetDoor));
        }
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize Start()
    /// </summary>
    private void InitStart()
    {
        // Get reference to SpriteRenderer that child object of 0 index has
        iconSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    #endregion Private Methods

    #endregion Methods
}
