using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Life Gauge class
/// </summary>
public class LifeGauge : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    private RectTransform _parentRectTransform;
    private Camera _camera;
    private MobStatus _status;

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }

    /// <summary>
    /// Initialize gauge
    /// </summary>
    /// <param name="parentRectTransform"></param>
    /// <param name="camera"></param>
    /// <param name="status"></param>
    public void Initialize(RectTransform parentRectTransform, Camera camera, MobStatus status)
    {
        // Receive and keep parameters for coordinate calculation
        _parentRectTransform = parentRectTransform;
        _camera = camera;
        _status = status;
        Refresh();
    }

    /// <summary>
    /// Refresh gauge
    /// </summary>
    private void Refresh()
    {
        // Display remaining life
        fillImage.fillAmount = _status.Life / _status.LifeMax;
        // Make gauge move to target Mob. Use RectTransformUtility when World or local coordinate is changed.
        var screenPoint = _camera.WorldToScreenPoint(_status.transform.position);
        Vector2 localPoint;
        // 今回はCanvasのRender ModeがScreen Space - Overlayなので第3引数にnullを指定している
        // Screen Space - Canvasの場合は対象のカメラを渡す必要がある
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, screenPoint, null, out localPoint);
        // ゲージがキャラに重なるので少し上にずらしている
        transform.localPosition = localPoint + new Vector2(0, 80);
    }
}
