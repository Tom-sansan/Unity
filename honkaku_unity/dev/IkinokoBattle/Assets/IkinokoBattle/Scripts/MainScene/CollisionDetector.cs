using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private TriggerEvent onTriggerEnter = new TriggerEvent();
    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke(other);
    }

    /// <summary>
    /// Called every time Is Trigger is ON and it's overlapped by other Colliders 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // Execute a process designated by onTriggerStay
        onTriggerStay.Invoke(other);
    }

    /// <summary>
    /// Shown on Inspector window by adding a class that inherits UnityEvent to attribute of [Serializable]
    /// </summary>
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {
    }
}
