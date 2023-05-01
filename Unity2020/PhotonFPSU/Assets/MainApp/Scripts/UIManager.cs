using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables

    #region Public
    // Ammunition Text
    public Text ammoText;
    #endregion

    #region Private

    #endregion

    #endregion

    #region Methods
    /// <summary>
    /// Update Text
    /// </summary>
    public void SetBulletsText(int ammoClip, int ammunition)
    {
        // Number of bullets in magasin / number of bullets in possession
        ammoText.text = ammoClip + "/" + ammunition;
    }
    #endregion
}
