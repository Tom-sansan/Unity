using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

/// <summary>
/// TeleportDestination Class
/// </summary>
public class TeleportDestination : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

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
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Move the XR Origin (XR Rig) GameObject to this position and orientation
    /// 自分の位置・向きに XR Origin(XR Rig) GameObject を移動させる
    /// </summary>
    public void Summons()
    {
        // Use the first TeleportationProvider Component found in the Scene
        // Scene 内の TeleportationProvider Component を検索し最初に見つかったものを利用する
        var teleportationProvider = FindFirstObjectByType<TeleportationProvider>();
        if (teleportationProvider == null) return;
        // Set the teleport destination
        // テレポート先を作成し設定する
        var teleportRequest = new TeleportRequest
        {
            // Specify orientation to match specified direction
            // 向きを指定方向に合わせるよう指定
            matchOrientation = MatchOrientation.TargetUpAndForward,
            // Specify own location and orientation as the teleport destination
            // テレポート先として自分の位置・向きを指定
            destinationPosition = transform.position,
            destinationRotation = transform.rotation,
            // Specify the requested time
            // 要求した時間を指定
            requestTime = Time.time
        };
        // Request teleport to the found TeleportationProvider Component
        // 見つけた TeleportationProvider Component にテレポート要求
        teleportationProvider.QueueTeleportRequest(teleportRequest);
    }
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
