using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        
    }

    void Update()
    {
        walletText.text = money.ToString("C0");
    }
}
