using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    protected int maxHp = 3;
    #endregion
    /// <summary>
    /// Current HP
    /// </summary>
    protected int hp = 0;
    /// <summary>
    /// Flag for being attacked
    /// </summary>
    private bool canHit = true;
    #endregion

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        
    }
    /// <summary>
    /// Initialization
    /// </summary>
    public virtual void Init()
    {
        hp = maxHp;
    }
    /// <summary>
    /// Collider enter process
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnEnemyColliderEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Arrow") && canHit)
        {
            // Acquire Arrow and perform processing when hit by an "Arrow" enemy
            var arrow = collision.gameObject.GetComponent<Arrow>();
            arrow.OnEnemyHit();
            // HP minus the attack power of the arrow
            hp -= arrow.Attack;
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
        Destroy(gameObject);
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
