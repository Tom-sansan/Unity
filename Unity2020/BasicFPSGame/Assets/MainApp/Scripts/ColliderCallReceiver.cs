using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Utility class for receiving collider callback
/// </summary>
public class ColliderCallReceiver : MonoBehaviour
{
    #region Collider Event Definition Class
    /// <summary>
    /// Collider Event Definition Class
    /// </summary>
    [Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }
    /// <summary>
    /// Collider Enter Event
    /// </summary>
    public CollisionEvent CollisionEnterEvent = new CollisionEvent();
    /// <summary>
    /// Collider Stay Event
    /// </summary>
    public CollisionEvent CollisionStayEvent = new CollisionEvent();
    /// <summary>
    /// Collider Exit Event
    /// </summary>
    public CollisionEvent CollisionExitEvent = new CollisionEvent();
    #endregion

    #region Trigger Event Definition Class
    /// <summary>
    /// Trigger Event Definition Class
    /// </summary>
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }
    /// <summary>
    /// Trigger Enter Event
    /// </summary>
    public TriggerEvent TriggerEnterEvent = new TriggerEvent();
    /// <summary>
    /// Trigger Stay Event
    /// </summary>
    public TriggerEvent TriggerStayEvent = new TriggerEvent();
    /// <summary>
    /// Trigger Exit Event
    /// </summary>
    public TriggerEvent TriggerExitEvent = new TriggerEvent();
    #endregion

    #region Collision Event
    /// <summary>
    /// Collider Enter Callback
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) =>
        CollisionEnterEvent?.Invoke(collision);
    /// <summary>
    /// Collider Stay Callback
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay(Collision collision) =>
        CollisionStayEvent?.Invoke(collision);
    /// <summary>
    /// Collider Exit Callback
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) =>
        CollisionExitEvent?.Invoke(collision);
    #endregion

    #region Trigger Event
    /// <summary>
    /// Trigger Enter Callback
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) =>
        TriggerEnterEvent?.Invoke(other);
    /// <summary>
    /// Trigger Stay Callback
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other) =>
        TriggerStayEvent?.Invoke(other);
    /// <summary>
    /// Trigger Exit Callback
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) =>
        TriggerExitEvent?.Invoke(other);
    #endregion
}
