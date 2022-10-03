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

    private Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vec = Vector3.right * horizontal * Speed * Time.deltaTime;

        switch (CurrentType)
        {
            case Type.Transform:
                transform.position += vec;
                break;
            case Type.RigidBody_Force:
                rigidbody.AddForce(vec, ForceMode.Force);
                break;
            case Type.RigidBody_Impulse:
                rigidbody.AddForce(vec, ForceMode.Impulse);
                break;
            case Type.RigidBody_Accel:
                rigidbody.AddForce(vec, ForceMode.Acceleration);
                break;
            case Type.RigidBody_VelocityChange:
                rigidbody.AddForce(vec, ForceMode.VelocityChange);
                break;
            default:
                break;
        }
    }
}
