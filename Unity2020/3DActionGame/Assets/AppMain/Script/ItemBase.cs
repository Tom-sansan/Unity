using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item Base Class
/// </summary>
[RequireComponent(typeof(ColliderCallReceiver))]
public class ItemBase : MonoBehaviour
{
    #region Variables
    [Header("Base Param")]
    #region SerializeField
    /// <summary>
    /// Effect prefab when scoring
    /// </summary>
    [SerializeField]
    private GameObject effectParticle = null;
    /// <summary>
    /// Item renderer
    /// </summary>
    [SerializeField]
    private Renderer itemRenderer= null;
    #endregion

    /// <summary>
    /// Collider call
    /// </summary>
    private ColliderCallReceiver colliderCall = null;
    /// <summary>
    /// Effect executed flag
    /// </summary>
    private bool isEffective = true;
    #endregion

    protected virtual void Start()
    {
        colliderCall= GetComponent<ColliderCallReceiver>();
        colliderCall.TriggerEnterEvent.AddListener(OnTriggerEnter);
    }

    private void Update()
    {
        
    }
    /// <summary>
    /// Process at the time of getting item
    /// </summary>
    protected virtual void ItemAction(Collider collider)
    {

    }
    /// <summary>
    /// Call when item trigger collider enter
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (!isEffective) return;
        if (collider.gameObject.tag == "Player")
        {
            // Execute overrideable process
            ItemAction(collider);
            isEffective = false;

            // Show effect
            if (effectParticle != null)
            {
                var pos = (itemRenderer == null) ? this.transform.position : itemRenderer.gameObject.transform.position;
                var obj = Instantiate(effectParticle, pos, Quaternion.identity);
                var particle = obj.GetComponent<ParticleSystem>();
                StartCoroutine(AutoDestroy(particle));
            }
            else Destroy(gameObject);

            Debug.Log("Got an item!");
        }
    }
    /// <summary>
    /// Destroy automatically
    /// </summary>
    /// <param name="particle"></param>
    /// <returns></returns>
    private IEnumerator AutoDestroy(ParticleSystem particle)
    {
        // Delete renderer first
        if (itemRenderer != null) itemRenderer.enabled = false;
        yield return new WaitUntil(() => !particle.isPlaying);
        // Destroy
        Destroy(gameObject);
    }
}
