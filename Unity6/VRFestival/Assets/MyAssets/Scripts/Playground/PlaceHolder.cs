using UnityEngine;

/// <summary>
/// PlaceHolder Class
/// </summary>
public class PlaceHolder : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    ///　GameObject to move to own position when Placement() is called
    /// Placement() が呼ばれたときに自分の位置に移動させる GameObject
    /// </summary>
    [SerializeField]
    private GameObject target;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

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

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Move GameObject specified by target to its own position
    /// target で指定される GameObject を自分の位置に移動させる
    /// </summary>
    public void Placement()
    {
        // Set position
        // 位置を合わせる
        target.transform.position = transform.position;
        // Set orientation
        // 向きを合わせる
        target.transform.rotation = transform.rotation;
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        Placement();
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
