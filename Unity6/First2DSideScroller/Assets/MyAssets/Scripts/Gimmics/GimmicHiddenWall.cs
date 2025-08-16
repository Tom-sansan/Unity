using UnityEngine;

/// <summary>
/// GimmicHiddenWall Class
/// Stage gimmic:Hidden wall
/// Set the class where Actor must not pass by
/// </summary>
public class GimmicHiddenWall : MonoBehaviour
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

    #region Public Const/Readonly Variables

    #endregion Public Const/Readonly Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Const/Readonly Variables

    #endregion Private Const/Readonly Variables

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
        GetComponent<SpriteRenderer>().color = Color.clear;
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
