using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Camera Class
/// </summary>
public class MainCamera : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Target object to be followed by the camera
    /// カメラが追従する対象オブジェクト（アバター）
    /// </summary>
    [SerializeField]
    private GameObject targetObject;
    /// <summary>
    /// Layers as obstacle
    /// </summary>
    [SerializeField]
    private LayerMask obstacleLayerMask;
    /// <summary>
    /// Speed at which the camera zooms
    /// </summary>
    [SerializeField]
    private float zoomSpeed = 100f;
    /// <summary>
    /// Minimum distance limit between camera and target object
    /// </summary>
    [SerializeField]
    private float minDistance = 1f;
    /// <summary>
    /// Maximum distance limit between camera and target object
    /// </summary>
    [SerializeField]
    private float maxDistance = 1f;
    /// <summary>
    /// Camera rotation speed
    /// </summary>
    [SerializeField]
    private float cameraRotateSpeed = 1000f;
    /// <summary>
    /// Minimum downward angle that limits the vertical rotation of the camera
    /// カメラの垂直回転を制限する下向きの最小角度
    /// </summary>
    [SerializeField]
    private float minVerticalAngle = -70f;
    /// <summary>
    /// Maximum upward angle that limits the vertical rotation of the camera
    /// カメラの垂直回転を制限する上向きの最大角度
    /// </summary>
    [SerializeField]
    private float maxVerticalAngle = 70f;
    /// <summary>
    /// The distance between camera and player, which is a condition for isNearObstacle to be false
    /// isNearObstacle を false にする条件となるカメラとプレイヤー間の距離
    /// </summary>
    [SerializeField]
    private float defaultDistance = 3f;
    /// <summary>
    /// Flag when the player is close to an obstacle
    /// プレイヤーが障害物に近い時のフラグ
    /// </summary>
    [SerializeField]
    private bool isNearObstacle = false;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Position of the target object that the camera follows
    /// カメラが追従する対象オブジェクトの位置
    /// </summary>
    private Vector3 targetObjectPosition;
    /// <summary>
    /// Information on collisions between light rays and objects
    /// 光線と物体の衝突情報
    /// </summary>
    private RaycastHit hit;
    /// <summary>
    /// Distance between camera and target object
    /// </summary>
    private float CameraTargetDistance;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    { 
        Init();
    }
    /// <summary>
    /// Method called after Update()
    /// </summary>
    void LateUpdate()
    {
        MoveCamera();
        ZoomInOut();
        RotateAroundAvatarByCamera();
        AvoidObstacle();
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        // Initialise targetObjectPosition position
        // targetObjectPosition の位置を初期化
        targetObjectPosition = targetObject.transform.position;
    }
    /// <summary>
    /// Move camera
    /// </summary>
    private void MoveCamera()
    {
        // When the player is away from an obstacle
        // プレイヤーが障害物から離れているとき
        if (!isNearObstacle)
        {
            // The camera also moves by the amount the target moves
            // targetの移動量分、カメラも移動する。下記右辺は1フレームの間にアバターが移動した差分。その差分だけ左辺のカメラの位置を動かす
            // (transform.position(カメラの位置) += targetObject.transform.position(現在アバターがいる位置)　- targetObjectPosition(前のフレームでアバターがいた位置))
            transform.position += targetObject.transform.position - targetObjectPosition;
        }
        targetObjectPosition = targetObject.transform.position;
        // Camera facing the target object
        // カメラが対象オブジェクトへ向く
        transform.LookAt(targetObjectPosition);
    }
    /// <summary>
    /// Zoom in and out functions
    /// </summary>
    private void ZoomInOut()
    {
        // Distance between camera and target object
        CameraTargetDistance = Vector3.Distance(transform.position, targetObjectPosition);
        // Amount of mouse wheel rotation(マウスホイールの回転量)
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        // If player is spinning the mouse wheel
        if (scroll != 0.0f)
        {
            // If the mouse wheel is turned up & The camera is further away from the target object than the shortest distance
            // マウスホイールが上に回されている & カメラが最短距離より対象オブジェクトから離れている
            if (scroll > 0 && CameraTargetDistance > minDistance)
            {
                // Camera moves in the direction of the target object and zooms in
                // カメラが対象オブジェクトの方向に移動してズームイン
                transform.position += transform.forward * zoomSpeed * Time.deltaTime;
            }
            // If the mouse wheel is turned down & The camera is closer to the target object than the shortest distance
            // マウスホイールが下に回されている & カメラが最短距離より対象オブジェクトに近づいている
            else if (scroll < 0 && CameraTargetDistance < maxDistance)
            {
                // Camera zooms out away from the direction of the target object
                // カメラが対象オブジェクトの方向から離れてズームアウト
                transform.position -= transform.forward * zoomSpeed * Time.deltaTime;
            }
        }
    }
    /// <summary>
    /// Camera rotates around avatar
    /// カメラがアバターの周りを回転する
    /// </summary>
    private void RotateAroundAvatarByCamera()
    {
        // Amount of mouse movement
        // マウスの移動量
        // Movement of mouse vertical direction
        var mouseInputX = Input.GetAxis("Mouse X");
        // Movement of mouse horizontal direction
        var mouseInputY = Input.GetAxis("Mouse Y");
        // If the left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            // Camera rotates (orbits) around the Y (horizontal) axis of the target object's position
            // 対象オブジェクトの位置のY軸を中心に、カメラが（縦）回転（公転）する
            transform.RotateAround(targetObjectPosition, Vector3.up, mouseInputX * Time.deltaTime * cameraRotateSpeed);
            // Camera rotates (orbits) (horizontally) around the position of the target object, with the axis to the right of the camera.
            // 対象オブジェクトの位置を中心に、カメラの右方向を軸としてカメラが（横）回転（公転）する
            transform.RotateAround(targetObjectPosition, transform.right, mouseInputY * Time.deltaTime * -cameraRotateSpeed);
            // Get the local forward vector of the current camera and calculates the angle to the horizontal plane
            // 現在のカメラのローカルな前方ベクトルを取得し、水平面に対する角度を計算
            var angleToHorizontalPlane = Vector3.Angle(transform.forward, Vector3.up) - 90f;
            // Calculate the new vertical angle of the camera based on the current angle and mouse input
            // 現在の角度とマウス入力に基づいてカメラの新しい垂直角度を計算する
            var updatedVerticalAngle = angleToHorizontalPlane + mouseInputY * Time.deltaTime * -10f;
            // If the vertical angle of the camera exceeds the limit range, it is corrected within the limit range
            // カメラの垂直角度が制限範囲を超える場合、制限範囲内に補正
            if (updatedVerticalAngle < minVerticalAngle)
                // Calculate the amount of mouse Y-axis input required to bring the camera back to the specified minimum downward angle
                // カメラを指定された下向きの最小角度まで戻す必要なマウスのY軸の入力量を計算
                mouseInputY = (minVerticalAngle - angleToHorizontalPlane) / (-10f * Time.deltaTime);
            else if (updatedVerticalAngle > maxVerticalAngle)
                // Calculate the amount of Y-axis mouse input required to bring the camera back to the maximum specified upward angle
                // カメラを指定された上向きの最大角度まで戻す必要なマウスのY軸の入力量を計算
                mouseInputY = (maxVerticalAngle - angleToHorizontalPlane) / (-10f * Time.deltaTime);
        }
        // Rotate the camera vertically around the target object using the corrected mouseInputY
        // 補正された mouseInputY を使用して、カメラを対象オブジェクトの周りで垂直方向に回転
        transform.RotateAround(targetObjectPosition, transform.right, mouseInputY * Time.deltaTime);
    }

    private void AvoidObstacle()
    {
        // If there is an obstacle between target object and camera
        // 対象オブジェクト(targetObjectPosition)とカメラ(transform.position)との間に障害物(obstacleLayerMask)がある場合
        if (Physics.Linecast(targetObjectPosition, transform.position, out hit, obstacleLayerMask))
        {
            // Move camera to the position of obstacle
            // 障害物の位置にカメラを移動させる
            transform.position = hit.point;
            isNearObstacle = true;
        }
        // Visual identification of the rays of light drawn between the camera and the target object
        // カメラと対象オブジェクトの間に引かれた光線を視覚的に確認できるようにする
        Debug.DrawLine(targetObjectPosition, transform.position, Color.red, 0f, false);
        // When camera and player are more than a certain distance apart.
        // カメラとプレイヤーが一定距離以上離れた場合
        if (CameraTargetDistance > defaultDistance) isNearObstacle = false;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug

}
