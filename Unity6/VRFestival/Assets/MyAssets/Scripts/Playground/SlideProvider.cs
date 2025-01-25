using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// SlideProvider Class
/// </summary>
public class SlideProvider : LocomotionProvider
{
    #region Variables

    #region Private Variables

    /// <summary>
    /// ログのプレフィックスを定義
    /// </summary>
    private const string LOG_PREFIX = nameof(DebugInVR);
    /// <summary>
    /// XROrigin
    /// </summary>
    private XROrigin xrOrigin;
    /// <summary>
    /// Target to track
    /// </summary>
    private Transform target;
    /// <summary>
    /// Offset from target (the person being tracked) to XR Origin at the start of the run
    /// 滑走開始時の target（追跡する相手）から XR Origin（自分自身）までのオフセット
    /// </summary>
    private Vector3 offset;
    /// <summary>
    /// During sliding(tracking)
    /// </summary>
    private bool isSliding = false;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }

    [System.Obsolete]
    void Update()
    {
        UpdateLocomotionPhase();
    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Start sliding(tracking)
    /// Move with the received target while maintaining the position of the target
    /// 受け取った target との位置を保って移動する
    /// </summary>
    /// <param name="target">Transform of target</param>
    public void StartSliding(Transform target)
    {
        if (xrOrigin == null || target == null) return;
        // If Locomotion is already active, do nothing
        // 既にLocomotionがアクティブの場合は何もしない
        if (isLocomotionActive) return;
        this.target = target;
        isSliding = true;
        // Store offset from sliding (tracking) to xrOrigin
        // 滑走（追跡）相手から xrOrigin までのオフセットを保存
        offset = xrOrigin.transform.position - target.position;
        var interactable = target.GetComponent<XRBaseInteractable>();
        //var selectInteractable = interactable as IXRSelectInteractable;
        //interactable.interactionManager?.CancelInteractableSelection(selectInteractable);
        if (interactable is IXRSelectInteractable selectInteractable)
        {
            var interactionManager = interactable.interactionManager;
            if (interactionManager != null)
            {
                // Release grip of sliding (tracking)
                // 滑走（追跡）相手の掴みを解除する
                interactionManager.CancelInteractableSelection(selectInteractable);
            }
        }
        // Locomotionを開始
        if (!TryPrepareLocomotion())
        {
            isSliding = false;
            return;
        }
    }
    /// <summary>
    /// Stop sliding (tracking)
    /// 滑走（追跡）を停止する
    /// </summary>
    public void StopSliding()
    {
        // Debug.Log($"{LOG_PREFIX} Start StopSliding");
        isSliding = false;
        if (isLocomotionActive) TryEndLocomotion();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // Get XROrigin in the scene
        // シーン内の XR Origin を取得
        xrOrigin = FindFirstObjectByType<XROrigin>();
        if (xrOrigin == null)
        {
            // Disable this script
            enabled = false;
            return;
        }
    }
    /// <summary>
    /// Update locomotionPhase
    /// </summary>
    private void UpdateLocomotionPhase()
    {
        if (isLocomotionActive && isSliding) StepSliding();
    }
    /// <summary>
    /// Process sliding
    /// Move XR Origin from the target position to the offset position
    /// XR Origin を、target の位置から offset の位置に移動させる
    /// </summary>
    private void StepSliding()
    {
        // XR Origin が見つからなければ何もしない
        // If XR Origin is not found, do nothing
        if (xrOrigin == null) return;
        // Move XR Origin from the target position to the offset position
        // XR Origin を、target の位置から offset の位置に移動させる
        // var rigTransform = xrOrigin.transform;
        xrOrigin.transform.position = target.position + offset;
        // After reaching the ground, the sliding (tracking) process is terminated
        // 地面に到達後、滑走（追跡）処理を終了
        if (xrOrigin.transform.position.y < 0.1) StopSliding();
    }

    #endregion Private Methods

    #endregion Methods
}
