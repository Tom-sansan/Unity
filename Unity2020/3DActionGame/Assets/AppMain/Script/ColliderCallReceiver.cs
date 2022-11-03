using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Utility class for receiving collider callbacks
/// </summary>
public class ColliderCallReceiver : MonoBehaviour
{
    /// <summary>
    /// Trigger event definition class
    /// </summary>
    public class TriggerEvent : UnityEvent<Collider> { }
    /// <summary>
    /// Trigger enter event
    /// </summary>
    public TriggerEvent TriggerEnterEvent = new TriggerEvent();
    /// <summary>
    /// Trigger stay event
    /// </summary>
    public TriggerEvent TriggerStayEvent = new TriggerEvent();
    /// <summary>
    /// Trigger exit event
    /// </summary>
    public TriggerEvent TriggerExitEvent = new TriggerEvent();

    /// <summary>
    /// Trigger enter callback
    /// </summary>
    /// <param name="other">Contacted collider</param>
    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterEvent?.Invoke(other);
    }
    /// <summary>
    /// Trigger stay callback
    /// </summary>
    /// <param name="other">Contacted collider</param>
    private void OnTriggerStay(Collider other)
    {
        TriggerStayEvent?.Invoke(other);
    }
    /// <summary>
    /// Trigger exit callback
    /// </summary>
    /// <param name="other">Contacted collider</param>
    private void OnTriggerExit(Collider other)
    {
        TriggerExitEvent?.Invoke(other);
    }
}
