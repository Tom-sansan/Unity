using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class EnemyTurtle : EnemyBase
{
    [Header("Sub Param")]
    /// <summary>
    /// Long-range attack collider call
    /// </summary>
    [SerializeField]
    private ColliderCallReceiver farAttackCall = null;
    /// <summary>
    /// Long-range attack rigid body
    /// </summary>
    [SerializeField]
    private Rigidbody farAttackRigid = null;
    /// <summary>
    /// Long range attack coroutine
    /// </summary>
    private Coroutine farAttackCor = null;
    /// <summary>
    /// Short range flag
    /// </summary>
    private bool isNear = true;

    protected override void Start()
    {
        base.Start();
        farAttackCall.TriggerEnterEvent.AddListener(OnFarAttackEnter);
    }


    protected override void Update()
    {
        base.Update();
    }
    /// <summary>
    /// Perimeter Radar Collider Stay Event Call
    /// </summary>
    /// <param name="collider"></param>
    protected override void OnAroundTriggerStay(Collider collider)
    {
        base.OnAroundTriggerStay(collider);
        if (collider.gameObject.tag == base.strPlayer)
        {
            var sqrMag = (this.transform.position - collider.gameObject.transform.position).sqrMagnitude;
            if (sqrMag > 3f) isNear = false;
            else isNear = true;
        }
    }
    /// <summary>
    /// Attack hit animation call
    /// </summary>
    protected override void Anim_AttackHit()
    {
        if (isNear) base.attackHitColliderCall.gameObject.SetActive(true);
        else
        {
            farAttackCall.gameObject.SetActive(true);
            var dir = (base.currentAttackTarget.position - this.transform.position);
            dir.y = 0f;
            farAttackRigid.AddForce(dir * 100f);

            if (farAttackCor != null)
            {
                StopCoroutine(farAttackCor);
                farAttackCor = null;
                ResetFarAttack();
            }
            farAttackCor = StartCoroutine(AutoErase());
        }
    }
    /// <summary>
    /// Call at the end of attack animation
    /// </summary>
    protected override void Anim_AttackEnd()
    {
        base.Anim_AttackEnd();
    }
    /// <summary>
    /// Long range attack collider enter call
    /// </summary>
    /// <param name="collider"></param>
    private void OnFarAttackEnter(Collider collider)
    {
        if (collider.gameObject.tag == base.strPlayer)
        {
            if (farAttackCor != null)
            {
                StopCoroutine(farAttackCor);
                farAttackCor = null;
                ResetFarAttack();
            }
            var player = collider.GetComponent<PlayerController>();
            player?.OnEnemyAttackHit(CurrentStatus.Power, this.transform.position);
            base.attackHitColliderCall.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Automatic deletion over time
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoErase()
    {
        yield return new WaitForSeconds(1f);
        ResetFarAttack();
    }
    /// <summary>
    /// Reset long-range attack
    /// </summary>
    private void ResetFarAttack()
    {
        farAttackCall.gameObject.SetActive(false);
        farAttackRigid.velocity = Vector3.zero;
        farAttackRigid.angularVelocity = Vector3.zero;
        var reset = Vector3.zero;
        reset.y = 0.4f;
        reset.z = 1f;
        farAttackCall.gameObject.transform.localPosition = reset;
    }
}
