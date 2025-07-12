using UnityEngine;

/// <summary>
/// メインカメラの可動範囲をこのオブジェクトが持つスプライトの大きさで指定できるようにする処理を行う
/// (スプライトは透明でも可)
/// </summary>
public class CameraMovingLimitter : MonoBehaviour
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

    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// SpriteRenderer class
    /// </summary>
    private SpriteRenderer spriteRenderer;

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
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// スプライトの上下左右の端座標をRect型にして返す
    /// </summary>
    /// <returns>上下左右端座標のRect</returns>
    public Rect GetSpriteRect()
    {
        // 短形情報
        Rect result = new Rect();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        // オブジェクトのスプライト情報
        Sprite sprite = spriteRenderer.sprite;
        // 短形範囲を計算
        // Spriteサイズの半分を取得
        float halfSizeX = sprite.bounds.extents.x;
        float halfSizeY = sprite.bounds.extents.y;
        // 左上頂点座標を取得
        Vector3 topLeft = new Vector3(-halfSizeX, halfSizeY, 0.0f);
        topLeft = spriteRenderer.transform.TransformPoint(topLeft);
        // 右下頂点座標を取得
        Vector3 bottomRight = new Vector3(halfSizeX, -halfSizeY, 0.0f);
        bottomRight = spriteRenderer.transform.TransformPoint(bottomRight);
        //- Rectで短形範囲を呼出元に渡す
        result.xMin = topLeft.x;
        result.yMin = topLeft.y;
        result.xMax = bottomRight.x;
        result.yMax = bottomRight.y;
        return result;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Make sprite transparent
        spriteRenderer.color = Color.clear;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
