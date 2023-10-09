using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game Manger Class
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Sell button
    /// </summary>
    [SerializeField]
    private Button sellButton;
    /// <summary>
    /// Wallet
    /// </summary>
    [SerializeField]
    private Wallet wallet;

    #region Unity Methods
    void Start()
    {
        sellButton.onClick.AddListener(SellAllWool);
    }
    #endregion Unity Methods

    #region Private Methods
    /// <summary>
    /// Sell all wool
    /// </summary>
    private void SellAllWool()
    {
        // All objects with Wool scripts on the screen are retrieved and stored in the Wool array woods.
        var wools = FindObjectsOfType<Wool>();
        foreach (var wool in wools) wool.Sell(wallet);
        SoundManager.Instance.Play(nameof(SoundManager.SoundType.Button));
    }
    #endregion Private Methods
}
