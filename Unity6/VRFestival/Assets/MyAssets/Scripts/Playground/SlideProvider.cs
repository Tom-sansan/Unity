using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// SlideProvider Class
/// </summary>
public class SlideProvider : LocomotionProvider
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

    #region Property

    #endregion Property

    #endregion Public Variables

    #region Private Variables

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

    // private XRBaseLocomotionProvider locomotionProvider;

    #endregion Private Variables

    #region Properties

    #endregion Properties

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // Init();
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
        var xrOrigin = system.xrOrigin;
        if (xrOrigin == null) return;
        // Set target
        this.target = target;
        isSliding = true;
        // Store offset from sliding (tracking) to xrOrigin
        // 滑走（追跡）相手から xrOrigin までのオフセットを保存
        offset = xrOrigin.transform.position - target.position;
        // Release grip of sliding (tracking)
        // 滑走（追跡）相手の掴みを解除する
        var interactable = target.GetComponent<XRBaseInteractable>();
        interactable.interactionManager.CancelInteractableSelection(interactable as IXRSelectInteractable);
        // If the activity phase is not the move phase, make the activity phase the move start phase
        // 活動段階を移動段階でないなら、活動段階を移動開始段階にする
        if (locomotionPhase != LocomotionPhase.Moving)
            locomotionPhase = LocomotionPhase.Started;
        //  SetLocomotionState(LocomotionState.Preparing);
    }
    /// <summary>
    /// Stop sliding (tracking)
    /// 滑走（追跡）を停止する
    /// </summary>
    public void StopSliding() =>
        isSliding = false;

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    //private void Init()
    //{

    //}

    /// <summary>
    /// Update locomotionPhase depending on locomotionPhase
    /// </summary>
    [System.Obsolete]
    private void UpdateLocomotionPhase()
    {
        // When the activity phase reaches the completion phase, make the activity phase the waiting phase
        // 活動段階が完了段階になったら、活動段階を待ち段階にする
        if (locomotionPhase == LocomotionPhase.Done)
        {
            locomotionPhase = LocomotionPhase.Idle;
            // SetLocomotionState(LocomotionState.Idle);
            return;
        }
        // During sliding
        if (isSliding)
        {
            // If the activity phase is not in the move phase, try to see if it can be moved to the move phase.
            // 活動段階が移動段階でないなら、移動段階に移行できないか試す
            if (locomotionPhase != LocomotionPhase.Moving)
            {
                // Confirm that it's allowed to manage own activities
                // If can't get permission, wait until the next time
                // 自分が活動を管理していいかを確認
                // 許可がおりないなら次の機会まで待つ
                if (!BeginLocomotion()) return;
                // If allowed, move the activity phase to the moving phase
                //  許可されたので活動段階を移動段階に移行する。
                locomotionPhase = LocomotionPhase.Moving;
            }
            // 滑走（追跡）処理を実行
            // Execute sliding (tracking) process
            StepSliding();
        }
        else if (locomotionPhase != LocomotionPhase.Idle)
        {
            // 滑走（追跡）中でないのに、活動段階が待ち段階でないので完了段階に移行する
            // If not sliding (tracking), move the activity phase to the completion phase as the activity phase is not the waiting phase
            EndLocomotion();
            locomotionPhase = LocomotionPhase.Done;
        }
    }
    /// <summary>
    /// Process sliding
    /// Move XR Origin from the target position to the offset position
    /// XR Origin を、target の位置から offset の位置に移動させる
    /// </summary>
    [System.Obsolete]
    private void StepSliding()
    {
        // XR Origin が見つからなければ何もしない
        // If XR Origin is not found, do nothing
        var xrOrigin = system.xrOrigin;
        if (xrOrigin == null) return;
        // Move XR Origin from the target position to the offset position
        // XR Origin を、target の位置から offset の位置に移動させる
        var rigTransform = xrOrigin.transform;
        rigTransform.position = target.position + offset;
        // After reaching the ground, the sliding (tracking) process is terminated
        // 地面に到達後、滑走（追跡）処理を終了
        if (rigTransform.position.y < 0.1) StopSliding();
    }

    /// <summary>
    /// Set locomotion state
    /// * Include this method instead of using obsolete locomotionPhase property
    /// </summary>
    /// <param name="state"></param>
    private void SetLocomotionState(LocomotionState state)
    {
        typeof(LocomotionProvider).GetProperty("locomotionState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, state);
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug

}
