using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C = Constant;

public class Arrow : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// Effect prefab
    /// </summary>
    [SerializeField]
    private GameObject effectPrefab = null;
    /// <summary>
    /// Attacking power
    /// </summary>
    [SerializeField]
    private int attack = 1;
    /// <summary>
    /// Attack power
    /// </summary>
    /// public int Attack = 1;
    /// <summary>
    /// Auto-destructive coroutine
    /// </summary>
    private Coroutine autoDestroyCor = null;
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rigid = null;
    /// <summary>
    /// Charge value
    /// </summary>
    private float charge = 1f;
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
    /// <param name="chargeValue"></param>
    public void Shoot(Vector3 direction, ForceMode forceMode, float chargeValue)
    {
        rigid.useGravity = false;
        rigid.isKinematic = false;
        rigid.AddForce(direction, forceMode);
        // StartCoroutine(AutoDestroy(1f));
        autoDestroyCor = StartCoroutine(AutoDestroy(1f, gameObject));
        charge = chargeValue;
    }
    /// <summary>
    /// Collider enter
    /// </summary>
    /// <param name="collision"></param>
    public void OnArrowCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Enemy"))
        if (collision.gameObject.tag.Equals(C.Ground))
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
        if (effectPrefab != null)
        {
            var go = Instantiate(effectPrefab, this.transform.position, this.transform.rotation);
            StartCoroutine(AutoDestroy(0.5f, go));
        }
        isAttack = false;
        Destroy(gameObject);
    }
    /// <summary>
    /// Get attack power
    /// </summary>
    /// <returns></returns>
    public float GetAttack() =>
        attack * charge;
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
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerator AutoDestroy(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}
