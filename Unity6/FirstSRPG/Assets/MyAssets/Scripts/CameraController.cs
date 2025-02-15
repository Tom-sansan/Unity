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
    /// カメラ回転速度
    /// Camera Rotation Speed
    /// </summary>
    [SerializeField]
    private float rotateSpeed = 30.0f;
    /// <summary>
    /// カメラズーム速度
    /// Camera Zoom Speed
    /// </summary>
    [SerializeField]
    private float zoomSpeed = 0.1f;
    /// <summary>
    /// Minimum field of view of camera
    /// カメラの最小の視野
    /// </summary>
    [SerializeField]
    private float zoomMIN = 40.0f;
    /// <summary>
    /// Maximum field of view of camera
    /// カメラの最大の視野
    /// </summary>
    [SerializeField]
    private float zoomMAX = 60.0f;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Main Camera
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// Camera rotation flag
    /// カメラ回転中フラグ
    /// </summary>
    private bool isCameraRotate;
    /// <summary>
    /// Rotation Direction Reversal Flag
    /// 回転方向反転フラグ
    /// </summary>
    private bool isMirror;
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
        UpdateCameraRotation();
        UpdateCameraZoom();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Process called when the camera move button is started to be pressed
    /// カメラ移動ボタンが押し始められた時に呼び出される処理
    /// </summary>
    /// <param name="rightMode">右向きフラグ(右移動ボタンから呼ばれた時trueになっている)</param>
    public void StartCameraRotate(bool rightMode)
    {
        // Turn on the flag during camera rotation
        // カメラ回転中フラグをON
        isCameraRotate = true;
        // Apply rotational direction reversal flag
        // 回転方向反転フラグを適用する
        isMirror = rightMode;
    }
    /// <summary>
    /// Process called when the camera move button is no longer pressed
    /// カメラ移動ボタンが押されなくなった時に呼び出される処理
    /// </summary>
    public void EndCameraRotate()
    {
        // Turn off the flag during camera rotation
        isCameraRotate = false;
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        mainCamera = Camera.main;
    }
    /// <summary>
    /// Update camera processing
    /// </summary>
    private void UpdateCameraRotation()
    {
        // Process for camera rotation
        // カメラ回転処理
        if (isCameraRotate)
        {
            // Caluculate rotation speed
            // 回転速度を計算する
            float speed = rotateSpeed * Time.deltaTime;
            // Speed reversal if the reversal flag for the direction of rotation is set
            // 回転方向反転フラグが立っているなら速度反転
            if (isMirror) speed *= -1.0f;
            // Rotate and move the camera around the position of the base point
            // 基点の位置を中心にカメラを回転移動させる
            transform.RotateAround
            (
                Vector3.zero,   // 基点の位置(0,0,0)
                Vector3.up,     // 回転軸(Y軸:上方向)
                speed           // 回転速度
            );
        }
    }
    /// <summary>
    /// Update camera zoom processing
    /// </summary>
    private void UpdateCameraZoom()
    {
        // If not multi-touch (two simultaneous touches), end processing
        // マルチタッチ(２点同時タッチ)でないなら終了
        if (Input.touchCount != 2) return;
        // Get touch information of two points
        // ２点のタッチ情報を取得する
        var touchOne = Input.GetTouch(0);
        var touchTwo = Input.GetTouch(1);
        // Caluculate the distance between two points one frame ahead
        // １フレーム前の２点間の距離を求める(Vector2.Distanceで２点間の距離を取得/deltaPositionには１フレーム前のタッチ位置が入っている)
        float previousTouchDistance = Vector2.Distance(touchOne.position - touchOne.deltaPosition, touchTwo.position - touchTwo.deltaPosition);
        // Caluculate the distance between the two current points
        // 現在の2点間の距離を求める
        float currentTouchDistance = Vector2.Distance(touchOne.position, touchTwo.position);
        // Zooms according to the amount of change in distance between two points (changes the field of view of the camera)
        // ２点間の距離の変化量に応じてズームする(カメラの視野の広さを変更する)
        float distanceMoved = previousTouchDistance - currentTouchDistance;
        mainCamera.fieldOfView += distanceMoved * zoomSpeed;
        // Keep the camera's field of view within a specified range
        // カメラの視野を指定の範囲に収める
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, zoomMIN, zoomMAX);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}

