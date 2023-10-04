using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Wallet Class
/// </summary>
public class Wallet : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Wallet text
    /// </summary>
    [SerializeField]
    private Text walletText;

    public BigInteger money;
    #endregion

    void Update()
    {
        walletText.text = money.ToString("C0");
    }
}
