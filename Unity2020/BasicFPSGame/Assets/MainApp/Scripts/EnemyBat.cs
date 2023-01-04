using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using C = Constant;

/// <summary>
/// Enemy Bat Class
/// </summary>
public class EnemyBat : EnemyBase
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// Animator
    /// </summary>
    [SerializeField]
    private Animator animator = null;
    /// <summary>
    /// CinemachineSmoothPath
    /// </summary>
    [SerializeField]
    private CinemachineSmoothPath smoothPath = null;
    /// <summary>
    /// CinemachineDollyCart
    /// </summary>
    [SerializeField]
    private CinemachineDollyCart dollyCart = null;
    /// <summary>
    /// Moving speed
    /// </summary>
    [SerializeField]
    private float moveSpeed = 1f;
    /// <summary>
    /// Flight speed of long-range attacks
    /// </summary>
    [SerializeField]
    private float attackSphereSpeed = 50f;
    /// <summary>
    /// Attack point
    /// </summary>
    [SerializeField]
    protected Transform attackPoint = null;
    /// <summary>
    /// Attack-determination prefab
    /// </summary>
    [SerializeField]
    protected GameObject attackSphere = null;
    #endregion
    /// <summary>
    /// Flag to avoid duplicate attacks
    /// </summary>
    private bool canAttack = true;
    #endregion

    protected override void Start()
    {
        base.Start();
        dollyCart.m_Speed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        UpdateCinemachineMove();
    }
    /// <summary>
    /// Dead process
    /// </summary>
    protected override void OnDead()
    {
        base.OnDead();
        dollyCart.m_Speed = 0;
        animator.SetTrigger(C.Die);
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
            dollyCart.m_Speed = moveSpeed;
            var bat_red = animator.GetComponent<Transform>();
            bat_red.localPosition = Vector3.zero;
            bat_red.localRotation = Quaternion.identity;
            Debug.Log("Neighborhood trigger exit Player");
        }
    }
    /// <summary>
    /// Creation of attack determination
    /// </summary>
    public void CreateAttackSphere()
    {
        if (!canAttack) return;
        canAttack = false;

        var go = Instantiate(attackSphere, attackPoint.position, attackPoint.rotation, attackPoint);
        var sSphere = go.GetComponent<AttackSphere>();
        sSphere.Init();
        // Launch Sphere forward
        var rigid = go.GetComponent<Rigidbody>();
        if (rigid != null && targetPlayer != null)
        {
            var bat = animator.gameObject.transform;
            var targetTransform = targetPlayer.transform.position;
            var dir = (targetTransform - bat.position).normalized;
            rigid.AddForce(dir * attackSphereSpeed);
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
    /// Coroutine for attack interval
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(2f);
        base.isAttacking = false;
        base.toAttack = false;
        canAttack = true;
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
    /// Cinemachine movement process
    /// </summary>
    private void UpdateCinemachineMove()
    {
        if (smoothPath.PathLength <= dollyCart.m_Position) dollyCart.m_Position = 0;
        animator.SetFloat(C.Speed, dollyCart.m_Speed);
    }
    /// <summary>
    /// Attack posture
    /// </summary>
    private void ToAttack()
    {
        if (isDead) return;
        base.toAttack = true;
        dollyCart.m_Speed = 0;
        if (isAttacking) return;

        var bat_red = animator.GetComponent<Transform>();
        animator.SetTrigger(C.Attack);
        var targetTransform = targetPlayer.transform.position;
        targetTransform.y = bat_red.position.y;
        var dir = (targetTransform - bat_red.position).normalized;
        bat_red.forward = dir;
        isAttacking = true;
    }
}
