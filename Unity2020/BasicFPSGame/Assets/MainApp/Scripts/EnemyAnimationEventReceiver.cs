using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Enemy animation event receiver class
/// </summary>
public class EnemyAnimationEventReceiver : MonoBehaviour
{
    /// <summary>
    /// Event at the attack
    /// </summary>
    public UnityEvent AttackHitEvent = new UnityEvent();
    /// <summary>
    /// Event at the end of attack
    /// </summary>
    public UnityEvent AttackEndEvent = new UnityEvent();

    /// <summary>
    /// At the animation event attack
    /// </summary>
    private void Animation_AttackHit()
    {
        AttackHitEvent?.Invoke();
        Debug.Log("Animation Hit. " + gameObject.name);
    }
    /// <summary>
    /// At the end of the animation event attack
    /// </summary>
    private void Animation_AttackEnd()
    {
        AttackEndEvent?.Invoke();
        Debug.Log("Animation Attack End " + gameObject.name);
    }
}
