using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C = Constant;

/// <summary>
/// Enemy Base
/// </summary>
public class EnemyBase : MonoBehaviour
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// Maximum HP
    /// </summary>
    //[SerializeField]
    protected float maxHp = 3f;
    #endregion
    /// <summary>
    /// App Game Controller
    /// </summary>
    protected AppGameController gameController = null;
    /// <summary>
    /// Target
    /// </summary>
    protected GameObject targetPlayer = null;
    /// <summary>
    /// Current HP
    /// </summary>
    // protected int hp = 0;
    protected float hp = 0;
    /// <summary>
    /// Attack power
    /// </summary>
    protected float attack = 1f;
    /// <summary>
    /// Attack posture flag
    /// </summary>
    protected bool toAttack = false;
    /// <summary>
    /// Attacking flag
    /// </summary>
    protected bool isAttacking = false;
    /// <summary>
    /// Flag for dead
    /// </summary>
    protected bool isDead = false;
    /// <summary>
    /// Flag for being attacked
    /// </summary>
    private bool canHit = true;
    #endregion

    protected virtual void Start()
    {
        // Init();
    }

    protected virtual void Update()
    {
        
    }
    /// <summary>
    /// Initialization
    /// </summary>
    public virtual void Init(AppGameController appGameManager, int maxHp, float attack)
    {
        this.maxHp = maxHp;
        this.attack = attack;
        hp = maxHp;
        gameController = appGameManager;
    }
    /// <summary>
    /// Collider enter process
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnEnemyColliderEnter(Collision collision)
    {
        if (isDead)
        {
            // Destroy if already dead
            Destroy(collision.gameObject);
            return;
        }
        if (collision.gameObject.tag.Equals(C.Arrow) && canHit)
        {
            // Acquire Arrow and perform processing when hit by an "Arrow" enemy
            var arrow = collision.gameObject.GetComponent<Arrow>();
            arrow.OnEnemyHit();
            // HP minus the attack power of the arrow
            // hp -= arrow.Attack;
            hp -= arrow.GetAttack();
            // Dead process
            if (hp <= 0) OnDead();
            else
            {
                Debug.Log("Hit " + gameObject.name + ". HP is " + hp + " left.");
                // Waiting time for next hit
                StartCoroutine(HitWait());
            }
        }
    }
    /// <summary>
    /// Dead process
    /// </summary>
    protected virtual void OnDead()
    {
        // Destroy(gameObject);
        isDead = true;
        gameController.OnDeadEnemy(this);
        Debug.Log(gameObject.name + " is beaten.");
    }
    /// <summary>
    /// Waiting process after an attack hit until the next attack hits
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitWait()
    {
        // Wait for specified time and return flag
        canHit = false;
        yield return new WaitForSeconds(0.5f);
        canHit = true;
    }
}
