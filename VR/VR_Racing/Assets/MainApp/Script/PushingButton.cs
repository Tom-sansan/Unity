using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Pushing Button for Smartphone Class
/// </summary>
public class PushingButton : MonoBehaviour
{
    /// <summary>
    /// Pushing event
    /// </summary>
    public UnityEvent PushingEvent = new UnityEvent();
    /// <summary>
    /// Pushing flag
    /// </summary>
    public bool IsPush = false;

    private void FixedUpdate()
    {
        if (IsPush) OnPushing();
    }
    /// <summary>
    /// Pointer down call
    /// </summary>
    public void OnPointerDown() =>
        IsPush = true;
    /// <summary>
    /// Pointer up call
    /// </summary>
    public void OnPointerUp() =>
        IsPush = false;
    /// <summary>
    /// Call back for Pushing
    /// </summary>
    private void OnPushing()
    {
        Debug.Log("Pushing!");
        PushingEvent?.Invoke();
    }

}
