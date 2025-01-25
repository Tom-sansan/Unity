using UnityEngine;

/// <summary>
/// FollowCamera Class
/// </summary>
public class FollowCamera : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// 追従するカメラ
    /// </summary>
    [SerializeField]
    private Transform targetCamera;
    /// <summary>
    /// カメラからのオフセット
    /// </summary>
    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, 1);

    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {
        UpdateTransform();
    }
    #endregion Unity Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // VR 環境ではメインカメラを取得
        if (targetCamera == null)
        {
            targetCamera = Camera.main.transform;
        }

        if (targetCamera == null)
        {
            enabled = false;
            return;
        }
    }
    /// <summary>
    /// Update this Transform
    /// </summary>
    private void UpdateTransform()
    {
        // カメラの回転に合わせてテキストを回転
        transform.rotation = targetCamera.rotation;
        // カメラの正面にオフセット分移動
        transform.position = targetCamera.position + targetCamera.TransformDirection(offset);
    }
    #endregion Private Methods

    #endregion Methods
}
