using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField, Range(0, 1000)]
    private float Speed = 1000f;

    [SerializeField]
    private Vector2 Direction = new Vector2(0, 1);

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(Direction.normalized * Speed * Time.deltaTime, ForceMode.Impulse);
    }

    private void Update()
    {
        if (Mathf.Abs(rigidBody.velocity.y) <= 1)
            rigidBody.velocity += Vector3.up * 5f * (rigidBody.velocity.y >= 0 ? 1 : -1) * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) =>
        rigidBody.velocity = rigidBody.velocity.normalized * Speed * Time.deltaTime;
}
