using UnityEngine;

/// <summary>
/// ActorGroundSensor Class
/// </summary>
public class ActorGroundSensor : MonoBehaviour
{
    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// Bool for checking on ground or not
    /// </summary>
    [HideInInspector]
    public bool IsGround = false;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Actor Controller
    /// </summary>
    private ActorController actorController;
    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        InitStart();
    }
    #endregion Unity Methods

    #region Private Methods
    /// <summary>
    /// Initialize this class
    /// </summary>
    private void InitStart()
    {
        actorController = GetComponentInParent<ActorController>();
    }
    /// <summary>
    /// OnTriggerStay2D Event
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision) =>
        CheckOnGround(collision, true);
    /// <summary>
    /// OnTriggerExit2D Event
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision) =>
        CheckOnGround(collision, false);
    /// <summary>
    /// Check and return if the actor is on the ground or not
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="isOnGround"></param>
    private void CheckOnGround(Collider2D collision, bool isOnGround)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Block")) IsGround = isOnGround;
    }
    #endregion Private Methods

    #endregion Methods
}
