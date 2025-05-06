using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spinner Class
/// </summary>
public class Spinner : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// PlayerGraphics GameObject
    /// </summary>
    [SerializeField]
    private GameObject playerGraphics;
    /// <summary>
    /// Spinner Speed
    /// </summary>
    [SerializeField]
    public float spinnerSpeed = 3600.0f;
    /// <summary>
    /// Spin flag
    /// </summary>
    [SerializeField]
    private bool isSpin = false;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rb;
    #region Private Const Variables

    #endregion Private Const Variables

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

    void FixedUpdate()
    {
        SpinPlayer();
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Spin player
    /// </summary>
    private void SpinPlayer()
    {
        if (isSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0, spinnerSpeed * Time.deltaTime, 0));
        }
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
