using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C = Constant;

public class AttackSphere : MonoBehaviour
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// Auto-destruct time
    /// </summary>
    [SerializeField]
    private float destroyTime = 0.5f;
    /// <summary>
    /// Attack power
    /// </summary>
    [SerializeField]
    private float attack = 1f;
    #endregion
    /// <summary>
    /// Coroutine
    /// </summary>
    private Coroutine cor = null;
    #endregion

    /// <summary>
    /// Initialization
    /// </summary>
    public void Init()
    {
        cor = StartCoroutine(AutoDestroy());
    }
    /// <summary>
    /// Automatic destruction coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
    /// <summary>
    /// Trigger Enter Callback. (Automatic execution)
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(C.Player))
        {
            var player = collider.gameObject.GetComponent<AppPlayerController>();
            player.OnEnemyAttackHit(attack);
            if (cor != null) StopCoroutine(cor);
            Destroy(gameObject);
        }
    }
}
