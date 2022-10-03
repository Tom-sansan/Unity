using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField, Range(0, 1000)]
    private float Speed = 1000f;

    public Vector2 Direction = new Vector2(0, 1);

    void Start()
    {
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(Direction.normalized * Speed * Time.deltaTime, ForceMode.Impulse);
    }

    void Update()
    {
        
    }
}
