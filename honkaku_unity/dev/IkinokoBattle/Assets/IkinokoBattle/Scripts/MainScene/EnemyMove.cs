using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    // Layer Mask
    [SerializeField] private LayerMask raycastLayerMask;
    // [SerializeField] private PlayerController playerController;
    private NavMeshAgent _agent;
    private RaycastHit[] _raycastHits = new RaycastHit[10];
    private EnemyStatus _status;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    //void Update()
    //{
        // Moving to Query-Chan-SD
        //_agent.destination = playerController.transform.position;
    //}

    // Receive collision setting to onTriggerStay in CollisionDedector
    public void OnDetectObject(Collider collider)
    {
        // If detected object has 'Player' tag the agent traces the object
        if (collider.CompareTag("Player"))
        {
            // Coordinates Diff between this and Player
            var positionDiff = collider.transform.position - transform.position;
            // Debug.Log("positionDiff: " + positionDiff);
            // Distance for Player
            var distance = positionDiff.magnitude;
            // Debug.Log("distance: " + distance);
            // Direction to Player
            var direction = positionDiff.normalized;
            // Debug.Log("direction: " + direction);
            // Stores Collider and coordinate info to _raycastHits
            // RaycastAll and RaycastNonAlloc are the same function, however, RaycastNonAlloc is recommended because garbage is not kept in memory
            // var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance);
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance, raycastLayerMask);
            Debug.Log("hitCount: " + hitCount);
            if (hitCount == 0)
            {
                // This Player uses CharacterController and it doesn't use Collider so that Raycast isn't hit.
                // This means, if hitCount is 0 then there is no obstacle between the enemy and the Player.
                _agent.isStopped = false;
                _agent.destination = collider.transform.position;
            }
            else
            {
                // If missed, stopped
                _agent.isStopped = true;
            }
            
        }
    }
}
