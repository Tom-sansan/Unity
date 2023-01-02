using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCrocodile : EnemyBase
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// Animator
    /// </summary>
    [SerializeField]
    private Animator animator = null;
    /// <summary>
    /// NavMeshAgent
    /// </summary>
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;
    /// <summary>
    /// Move target list
    /// </summary>
    [SerializeField]
    private List<Transform> moveTargets = new List<Transform>();
    #endregion

    #endregion

    protected override void Start()
    {
        base.Start();
        var target = GetMoveTarget();
        if (target != null) navMeshAgent.SetDestination(target.position);
    }

    protected override void Update()
    {
        base.Update();
        SetNext();
        animator.SetFloat("Speed", navMeshAgent.velocity.sqrMagnitude);
    }
    /// <summary>
    /// Dead process
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Dead");
        StartCoroutine(WaitDead());
    }
    /// <summary>
    /// Neighborhood collider staty call back
    /// </summary>
    /// <param name="collider"></param>
    public void OnNeighborhoodTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            base.targetPlayer = collider.gameObject;
            ToAttack();
            Debug.Log("Neighborhood trigger in Player");
        }
    }
    /// <summary>
    /// Callback when leaving the perimeter collider
    /// </summary>
    /// <param name="collider"></param>
    public void OnNeighborhoodTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Player"))
        {
            base.targetPlayer = null;
            base.toAttack = false;
            Debug.Log("Neighborhood trigger exit Player");
        }
    }
    /// <summary>
    /// Standby coroutine for destruction at death
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitDead()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    /// <summary>
    /// Randomly obtain a destination for movement
    /// </summary>
    /// <returns></returns>
    private Transform GetMoveTarget()
    {
        if (moveTargets == null || moveTargets.Count == 0) return null;
        var num = Random.Range(0, moveTargets.Count);
        return moveTargets[num];
    }
    /// <summary>
    /// Measuring distances and setting destinations
    /// </summary>
    private void SetNext()
    {
        if (base.toAttack || base.isAttacking) return;
        var dis = navMeshAgent.remainingDistance;
        if (dis < 0.5f)
        {
            var target = GetMoveTarget();
            if (target != null) navMeshAgent.SetDestination(target.position);
        }
        else if (navMeshAgent.isStopped)
        {
            // Resume from isStop
            navMeshAgent.isStopped = false;
            var target = GetMoveTarget();
            if (target != null) navMeshAgent.SetDestination(target.position);
        }
    }
    /// <summary>
    /// Attack to player
    /// </summary>
    private void ToAttack()
    {
        if (base.isAttacking || base.isDead) return;
        base.toAttack = true;
        navMeshAgent.SetDestination(base.targetPlayer.transform.position);
        var dis = navMeshAgent.remainingDistance;
        if (dis < 4f)
        {
            navMeshAgent.isStopped = true;
        }
        else navMeshAgent.isStopped = false;
    }
}
