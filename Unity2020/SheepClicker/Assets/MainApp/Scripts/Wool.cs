using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Wool rigidbody
    /// </summary>
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    /// <summary>
    /// Wool SpriteRenderer
    /// </summary>
    [SerializeField]
    private SpriteRenderer woolSpriteRenderer;
    /// <summary>
    /// Coin prefab
    /// </summary>
    [SerializeField]
    private Coin coinPrefab;
    #endregion SerializeField

    #region Public Variables
    /// <summary>
    /// Wool color
    /// </summary>
    public Color woolColor;
    /// <summary>
    /// Wool sale price
    /// </summary>
    public int price = 100;
    #endregion Public Variables

    #endregion

    #region Unity Methods
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Wool sale processing
    /// </summary>
    /// <param name="wallet"></param>
    public void Sell(Wallet wallet)
    {
        var coin = Instantiate(coinPrefab, transform.position, transform.rotation);
        coin.value = price;
        coin.wallet = wallet;
        wallet.money += price;
        Destroy(gameObject);
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        _rigidbody2D.AddForce(Quaternion.Euler(0, 0, Random.Range(-15.0f, 15.0f)) * Vector2.up * 4, ForceMode2D.Impulse);
        transform.localScale = Vector3.one * Random.Range(0.4f, 1.5f);
        SetWool();
    }
    /// <summary>
    /// Set wool settings
    /// </summary>
    private void SetWool()
    {
        woolColor.a = 0.9f;
        woolSpriteRenderer.color = woolColor;
    }
    #endregion Private Methods
}
