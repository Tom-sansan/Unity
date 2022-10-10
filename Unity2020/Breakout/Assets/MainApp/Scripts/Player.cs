using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Type
    {
        Transform,
        RigidBody_Force,
        RigidBody_Impulse,
        RigidBody_Accel,
        RigidBody_VelocityChange
    }
    public Type CurrentType = Type.Transform;

    [Range(0, 100)]
    public float Speed = 1f;

    private Rigidbody rigidBody;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vec = Vector3.right * horizontal * Speed * Time.deltaTime;

        switch (CurrentType)
        {
            case Type.Transform:
                transform.position += vec;
                break;
            case Type.RigidBody_Force:
                rigidBody.AddForce(vec, ForceMode.Force);
                break;
            case Type.RigidBody_Impulse:
                rigidBody.AddForce(vec, ForceMode.Impulse);
                break;
            case Type.RigidBody_Accel:
                rigidBody.AddForce(vec, ForceMode.Acceleration);
                break;
            case Type.RigidBody_VelocityChange:
                rigidBody.AddForce(vec, ForceMode.VelocityChange);
                break;
            default:
                break;
        }
    }

    public void Stop()
    {
        enabled = false;
        transform.position = startPosition;
        rigidBody.velocity = Vector3.zero;
    }
}
