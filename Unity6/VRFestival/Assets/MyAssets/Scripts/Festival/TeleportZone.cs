using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Climbing;

/// <summary>
/// TeleportZone Class
/// </summary>
public class TeleportZone : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

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

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Properties

    #endregion Private Properties

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

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
