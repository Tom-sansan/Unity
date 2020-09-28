using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MobStatus))]
public class MobAttack : MonoBehaviour
{
    // Cool down after attack(seconds)
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private AudioSource swingSound;
    private MobStatus _status;

    // Start is called before the first frame update
    private void Start()
    {
        _status = GetComponent<MobStatus>();    
    }

    /// <summary>
    /// Attack if the status is attackable
    /// </summary>
    public void AttackIfPossible()
    {
        if (!_status.IsAttackable) return;
        _status.GoToAttackStateIfPossible();
    }

    /// <summary>
    /// Called when attack target is in attack area
    /// </summary>
    /// <param name="collider"></param>
    public void OnAttackRangeEnter(Collider collider) => AttackIfPossible();

    /// <summary>
    /// Called at the time of start of attack
    /// </summary>
    public void OnAttackStart()
    {
        attackCollider.enabled = true;
        if (swingSound != null)
        {
            // Play sound of swing
            // Make the sound a little different every time by changing pitch at ramdom
            swingSound.pitch = Random.Range(0.7f, 1.3f);
            swingSound.Play();
        }
    }

    public void OnHitAttack(Collider collider)
    {
        var targetMob = collider.GetComponent<MobStatus>();
        
        if (null == targetMob) return;
        // Damage to Player
        targetMob.Damage(collider.tag, 1);
    }

    /// <summary>
    /// Called at the time of end of attack
    /// </summary>
    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        _status.GoToNormalStateIfPossible();
    }
}
