using UnityEngine;

/// <summary>
/// CameraController Class
/// </summary>
public class CameraController : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Background sprite Transform
    /// </summary>
    [SerializeField]
    Transform backGroundTransform;
    /// <summary>
    /// CameraMovingLimitter class
    /// </summary>
    [SerializeField]
    private CameraMovingLimitter cameraMovingLimitter;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const Variables
    /// <summary>
    /// Background scroll speed
    /// (Same as camera movement at 1.0)
    /// </summary>
    public const float BG_Scroll_Speed = 0.5f;
    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Root point coordinates
    /// </summary>
    private Vector2 basePos;
    /// <summary>
    /// Camera position
    /// </summary>
    private Vector3 cameraPos;
    /// <summary>
    /// Background position
    /// </summary>
    private Vector3 backGroundPos;
    /// <summary>
    /// 有効中のカメラ移動制限範囲
    /// </summary>
    private Rect limitQuad;
    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Awake()
    {
        InitAwake();
    }
    void Start()
    {
        InitStart();
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        FixedUpdateCamera();
        FixedUpdateBackground();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Move camera position
    /// </summary>
    /// <param name="targetPos"></param>
    public void SetPosition(Vector2 targetPos)
    {
        basePos = targetPos;
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize Awake()
    /// </summary>
    private void InitAwake()
    {

    }
    /// <summary>
    /// Initialize Start()
    /// </summary>
    private void InitStart()
    {
        // カメラ可動制限範囲を取得
        limitQuad = cameraMovingLimitter.GetSpriteRect();
    }
    /// <summary>
    /// Fixed update Camera
    /// </summary>
    private void FixedUpdateCamera()
    {
        // Move camera position
        cameraPos = transform.localPosition;
        // Correct the X and Y coordinates to reflect the actor's current position slightly to the upper right
        cameraPos.x = basePos.x + 2.5f;
        cameraPos.y = basePos.y + 1.5f;
        // Z coordinate is the current value (transform.localPosition)

        // カメラ可動範囲を反映
        // x方向の可動範囲
        cameraPos.x = Mathf.Clamp(cameraPos.x, limitQuad.xMin, limitQuad.xMax);
        // y方向の可動範囲
        cameraPos.y = Mathf.Clamp(cameraPos.y, limitQuad.yMin, limitQuad.yMax);
        // Reflects calculated camera coordinates
        transform.localPosition = Vector3.Lerp(transform.localPosition, cameraPos, 0.08f);
    }
    /// <summary>
    /// Fixed update Background
    /// </summary>
    private void FixedUpdateBackground()
    {
        backGroundPos = transform.localPosition * BG_Scroll_Speed;
        backGroundPos.z = backGroundTransform.position.z;
        backGroundTransform.position = backGroundPos;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
