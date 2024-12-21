using UnityEngine;

/// <summary>
/// SlidingArea Class
/// </summary>
public class SlidingArea : MonoBehaviour
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

    /// <summary>
    /// SlideProvider
    /// 滑走（追跡）動作提供者
    /// </summary>
    public SlideProvider slideProvider;
    
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
