using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C = Constant;

public class Arrow : MonoBehaviour
{
    #region Variables

    #region SerializeField
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

    [SerializeField]
    private float gravityEnabledTime = 0.5f;
    #endregion

    #region Private
    /// <summary>
    /// Attack power
    /// </summary>
    /// public int Attack = 1;
    /// <summary>
    /// Auto-destructive coroutine
    /// </summary>
    private Coroutine autoDestroyCor = null;
    /// <summary>
    /// Gravity-enabled coroutine
    /// </summary>
    private Coroutine gravityCor = null;
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

    #endregion

    #region Methods

    #region Public
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
        gravityCor = StartCoroutine(GravityActivation());
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
            CancelGravityActivation();
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
        CancelGravityActivation();
        Destroy(gameObject);
    }
    /// <summary>
    /// Get attack power
    /// </summary>
    /// <returns></returns>
    public float GetAttack() =>
        attack * charge;
    #endregion

    #region Private
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
        CancelGravityActivation();
        Destroy(go);
    }
    /// <summary>
    /// Gravity-enabled coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator GravityActivation()
    {
        yield return new WaitForSeconds(gravityEnabledTime);
        if (rigid != null) rigid.useGravity = true;
    }
    /// <summary>
    /// Cancel coroutine for gravity
    /// </summary>
    private void CancelGravityActivation()
    {
        if (gravityCor != null)
        {
            StopCoroutine(gravityCor);
            gravityCor = null;
        }
    }
    #endregion
    #endregion
}
