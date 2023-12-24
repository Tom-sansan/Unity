using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Battle Manager Class
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region Variables

    #region Public Variables

    /// <summary>
    /// Field manager class
    /// </summary>
    [SerializeField]
    private FieldManager fieldManager;

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // Field manager initialization
        fieldManager.Init(this);
        Debug.Log(nameof(BattleManager) + ".cs : Complete initialization ");
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
