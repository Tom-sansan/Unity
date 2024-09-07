using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// カメラが追従する対象オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject targetObject;

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
        // targetObjectPositionの位置を初期化
        targetObjectPosition = targetObject.transform.position;
    }
    /// <summary>
    /// Move camera
    /// </summary>
    private void MoveCamera()
    {
        // The camera also moves by the amount the target moves
        // targetの移動量分、カメラも移動する
        transform.position = targetObject.transform.position - targetObjectPosition;
        targetObjectPosition = targetObject.transform.position;
        // カメラが対象オブジェクトへ向く
        // Camera facing the target object
        transform.LookAt(targetObjectPosition);
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug

}
