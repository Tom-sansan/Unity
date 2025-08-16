using UnityEngine;

/// <summary>
/// ScrollEnter Class
/// トリガー範囲内に進入したキャラクターをスクロールさせる
/// </summary>
public class ScrollEnter : MonoBehaviour
{
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
    /// <summary>
    /// Sprite hidden wall
    /// </summary>
    [SerializeField]
    private GameObject hiddenWall;
    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        InitStart();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Actor")
        {
            targetAreaManager.ActivateArea();
            hiddenWall.SetActive(true);
        }
    }
    #endregion Unity Methods

    #region Private Methods
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
}
