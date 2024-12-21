using UnityEngine;

/// <summary>
/// PullPoseDriver Class
/// </summary>
public class PullPoseDriver : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Pull point of bow
    /// </summary>
    [SerializeField]
    public Transform pullPoint;
    /// <summary>
    /// Position of the Bone that moves the bowstring
    /// </summary>
    [SerializeField]
    private Transform stringPullPoint;
    /// <summary>
    /// Travel distance required for traction to be 1
    /// 牽引力が1となるために必要な移動距離
    /// </summary>
    [SerializeField]
    private float maxDistance = 1.0f;
    /// <summary>
    /// When the traction force is less than this value, do not check if the traction is in the ideal direction
    /// この値より小さい牽引力の時は、理想方向に牽引できているか確認しない
    /// </summary>
    [SerializeField]
    private float tolerancePower = 0.2f;
    /// <summary>
    /// If the angle is greater than this value, it is regarded as not being towed in the ideal direction
    /// この値より角度が大きい場合は理想方向に牽引できていないとみなす
    /// </summary>
    [SerializeField]
    private float maxTractionAngle = 20.0f;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Property

    /// <summary>
    /// Calculate and return traction
    /// </summary>
    public float Power => CalculateTraction(CalculateVectorTractionDirection(pullPoint.position));

    #endregion Property

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Initial position of pull point of bow
    /// </summary>
    private Vector3 origin;
    /// <summary>
    /// Initial position of the Bone that moves the bowstring
    /// </summary>
    private Vector3 stringPullPointOrigin;

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
        SetPullPointPosition();
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Reset pullpoint position to initial position
    /// </summary>
    public void ResetPosition() =>
        pullPoint.position = transform.TransformPoint(origin);
    /// <summary>
    /// Move the position of the pullPoint to the position specified by position (world coordinates)
    /// The pullPoint is moved only when the position is exactly where the string can be pulled.
    /// Returns true if the position is moved as specified, false otherwise
    /// pullPoint の位置を position （ワールド座標）で指定された位置に移動させる
    /// pullPoint を移動させるのは position が正確に弦を引ける位置の時のみ
    /// 指定通り位置を移動できたら true、そうでなければ false を返す
    /// </summary>
    /// <param name="position"></param>
    /// <param name="canAdjustPullPoint"></param>
    public void SetPosition(Vector3 position, out bool canAdjustPullPoint)
    {
        // Calculate the tow vector
        Vector3 traction = CalculateVectorTractionDirection(position);
        // Calculate traction（牽引力）
        float power = CalculateTraction(traction);
        if (power < tolerancePower || ValidTraction(traction))
        {
            // Adjust the position of the pullPoint so it can be pulled correctly
            // 正しく引けるので pullPoint の位置調整
            pullPoint.position = position;
            canAdjustPullPoint = true;
        }
        canAdjustPullPoint = false;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // Get the initial position of the pull point
        origin = transform.InverseTransformPoint(pullPoint.position);
        stringPullPointOrigin = transform.InverseTransformPoint(stringPullPoint.position);
    }
    /// <summary>
    /// Set pull point position
    /// </summary>
    private void SetPullPointPosition()
    {
        // Get initial position of PullPoint
        var traction = CalculateVectorTractionDirection(pullPoint.position);
        // Calculate traction
        var power = CalculateTraction(traction);
        stringPullPoint.position = power < tolerancePower || ValidTraction(traction) ?
                                   pullPoint.position :
                                   transform.TransformPoint(stringPullPointOrigin);
    }
    /// <summary>
    /// Calculate the vector in the pulling direction
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private Vector3 CalculateVectorTractionDirection(Vector3 position) =>
        // Subtracting the initial position from the current position determines the direction
        transform.InverseTransformPoint(position) - origin;
    /// <summary>
    /// Calculate traction
    /// </summary>
    /// <param name="traction"></param>
    /// <returns></returns>
    private float CalculateTraction(Vector3 traction) =>
        traction.magnitude / maxDistance;
    /// <summary>
    /// Return false if not pulled in the ideal direction
    /// </summary>
    /// <param name="traction"></param>
    /// <returns></returns>
    private bool ValidTraction(Vector3 traction)
    {
        // Calculate the angle between two vectors
        var angle = Vector3.Angle(traction, -Vector3.right);
        return angle < maxTractionAngle;
    }

    #endregion Private Methods



    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug

}
