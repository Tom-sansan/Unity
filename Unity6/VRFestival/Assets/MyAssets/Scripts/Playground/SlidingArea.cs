using UnityEngine;

/// <summary>
/// SlidingArea Class
/// </summary>
public class SlidingArea : MonoBehaviour
{
    #region Variables

    #region Public Variables

    /// <summary>
    /// SlideProvider
    /// 滑走（追跡）動作提供者
    /// </summary>
    public SlideProvider slideProvider;

    #endregion Public Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    /// <summary>
    /// Process when Collider is touched
    /// Colliderに触れた時の処理
    /// </summary>
    /// <param name="other"></param>
    /// <remarks>この実装で、Hook GameObject -> XR Grab Interactable -> Interactable Events -> Activated に付けた処理は不要になるが、後学のため残しておく</remarks>
    private void OnTriggerEnter(Collider other)
    {
        // Request sliding (tracking) action to the sliding (tracking) action provider
        // Pass the GameObject(that is the Hook GameObject) to which Rigidbody attached to Collider touched as the tracking partner
        // 滑走（追跡）動作提供者に滑走（追跡）動作を要求する
        // 追跡相手として Collider に触れた Rigidbody が取り付けられている GameObject を渡す
        slideProvider.StartSliding(other.attachedRigidbody.gameObject.transform);
    }
    #endregion Unity Methods

    #endregion Methods
}
