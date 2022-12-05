using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wool : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    #endregion

    void Start()
    {
        _rigidbody2D.AddForce(Quaternion.Euler(0, 0, Random.Range(-15.0f, 15.0f)) * Vector2.up * 4, ForceMode2D.Impulse);
        transform.localScale = Vector3.one * Random.Range(0.4f, 1.5f);
    }

    void Update()
    {
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
}
