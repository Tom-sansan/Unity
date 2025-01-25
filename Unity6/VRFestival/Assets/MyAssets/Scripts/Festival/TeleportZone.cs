using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing;

/// <summary>
/// TeleportZone Class
/// </summary>
public class TeleportZone : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// ClimbInteractable attached to ladder
    /// </summary>
    [SerializeField]
    private ClimbInteractable ladder;
    /// <summary>
    /// Hook holder
    /// </summary>
    [SerializeField]
    private PlaceHolder hookHolder;
    /// <summary>
    /// Teleport destination
    /// </summary>
    [SerializeField]
    private TeleportDestination teleportDestination;

    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    /// <summary>
    /// OnTriggerEnter event
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // Skip processing of OnTriggerEnter event when the Ladder Game Object has already touched TeleportZoneIn at app startup
        // アプリ起動時に Ladder Game Object が TeleportZoneIn に既に触れているため、その際の OnTriggerEnter イベントの処理をスキップする
        if (!other.gameObject.CompareTag("Ladder"))
        {
            // Stop climbing the ladder
            // 梯子登りを止める
            ladder.interactionManager.CancelInteractableSelection(ladder as IXRSelectInteractable);
            // Move the hook to the destination
            // テレポート先に召喚
            teleportDestination.Summons();
            // Move the hook to the hook holder
            // フックの置き場所にフックも移動させる
            hookHolder.Placement();
        }
    }

    #endregion Unity Methods

    #endregion Methods
}
