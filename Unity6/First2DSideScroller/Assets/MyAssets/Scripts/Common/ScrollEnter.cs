using UnityEngine;

/// <summary>
/// ScrollEnter Class
/// トリガー範囲内に進入したキャラクターをスクロールさせる
/// </summary>
public class ScrollEnter : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Camera class
    /// </summary>
    [SerializeField]
    private CameraController cameraController;
    /// <summary>
    /// AreaManger of Target Area
    /// </summary>
    [Header("AreaManger of Target Area")]
    [SerializeField]
    private AreaManager targetAreaManager;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Actor") targetAreaManager.ActivateArea();
    }
    #endregion Unity Methods

    #region Public Methods

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
        cameraController = Camera.main.gameObject.GetComponent<CameraController>();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
