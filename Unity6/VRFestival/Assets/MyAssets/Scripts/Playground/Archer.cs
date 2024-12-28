using UnityEngine;

public class Archer : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Momentum(kgm/s)
    /// </summary>
    [SerializeField]
    private float momentum = 2.0f;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Specifies how far (in meters) from the arrow tail and the pullpoint of the bow-drawing device to initiate traction
    /// 矢の尾と弓を引く装置のpullpointの位置がどれくらいの距離（m）なら牽引を開始するか指定
    /// </summary>
    private const float hitRadius = 0.05f;
    /// <summary>
    /// Arrow tail
    /// 弓の尾
    /// </summary>
    private Transform arrowTail;
    /// <summary>
    /// Bow-drawing device
    /// 弓を引く装置
    /// </summary>
    private PullPoseDriver pullPoseDriver;
    /// <summary>
    /// Whether in tow or not
    /// 牽引中かどうか
    /// </summary>
    private bool isTracking = false;

    #endregion Private Variables

    #region Properties

    #endregion Properties

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {
        AdjustPullPointPosition();
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Set arrow tail
    /// </summary>
    public void SetArrowTail(Transform arrowTail)
    {
        if (pullPoseDriver == null) return;
        // Reset pullPoint position to initial position
        pullPoseDriver.ResetPosition();
        this.arrowTail = arrowTail;
        // Exclude the parent (arrow) of the arrow tail from the collision determination
        // 矢の尾の親（矢）を衝突判定から除外する
        IgnoreArrow(arrowTail.parent.gameObject);
    }
    /// <summary>
    /// Unset bow tails
    /// If in traction, stop traction
    /// 弓の尾を未設定にする
    /// 牽引中ならなら牽引を中止する
    /// </summary>
    public void ClearArrowTail()
    {
        if (isTracking)
        {
            // Return the pullPoint to its initial position and note the tow
            pullPoseDriver.ResetPosition();
            isTracking = false;
            Shoot();
        }
        arrowTail = null;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        pullPoseDriver = GetComponent<PullPoseDriver>();
    }
    /// <summary>
    /// Adjust pullPoint position
    /// </summary>
    private void AdjustPullPointPosition()
    {
        // If no bow tails are set, do nothing and return
        // 弓の尾が設定されていなければ何もしないで戻る
        if (arrowTail == null) return;
        // Decide whether to tow if not in tow with bow tail
        // 弓の尾で牽引中でなければ牽引するかどうかを決定する
        if (!isTracking)
        {
            // If the position of the bow tail and the pullpoint of the bow-drawing device are within the hitRadius
            // Pull the pullpoint with the tail of the arrow
            // 弓の尾と弓を引く装置の pullpoint の位置が hitRadius 内なら
            // 矢の尾で pullpoint を牽引する
            isTracking = (Vector3.Distance(pullPoseDriver.pullPoint.position, arrowTail.position) < hitRadius);
        }
        if (isTracking)
        {
            // Adjusting the pullpoint position of the bow puller as it is being towed
            // 牽引中なので弓を引く装置の pullpoint の位置を調整する
            pullPoseDriver.SetPosition(arrowTail.position, out bool canAdjustPullPoint);
            if (!canAdjustPullPoint)
            {
                // As not able to adjust the position of the pullPoint
                // Return the position of the pullPoint to the initial position and stop the towing
                // pullPoint の位置を調整出来なかったので
                // pullPoint の位置を初期位置に戻し牽引を中止する
                pullPoseDriver.ResetPosition();
                isTracking = false;
            }
        }
    }
    /// <summary>
    /// Exclude the parent (arrow) of the arrow tail from the collision determination
    /// 矢の尾の親（矢）を衝突判定から除外する
    /// </summary>
    /// <param name="arrow"></param>
    private void IgnoreArrow(GameObject arrow)
    {
        var arrowCollider = arrow.GetComponent<Collider>();
        var pullPointCollider = pullPoseDriver.pullPoint.GetComponent<Collider>();
        // Ignore the collision
        Physics.IgnoreCollision(arrowCollider, pullPointCollider);
    }
    /// <summary>
    /// Shoot arrow
    /// </summary>
    private void Shoot()
    {
        // Get traction
        var power = pullPoseDriver.Power;
        // Get the Rigitbody of the arrow's tail parent (arrow) and change it to free-fall
        // 矢の尾の親（矢）の Rigitbody を取り出し自由落下するように変更する
        var body = arrowTail.parent.gameObject.GetComponent<Rigidbody>();
        body.isKinematic = false;
        body.useGravity = true;
        // Adjust arrow position to avoid hitting the arrow
        // 矢に当たらないように矢の位置を調整
        body.gameObject.transform.position += arrowTail.forward * 1.0f;
        // Giving arrows the power to strike
        // 矢に撃力を与える
        body.AddForce(arrowTail.forward * power * momentum, ForceMode.Impulse);
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
