using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Enemy Base Class
/// </summary>
public class EnemyBase : MonoBehaviour
{
    #region Class
    /// <summary>
    /// Status Class
    /// </summary>
    [Serializable]
    public class Status
    {
        /// <summary>
        /// HP
        /// </summary>
        public int Hp = 10;
        /// <summary>
        /// Attack power
        /// </summary>
        public int Power = 1;
    }
    /// <summary>
    /// Event class for Enemy move
    /// </summary>
    public class EnemyEvent : UnityEvent<EnemyBase> { }
    #endregion

    #region Variables
    /// <summary>
    /// HP bar slider
    /// </summary>
    [SerializeField]
    private Slider hpBar = null;
    /// <summary>
    /// Attack interval
    /// </summary>
    [SerializeField]
    private float attackInterval = 3f;
    /// <summary>
    /// Peripheral radar collider call
    /// </summary>
    [SerializeField]
    private ColliderCallReceiver aroundColliderCall = null;
    /// <summary>
    /// Collider call for attack judgment
    /// </summary>
    [SerializeField]
    protected ColliderCallReceiver attackHitColliderCall = null;
    /// <summary>
    /// Collider of enemy
    /// </summary>
    [SerializeField]
    private Collider myCollider = null;
    /// <summary>
    /// Effect prefab at the time of attack hit
    /// </summary>
    [SerializeField]
    private GameObject hitParticlePrefab = null;
    /// <summary>
    /// Basic status
    /// </summary>
    [SerializeField]
    private Status DefaultStatus = new Status();
    /// <summary>
    /// Current status
    /// </summary>
    public Status CurrentStatus = new Status();
    /// <summary>
    /// Attack status flag
    /// </summary>
    public bool IsBattle = false;
    /// <summary>
    /// Evnet for destination setting
    /// </summary>
    public EnemyEvent ArrivalEvent = new EnemyEvent();
    /// <summary>
    /// Death animation end call
    /// </summary>
    public EnemyEvent DestroyEvent = new EnemyEvent();
    /// <summary>
    /// Nav mesh
    /// </summary>
    private NavMeshAgent navMeshAgent = null;
    /// <summary>
    /// Animator
    /// </summary>
    private Animator animator = null;
    /// <summary>
    /// Start position
    /// </summary>
    private Vector3 startPosition = new Vector3();
    /// <summary>
    /// Start rotation
    /// </summary>
    private Quaternion startRotation = new Quaternion();
    /// <summary>
    /// The current destination
    /// </summary>
    private Transform currentTarget = null;
    /// <summary>
    /// The current attack target
    /// </summary>
    protected Transform currentAttackTarget = null;
    /// <summary>
    /// Attack status flag
    /// </summary>
    // private bool isBattle = false;
    /// <summary>
    /// For measuring attack time
    /// </summary>
    private float attackTimer = 0f;

    protected string strPlayer = "Player";
    #endregion

    protected virtual void Start()
    {
        // Get and keep animator
        animator = GetComponent<Animator>();
        // First set the current status as the basic status
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        RegistEventListeners();
        attackHitColliderCall.gameObject.SetActive(false);
        // Store starting position rotation
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        // Initialize slider
        hpBar.maxValue = DefaultStatus.Hp;
        hpBar.value = CurrentStatus.Hp;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // When ready to attack
        if (IsBattle) OnAttackEvent();
        else OnMoveEvent();
    }
    /// <summary>
    /// Attack hit call
    /// </summary>
    /// <param name="damage"></param>
    public void OnAttackHit(int damage, Vector3 attackPosition)
    {
        CurrentStatus.Hp -= damage;
        hpBar.value = CurrentStatus.Hp;
        Debug.Log("Hit Damage " + damage + "/CurrentHp = " + CurrentStatus.Hp);

        var position = myCollider.ClosestPoint(attackPosition);
        var obj = Instantiate(hitParticlePrefab, position, Quaternion.identity);
        var particle = obj.GetComponent<ParticleSystem>();
        StartCoroutine(WaitDestroy(particle));

        if (CurrentStatus.Hp <= 0) OnDie();
        else animator.SetTrigger("isHit");
    }
    public void OnRetry()
    {
        // Initialize the current status
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        hpBar.value = CurrentStatus.Hp;
        // Returns the rotational position to the initial position
        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        // Re activate the enemy
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// Set the next destination for NavMesh
    /// </summary>
    /// <param name="target"></param>
    public void SetNextTarget(Transform target)
    {
        if (target == null) return;
        if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.SetDestination(target.position);
        currentTarget = target;
        Debug.Log(gameObject.name + " is moving to the target;" + target.gameObject.name);
    }
    /// <summary>
    /// Regist event listeners
    /// </summary>
    private void RegistEventListeners()
    {
        // Peripheral collider event registration
        aroundColliderCall.TriggerEnterEvent.AddListener(OnAroundTriggerEnter);
        aroundColliderCall.TriggerStayEvent.AddListener(OnAroundTriggerStay);
        aroundColliderCall.TriggerExitEvent.AddListener(OnAroundTriggerExit);
        // Attack collider event registration
        attackHitColliderCall.TriggerEnterEvent.AddListener(OnAttackTriggerEnter);
    }
    /// <summary>
    /// Enemy attack event
    /// </summary>
    private void OnAttackEvent()
    {
        attackTimer += Time.deltaTime;
        animator.SetBool("isRun", false);
        if (attackTimer >= attackInterval)
        {
            animator.SetTrigger("isAttack");
            attackTimer = 0;
        }
    }
    /// <summary>
    /// Enemy move event
    /// </summary>
    private void OnMoveEvent()
    {
        attackTimer = 0;
        // Measure distance to target and execute event
        if (currentTarget == null)
        {
            animator.SetBool("isRun", false);
            ArrivalEvent?.Invoke(this);
            Debug.Log(gameObject.name + " has started to move!");
        }
        else
        {
            var sqrDistance = (currentTarget.position - this.transform.position).sqrMagnitude;
            if (sqrDistance < attackInterval) ArrivalEvent?.Invoke(this);
        }
    }
    /// <summary>
    /// Peripheral radar collider enter event call
    /// </summary>
    /// <param name="collider"></param>
    private void OnAroundTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == strPlayer)
        {
            IsBattle = true;
            navMeshAgent.SetDestination(this.transform.position);
            currentTarget = null;
        }
    }
    /// <summary>
    /// Peripheral radar collider stay event call
    /// </summary>
    protected virtual void OnAroundTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == strPlayer)
        {
            var dir = (collider.gameObject.transform.position - this.transform.position).normalized;
            dir.y = 0;
            this.transform.forward = dir;
            currentAttackTarget = collider.gameObject.transform;
        }
    }
    /// <summary>
    /// Peripheral radar collider exit event call
    /// </summary>
    /// <param name="collider"></param>
    protected virtual void OnAroundTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == strPlayer)
        {
            IsBattle = false;
            currentAttackTarget = null;
        }
    }
    /// <summary>
    /// Attack collider enter event call
    /// </summary>
    /// <param name="collider"></param>
    private void OnAttackTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == strPlayer)
        {
            var player = collider.GetComponent<PlayerController>();
            player?.OnEnemyAttackHit(CurrentStatus.Power, this.transform.position);
            attackHitColliderCall.gameObject.SetActive(false);
            Debug.Log("Enemy's hit player!!" + CurrentStatus.Power + " attack by force!");
        }
    }
    /// <summary>
    /// Destroy particle when it ends
    /// </summary>
    /// <param name="particle"></param>
    /// <returns></returns>
    private IEnumerator WaitDestroy(ParticleSystem particle)
    {
        yield return new WaitUntil(() => particle.isPlaying == false);
        Destroy(particle.gameObject);
    }
    /// <summary>
    /// Death call
    /// </summary>
    private void OnDie()
    {
        animator.SetBool("isDie", true);
        Debug.Log("Death");
    }
    /// <summary>
    /// Attack hit animation call
    /// </summary>
    protected virtual void Anim_AttackHit()
    {
        attackHitColliderCall.gameObject.SetActive(true);
    }
    /// <summary>
    /// Attack animation end call
    /// </summary>
    protected virtual void Anim_AttackEnd()
    {
        attackHitColliderCall.gameObject.SetActive(false);
    }
    /// <summary>
    /// Attack hit animation call
    /// </summary>
    //private void Amin_AttackHit()
    //{
    //    this.gameObject.SetActive(false);
    //}
    /// <summary>
    /// Death animation call
    /// </summary>
    private void Anim_DieEnd()
    {
        //this.gameObject.SetActive(false);
        //Destroy(gameObject);
        DestroyEvent?.Invoke(this);
    }
}
