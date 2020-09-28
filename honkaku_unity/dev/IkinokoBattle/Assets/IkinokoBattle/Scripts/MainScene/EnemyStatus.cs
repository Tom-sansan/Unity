using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatus : MobStatus
{
    private NavMeshAgent _agent;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    private void Update()
    {
        // Get 
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
    }

    protected override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DestroyCoroutine());
    }

    /// <summary>
    /// Destroy coroutine when dying...
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
