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

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Mathf.Abs(rigidBody.velocity.y) <= 1)
            rigidBody.velocity += Vector3.up * 5f * (rigidBody.velocity.y >= 0 ? 1 : -1);
    }

    public void Restart()
    {
        enabled = true;
        rigidBody.AddForce(Direction.normalized * Speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Stop()
    {
        enabled = false;
        transform.position = startPosition;
        rigidBody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision) =>
        rigidBody.velocity = rigidBody.velocity.normalized * Speed * Time.deltaTime;
}
