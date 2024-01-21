using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Each product item UI processing class
/// </summary>
public class ShoppingItem : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Item Icons
    /// </summary>
    [SerializeField]
    private Image iconImage = null;
    /// <summary>
    /// Item name Text
    /// </summary>
    [SerializeField]
    private Text nameText = null;
    /// <summary>
    /// Item explanation Text
    /// </summary>
    [SerializeField]
    private Text explainText = null;
    /// <summary>
    /// Obtain button
    /// </summary>
    [SerializeField]
    private Button obtainButton = null;
    /// <summary>
    /// Price Text
    /// </summary>
    [SerializeField]
    private Text priceText = null;

    #endregion SerializeField

    #region Public Variables

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

    /// <summary>
    /// Processing when purchase button is pressed
    /// </summary>
    public void ObtainButton()
    {

    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
