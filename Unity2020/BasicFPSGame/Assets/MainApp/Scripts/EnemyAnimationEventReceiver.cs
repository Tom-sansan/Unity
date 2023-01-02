using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Enemy animation event receiver class
/// </summary>
public class EnemyAnimationEventReceiver : MonoBehaviour
{
    /// <summary>
    /// Event at the end of attack
    /// </summary>
    public UnityEvent AttackEndEvent = new UnityEvent();
    /// <summary>
    /// 
    /// </summary>
    private void Animation_AttackEnd()
    {
        AttackEndEvent?.Invoke();
        Debug.Log("Animation Attack End " + gameObject.name);
    }
}
