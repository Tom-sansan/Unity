using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    #endregion

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
        animator.SetTrigger("Die");
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
            dollyCart.m_Speed = moveSpeed;
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
    /// Cinemachine movement process
    /// </summary>
    private void UpdateCinemachineMove()
    {
        if (smoothPath.PathLength <= dollyCart.m_Position) dollyCart.m_Position = 0;
        animator.SetFloat("Speed", dollyCart.m_Speed);
    }
    /// <summary>
    /// Attack
    /// </summary>
    private void ToAttack()
    {
        base.toAttack = true;
        dollyCart.m_Speed = 0;
    }
}
