using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Attack power
    /// </summary>
    public int Attack = 1;
    /// <summary>
    /// Auto-destructive coroutine
    /// </summary>
    private Coroutine autoDestroyCor = null;
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rigid = null;
    /// <summary>
    /// Flag for attack
    /// </summary>
    private bool isAttack = true;
    #endregion

    /// <summary>
    /// Generation-time callback
    /// </summary>
    public void OnCreated()
    {
        Init();
    }
    /// <summary>
    /// Shoot
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="forceMode"></param>
    public void Shoot(Vector3 direction, ForceMode forceMode)
    {
        rigid.useGravity = false;
        rigid.isKinematic = false;
        rigid.AddForce(direction, forceMode);
        // StartCoroutine(AutoDestroy(1f));
        autoDestroyCor = StartCoroutine(AutoDestroy(1f));
    }
    /// <summary>
    /// Collider enter
    /// </summary>
    /// <param name="collision"></param>
    public void OnArrowCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Enemy"))
        if (collision.gameObject.tag.Equals("Ground"))
        {
            if (isAttack)
            {
                rigid.isKinematic = true;
                rigid.velocity = Vector3.zero;
                rigid.angularVelocity = Vector3.zero;
                isAttack = false;
                Destroy(gameObject);
                Debug.Log("Ground or Enemy!!! " + collision.gameObject.name);
            }
        }
    }
    /// <summary>
    /// Handling enemy hits
    /// </summary>
    public void OnEnemyHit()
    {
        if (autoDestroyCor != null) StopCoroutine(autoDestroyCor);
        rigid.isKinematic = true;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        isAttack = false;
        Destroy(gameObject);
    }
    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Automatic destruction coroutine
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator AutoDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
