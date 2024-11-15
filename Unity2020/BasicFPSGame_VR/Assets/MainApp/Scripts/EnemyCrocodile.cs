using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using C = Constant;

public class EnemyCrocodile : EnemyBase
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// Attack position
    /// </summary>
    [SerializeField]
    protected Transform attackPoint = null;
    /// <summary>
    /// Attack-determination prefab
    /// </summary>
    [SerializeField]
    protected GameObject attackSphere = null;
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
    // [SerializeField]
    private List<Transform> moveTargets = new List<Transform>();
    /// <summary>
    /// The number of move point
    /// </summary>
    [SerializeField]
    private int movePointCount = 5;
    #endregion

    #endregion

    protected override void Start()
    {
        base.Start();
        // var target = GetMoveTarget();
        // if (target != null) navMeshAgent.SetDestination(target.position);
    }

    protected override void Update()
    {
        base.Update();
        SetNext();
        animator.SetFloat(C.Speed, navMeshAgent.velocity.sqrMagnitude);
    }
    /// <summary>
    /// Dead process
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();
        navMeshAgent.SetDestination(transform.position);
        navMeshAgent.isStopped = true;
        animator.SetTrigger(C.Dead);
        StartCoroutine(WaitDead());
    }
    /// <summary>
    /// Neighborhood collider staty call back
    /// </summary>
    /// <param name="collider"></param>
    public void OnNeighborhoodTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag.Equals(C.Player))
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
        if (collider.gameObject.tag.Equals(C.Player))
        {
            base.targetPlayer = null;
            base.isAttacking = false;
            base.toAttack = false;
            Debug.Log("Neighborhood trigger exit Player");
        }
    }
    /// <summary>
    /// Animation end registration process
    /// </summary>
    public void OnAttackEnd()
    {
        StartCoroutine(WaitAttack());
    }
    /// <summary>
    /// Generation of attack point
    /// </summary>
    public void CreateAttackSphere()
    {
        var go = Instantiate(attackSphere, attackPoint.position, attackPoint.rotation, attackPoint);
        var aSphere = go.GetComponent<AttackSphere>();
        // aSphere.Init();
        aSphere.Init(attack);
    }
    /// <summary>
    /// Set target
    /// </summary>
    public void SetTarget()
    {
        var target = GetRandomTarget(moveTargets);
        if (target != null) navMeshAgent.SetDestination(target.position);
    }
    /// <summary>
    /// Create destination list
    /// </summary>
    public void SetMoveTargetList(List<Transform> pointList)
    {
        if (pointList == null || pointList.Count == 0)
        {
            Debug.LogWarning("Please set a point of enemy movement");
            return;
        }
        else if (pointList.Count < movePointCount)
        {
            moveTargets = new List<Transform>(pointList);
            return;
        }
        else
        {
            var r = new System.Random();
            moveTargets = pointList.OrderBy(x => System.Guid.NewGuid()).Take(movePointCount).ToList();
            SetTarget();

            // Get the required number of points from the point list without duplication
            //for (int i = 0; i < movePointCount; i++)
            //{
            //    var point = GetRandomTarget(pointList);
            //    while (moveTargets.Contains(point))
            //    {
            //        point = GetRandomTarget(pointList);
            //        if (!moveTargets.Contains(point)) break;
            //    }
            //    moveTargets.Add(point);
            //}
            //SetTarget();
        }
    }
    /// <summary>
    /// Coroutine for attack interval
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(2f);
        isAttacking = false;
        toAttack = false;
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
    //private Transform GetMoveTarget()
    //{
    //    if (moveTargets == null || moveTargets.Count == 0) return null;
    //    var num = Random.Range(0, moveTargets.Count);
    //    return moveTargets[num];
    //}
    /// <summary>
    /// Get destination randomly
    /// </summary>
    /// <returns></returns>
    private Transform GetRandomTarget(List<Transform> list)
    {
        if (list == null || list.Count == 0) return null;
        var num = Random.Range(0, list.Count);
        return list[num];
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
            //var target = GetMoveTarget();
            //if (target != null) navMeshAgent.SetDestination(target.position);
            SetTarget();
        }
        else if (navMeshAgent.isStopped)
        {
            // Resume from isStopped
            navMeshAgent.isStopped = false;
            //var target = GetMoveTarget();
            //if (target != null) navMeshAgent.SetDestination(target.position);
            SetTarget();
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
            animator.SetTrigger(C.Attack);
            // Keep target player position
            var targetTransform = base.targetPlayer.transform.position;
            targetTransform.y = gameObject.transform.position.y;
            var dir = (targetTransform - gameObject.transform.position).normalized;
            // Face the player's direction
            transform.forward = dir;
            isAttacking = true;
        }
        else navMeshAgent.isStopped = false;
    }
}
