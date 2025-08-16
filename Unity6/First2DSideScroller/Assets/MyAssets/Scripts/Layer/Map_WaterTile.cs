using UnityEngine;

/// <summary>
/// Map_WaterTile Class
/// </summary>
public class Map_WaterTile : MonoBehaviour
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// ActorController class
    /// </summary>
    [SerializeField]
    private ActorController actorController;
    #endregion SerializeField
    #endregion Variables

    #region Unity Methods
    private void OnTriggerStay2D(Collider2D collision)
    {
        SetWaterMode(collision, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SetWaterMode(collision, false);
    }
    #endregion Unity Methods

    #region Private Methods
    /// <summary>
    /// Set water mode
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="isWaterMode"></param>
    private void SetWaterMode(Collider2D collision, bool isWaterMode)
    {
        string tag = collision.gameObject.tag;
        // Meke Actor water mode or not
        if (tag == "Actor") actorController.SetWaterMode(isWaterMode);
    }
    #endregion Private Methods
}
