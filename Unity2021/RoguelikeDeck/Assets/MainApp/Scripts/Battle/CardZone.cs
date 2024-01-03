using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Card Zone Class
/// </summary>
public class CardZone : MonoBehaviour
{
    #region Enum

    /// <summary>
    /// Zone type
    /// </summary>
    public enum ZoneType
    {
        Hand,
        PlayBoard0,
        PlayBoard1,
        PlayBoard2,
        PlayBoard3,
        PlayBoard4,
        Trash
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Zone type
    /// </summary>
    public ZoneType zoneType;

    #endregion Public Variables

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

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
