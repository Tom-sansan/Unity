using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Utility class for receiving collider callbacks
/// </summary>
public class ColliderCallReceiver : MonoBehaviour
{
    // Collider event class
    public class CollisionEvent : UnityEvent<Collision> { }
    // Collider enter event
    public CollisionEvent CollisionEnterEvent = new CollisionEvent();
    // Collider stay event
    public CollisionEvent CollisionStayEvent = new CollisionEvent();
    // Collider exit event
    public CollisionEvent CollisionExitEvent = new CollisionEvent();

    // Trigger event class
    public class TriggerEvent : UnityEvent<Collider> { }
    // Trigger enter event
    public TriggerEvent TriggerEnterEvent = new TriggerEvent();
    // Trigger stay event
    public TriggerEvent TriggerStayEvent = new TriggerEvent();
    // Trigger exit event
    public TriggerEvent TriggerExitEvent = new TriggerEvent();

    void Start()
    {
        
    }

    /// <summary>
    /// Collider enter callback
    /// </summary>
    /// <param name="col"> Contacted collider </param>
    void OnCollisionEnter(Collision col)
    {
        CollisionEnterEvent?.Invoke(col);
    }
    /// <summary>
    /// Collider stay callback
    /// </summary>
    /// <param name="col"> Contacted collider </param>
    void OnCollisionStay(Collision col)
    {
        CollisionStayEvent?.Invoke(col);
    }
    /// <summary>
    /// Collider exit callback
    /// </summary>
    /// <param name="col"> Contacted collider </param>
    void OnCollisionExit(Collision col)
    {
        CollisionExitEvent?.Invoke(col);
    }
    /// <summary>
    /// Trigger enter callback
    /// </summary>
    /// <param name="other"> Contacted collider </param>
    void OnTriggerEnter(Collider other)
    {
        TriggerEnterEvent?.Invoke(other);
    }
    /// <summary>
    /// Trigger stay callback
    /// </summary>
    /// <param name="other"> Contacted collider </param>
    void OnTriggerStay(Collider other)
    {
        TriggerStayEvent?.Invoke(other);
    }
    /// <summary>
    /// Trigger exit callback
    /// </summary>
    /// <param name="other"> Contacted collider </param>
    void OnTriggerExit(Collider other)
    {
        TriggerExitEvent?.Invoke(other);
    }
}
