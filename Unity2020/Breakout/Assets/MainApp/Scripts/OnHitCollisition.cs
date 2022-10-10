using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnHitCollisition : MonoBehaviour
{
    [SerializeField]
    public UnityEvent OnEnter;

    [SerializeField]
    public string ObjName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == ObjName) OnEnter.Invoke();
    }
}
